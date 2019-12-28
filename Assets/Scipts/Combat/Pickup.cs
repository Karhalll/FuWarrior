using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuWarrior.Combat
{
    public class Pickup : MonoBehaviour 
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float respawnTime = 2f;
    
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.tag == "Player")
            {
                PickupObject(other.gameObject);
            }
        }

        private void PickupObject(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            // if (healthToRestore > 0)
            // {
            //     subject.GetComponent<Health>().Heal(healthToRestore);
            // }

            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider2D>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}