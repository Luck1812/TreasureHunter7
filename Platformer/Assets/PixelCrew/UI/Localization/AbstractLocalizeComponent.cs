using PixelCrew.Model.Definitions.Localization;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    public abstract class AbstractLocalizeComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            LocalizationManager.Instance.OnLocaleChanged += OnLocaleChanged;
            Localize();
        }
        private void OnLocaleChanged()
        {
            Localize();
        }

        protected abstract void Localize();
        private void OnDestroy()
        {
            LocalizationManager.Instance.OnLocaleChanged -= OnLocaleChanged;
        }
    }
}