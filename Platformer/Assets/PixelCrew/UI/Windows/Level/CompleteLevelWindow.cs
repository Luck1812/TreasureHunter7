using System;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Level
{
    public class CompleteLevelWindow : AnimatedWindow
    {
        [SerializeField] private Text _countResult;
        private GameSession _session;
        private Action _closeAction;
        private int NumCoins => _session.Data.Inventory.Count("Coin");
        
        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();
            
            _session = FindObjectOfType<GameSession>();
            _countResult.text = NumCoins.ToString();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void CompleteLevel()
        {
            Complete();
        }
        private void Complete()
        {
            Time.timeScale = _defaultTimeScale;
            
            _closeAction = () =>
            {
                var levelLoader = FindObjectOfType<LevelLoader>();
                levelLoader.LevelLoad("MainMenu");
            };

            var countCoins = NumCoins;
            GameData.Instance.AddCoins(countCoins);
            
            _closeAction?.Invoke();
        }
        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}