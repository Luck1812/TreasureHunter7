using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BombsController : MonoBehaviour
    {
        private SpawnInAreaComponent _spawner;

        private void Start()
        {
            _spawner = FindObjectOfType<SpawnInAreaComponent>();
        }

        public void StartBombSpawn()
        {
            _spawner.StartSpawn();
        }

        public void StopBombSpawn()
        {
            _spawner.StopSpawn();
        }

    }
}