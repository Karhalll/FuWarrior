using UnityEngine;

namespace FW.Combat
{
    public class Weapon : MonoBehaviour 
    {
        [SerializeField] Transform projectileSpawnPoint = null;
        [SerializeField] Transform bulletShellSpawnPoint = null;

        public Transform GetProjectileSpawnPoint()
        {
            return projectileSpawnPoint;
        }

        public Transform GetShellSpawnPoint()
        {
            return bulletShellSpawnPoint;
        }
    }
}