using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.UseInventory();
            }
        }
        public void OnUsePerk(InputAction.CallbackContext context)
        {
            if (context.started)
            { 
                _hero.StartShield();
            }
        }
        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.NextItem();
            }
        }
        public void OnUseFlashlight(InputAction.CallbackContext context)
        {
            if (context.started)
            {
               _hero.UseFlashlight();
            }

        }
    }
}