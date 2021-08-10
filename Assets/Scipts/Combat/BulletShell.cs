using UnityEngine;

namespace FW.Combat
{
    public class BulletShell : MonoBehaviour 
    {
        [SerializeField] float releaseVelocity = 30f;
        [SerializeField] float torqueForce = 30f;
        [SerializeField] float shellDestroyTime = 5f;

        Rigidbody2D myRigidBody = null;

        private void Start() 
        {   
            myRigidBody = GetComponent<Rigidbody2D>();

            myRigidBody.AddForce(new Vector2(-0.1f, 0.3f) * releaseVelocity);
            myRigidBody.AddTorque(torqueForce, ForceMode2D.Impulse);

            Destroy(gameObject, shellDestroyTime);
        }

    }
}