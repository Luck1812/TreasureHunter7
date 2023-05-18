using System;
using UnityEngine;

namespace PixelCrew.UI
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();

            _animator.SetTrigger(Show);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }

        protected virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}