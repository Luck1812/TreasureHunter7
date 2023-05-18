using PixelCrew.Components;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions.Repositories.Appearance;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Shop
{
    [RequireComponent(typeof(CreateMessageComponent))]
    public class ClothingWidget : MonoBehaviour, IItemRenderer<AppearanceDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Text _buyTextNormal;
        [SerializeField] private Text _buyTextPressed;
        [SerializeField] private Image _priceIconNormal;
        [SerializeField] private Image _priceIconPressed;

        private const string ButtonUseLocale = "button_use"; 
        private const string ButtonChooseLocale = "button_choose";
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private ClothingStoreWindows _storeWindows;

        public PlayerPersistentProperty _model;
        private AppearanceDef _data;
        private CreateMessageComponent _messageExclamation;

        private void Start()
        {
            _storeWindows = FindObjectOfType<ClothingStoreWindows>();
            _messageExclamation = GetComponent<CreateMessageComponent>();
            UpdateView();
        }

        public void SetData(AppearanceDef data, int index)
        {
            _data = data;
            
            UpdateView();
        }
        
        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _trash.Dispose();

            if (GameData.Instance.IsUseAppearance(_data.Id))
            {
                SetTheCost(ButtonChooseLocale);
                TurnOnButton(false,false);
            }
            else if (GameData.Instance.IsBuyAppearance(_data.Id))
            {
                SetTheCost(ButtonUseLocale);
                TurnOnButton(true,false);
                _trash.Retain(_buyButton.onClick.Subscribe(ChooseAppearance));
            }
            else
            {
                _buyTextNormal.text = _data.Price.Count.ToString();
                _buyTextPressed.text = _data.Price.Count.ToString();
                TurnOnButton(true, true);
                _trash.Retain(_buyButton.onClick.Subscribe(BuyAppearance));
            }
            
        }
        private void TurnOnButton(bool interact, bool visibilityPrice)
        {
            _buyButton.interactable = interact;
            _priceIconNormal.enabled = visibilityPrice;
            _priceIconPressed.enabled = visibilityPrice;
        }
        private void SetTheCost(string costText)
        {
            _buyTextNormal.text = LocalizationExtensions.Localize(costText);
            _buyTextPressed.text = LocalizationExtensions.Localize(costText);
        }
        private void ChooseAppearance()
        {
            _storeWindows.Store.ChooseAppearance(_data.Id);
        }
        private void BuyAppearance()
        {
            if (_storeWindows.Store.IsEnoughCoins(_data.Price.Count))
            {
                _storeWindows.Store.AddAppearanceInCollection(_data.Id);
                _storeWindows.UpdateDisplayPrice();
            }
            else
                _messageExclamation.CreateMessage();
        }
        
        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
    
}