using System;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data.Model
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();
        public readonly Cooldown Cooldown = new Cooldown();

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;
        public string Used => _data.Perks.Used.Value;
        public bool IsSuperThrowSupported => _data.Perks.Used.Value == "Super-throw" && Cooldown.IsReady;
        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "Double-jump"  && Cooldown.IsReady;
        public bool IsShieldSupported => _data.Perks.Used.Value == "Shield"  && Cooldown.IsReady;

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.Instance.Perks.All[0].Id;
            
            _trash.Retain(_data.Perks.Used.Subscribe((x,y) => OnChanged?.Invoke()));
            _trash.Retain( InterfaceSelection.Subscribe((x,y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.Instance.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if(isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.Id, def.Price.Count);
                _data.Perks.AddPerk(id);
                
                OnChanged?.Invoke();
            }
        }
        public void SelectPerk(string selected)
        {
            var perkDef = DefsFacade.Instance.Perks.Get(selected);
            Cooldown.Value = perkDef.Cooldown.Value;
            _data.Perks.Used.Value = selected;
        }
        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.Instance.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }
        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}