using System;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data.Model
{
    public class StoreModel : IDisposable
    {
        public Action OnChanged;

        public StoreModel()
        {
        }

        public void AddAppearanceInCollection(string appearanceKey)
        { 
            GameData.Instance.AddNewAppearance(appearanceKey);
           OnChanged?.Invoke();
        }
        public void ChooseAppearance(string appearanceKey)
        {
            GameData.Instance.SetAppearance(appearanceKey);
            OnChanged?.Invoke();
        }
        public bool IsEnoughCoins(int coins)
        {
            return GameData.Instance.SubtractCoins(coins);
        }
        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

      
        public void Dispose()
        {
            
        }
    }
}