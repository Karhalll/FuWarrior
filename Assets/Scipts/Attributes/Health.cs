using UnityEngine;

namespace FuWarrior.Attributes
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] float health = 1000f;

        public void GetDamage(float damage)
        {
            health -= damage;
        }
    }
}