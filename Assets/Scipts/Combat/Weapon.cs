using UnityEngine;

namespace FuWarrior.Combat
{
    public class Weapon : MonoBehaviour 
    {
        [SerializeField] Transform projectileSpawnPoint = null;

        public Transform GetProjectileSpawnPoint()
        {
            return projectileSpawnPoint;
        }
    }
}