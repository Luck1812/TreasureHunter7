using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Weapons
{
    public class CreationsObjectComponent : MonoBehaviour
    {
        [SerializeField] private bool _loop;
        [SerializeField] private int _timeBetweenCreation;
        [SerializeField] private Transform _placeOfCreation;
        [SerializeField] private GameObject _prefabForCreation;
        [SerializeField] private UnityEvent _onCreation;
        private float _timeHasPassed;

        [ContextMenu("Spawn")]
        private void InstanceObject()
        {
            _onCreation?.Invoke();
            var instantiate = Instantiate(_prefabForCreation, _placeOfCreation.position, Quaternion.identity);
        }

        private void Update()
        {
            _timeHasPassed += Time.deltaTime;
            if (_timeHasPassed >= _timeBetweenCreation)
            {
                if (_loop)
                {
                    InstanceObject();
                    _timeHasPassed = 0f;
                }
                else
                {
                    InstanceObject();
                    enabled = false;
                }
            }

        }
    }
}

