using System;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private PerksData _perks;
        [SerializeField] private LevelData _levels;
        [SerializeField] private FloatProperty _fuel;

        public InventoryData Inventory => _inventory;
        public PerksData Perks => _perks;
        public LevelData Levels => _levels;
        public FloatProperty Fuel => _fuel;

        public IntProperty Hp = new IntProperty();

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

}
