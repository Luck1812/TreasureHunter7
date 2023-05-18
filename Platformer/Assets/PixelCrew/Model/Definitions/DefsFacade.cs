using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Appearance;
using PixelCrew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(fileName = "DefsFacade", menuName = "Defs/DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private PlayerDef _player;
        [SerializeField] private ThrowableRepository _throwableItems;
        [SerializeField] private PotionRepository _potions;
        [SerializeField] private PerksRepository _perks;
        [SerializeField] private AppearanceRepository _appearance;

        public InventoryItemsDef Items => _items;
        public PlayerDef Player => _player;
        public ThrowableRepository Throwable => _throwableItems;
        public PotionRepository Potions => _potions;
        public PerksRepository Perks => _perks;
        public AppearanceRepository Appearance => _appearance;
        
        private static DefsFacade _instance;
        public static DefsFacade Instance => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
        
    }
}

