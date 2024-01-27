using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolControl : MonoBehaviour
{   
    public static PatrolControl instance;
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    private List<int> enemiesAtSamePatrolPoint = new List<int>();
    [Header ("Patrol point details")]
    [SerializeField] private int maxWorkersAtOnePatrolPoint = 2;
    [SerializeField] private int maxWhileLoops = 50;
    private int numberOfEnemiesCurrentlyAllowedToPatrol;
    private int numberOfEnemiesCurrentlyPatrolling;
    [Header ("Enemy spawn manager")]
    [SerializeField] private int maxEnemiesAllowedToPatrol = 6;
    [SerializeField] private int percentageOfProductivityDropForOneEnemyToSpawn = 5;
    

    private void Awake() {
        numberOfEnemiesCurrentlyPatrolling = 0;
        instance = this;
        SettingFullList(); 
    }

    public Transform Choosing(int minRange, int maxRange, ref bool patrolMode){
        try{
            int maxLoops = maxWhileLoops;
            int indexOfPatrolPoint;
            indexOfPatrolPoint = Random.Range(minRange, maxRange);
            if(!patrolMode){
                numberOfEnemiesCurrentlyPatrolling++;
            }
            patrolMode = true;
            Debug.Log("people on the point " + patrolPoints[indexOfPatrolPoint] + " " + enemiesAtSamePatrolPoint[indexOfPatrolPoint]);
            Debug.Log("New patrol point destination" + (indexOfPatrolPoint+1));
            return patrolPoints[indexOfPatrolPoint];
        }
        catch{
            Debug.Log("Index out of range");
            return null;
        }
    }

    private void SettingFullList(){
        for(int i=0; i<patrolPoints.Count; i++){
            enemiesAtSamePatrolPoint.Add(0);
        }
    }

    public void LeavingAPatrolPoint(Transform currentPatrolPoint){
        for(int i = 0; i < patrolPoints.Count-1; i++){
            if(patrolPoints[i] == currentPatrolPoint){
                Debug.Log("Oduzet broj ljudi s " + patrolPoints[i]);
                enemiesAtSamePatrolPoint[i]--;
            }
        }
    }

    public bool CheckingIfNewEnemyCanStartPatrol(){
        numberOfEnemiesCurrentlyAllowedToPatrol = Mathf.Clamp((int)(GameState.MaxProductivity - GameState.Productivity / GameState.MaxProductivity * 100) / percentageOfProductivityDropForOneEnemyToSpawn, 0, maxEnemiesAllowedToPatrol);
        Debug.Log("trenutno moguce spawnat " + numberOfEnemiesCurrentlyAllowedToPatrol + "enemija");
        Debug.Log("trenutno enemija" + numberOfEnemiesCurrentlyPatrolling);
        if(numberOfEnemiesCurrentlyAllowedToPatrol == numberOfEnemiesCurrentlyPatrolling){
            return false;
        }
        else{
            return true;
        }
    }

    public void EnteringAPatrolPoint(Transform currentPatrolPoint, int minRange, int maxRange){
        for(int i = 0; i < patrolPoints.Count-1; i++){
            if(patrolPoints[i] == currentPatrolPoint){
                if(enemiesAtSamePatrolPoint[i] < maxWorkersAtOnePatrolPoint){
                    enemiesAtSamePatrolPoint[i]++;
                }
            }
        }
    }

    public void RemovingEnemyFromPatrol(){
        numberOfEnemiesCurrentlyPatrolling--;
    }
}
