using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FuWarror.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float runSpeed = 5f;
        [SerializeField] float jumpSpeed = 5f;
        [SerializeField] float timeInPreparedState = 3f;

        float timeSinceLastAttack;

        Rigidbody2D myRigidBody;
        Animator myAnimator;

        // Start is called before the first frame update
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            timeSinceLastAttack = Mathf.Infinity;
        }

        // Update is called once per frame
        void Update()
        {
            Run();
            Jump();
            Attack();
            Kick();
            FlipSprite();

            TimeUpdaters();

            if (timeSinceLastAttack > timeInPreparedState)
            {
                myAnimator.SetBool("Prepared", false);
            }
        }

        private void Run()
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is betwen -1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;

            bool playerHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", playerHorizontalSpeed);
        }

        private void Jump()
        {
            //if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity += jumpVelocityToAdd;
            }
        }

        private void Attack()
        {
            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                myAnimator.SetBool("Prepared", false);
                timeSinceLastAttack = 0;
                myAnimator.SetBool("isAttacking", true);
            }
            else
            {
                myAnimator.SetBool("isAttacking", false);
                timeSinceLastAttack = 0;
                myAnimator.SetBool("Prepared", true);
            }
        }

        private void Kick()
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            {
                print(gameObject.name + " is kicking");
            }
        }

        private void FlipSprite()
        {
            bool playerHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
            if (playerHorizontalSpeed)
            {
                transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
            }
        }

        private void TimeUpdaters()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // Unitz animations events
        public void Hit()
        {
            print("Plesk");
        }
    }

}

