using System;
using PixelCrew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{
    [CreateAssetMenu(fileName = "Potions", menuName = "Defs/Potions")]
    public class PotionRepository : DefRepository<PotionDef>
    {
        
    }

    [Serializable]
    public struct PotionDef : IHaveId
    {
       [InventoryId] [SerializeField] private string _id;
       [SerializeField] private Effect _effect;
       [SerializeField] private float _value;
       [SerializeField] private float _time;
        public string Id => _id;
        public Effect Effect => _effect;
        public float Value => _value;

        public float Time => _time;
    }

    public enum Effect
    {
        AddHp,
        SpeedUp
    }
}