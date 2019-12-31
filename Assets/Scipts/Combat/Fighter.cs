using UnityEngine;

using FuWarrior.Control;
using FuWarrior.Attributes;

namespace FuWarrior.Combat
{
    [RequireComponent(typeof(Health))]
    public class Fighter : MonoBehaviour 
    {
        [SerializeField] WeaponConfig weaponConfig = null;
        [SerializeField] Transform weaponSlot = null;
        [SerializeField] Transform arms = null;

        Weapon currentWeapon = null;
        PlayerController playerController = null;
        Health myHealth = null;
        Animator myAnimator = null;

        Transform target = null;
        Vector3 targetPosition;

        bool isPlayer = false;
        bool isFliped = false;
        bool isHitting = false;
        string myOwner = null;

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
        }

        private void LateUpdate() 
        {
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

        private void AimWeapon()
        {
            if (isPlayer)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {   
                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetPosition = target.position;
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

        //Unity animation events
        public void Fire()
        {
            weaponConfig.LaunchProjectile(currentWeapon.GetProjectileSpawnPoint(), playerController.MousePositionInWorldSpace(), gameObject.tag);
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