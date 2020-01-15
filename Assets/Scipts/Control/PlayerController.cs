using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using FuWarrior.Combat;
using FuWarrior.Attributes;
using FuWarrior.Movement;

namespace FuWarrior.Control
{
    public class PlayerController : MonoBehaviour 
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        Rigidbody2D RigidBody2D = null;
        Fighter fighter = null;
        Health health = null;
        Mover mover = null;

        private void Awake() 
        {
            RigidBody2D = GetComponent<Rigidbody2D>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Update() 
        {
            if (health.IsDead()) 
            {
                return;
            }

            HandleMovement();

            if (InteractWithComponent()) return;

            SetCursor(CursorType.Aim);
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

        private void HandleMovement()
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is betwen -1 to +1
            mover.Moveing(controlThrow);
        }

        private bool InteractWithComponent()
        {
            RaycastHit2D[] hits = RaycastAllSorted();
            foreach (RaycastHit2D hit in hits)
            {
                IRaycastable[] raycastables = hit.collider.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit2D[] RaycastAllSorted()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMouseWorldPosition(), Vector2.zero);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        public Vector2 GetMouseWorldPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}