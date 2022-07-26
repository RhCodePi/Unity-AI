using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace rhcodepi
{
    public class AIChase : MonoBehaviour
    {
        public enum AIState{PATROL= 0, CHASE =1 , ATTACK = 2};
        private NavMeshAgent enemyAgent;
        //[SerializeField] private Transform player_Tr = null;
        [SerializeField] private GameObject[] waypoints;
        private GameObject current_Waypoint;
        public AIState currentState = AIState.PATROL;
        private float distance = 2f;

        private void Awake() {
            enemyAgent = GetComponent<NavMeshAgent>();
            current_Waypoint  = waypoints[Random.Range(0, waypoints.Length)];
        }

        private void Update() {
            StartCoroutine(StratPatrol());
        }

        IEnumerator StratPatrol()
        {
            enemyAgent.SetDestination(current_Waypoint.transform.position);

            if(Vector3.Distance(transform.position, current_Waypoint.transform.position) < distance)
            {
                current_Waypoint = waypoints[Random.Range(0, waypoints.Length)];
            }

            yield return null;
        }

    }
}

