using System;
using UnityEngine;

namespace FuWarrior.Attributes
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] float maxHealth = 1000f;
        [SerializeField] float vanishingTimeOfCorpse = 5f;

        Animator myAnimator = null;

        float health = 0f;
        bool isDead = false;

        public event Action onTakeDamage;

        private void Awake() 
        {
            myAnimator = GetComponent<Animator>();
            health = maxHealth;
        }

        private void Start() 
        {
        }

        public float GetHealthInPercent()
        {
            return (100 * health) / maxHealth;
        }

        public void GetDamage(float damage)
        {
            if (health - damage > 0)
            {
                health -= damage;
            }
            else
            {
                Die();
            }

            if(gameObject.tag == "Player")
            {
                onTakeDamage();
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void Die()
        {
            if (!isDead)
                {
                    PlayDeathAnimation();
                    Destroy(gameObject, vanishingTimeOfCorpse);
                }
                health = 0;
                isDead = true;
        }

        private void PlayDeathAnimation()
        {
            myAnimator.SetBool("isDead", true);
        }
    }
}