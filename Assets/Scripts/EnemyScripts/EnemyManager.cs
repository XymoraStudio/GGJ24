using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private List<GameObject> cubicles = new List<GameObject>();
    [SerializeField] private float minTimeActive = 5f;
    [SerializeField] private float maxTimeActive = 8f;
    private float currentTimeActive;
    private int currentActiveCubicle;
    private int choosenCubicle;
    public bool allMisbehaving;


    private void Awake() {
        instance = this;
        choosenCubicle = 0;
        allMisbehaving = false;
    }

    private void Start() {
        ChoosingAMisbehavingCubicle();
    }


    private void Update(){
        if(!allMisbehaving){
            if(currentTimeActive > 0){
                currentTimeActive -= Time.deltaTime;
            }
            else{
                //DeactivatingCurrentMisbehavingCubicle();
                ChoosingAMisbehavingCubicle();
            }
        }
    }

    
    private void ChoosingAMisbehavingCubicle(){
        if(GameState.currentNumberOfActiveEnemies != GameState.MaxActiveEnemies){
            if(choosenCubicle != cubicles.Count){
                cubicles[choosenCubicle].GetComponentInChildren<EnemyActions>().active = false;
                GameState.currentNumberOfActiveEnemies++;
                currentTimeActive = Random.Range(minTimeActive, maxTimeActive);
                choosenCubicle++;
            }
            else{
                allMisbehaving = true;
            }
        }
    }

    private void DeactivatingCurrentMisbehavingCubicle(){
        try{
            cubicles[currentActiveCubicle].GetComponentInChildren<EnemyActions>().active = true;
            GameState.currentNumberOfActiveEnemies--;
        }
        catch
        {}
    }
}
