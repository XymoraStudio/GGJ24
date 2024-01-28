using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] private List<GameObject> materialForWalls = new List<GameObject>();
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private float durationOfAbility = 2f;
    private float timerForDurationOfAbility;

    private void Update() {
        if(Input.GetKeyDown(KeyBinds.instance.ability)){
            TransparencyEnabling(true);
            timerForDurationOfAbility = durationOfAbility;
        }
        if(timerForDurationOfAbility > 0){
            timerForDurationOfAbility -= Time.deltaTime;
            if(timerForDurationOfAbility <= 0){
                TransparencyEnabling(false);
            }
        }
    }

    void TransparencyEnabling(bool enable){
        if(enable){
            for(int i=0; i<materialForWalls.Count; i++){
                materialForWalls[i].GetComponent<Renderer>().material = transparentMaterial;
            }
        }
        else{
            for(int i=0; i<materialForWalls.Count; i++){
                materialForWalls[i].GetComponent<Renderer>().material = normalMaterial;
            }
        }
    }
}
