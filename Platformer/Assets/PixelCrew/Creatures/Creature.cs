using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] protected PlaySoundsComponent Sounds;
        [SerializeField] protected GameObject _attackObject;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] protected SpawnListComponent _particles;
        

        protected Vector2 Direction;
        protected Rigidbody2D RigidBody;
        protected Animator Animator;
        private CheckCircleOverlap _attackRange;
        protected PropertiesPropsComponent _attack;
        protected bool IsGrounded;
        private bool _isJump;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
        protected virtual void Awake()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            _attackRange = _attackObject.GetComponent<CheckCircleOverlap>();
            _attack = _attackObject.GetComponent<PropertiesPropsComponent>();
        }
        private void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();

            RigidBody.velocity = new Vector2(xVelocity, yVelocity);
            Animator.SetBool(IsGroundKey, _groundCheck.IsTouchingLayer);
            Animator.SetFloat(VerticalVelocity, RigidBody.velocity.y);
            Animator.SetBool(IsRunning, Direction.x != 0);

            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateXVelocity()
        {
            return Direction.x * CalculateSpeed();
        }

        protected virtual float CalculateSpeed()
        {
            return _speed;
        }
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = RigidBody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _isJump = false;
            }

            if (isJumpPressing)
            {
                _isJump = true;
                var isFalling = RigidBody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (RigidBody.velocity.y > 0 && _isJump)
            {
                yVelocity *= 0.5f;
            }
            return yVelocity;
        }
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity = _jumpSpeed;
                DoJumpVfx();
            }
            return yVelocity;
        }
        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
        }
        public virtual void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0) transform.localScale = new Vector3(multiplier, 1, 1);
            else if (direction.x < 0) transform.localScale = new Vector3(-1 * multiplier, 1, 1);
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }


        public virtual void TakeDamage()
        {
            _isJump = false;
            Animator.SetTrigger(Hit);
            RigidBody.velocity = new Vector2(RigidBody.velocity.x, _damageVelocity);
        }
        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }

        public void OnExecuteAttack()
        {
            _attackRange.Check();
        }

    }
}

