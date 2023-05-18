using System.Collections;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpawnInAreaComponent : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _container;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _delay;
        [SerializeField] private int _count;

        private Bounds _area;
        private Coroutine _coroutine;

        private void Start()
        {
            _area = _container.bounds;
        }

        [ContextMenu("Spawn")]
        public void StartSpawn()
        {
            if(_coroutine != null) return;
            
            _coroutine = StartCoroutine(SpawnObjects());
        }
        public void StopSpawn()
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        private IEnumerator SpawnObjects()
        {
            for (int i = 0; i < _count; i++)
            {
                var xPosition = Random.Range(_area.min.x, _area.max.x);
                var yPosition = Random.Range(_area.min.y, _area.max.y);
                var position = new Vector2(xPosition, yPosition);
                SpawnUtils.Spawn(_prefab, position);
                yield return new WaitForSeconds(_delay);
            }
        }
        
    }
}