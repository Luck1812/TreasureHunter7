using UnityEngine;

namespace PixelCrew.Weapons
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class CannonBullet : MonoBehaviour
    {
        [SerializeField] private float _timeOfExistence;
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _flightDirections;

        private float _timeHasPassed;
        private Rigidbody2D _rigidbody;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            var xVelocity = _flightDirections.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            _timeHasPassed += Time.deltaTime;
            if(_timeHasPassed >= _timeOfExistence)
                Destroy(this.gameObject);
        }
        private float CalculateYVelocity()
        {
            var yVelocity = _flightDirections.y;
            return yVelocity;
        }
    }

}
