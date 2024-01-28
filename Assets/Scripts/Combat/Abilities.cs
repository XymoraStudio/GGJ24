using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public static Abilities instance;
    public List<GameObject> materialForWalls = new List<GameObject>();
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private float durationOfAbility = 10f;
    private float timerForDurationOfAbility;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyBindsPlayer.ability)){
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
                Debug.Log("changing");
                Debug.Log(materialForWalls[i].GetComponent<MeshRenderer>().materials[materialForWalls[i].GetComponent<MeshRenderer>().materials.Count()-1]);
                materialForWalls[i].GetComponent<MeshRenderer>().sharedMaterials[2] = transparentMaterial;
            }
        }
        else{
            Debug.Log("changing back");
            for(int i=0; i<materialForWalls.Count; i++){
                materialForWalls[i].GetComponent<Renderer>().sharedMaterials[materialForWalls[i].GetComponent<MeshRenderer>().materials.Count() - 1] = normalMaterial;
            }
        }
    }
}
