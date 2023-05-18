using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(Volume))]
    public class VignetteEffect : MonoBehaviour
    {
        
        [SerializeField] private float _intesivity;
        [SerializeField] private float _smoothness;
        private Volume _volume;
        private Vignette _vignetteEffect;
        private void Awake()
        {
            _volume = GetComponent<Volume>();
            
            var profile = _volume.profile.components;
            foreach (var item in profile)
            {
                if (item.name == "Vignette(Clone)")
                {
                    _vignetteEffect = (Vignette)item;
                }
            }
        }

        public void EffectStart()
        {
            _vignetteEffect.intensity.value = _intesivity;
            _vignetteEffect.smoothness.value = _smoothness;
        }

        public void EffectClose()
        {
            _vignetteEffect.intensity.value = 0f;
            _vignetteEffect.smoothness.value = 0f;
        }
    }
}
