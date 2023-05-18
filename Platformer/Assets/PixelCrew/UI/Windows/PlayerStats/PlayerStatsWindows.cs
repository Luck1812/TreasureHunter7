using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.PlayerStats
{
    public class PlayerStatsWindows : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private StatWidget _prefab;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private ItemWidget _price;
        private DataGroup<StatDef, StatWidget> _dataGroup;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;
        protected override void Start()
        {
            base.Start();

            _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _container);
            
            _session = FindObjectOfType<GameSession>();
            _session.StatsModel.InterfaceSelectedStat.Value = DefsFacade.Instance.Player.Stats[0].ID;
            _trash.Retain(_session.StatsModel.Subscribe(OnStatsChanged));
            _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgrade));
            
            OnStatsChanged();
        }

        private void OnStatsChanged()
        {
            var stats = DefsFacade.Instance.Player.Stats;
            _dataGroup.SetData(stats);

            var selected = _session.StatsModel.InterfaceSelectedStat.Value;
            var nextLevel = _session.StatsModel.GetCurrentLevel(selected) + 1;
            var def = _session.StatsModel.GetLevelDef(selected, nextLevel);
            _price.SetData(def.Price);
            
            _price.gameObject.SetActive(def.Price.Count != 0);
             _upgradeButton.gameObject.SetActive(def.Price.Count != 0);
        }
        private void OnUpgrade()
        {
             var selected = _session.StatsModel.InterfaceSelectedStat.Value;
             _session.StatsModel.LevelUp(selected);
        }
        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}