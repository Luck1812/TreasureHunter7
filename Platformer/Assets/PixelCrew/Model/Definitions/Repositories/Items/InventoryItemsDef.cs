using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories.Items
{
    [CreateAssetMenu(fileName = "InventoryItems", menuName = "Defs/InventoryItems")]
    public class InventoryItemsDef : DefRepository<ItemDef>
    {

#if UNITY_EDITOR
    public ItemDef[] ItemsForEditor => _collection;
#endif
    }



    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;
        
        public Sprite Icon => _icon;
        public string Id => _id;
        public bool IsVoid => string.IsNullOrEmpty(_id);

        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }
        
    }
}