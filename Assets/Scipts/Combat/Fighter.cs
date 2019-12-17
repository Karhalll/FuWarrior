using UnityEngine;

namespace FuWarrior.Combat
{
    public class Fighter : MonoBehaviour 
    {
        [SerializeField] WeaponConfig weaponConfig = null;
        [SerializeField] Transform weaponSlot = null;

        Weapon currentWeapon = null;

        private void Start() 
        {
            EquipWeapon();
        }

        //Unity animation events
        public void Fire()
        {
            weaponConfig.LaunchProjectile(currentWeapon.GetProjectileSpawnPoint());
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