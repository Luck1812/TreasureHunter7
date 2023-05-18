using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using Unity.VisualScripting;
using UnityEngine;

namespace PixelCrew.UI.Hud.PerksBox
{
    public class QuickPerksController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        
        private PredefinedDataGroup<PerkDef, QuickPerkWidget> _dataGroup;
        private List<PerkDef> _perks;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        
        private GameSession _session;
        private readonly PlayerData _data;
        

        private void Start()
        {
            _dataGroup = new PredefinedDataGroup<PerkDef, QuickPerkWidget>(_container);
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.PerksModel.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            var perksUsed = _session.Data.Perks.Used;
            _perks = new List<PerkDef>();
            if (perksUsed.Value.NullIfEmpty() != null)
            {
                var perkDef = DefsFacade.Instance.Perks.Get(perksUsed.Value);
                _perks.Add(perkDef);
            }
            _dataGroup.SetData(_perks);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}