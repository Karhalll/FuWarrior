using UnityEngine;

using FuWarrior.Combat;
using FuWarrior.Attributes;

namespace FuWarrior.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float spotDistance = 5f;

        GameObject player = null;
        Fighter fighter = null;

        bool isPlayerSpotted = false;

        private void Awake() 
        {   
            fighter = GetComponent<Fighter>();
        }

        private void Start() 
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update() 
        {
            if (player == null) {return;}
            isPlayerSpotted = Vector2.Distance(transform.position, player.transform.position) < spotDistance;
            if (isPlayerSpotted && !player.GetComponent<Health>().IsDead())
            {
                fighter.Attack();
            }
            else
            {
                fighter.Prepared();
            }
        }
    }
}
