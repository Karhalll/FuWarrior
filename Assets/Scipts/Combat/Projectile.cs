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

        Vector2 target;

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
            if(other.name == "Head")
            {
                if (other.gameObject.transform.Find("Head Slot").Find("Naci_Helmet"))
                {
                    Transform helm = other.gameObject.transform.Find("Head Slot").Find("Naci_Helmet");
                    GameObject helmDynamics = Instantiate(helm.gameObject, helm.position, helm.rotation);

                    Destroy(helm.gameObject);
                    
                    helmDynamics.GetComponent<Rigidbody2D>().isKinematic = false;
                    helmDynamics.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.position * 200f);
                }

                other.GetComponentInParent<Health>().GetDamage(finalDamage);
            }

            GameObject bloodIn = Instantiate(bloodInPrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(bloodIn, bloosEffectStickTime);

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

        private void OnTriggerExit2D(Collider2D other) 
        {
            if (!isStuck)
            {
                GameObject bloodOut = Instantiate(bloodOutPrefab, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(bloodOut, bloosEffectStickTime);
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
    }
}