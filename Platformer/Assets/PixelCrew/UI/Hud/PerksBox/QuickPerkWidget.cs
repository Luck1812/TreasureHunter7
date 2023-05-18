using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.PerksBox
{
    public class QuickPerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon; 
        [SerializeField] private Image _colldownShade;
        [SerializeField] private Text _keyDesignation;
        private GameSession _session;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void SetData(PerkDef data, int index)
        {
            _icon.sprite = data.Icon;
            _keyDesignation.text = data.KeyDesignation;
        }

        private void Update()
        {
            var cooldown = _session.PerksModel.Cooldown;
            _colldownShade.fillAmount = cooldown.RemainingTime / cooldown.Value;
        }
    }
}