using UnityEngine;

namespace FuWarrior.Movement
{
    public class Mover : MonoBehaviour 
    {
        [SerializeField] float runSpeed = 50f;
        //[SerializeField] float walkSpeed = 5f;

        float moveSpeed;

        Rigidbody2D myRigidBody = null;
        Animator myAnimator = null;

        private void Awake() 
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();

            moveSpeed = runSpeed;
        }

        public void Moveing(float controlThrow)
        {
            Vector2 charVelocity = new Vector2(controlThrow * moveSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = charVelocity;

            bool charHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", charHorizontalSpeed);
            myAnimator.SetFloat("forwardSpeed", moveSpeed);
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void MoveTowardsHorizontaly(Vector3 nextPosition)
        {
            Vector3 charPosition = transform.position;

            if (nextPosition.x < charPosition.x)
            {
                Moveing(-1f);
            }
            else
            {
                Moveing(1f);
            }
        }
    }
}