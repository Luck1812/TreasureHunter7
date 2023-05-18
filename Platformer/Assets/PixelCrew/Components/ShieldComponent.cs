using PixelCrew.Components.Health;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShieldComponent : MonoBehaviour
    {
        [SerializeField] private Cooldown _timeOfAction;
        [SerializeField] private HealthComponent _health;

        public void Use()
        {
            _health.Immune.Retain(this);
            _timeOfAction.Reset();
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if(_timeOfAction.IsReady)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _health.Immune.Release(this);
        }
    }
}