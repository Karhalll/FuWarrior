using UnityEngine;

using FuWarrior.Control;

namespace FuWarrior.Combat
{
    public class WeakPoint : MonoBehaviour, IRaycastable
    {
        private void OnCollisionEnter2D(Collision2D other) 
        {
            Debug.Log("Weak point Hitted!");
        }

        public CursorType GetCursorType()
        {
            return CursorType.AimToWeakPoint;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            //  if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            // {
            //     return false;
            // }

            // if (Input.GetMouseButton(0))
            // {
            //     callingController.GetComponent<Fighter>().Attack(gameObject);
            // }

            return true;
        }
    }
}