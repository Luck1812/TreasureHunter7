using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Model;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;
        public PlayerData Data => _data;
        public PlayerData _save;
        public PerksModel PerksModel { get; private set; }
        public QuickInventoryModel QuickInventory { get; private set; }
        public StatsModel StatsModel { get; private set; }
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private readonly List<string> _checkpoints = new List<string>();

        private void Awake()
        {
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint);
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            
            LoadUI();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkPoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();
            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.Id == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);
            
            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);
        }

        private void LoadUI()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
            /*SceneManager.LoadScene("Controls", LoadSceneMode.Additive);*/
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            
            _trash.Dispose();
            InitModels();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id);
            }
        }
        private void OnDestroy()
        {
            _trash.Dispose();
        }

        private readonly List<string> _removedItems = new List<string>();

        public bool RestoreState(string itemId)
        {
            return _removedItems.Contains(itemId);
        }

        public void StoreState(string itemId)
        {
            if(!_removedItems.Contains(itemId))
                _removedItems.Add(itemId);
        }
    }
}