using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public bool active;
    private Animator animatorControl;
    [SerializeField] private string workingParametarName;
    [SerializeField] private string misbehaviourParametarName;

    private void Awake() {
        animatorControl = GetComponent<Animator>();
    } 
    
    private void Update() {
        if(active){
            animatorControl.SetBool(workingParametarName, active);
            animatorControl.SetBool(misbehaviourParametarName, !active);
        }
        else{
            animatorControl.SetBool(workingParametarName, active);
            animatorControl.SetBool(misbehaviourParametarName, !active);
        }
    }
}
