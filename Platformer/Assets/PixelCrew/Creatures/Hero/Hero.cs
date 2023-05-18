using System.Collections;
using System.Linq;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Appearance;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;


namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hero : Creature
    {
        
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private ProbabilityDropComponent _hitDrop;
        [SerializeField] private GameObject _flashlight;
        [SerializeField] private Canvas _playerCanvas;
        [SerializeField] private Text _nickname;
        [SerializeField] private float _slamDownVelocity;
        [Header("ShieldPerk")]
        [SerializeField] private ShieldComponent _shield;

        [Header("SuperThrowPerk")] [SerializeField]
        private Cooldown _superThrowCooldown;

        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;

        [SerializeField] private SpawnComponent _throwSpawner;
        
        private Cooldown _speedUpCooldown = new Cooldown();
        private GameSession _session;
        private HealthComponent _healthComponent;
        private RuntimeAnimatorController _armed;
        private RuntimeAnimatorController _disarmed;
        private CameraShakeEffect _cameraShake;
        
        private bool _allowDoubleJump;
        private bool _isOnWall;
        private bool _superThrow;

        private float _defaultGravityScale;
        private float _additionalSpeed;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWallKey = Animator.StringToHash("is-on-wall");

        private const string SwordId = "Sword";
        
        private const string ArmedAnimatorID = "Armed";
        private const string UnArmedAnimatorID = "UnArmed";
        private int NumCoins => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;
                
                var def = DefsFacade.Instance.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _defaultGravityScale = RigidBody.gravityScale;
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void Start()
        {
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgrade;
            _nickname.text = GameSettings.Instance.Nickname.Value;
                
            var appearanceDefs = DefsFacade.Instance.Appearance.All;
            AppearanceDef appearance = appearanceDefs.ToList().Find(x => x.Id == GameData.Instance.GameItems.ChosenAppearanceKey);

            AppointmentOfAnimator animatorArmed = appearance.AnimatorAssignments.ToList().Find(x => x.Id == ArmedAnimatorID);
            AppointmentOfAnimator animatorUnArmed = appearance.AnimatorAssignments.ToList().Find(x => x.Id == UnArmedAnimatorID);

            _armed = animatorArmed.Animator != null ? animatorArmed.Animator : animatorArmed.OverrideAnimator;
            _disarmed = animatorUnArmed.Animator != null ? animatorUnArmed.Animator : animatorUnArmed.OverrideAnimator;


            
            
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        public override void UpdateSpriteDirection(Vector2 direction)
        {
            base.UpdateSpriteDirection(Direction);
            
            if (direction.x < 0) _playerCanvas.transform.rotation = new Quaternion(0f,180f,0f,0f);
            else if (direction.x > 0) _playerCanvas.transform.rotation = new Quaternion(0f,0f,0f,0f);

        }
        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }
        private void OnHeroUpgrade(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = _session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = (int)health;
                    _healthComponent.SetHealth((int)health);
                    break;
                case StatId.Damage:
                    var damage = _session.StatsModel.GetValue(statId);
                    _attack.Damage = (int)damage;
                    break;
            }
        }
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }

        protected override void Update()
        {
            base.Update();
            
            
            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                RigidBody.gravityScale = 0;
                RigidBody.velocity = new Vector2(0f, 0f);
            }
            else
            {
                _isOnWall = false;
                RigidBody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(IsOnWallKey, _isOnWall);
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                DoJumpVfx();
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            _cameraShake.Shake();
            if (NumCoins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispone = Mathf.Min(NumCoins, 5);

            _session.Data.Inventory.Remove("Coin", numCoinsToDispone);

            _hitDrop.SetCount(numCoinsToDispone);
            _hitDrop.CalculateDrop();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public override void Attack()
        {
            if (SwordCount <= 0 || !IsGrounded) return;

            ModifyAttackDamage();
            base.Attack();
            _particles.Spawn("AttackOne");
            Sounds.Play("Attack");
        }

        private void ModifyAttackDamage()
        {
            var modifyDamage = ModifyDamageByCrit(_attack.Damage);
            if (modifyDamage == _attack.Damage)
                _attack.AdditionalDamage = 0;
            else
                _attack.AdditionalDamage = _attack.Damage;
        }

        public void OnHealthChanged(int currentHealth)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var health = Mathf.Min(currentHealth, (int)maxHealth);
            _healthComponent.SetHealth(health);
            _session.Data.Hp.Value = health;
        }

        public void OnExecuteThrow()
        {
            if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                
                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                _session.PerksModel.Cooldown.Reset();
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.Instance.Throwable.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            var instance = _throwSpawner.SpawnInstance();
            ApplyRangeDamageStat(instance);
            _session.Data.Inventory.Remove(throwableId, 1);
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
            var hpModify = projectile.GetComponent<PropertiesPropsComponent>();
            var damageValue = (int) _session.StatsModel.GetValue(StatId.Damage);
            damageValue = ModifyDamageByCrit(damageValue);
            hpModify.Damage = damageValue;
        }

        private int ModifyDamageByCrit(int damage)
        {
            var critDamage = _session.StatsModel.GetValue(StatId.CritDamage);
            if (Random.value * 100 <= critDamage)
            {
                return damage * 2;
            }

            return damage;
        }
        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void UseInventory()
        {
            if(IsSelectedItem(ItemTag.Throwable))
                PerformThrowing();
            else if(IsSelectedItem(ItemTag.Potion))
                UsePotion();

        }
        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }
        private void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThrow || _isOnWall) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            Sounds.Play("ThrowAttack");
            _throwCooldown.Reset();
        }
        private void UsePotion()
        {
            var potion = DefsFacade.Instance.Potions.Get(SelectedItemId);
            switch (potion.Effect)
            {
                case Effect.AddHp:
                    _healthComponent.ApplyHealing((int) potion.Value);
                    break;
                case Effect.SpeedUp:
                    _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }
            
            _session.Data.Inventory.Remove(potion.Id, 1);
        }
        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;
            
            return _session.StatsModel.GetValue(StatId.Speed) + _additionalSpeed;
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void StartShield()
        {
            if(!_session.PerksModel.IsShieldSupported) return;
            
            _shield.Use();
            _session.PerksModel.Cooldown.Reset();
        }

        public void UseFlashlight()
        {
            _flashlight.SetActive(!_flashlight.activeSelf);
        }
    }
}