using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class GameDataItems
    {
        [SerializeField] private int _coins;
        [SerializeField] private string _chosenAppearanceKey;
        [SerializeField] private List<string> _purchasedLooksAppearanceKeys;

        public GameDataItems(int coins, string chosenAppearanceKey, List<string> purchasedLooksAppearanceKeys)
        {
            this._coins = coins;
            this._chosenAppearanceKey = chosenAppearanceKey;
            this._purchasedLooksAppearanceKeys = purchasedLooksAppearanceKeys;
        }
        public int Coins
        {
            get => _coins;
            set => _coins = value;
        }

        public string ChosenAppearanceKey
        {
            get => _chosenAppearanceKey;
            set => _chosenAppearanceKey = value;
        }
        public List<string> PurchasedLooksAppearance => _purchasedLooksAppearanceKeys;

    }
}