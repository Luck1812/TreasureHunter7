using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FallingBomb : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] protected bool _invertY;
        private Rigidbody2D _rigidbody;
        private int _direction;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            var invertY = _invertY ? 1 : -1;
            _direction = invertY * transform.lossyScale.y > 0 ? 1 : -1;
            var force = new Vector2(0, _direction * _speed);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
