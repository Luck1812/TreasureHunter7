using System;
using System.Globalization;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.PlayerStats
{
    public class StatWidget : MonoBehaviour,  IItemRenderer<StatDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _nameStats;
        [SerializeField] private Text _currentValue;
        [SerializeField] private Text _increaseValue;
        [SerializeField] private ProgressBarWidget _progressBar;
        [SerializeField] private GameObject _selected;

        private GameSession _session;
        private StatDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }

        public void SetData(StatDef data, int index)
        {
            _data = data;
            if(_session != null)
                UpdateView();
        }
        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _nameStats.text = LocalizationManager.Instance.Localize(_data.Name);
            var statsModel = _session.StatsModel;
            var currentLevelValue = statsModel.GetValue(_data.ID);
            _currentValue.text = currentLevelValue.ToString(CultureInfo.InvariantCulture);

            var currentLevel = statsModel.GetCurrentLevel(_data.ID);
            var nextLevel = currentLevel + 1;
            var nextLevelValue = statsModel.GetValue(_data.ID, nextLevel);
            var increaseValue = nextLevelValue - currentLevelValue;
            _increaseValue.text = $"+ {increaseValue.ToString(CultureInfo.InvariantCulture)}";
            _increaseValue.gameObject.SetActive(increaseValue > 0);

            var maxLevel= DefsFacade.Instance.Player.GetStat(_data.ID).Levels.Length - 1;
            _progressBar.SetProgress(currentLevel/ (float) maxLevel);
            
            _selected.SetActive(statsModel.InterfaceSelectedStat.Value == _data.ID);
        }
        public void OnSelect()
        {
            _session.StatsModel.InterfaceSelectedStat.Value = _data.ID;
        }
    }
}