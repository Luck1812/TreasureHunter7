using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(Light2D))]
    public class HeroFlashlightComponent : MonoBehaviour
    {
        [SerializeField] private float _rateOfLoss = 1f;
        [SerializeField] private float _doorstep = 10f;

        private Light2D _light;
        private GameSession _session;
        private Color _defaultAlpha;
        private void Start()
        {
            _light = GetComponent<Light2D>();
            _session = FindObjectOfType<GameSession>();
            
            _defaultAlpha = _light.color;
        }

        private void Update()
        {
            var fuel = Mathf.Max(_session.Data.Fuel.Value - Time.deltaTime * _rateOfLoss, 0);
            _session.Data.Fuel.Value = fuel;

            if (fuel > _doorstep)
                _light.color = _defaultAlpha;
            
            if (fuel <= _doorstep)
            {
                DarkeningLight();
            }
        }

        private void DarkeningLight()
        {
            var color = _light.color;
            var colorAlpha = color.a;
            colorAlpha = Mathf.MoveTowards(colorAlpha, 0, colorAlpha/_rateOfLoss*Time.deltaTime);
            color.a = colorAlpha;
            
            if (_session.Data.Fuel.Value <= 0)
                color.a = 0;
            
            _light.color = color;
        }
    }
}
