using UnityEngine;

using FW.Combat;
using FW.Attributes;
using FW.Movement;

namespace FW.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float spotDistance = 5f;

        [Header("Patroling")]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float patrolSpeed = 5f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float dwellingTime = 3f;
        [SerializeField] float suspicionTime = 5f;

        GameObject player = null;
        Fighter fighter = null;
        Mover mover = null;

        bool isPlayerSpotted = false;
        bool isPatroling = true;

        int curretWaypointIndex = 0;
        float timeSinceStartDwelling = Mathf.Infinity;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Awake() 
        {   
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
        }

        private void Start() 
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update() 
        {
            if (player == null || GetComponent<Health>().IsDead()) 
            {
                fighter.Prepared();
                return;
            }

            isPlayerSpotted = Vector2.Distance(transform.position, player.transform.position) < spotDistance;
            if (isPlayerSpotted && !player.GetComponent<Health>().IsDead())
            {
                fighter.SetNewTarget(player.transform.position);
                fighter.Attack();
                isPatroling = false;
                timeSinceLastSawPlayer = 0f;

                mover.SetMoveSpeed(0f);
                mover.MoveTowardsHorizontally(player.transform.position);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                fighter.Prepared();
                isPatroling = false;

                mover.SetMoveSpeed(0f);
                mover.MoveTowardsHorizontally(player.transform.position);
            }
            else
            {
                isPatroling = true;
            }

            
            if (isPatroling)
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceStartDwelling += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = transform.position;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceStartDwelling = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceStartDwelling > dwellingTime)
            {
                mover.SetMoveSpeed(patrolSpeed);
                mover.MoveTowardsHorizontally(nextPosition);
                fighter.SetNewTarget(GetCurrentWaypoint());
            }
            else
            {
                mover.SetMoveSpeed(0f);
                mover.MoveTowardsHorizontally(nextPosition);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Mathf.Abs(transform.position.x - GetCurrentWaypoint().x);
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            curretWaypointIndex = patrolPath.GetNextIndex(curretWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(curretWaypointIndex);
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spotDistance);
        }
    }
}
