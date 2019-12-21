using UnityEngine;

namespace FuWarrior.Attributes
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] float health = 1000f;

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
                }

                health = 0;
                isDead = true;
            }
        }

        public bool GetIsDead()
        {
            return isDead;
        }

        private void PlayDeathAnimation()
        {
            myAnimator.SetBool("isDead", true);
        }
    }
}