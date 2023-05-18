using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Appearance;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Shop
{
    public class ClothingStoreWindows : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Button _nextLeft;
        [SerializeField] private Button _nextRight;
        [SerializeField] private Text _countCoins;
        public StoreModel Store { get; private set; }
        
        private PredefinedDataGroup<AppearanceDef,ClothingWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        

        private AppearanceDef[] _appearanceData;
        private AppearanceDef[] _appearanceForSale;
        private int _currentAppearanceIndex = 1;
        
        private const int _countProducts = 3;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new PredefinedDataGroup<AppearanceDef, ClothingWidget>(_container);
            _appearanceData = DefsFacade.Instance.Appearance.All;
            UpdateDisplayPrice();
            InitModels();
            _trash.Retain(Store.Subscribe(OnClothingChanges));
            OnClothingChanges();
        }

        public void UpdateDisplayPrice()
        {
            _countCoins.text = GameData.Instance.GameItems.Coins.ToString();
        }

        private void InitModels()
        {
            Store = new StoreModel();
            _trash.Retain(Store);
        }

        private void OnClothingChanges()
        {
            ToFillAppearanceArray();

            _dataGroup.SetData(_appearanceForSale);
        }
        
        private void ToFillAppearanceArray()
        {
            if (_appearanceData.Length > 0)
            {
                _appearanceForSale = new AppearanceDef[_countProducts];
                var index = _currentAppearanceIndex;
                for (int i = 0; i < _countProducts; i++)
                {
                    index = IsCorrectIndex(index, _appearanceData);

                    _appearanceForSale[i] = _appearanceData[index];
                    index++;
                }
            }
        }
        public void OnChangePageNumberTo(int value)
        {
            _currentAppearanceIndex += value;
            _currentAppearanceIndex = IsCorrectIndex(_currentAppearanceIndex, _appearanceData);

            OnClothingChanges();
        }

        private static int IsCorrectIndex(int index, Array dataArray)
        {
            if (index > dataArray.Length-1)
                return 0;
            else if (index < 0)
                return dataArray.Length - 1;
            
            return index;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}