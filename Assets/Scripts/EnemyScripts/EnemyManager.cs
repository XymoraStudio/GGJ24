using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private List<GameObject> cubicles = new List<GameObject>();
    [SerializeField] private float minTimeActive = 5f;
    [SerializeField] private float maxTimeActive = 8f;
    private float currentTimeActive;
    private int currentActiveCubicle;


    private void Awake() {
        instance = this;
    }

    private void Start() {
        ChoosingAMisbehavingCubicle();
    }


    private void Update() {
        if(currentTimeActive > 0){
            currentTimeActive -= Time.deltaTime;
        }
        else{
            DeactivatingCurrentActiveCubicle();
            ChoosingAMisbehavingCubicle();
        }
    }

    
    private void ChoosingAMisbehavingCubicle(){
        int choosenCubicle = Random.Range(0, cubicles.Count-1);
        currentActiveCubicle = choosenCubicle;
        cubicles[choosenCubicle].GetComponent<EnemyActions>().active = true;
        cubicles[choosenCubicle].SetActive(true);
        currentTimeActive = Random.Range(minTimeActive, maxTimeActive);
    }

    private void DeactivatingCurrentActiveCubicle(){
        try{
            cubicles[currentActiveCubicle].GetComponent<EnemyActions>().active = false;
            cubicles[currentActiveCubicle].SetActive(false);
        }
        catch
        {}
    }
}
