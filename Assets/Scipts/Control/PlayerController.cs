using UnityEngine;

using FuWarrior.Combat;

namespace FuWarrior.Control
{
    public class PlayerController : MonoBehaviour 
    {
        Fighter fighter = null;

        private void Awake() 
        {
            fighter = GetComponent<Fighter>();
        }

        private void Update() 
        {

        }

        public Vector2 MousePositionInWorldSpace()
        {
            Vector3 mousePointScreen = Input.mousePosition;
            mousePointScreen = Camera.main.ScreenToWorldPoint(mousePointScreen);

            Vector2 mousePoint = new Vector2(
                mousePointScreen.x,
                mousePointScreen.y
            );

            return mousePoint;
        }
    }
}