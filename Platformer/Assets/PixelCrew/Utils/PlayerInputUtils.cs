using PixelCrew.Creatures.Hero;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Utils
{
    public class PlayerInputUtils
    {
        public static void SetActive(bool isActive)
        {
            var hero = GameObject.FindObjectOfType<Hero>();
            var _heroInput = hero.GetComponent<PlayerInput>();
            _heroInput.enabled = isActive;
        }
    }
}