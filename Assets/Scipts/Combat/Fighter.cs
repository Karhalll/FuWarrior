using System;
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

        Weapon currentWeapon = null;
        PlayerController playerController = null;

        private void Awake() 
        {
            if (this.gameObject.tag == "Player")
            {
                 playerController = GetComponent<PlayerController>();
            }         
        }

        private void Start() 
        {
            EquipWeapon();
        }

        private void Update() 
        {
            if (gameObject.tag == "Player")
            {
                currentWeapon.transform.LookAt(playerController.MousePositionInWorldSpace());
                currentWeapon.transform.Rotate(Vector3.up, -90f);
            }
        }

        //Unity animation events
        public void Fire()
        {
            weaponConfig.LaunchProjectile(currentWeapon.GetProjectileSpawnPoint(), playerController.MousePositionInWorldSpace(), gameObject.tag);
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
    }
}