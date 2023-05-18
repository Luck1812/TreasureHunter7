using PixelCrew.Animations;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] public ColliderCheck _vision;
        [SerializeField] private Cooldown _rangeAttackCooldown;
        [SerializeField] private SpriteAnimation _animation;

        private void Update()
        {
            if (_vision.IsTouchingLayer && _rangeAttackCooldown.IsReady)
            {
                Shoot();
            }
        }

        public void Shoot()
        {
            _rangeAttackCooldown.Reset();
            _animation.SetClip("Start-attack");
        }
    }
}


