using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace rhcodepi
{
    public class AIChase : MonoBehaviour
    {
        public enum AIState { PATROL = 0, CHASE = 1, ATTACK = 2};
        private NavMeshAgent enemyAgent;
        [SerializeField] private GameObject player_Tr = null;
        [SerializeField] private GameObject[] waypoints;
        private GameObject current_Waypoint;
        private float distance = 2f, attackDistance = 1.5f;
        [SerializeField] private AIState currentState;
        
        #region State Control
        public AIState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                StopAllCoroutines();
                currentState = value;

                switch (currentState)
                {
                    case AIState.PATROL:
                        StartCoroutine(PatrolState());
                        break;
                    case AIState.CHASE:
                        StartCoroutine(ChaseState());
                        break;
                    case AIState.ATTACK:
                        StartCoroutine(AttackState());
                        break;

                }

            }
        }
        #endregion
        private void Awake()
        {
            enemyAgent = GetComponent<NavMeshAgent>();
            current_Waypoint = waypoints[Random.Range(0, waypoints.Length)];
        }
        private void Start()
        {
            CurrentState = AIState.PATROL;
        }
        #region States Setup
        IEnumerator PatrolState()
        {
            while (currentState == AIState.PATROL)
            {
                enemyAgent.SetDestination(current_Waypoint.transform.position);

                if (Vector3.Distance(transform.position, current_Waypoint.transform.position) < distance)
                {
                    current_Waypoint = waypoints[Random.Range(0, waypoints.Length)];
                }

                yield return null;
            }

        }

        IEnumerator ChaseState()
        {

            while (currentState == AIState.CHASE)
            {
                if (Vector3.Distance(transform.position, player_Tr.transform.position) < attackDistance)
                {
                    CurrentState = AIState.ATTACK;
                }
                enemyAgent.SetDestination(player_Tr.transform.position);

                yield return null;
            }

        }

        IEnumerator AttackState()
        {

            while (currentState == AIState.ATTACK)
            {
                if (Vector3.Distance(transform.position, player_Tr.transform.position) > attackDistance)
                {
                    CurrentState = AIState.CHASE;
                }
                print("ATTACK!");
                enemyAgent.SetDestination(player_Tr.transform.position);

                yield return null;
            }

            yield return null;
        }
        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) CurrentState = AIState.CHASE;
        }
    }
}

