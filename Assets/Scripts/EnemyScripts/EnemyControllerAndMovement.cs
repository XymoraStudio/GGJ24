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
    [SerializeField] private int maxWaitTimeAtPatrolPoint = 5;
    private float currentTimeAtPatrolPoint;
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] private string walkingParametarName;
    [SerializeField] private Animator animatorControl;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        startingPoint = transform.position;
        currentTimeAtPatrolPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimeAtPatrolPoint <= 0){
            Patrolling();
            animatorControl.SetBool(walkingParametarName, true);
        }
        else if(agent.destination == transform.position){
            currentTimeAtPatrolPoint -= Time.deltaTime;
        }
    }

    private void Patrolling(){
        int choosenPatrolPoint = Random.Range(0, patrolPoints.Count-1);
        agent.destination = patrolPoints[choosenPatrolPoint].position;
        currentTimeAtPatrolPoint = Random.Range(minWaitTimeAtPatrolPoint, maxWaitTimeAtPatrolPoint);
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("uslo");
        if(other.tag == "PatrolP"){
            agent.destination = transform.position;
            animatorControl.SetBool(walkingParametarName, false);
        }
    }

    private void ReturningToHisDesk(){
        agent.destination = startingPoint;
    }
}
