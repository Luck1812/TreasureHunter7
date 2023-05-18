using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class SeashellTrapAI : MonoBehaviour
    {


        [Header("Bite attack")]
        [SerializeField] private ColliderCheck _canBiteAttack;
        [SerializeField] private CheckCircleOverlap _biteAtack;
        [SerializeField] private Cooldown _biteAttackCooldown;


        [Header("Range attack")]
        [SerializeField] private ColliderCheck _vision;
        [SerializeField] private SpawnComponent _rangeAttack;
        [SerializeField] private Cooldown _rangeAttackCooldown;


        private static readonly int HitKey = Animator.StringToHash("hit");
        private static readonly int RangeAttackKey = Animator.StringToHash("range-attack");
        private static readonly int BiteKey = Animator.StringToHash("bite");

        private Animator _animator;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_canBiteAttack.IsTouchingLayer)
                {
                    if (_biteAttackCooldown.IsReady)
                        BiteAttack();
                    return;
                }

                if (_rangeAttackCooldown.IsReady)
                    RangeAttack();
            }
        }


        private void BiteAttack()
        {
            _biteAttackCooldown.Reset();
            _animator.SetTrigger(BiteKey);
        }
        public void OnBiteAttack()
        {
            _biteAtack.Check();
        }
        private void RangeAttack()
        {
            _rangeAttackCooldown.Reset();
            _animator.SetTrigger(RangeAttackKey);
        }
        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
