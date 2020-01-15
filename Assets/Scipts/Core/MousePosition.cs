using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuWarrior.Core
{
    public class MousePosition : MonoBehaviour
    {
        [SerializeField] Transform player = null;
        [SerializeField] float cameraClampX = 80f;
        [SerializeField] float cameraClampY = 40f;
        [SerializeField] float mouseCameraToleration = 10f;
        [SerializeField] float smoothTime = 1f;

        private Vector3 velocity = Vector3.zero;

        void FixedUpdate()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(Mathf.Abs(mousePosition.x - player.position.x) <= cameraClampX && Mathf.Abs(mousePosition.y - player.position.y) <= cameraClampY)
            {
                if((mousePosition - transform.position).magnitude >= mouseCameraToleration)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, mousePosition, ref velocity, smoothTime);
                } 
                else
                {
                    transform.position = mousePosition;
                }
            }

            if((player.position - transform.position).magnitude >= cameraClampX)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, 0.5f);
                }

        }
    }
}
