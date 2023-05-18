using UnityEngine;


namespace PixelCrew.Components.Health
{
    public class PropertiesPropsComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _healing;
        private int _additionalDamage;
        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }
        public int AdditionalDamage
        {
            get => _additionalDamage;
            set => _additionalDamage = value;
        }
        public void ApplyDamage(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyDamage(_damage + _additionalDamage);
            }
        }
        public void ApplyHealing(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyHealing(_healing);
            }
        }
    }

}
