using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerAndMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    
    private Vector3 startingPoint;
    private Transform destinationPoint;
    [SerializeField] private int minWaitTimeAtPatrolPoint = 3;
    [SerializeField] private int maxWaitTimeAtPatrolPoint = 10;
    private float waitTime;
    private bool atPatrolPoint;
    private Transform currentPatrolPoint;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        startingPoint = transform.position;
        atPatrolPoint = false;
    }

    private void Start() {
        Patrolling();
    }

    // Update is called once per frame
    void Update()
    {
        if(atPatrolPoint && waitTime > 0){
            Debug.Log(waitTime);
            waitTime = waitTime - Time.deltaTime;
            if(waitTime <= 0){
                destinationPoint = null;
            }
        }
        if(destinationPoint == null){
            Patrolling();
        }
    }

    private void Patrolling(){
        try{
            destinationPoint = PatrolControl.instance.Choosing();
            agent.destination = destinationPoint.position;
            waitTime = Random.Range(minWaitTimeAtPatrolPoint, maxWaitTimeAtPatrolPoint);
        }catch{
            Debug.Log("Slobodni patrol point nije naden");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Patrol point"){
            atPatrolPoint = true;
            currentPatrolPoint = other.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Patrol point"){
            atPatrolPoint = false;
            PatrolControl.instance.LeavingAPatrolPoint(currentPatrolPoint);
        }
    }

}
