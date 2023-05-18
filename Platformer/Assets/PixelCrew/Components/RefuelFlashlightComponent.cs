using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public class RefuelFlashlightComponent : MonoBehaviour
    {
        [SerializeField] private float _countFuel;

        private GameSession _session;

        public void ReplenishFuel()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Fuel.Value = _countFuel;
        }
    }
}
