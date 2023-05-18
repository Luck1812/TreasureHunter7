using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;

        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public HealthChangeEvent _onChange;
        private Lock _immune = new Lock();
        public int Health => _health;
        public Lock Immune => _immune;
        public void ApplyDamage(int damageValue)
        {
            if(_immune.IsLocked) return;
            
            if (_health <= 0) return;

            _health -= damageValue;
            _onChange?.Invoke(_health);

            _onDamage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void ApplyHealing(int healingValue)
        {
            _health += healingValue;
            _onChange?.Invoke(_health);

            _onHeal?.Invoke();
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }
        
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}

