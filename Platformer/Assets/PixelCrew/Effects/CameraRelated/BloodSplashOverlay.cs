using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(Animator))]
    public class BloodSplashOverlay : MonoBehaviour
    {
        [SerializeField] private Transform _overlay;
        
        private Animator _animator;
        private Vector3 _overScale;
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly int Health = Animator.StringToHash("Health");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _overScale = _overlay.localScale - Vector3.one;

            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHpChange));
        }

        private void OnHpChange(int newValue, int _)
        {
            var maxHp = _session.StatsModel.GetValue(StatId.Hp);
            var hpNormalized = newValue / maxHp;
            _animator.SetFloat(Health, hpNormalized);


            var overlayModifer = Mathf.Max(hpNormalized - 0.3f, 0f);
            _overlay.localScale = Vector3.one + _overScale * overlayModifer;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}