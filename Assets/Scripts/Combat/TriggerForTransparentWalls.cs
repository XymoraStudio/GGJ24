using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForTransparentWalls : MonoBehaviour
{
    [SerializeField] private GameObject walls;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger");
        if(other.tag == "Player"){
            Debug.Log("player");
                Abilities.instance.materialForWalls.Clear();
            for(int i = 0; i<walls.transform.childCount; i++){
                Abilities.instance.materialForWalls.Add(walls.transform.GetChild(i).gameObject);
            }
        }
    }
}
