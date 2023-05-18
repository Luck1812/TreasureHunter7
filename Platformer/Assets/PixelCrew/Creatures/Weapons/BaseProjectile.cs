using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected bool _invertX;
        protected Rigidbody2D Rigidbody;
        protected int Direction;
        protected virtual void Start()
        {
            var invertX = _invertX ? 1 : -1;
            Direction = invertX * transform.lossyScale.x > 0 ? 1 : -1;
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }

}
