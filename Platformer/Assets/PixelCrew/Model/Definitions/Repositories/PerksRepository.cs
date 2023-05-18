using System;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{
    [CreateAssetMenu(fileName = "Perks", menuName = "Defs/Perks")]
    public class PerksRepository  : DefRepository<PerkDef>
    {
        
    }
    [Serializable]
    public struct PerkDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _info;
        [SerializeField] private string _keyDesignation;
        [SerializeField] private ItemWithCount _price;
        [SerializeField] private Cooldown _cooldown;

        public string Id => _id;

        public Sprite Icon => _icon;

        public string Info => _info;
        public string KeyDesignation => _keyDesignation;

        public ItemWithCount Price => _price;
        public Cooldown Cooldown => _cooldown;
    }

    [Serializable]
    public struct ItemWithCount 
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public string Id => _id;
        public int Count => _count;
    }
}