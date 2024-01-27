using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolControl : MonoBehaviour
{   
    public static PatrolControl instance;
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    private List<int> enemiesAtSamePatrolPoint = new List<int>();  
    [SerializeField] int maxWorkersAtOnePatrolPoint = 2;
    [SerializeField] int maxWhileLoops = 50;

    private void Awake() {
        instance = this;
        SettingFullList(); 
    }

    public Transform Choosing(){
        int counterForLoops = maxWhileLoops;
        int indexOfPatrolPoint = Random.Range(0, patrolPoints.Count-1);;
        while(counterForLoops-- > 0 && enemiesAtSamePatrolPoint[indexOfPatrolPoint] == maxWorkersAtOnePatrolPoint){
            indexOfPatrolPoint = Random.Range(0, patrolPoints.Count-1);
        }
        if(counterForLoops == 0){
            return null;
        }
        else{
            enemiesAtSamePatrolPoint[indexOfPatrolPoint]++;
            Debug.Log("New patrol point destination" + (indexOfPatrolPoint+1));
            return patrolPoints[indexOfPatrolPoint];
        }
    }

    private void SettingFullList(){
        for(int i=0; i<patrolPoints.Count; i++){
            enemiesAtSamePatrolPoint.Add(0);
        }
    }

    [HideInInspector] public void LeavingAPatrolPoint(Transform currentPatrolPoint){
        for(int i = 0; i < patrolPoints.Count-1; i++){
            if(patrolPoints[i] == currentPatrolPoint){
                Debug.Log("Oduzet broj ljudi s " + patrolPoints[i]);
                enemiesAtSamePatrolPoint[i]--;
            }
        }
    }
}
