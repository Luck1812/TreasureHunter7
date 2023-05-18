using System;
using PixelCrew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(fileName = "Throwable", menuName = "Defs/Throwable")]
    public class ThrowableRepository: DefRepository<ThrowableDef>
    {

    }

    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;
        
        public string Id => _id;
        public GameObject Projectile => _projectile;
        
    }
}