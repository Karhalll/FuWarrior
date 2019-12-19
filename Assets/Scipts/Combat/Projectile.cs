using System;
using UnityEngine;

using FuWarrior.Attributes;

namespace FuWarrior.Combat
{
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] GameObject bloodInPrefab = null;
        [SerializeField] GameObject bloodOutPrefab = null;
        [SerializeField] float bloosEffectStickTime = 1f;
        [SerializeField] float speed = 100f;
        [SerializeField] float lifeTime = 5f;
        [Range(0,100)]
        [SerializeField] float StackChanceInPercents = 30f;

        float finalDamage = 0f;
        bool isStuck = false;
        string myOwner = null;
        Vector2 bulet;

        Vector2 target;
        Rigidbody2D myRigidBody = null;

        private void Awake() 
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        private void Start() 
        {
            transform.LookAt(target);
            transform.Rotate(Vector3.up, -90f); 
            Destroy(gameObject, lifeTime);
        }

        private void Update() 
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (!other.gameObject.GetComponentInParent<Fighter>()) {return;}
            if (myOwner != other.gameObject.GetComponentInParent<Fighter>().tag)
            {
                if (other.GetComponent<BlowOffable>())
                {
                    BlowOffable blowOffable = other.GetComponent<BlowOffable>();

                    Vector3 bulletDirection = gameObject.transform.rotation * new Vector3(1,1,1);
                    blowOffable.BlowOff(bulletDirection);
                }
                else
                {
                    GameObject bloodIn = Instantiate(bloodInPrefab, gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(bloodIn, bloosEffectStickTime);
                }

                Health health = other.GetComponent<Health>();
                if (health)
                {
                    health.GetDamage(finalDamage);
                }

                float rollADice = UnityEngine.Random.Range(0f, 100f);
                if (rollADice <= StackChanceInPercents)
                {
                    isStuck = true;
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            if (!other.gameObject.GetComponentInParent<Fighter>()) {return;}
            if (myOwner != other.gameObject.GetComponentInParent<Fighter>().tag)
            {
                if (!isStuck)
                {
                    GameObject bloodOut = Instantiate(bloodOutPrefab, gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(bloodOut, bloosEffectStickTime);
                }
            }
        
        }

        public void SetTarget(Vector2 target)
        {
            this.target = target;
        }

        public void IncreaseDamage(float damage)
        {
            finalDamage += damage;
        }

        public void SetMyOwner(string tag)
        {
            myOwner = tag;
        }
    }
}