using UnityEngine;

namespace PixelCrew.UI.Hud
{
    public class ControlEnabledComponent : MonoBehaviour
    {
        private Canvas _canvas;
    
        private void Start()
        {
#if MOBILE_CONTROLS
            var controls = GameObject.FindWithTag("ControlsUICanvas");
            _canvas = controls.GetComponent<Canvas>();
#endif
        }

        public void SetActive(bool isEnabled)
        {
#if MOBILE_CONTROLS
            _canvas.enabled = isEnabled;
#endif
        }
    }
}

