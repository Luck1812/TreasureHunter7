using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private GameDataItems _gameItems;
        public GameDataItems GameItems => _gameItems;

        private static GameData _instance;
        public static GameData Instance => _instance == null ? LoadGameData() : _instance;
        private static GameData LoadGameData()
        {
            return _instance = Resources.Load<GameData>("GameData");
        }
        /*private void OnEnable()
        {
            _gameItems = new PlayerPersistentProperty(new GameDataItems(5000, "BlueAppearance", new List<string>(){"BlueAppearance"}), "GameItems");
        }*/
        /*private void OnValidate()
        {
            GameItems.Validate();
        }*/
        public bool IsBuyAppearance(string appearanceKey)
        {
            foreach (var appearance in _gameItems.PurchasedLooksAppearance)
            {
                if (appearance == appearanceKey)
                    return true;
            }
            return false;
        }

        public bool IsUseAppearance(string appearanceKey)
        {
            return _gameItems.ChosenAppearanceKey == appearanceKey;
        }

        public void AddNewAppearance(string appearanceKey)
        {
            if(IsBuyAppearance(appearanceKey))
                return;
            
            _gameItems.PurchasedLooksAppearance.Add(appearanceKey);
        }

        public bool SubtractCoins(int coins)
        {
            if (_gameItems.Coins < coins) return false;

            _gameItems.Coins -= coins;
            return true;
        }
        public void SetAppearance(string appearanceKey)
        {
            _gameItems.ChosenAppearanceKey = appearanceKey;
        }

        public void AddCoins(int coins)
        {
            _gameItems.Coins += coins;
        }
    }
}