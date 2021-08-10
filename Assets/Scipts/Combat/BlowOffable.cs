using UnityEngine;

namespace FW.Combat
{
    public class BlowOffable : MonoBehaviour 
    {
        [SerializeField] Rigidbody2D blowOffPrefab = null;

        [Header("Settings")]
        [SerializeField] float blowOfVelocity = 30f;
        [SerializeField] float torqueForce = 30f;
        [SerializeField] float helmetDestroyTime = 5f;

        public void BlowOff(Vector3 directionOfBlowOff)
        {
            GameObject helmDynamics = Instantiate(blowOffPrefab.gameObject, transform.position, transform.rotation);
            Rigidbody2D helmDynamicsRB = helmDynamics.GetComponent<Rigidbody2D>();

            helmDynamicsRB.velocity = directionOfBlowOff * blowOfVelocity;
            helmDynamicsRB.AddTorque( (directionOfBlowOff.x * torqueForce * -1f), ForceMode2D.Impulse);

            Destroy(helmDynamics, helmetDestroyTime);
            Destroy(gameObject);
        }  
    }
}