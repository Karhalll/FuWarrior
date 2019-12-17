using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuWarrior.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "FuWarrior/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject 
    {
        [SerializeField] Weapon weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animationOverride = null;
        [SerializeField] Projectile projectilePrfab = null;
        [SerializeField] GameObject blastPrefab = null;
        [SerializeField] float blastTimeDuration = 2f;
        
        [Header("Weapon Properities")]
        [SerializeField] float weaponDamage = 0f;
        [SerializeField] float attackSpeed = 1f;


        public Weapon Spawn(Transform spawnPosition, Animator animator)
        {
            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
                animator.SetFloat("attackSpeed", attackSpeed);
            }
            else 
            {
                Debug.LogWarning(this.name + " weapon config is missing Animation Override");
            }
            
            if (weaponPrefab != null)
            {
               return Instantiate(weaponPrefab, spawnPosition);
            }
            else 
            {
                Debug.LogWarning(this.name + " weapon config is missing Weapon Prefab");
            }
            
            return null;
        }

        public void LaunchProjectile(Transform spawnPoint)
        {
            Projectile projectileInstance = Instantiate(projectilePrfab, spawnPoint.position, spawnPoint.rotation);

            if (blastPrefab != null)
            {
                GameObject blast = Instantiate(blastPrefab, spawnPoint.position, spawnPoint.rotation);
                Destroy(blast, blastTimeDuration);
            }
        }

    }
}


