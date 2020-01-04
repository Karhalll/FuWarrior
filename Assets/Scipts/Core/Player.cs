using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using FuWarrior.Attributes;
using FuWarrior.Combat;

namespace FuWarrior.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float runSpeed = 5f;
        [SerializeField] float jumpSpeed = 5f;
        [SerializeField] float timeInPreparedState = 3f;
        [SerializeField] float loweringTime = 0.5f;

        float timeSinceLastAttack;

        Rigidbody2D myRigidBody;
        Animator myAnimator;
        Health myHealth;
        Collider2D myFeet;
        Fighter myFighter;

        private void Awake() 
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            myHealth = GetComponent<Health>();
            myFeet = GetComponent<Collider2D>();
            myFighter = GetComponent<Fighter>();
        }

        void Update()
        {
            if (myHealth.IsDead()) {return;}

            Run();
            Jump();
            HandleAttack();
            Kick();
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
            if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Enviroment"))) { return; }

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity += jumpVelocityToAdd;
            }
        }

        private void HandleAttack()
        {
            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                myFighter.Attack();
            }
            else
            {
                myFighter.Prepared();
            }
        }

        private void Kick()
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            {
                print(gameObject.name + " is kicking");
            }
        }
    }

}

