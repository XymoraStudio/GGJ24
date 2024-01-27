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
    [SerializeField] private int minRange;
    [SerializeField] private int maxRange;
    private float waitTime;
    private bool atPatrolPoint;
    private Transform currentPatrolPoint;
    private bool patrolMode;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        startingPoint = transform.position;
        atPatrolPoint = false;
        destinationPoint = null;
        agent.destination = startingPoint;
        patrolMode = false;
    }

    private void Start() {
        
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

    public void Patrolling(){
        try{
            if(PatrolControl.instance.CheckingIfNewEnemyCanStartPatrol() || patrolMode){
                destinationPoint = PatrolControl.instance.Choosing(minRange, maxRange, ref patrolMode);
                agent.destination = destinationPoint.position;
                waitTime = Random.Range(minWaitTimeAtPatrolPoint, maxWaitTimeAtPatrolPoint);
            }
        }
        catch{
            Debug.Log("Slobodni patrol point nije naden");
            destinationPoint = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Patrol point"){
            atPatrolPoint = true;
            currentPatrolPoint = other.gameObject.transform;
            PatrolControl.instance.EnteringAPatrolPoint(currentPatrolPoint, minRange, maxRange);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Patrol point"){
            atPatrolPoint = false;
            PatrolControl.instance.LeavingAPatrolPoint(currentPatrolPoint);
            Debug.Log("Napusta trenutni patrol point");
        }
    }

    private void ReturningToHisDesk(){
        agent.destination = startingPoint;
        PatrolControl.instance.RemovingEnemyFromPatrol();
        patrolMode = false;
    }
}
