using System;
using System.Collections;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Components.GoBased
{
    public class CircularProjectileSpawn : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        private Coroutine _coroutine;
        public int Stage { get; set; }


        [ContextMenu("SpawnProject")]
        public void LaunchProjectile()
        {
            _coroutine = StartCoroutine(SpawnProjectile());
        }

        private IEnumerator SpawnProjectile()
        {
            for (int b = 0; b < _settings[Stage].ItemPerBurst; b++)
            {
                var setting = _settings[Stage];
                var sectorStep = 2 * Mathf.PI / setting.BurstCount;
                for (int i = 0; i < setting.BurstCount; i++)
                {
                    var angle = sectorStep * i;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(direction);
                    yield return new WaitForSeconds(setting.Delay);
                    
                }
                
            }
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [FormerlySerializedAs("_projectile")] [SerializeField] private DirectionalProjectile prefab;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;
        [SerializeField] private int _itemPerBurst;

        public DirectionalProjectile Prefab => prefab;

        public int BurstCount => _burstCount;

        public float Delay => _delay;
        public int ItemPerBurst => _itemPerBurst;
    }
}