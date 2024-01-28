using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patroller : MonoBehaviour
{
    [Header("NavMesh")]
    [SerializeField] NavMeshAgent agent;

    [Header("Settings")]
    [SerializeField] private int minWaitTimeAtPatrolPoint = 3;
    [SerializeField] private int maxWaitTimeAtPatrolPoint = 5;
    private float currentTimeAtPatrolPoint;
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] Animator animatorControl;

    private void Awake() {
        currentTimeAtPatrolPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimeAtPatrolPoint <= 0){
            Patrolling();
            animatorControl.SetTrigger("Walk");
        }
        // koristim distance jer == cesto moze bit ne jednak za floatove
        else if(Mathf.Abs(Vector3.Distance(agent.destination, transform.position)) < 1f){
            currentTimeAtPatrolPoint -= Time.deltaTime;
        }
    }

    private void Patrolling(){
        int choosenPatrolPoint = Random.Range(0, patrolPoints.Count-1);
        agent.destination = patrolPoints[choosenPatrolPoint].position;
        currentTimeAtPatrolPoint = Random.Range(minWaitTimeAtPatrolPoint, maxWaitTimeAtPatrolPoint);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "PatrolP"){
            agent.destination = transform.position;
            animatorControl.SetTrigger("Idle");
        }
    }
}
