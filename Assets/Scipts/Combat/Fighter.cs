using UnityEngine;

using FW.Control;
using FW.Attributes;

namespace FW.Combat
{
    [RequireComponent(typeof(Health))]
    public class Fighter : MonoBehaviour 
    {
        [SerializeField] WeaponConfig weaponConfig = null;
        [SerializeField] Transform weaponSlot = null;
        [SerializeField] Transform arms = null;

        [SerializeField] float timeInPreparedState = 3f;

        Weapon currentWeapon = null;
        PlayerController playerController = null;
        Health myHealth = null;
        public Animator myAnimator = null;

        Vector3 target = new Vector3(0, 0, 0);
        Vector3 targetPosition;

        bool isPlayer = false;
        bool isFliped = false;
        bool isHitting = false;
        string myOwner = null;

        float timeSinceLastAttack;

        private void Awake() 
        {
            myAnimator = GetComponent<Animator>();
            myHealth = GetComponent<Health>();

            if (gameObject.tag == "Player")
            {
                playerController = GetComponent<PlayerController>();
                isPlayer = true;
            }         
        }

        private void Start() 
        {
            EquipWeapon(weaponConfig);
            timeSinceLastAttack = Mathf.Infinity;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (!other.gameObject.GetComponentInParent<Fighter>()) {return;}
            if (myOwner != other.gameObject.GetComponentInParent<Fighter>().tag && isHitting)
            {
                Debug.Log("Hitting " + other.gameObject.name);

                if (other.gameObject.GetComponent<WeakPoint>())
                {
                    Debug.Log("Critical Hit Attempt");
                    other.gameObject.GetComponentInParent<Health>().GetDamage(weaponConfig.GetWeaponCriticalDamage());
                    Debug.Log(weaponConfig.GetWeaponCriticalDamage());
                }
                else
                {
                    Debug.Log("Hit Attempt");
                    other.gameObject.GetComponentInParent<Health>().GetDamage(weaponConfig.GetWeaponDamage());
                    Debug.Log(weaponConfig.GetWeaponDamage());
                }
            }
        }

        private void Update()
        {
            if (myHealth.IsDead())
            {
                return;
            }

            FlipSprite();
            isPrepared();

            TimeUpdaters();
        }   

        private void LateUpdate() 
        {
            if (myHealth.IsDead())
            {
                return;
            }
            AimWeapon(); 
        }

        public bool GetIsFliped()
        {
            return isFliped;
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            Animator animation = GetComponent<Animator>();

            if (weapon != null && weaponSlot != null)
            {
                weaponConfig = weapon;
                currentWeapon = weapon.Spawn(weaponSlot, animation);
            }
            else
            {
                Debug.LogError(this.gameObject.name + " don't have Weapon Config or Weapon Slot set");
            }
            
        }

        public float RotationAngleTowardsMouse()
        {
            Vector3 direction = (targetPosition - arms.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (isFliped)
            {
                angle += 180f;
            }

            return angle;
        }

        public Transform GetArms()
        {
            return arms;
        }

        public void Attack()
        {
            myAnimator.SetBool("Prepared", false);
            timeSinceLastAttack = 0;
            myAnimator.SetBool("isAttacking", true);
        }

        public void Prepared()
        {
            myAnimator.SetBool("isAttacking", false);
            myAnimator.SetBool("Prepared", true);
        }

        public void SetNewTarget(Vector3 newtarget)
        {
            target = newtarget;
        }

        private void isPrepared()
        {
            if (timeSinceLastAttack > timeInPreparedState)
            {
                myAnimator.SetBool("Prepared", false);
            }
        }

        private void AimWeapon()
        {
            if (isPlayer)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (GameObject.FindGameObjectWithTag("Player"))
            {   
                //target = GameObject.FindGameObjectWithTag("Player").transform;
                targetPosition = target;
            }

            if (myAnimator.GetBool("Prepared") || myAnimator.GetBool("isAttacking"))
            {
                float angle = RotationAngleTowardsMouse();
                arms.transform.eulerAngles = new Vector3(0, 0, angle);
            }

        }

        private void FlipSprite()
        {
            bool lookingRight = transform.position.x < targetPosition.x;

            if (lookingRight)
            {
                transform.localScale = new Vector2(1f, 1f);
                isFliped = false;
            }
            else
            {
                transform.localScale = new Vector2(-1f, 1f);
                isFliped = true;
            }
        }

        private void TimeUpdaters()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        //Unity animation events
        public void Fire()
        {
            weaponConfig.LaunchProjectile(currentWeapon.GetProjectileSpawnPoint(), targetPosition, gameObject.tag);
            weaponConfig.ReleaseBulletShell(currentWeapon.GetShellSpawnPoint());
        }

        public void HitStart()
        {
            isHitting = true;
        }

        public void HitStop()
        {
            isHitting = false;
        }
    }
}