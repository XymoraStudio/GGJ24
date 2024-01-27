using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public bool active;

    public void ActivatingOrDeactivating(bool active){
        gameObject.SetActive(active);
    }
}
