using System.Collections;
using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Patrolling;

namespace PixelCrew.Creatures.Mobs
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] protected float _alarmDelay = 0.5f;
        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] protected float _missHeroCooldown = 0.5f;
        [SerializeField] protected LayerCheck _vision;
        [SerializeField] protected LayerCheck _canAttack;


        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");


        private IEnumerator _current;
        protected GameObject _target;
        protected Creature _creature;
        protected SpawnListComponent _particles;
        protected Animator _animator;
        protected Patrol _patrol;

        private bool _isDead;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }
        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        protected void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);

            _current = coroutine;
            StartCoroutine(coroutine);
        }

        protected virtual IEnumerator AgroToHero()
        {
            LookAtHero();
            yield return null;
        }

        protected void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }
        
        protected void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public void OnDie()
        {
            _creature.SetDirection(Vector2.zero);
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            if (_current != null)
                StopCoroutine(_current);
        }
        public virtual void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            
            _target = go;
            StartState(AgroToHero());
        }
    }

}

