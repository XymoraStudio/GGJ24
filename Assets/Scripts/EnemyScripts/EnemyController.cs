using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    
    private Vector3 startingPoint;
    private Transform destinationPoint;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        startingPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Patrolling(){
        try{
            destinationPoint = PatrolControl.instance.Choosing();
        }catch{
            Debug.Log("Slobodni patrol point nije naden");
        }
    }
}
