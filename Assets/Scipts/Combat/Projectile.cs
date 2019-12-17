using UnityEngine;

namespace FuWarrior.Combat
{
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] float speed = 100f;
        [SerializeField] float lifeTime = 5f;

        private void Start() 
        {
            Destroy(gameObject, lifeTime);
        }
    
        private void Update() 
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
}