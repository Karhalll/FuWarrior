using UnityEngine;

using FuWarrior.Combat;
using FuWarrior.Attributes;
using System;

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

        [SerializeField] float raycastRadius = 1f;

        Rigidbody2D RigidBody2D = null;
        Fighter fighter = null;
        Health health = null;

        private void Awake() 
        {
            RigidBody2D = GetComponent<Rigidbody2D>();
            fighter = GetComponent<Fighter>();
        }

        private void Update() 
        {
            //if (InteractWithUI()) return;
            // if (health.IsDead()) 
            // {
            //     SetCursor(CursorType.Dead);
            //     return;
            // }
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