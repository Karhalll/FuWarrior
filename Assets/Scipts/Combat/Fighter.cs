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
        [SerializeField] Transform leftArm = null;
        [SerializeField] Transform rightArm = null;

        [SerializeField] float firingAngele = 60f;

        Weapon currentWeapon = null;
        PlayerController playerController = null;
        Health myHealth = null;
        Animator myAnimator = null;

        Transform target = null;
        Vector3 targetPosition;

        bool isPlayer = false;
        bool isFliped = false;

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
            EquipWeapon();
        }

        private void Update()
        {
            if (myHealth.IsDead())
            {
                return;
            }
        }

        private void LateUpdate() 
        {
            AimWeapon();
            FlipSprite();
        }

        private void AimWeapon()
        {
            if (isPlayer)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {   
                targetPosition = target.position;
            }

            Vector2 direction = targetPosition - currentWeapon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (isFliped)
            {
                angle += 180f;
            }
            
            Quaternion rotation = Quaternion.AngleAxis(angle - firingAngele, Vector3.forward);
            leftArm.transform.rotation = rotation;
            rightArm.transform.rotation = rotation;
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

        private void EquipWeapon()
        {
            Animator animation = GetComponent<Animator>();

            if (weaponConfig != null && weaponSlot != null)
            {
                currentWeapon = weaponConfig.Spawn(weaponSlot, animation);
            }
            else
            {
                Debug.LogError(this.gameObject.name + " don't have Weapon Config or Weapon Slot set");
            }
            
        }

        //Unity animation events
        public void Fire()
        {
            weaponConfig.LaunchProjectile(currentWeapon.GetProjectileSpawnPoint(), playerController.MousePositionInWorldSpace(), gameObject.tag);
            weaponConfig.ReleaseBulletShell(currentWeapon.GetShellSpawnPoint());
        }
    }
}