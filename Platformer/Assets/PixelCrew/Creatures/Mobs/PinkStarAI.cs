using System.Collections;
using PixelCrew.Components.ColliderBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class PinkStarAI : MobAI
    {
        [SerializeField] private LayerCheck _groundCheck;
        
        [SerializeField] private LayerCheck _obstacleCheck;
        
        private static readonly int RotationalAttackKey = Animator.StringToHash("rotational-attack");

        private bool _isRotational;
        private IEnumerator GoToRotational()
        {
            _animator.SetBool(RotationalAttackKey,true);
            _isRotational = true;
            _creature.Attack();
            SetDirectionToTarget();
            while (_groundCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    _creature.OnExecuteAttack();
                    yield return new WaitForSeconds(_attackCooldown);
                }
                yield return null;
            }
            
            _animator.SetBool(RotationalAttackKey,false);
            _isRotational = false;
            _creature.SetDirection(Vector2.zero);
            yield return new WaitForSeconds(_missHeroCooldown);

            if (_vision.IsTouchingLayer)
            {
                LookAtHero();
                StartState(GoToRotational());
            }
            else StartState(_patrol.DoPatrol());
        }
        protected override IEnumerator AgroToHero()
        {
            base.AgroToHero();
            
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToRotational());
        }
        public override void OnHeroInVision(GameObject go)
        {
           if(_isRotational) return;
           
           base.OnHeroInVision(go);
        }
    }
    
}