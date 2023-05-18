using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(fileName = "PlayerDef", menuName = "Defs/PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _maxHealt;
        [SerializeField] private StatDef[] _stats;
        
        public int InventorySize => _inventorySize;
        
        public StatDef[] Stats => _stats;
        public StatDef GetStat(StatId id) => _stats.FirstOrDefault(x => x.ID == id);
    }
}