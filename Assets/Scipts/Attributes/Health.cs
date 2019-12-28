using UnityEngine;

namespace FuWarrior.Attributes
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] float health = 1000f;
        [SerializeField] float vanishingTimeOfCorpse = 5f;

        Animator myAnimator = null;

        bool isDead = false;

        private void Awake() 
        {
            myAnimator = GetComponent<Animator>();
        }

        public void GetDamage(float damage)
        {
            if (health - damage > 0)
            {
                health -= damage;
            }
            else
            {
                if (!isDead)
                {
                    PlayDeathAnimation();
                    //GetComponent<Rigidbody2D>().isKinematic = true;
                    Destroy(gameObject, vanishingTimeOfCorpse);
                }
                health = 0;
                isDead = true;
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void PlayDeathAnimation()
        {
            myAnimator.SetBool("isDead", true);
        }
    }
}