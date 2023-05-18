using System.Collections;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;

        private Vector3 _vectorZero = Vector3.zero;
        private float _startTime;
        private void Start()
        {
            _startTime = Time.time;
        }
        public void Teleport(GameObject target)
        {
            PlayerInputUtils.SetActive(false);

            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();

            yield return AlphaAnimation(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);

            target.SetActive(true);
            
            PlayerInputUtils.SetActive(true);
            
            yield return AlphaAnimation(sprite, 1);
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            while (target.transform.position != Vector3.SmoothDamp(target.transform.position, _destTransform.position, ref _vectorZero, _moveTime))
            {
                var progress = _moveTime + Time.deltaTime;
                target.transform.position = Vector3.SmoothDamp(target.transform.position, _destTransform.position, ref _vectorZero, progress);
                yield return null;
            }
        }
        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {

            var time = 0f;
            var spriteAlpha = sprite.color.a;

            while (time < _alphaTime)
            {
                time += Time.deltaTime;

                var progress = time / _alphaTime;
                var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                yield return null;
            }
        }
    }
}

