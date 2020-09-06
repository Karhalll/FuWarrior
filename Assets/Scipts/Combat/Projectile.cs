using UnityEngine;

using FuWarrior.Attributes;

namespace FuWarrior.Combat
{
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] GameObject bloodInPrefab = null;
        [SerializeField] Transform bloodInReferPoint = null;
        [SerializeField] GameObject bloodOutPrefab = null;
        [SerializeField] float bloosEffectStickTime = 1f;
        [SerializeField] float speed = 100f;
        [SerializeField] float maxLifeTime = 5f;
        [Range(0,100)]
        [SerializeField] float StackChanceInPercents = 30f;

        float finalDamage = 0f;
        bool isStuck = false;
        bool isInCharacter = false;
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
            Destroy(gameObject, maxLifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }


        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (!other.gameObject.GetComponentInParent<Fighter>()) 
            {
                Destroy(gameObject);
                return;
            }
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
                    if (isInCharacter) { return; }
                    LayerMask mask = LayerMask.GetMask("Enemy", "Player");
                    RaycastHit2D hit = Physics2D.Raycast(bloodInReferPoint.position, transform.right, 1000f, mask);

                    GameObject bloodIn = Instantiate(bloodInPrefab, hit.point, transform.rotation, other.transform);
                    if (other.GetComponentInParent<Fighter>().GetIsFliped())
                    {
                        bloodIn.transform.localScale = new Vector2(-1f, 1f);
                    }
                    Destroy(bloodIn, bloosEffectStickTime);

                    
                }

                Health health = other.GetComponentInParent<Health>();
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

        private void OnTriggerStay2D(Collider2D other) 
        {
            isInCharacter = true;
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            if (!other.gameObject.GetComponentInParent<Fighter>()) {return;}
            if (myOwner != other.gameObject.GetComponentInParent<Fighter>().tag)
            {
                if (!isStuck)
                {
                    
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right);

                    GameObject bloodOut = Instantiate(bloodOutPrefab, hit.point, transform.rotation, other.transform);
                    if (other.GetComponentInParent<Fighter>().GetIsFliped())
                    {
                        bloodOut.transform.localScale = new Vector2(-1f, 1f);
                    }
                    Destroy(bloodOut, bloosEffectStickTime);
                    isInCharacter = false;
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