using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class SharkyAI : MobAI
    {
        [SerializeField] private float _horizontalTreshold = 0.2f;
        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    var horizontalDelta = Mathf.Abs(_target.transform.position.x - transform.position.x);
                    if (horizontalDelta <= _horizontalTreshold)
                        _creature.SetDirection(Vector2.zero);
                    else
                        SetDirectionToTarget();
                }
                yield return null;
            }

            yield return new WaitForSeconds(_missHeroCooldown);
            _particles.Spawn("MissHero");
            _creature.SetDirection(Vector2.zero);
            yield return new WaitForSeconds(_missHeroCooldown);
            StartState(_patrol.DoPatrol());
        }
        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToHero());
        }

        protected override IEnumerator AgroToHero()
        {
            base.AgroToHero();
            
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }
    }
}