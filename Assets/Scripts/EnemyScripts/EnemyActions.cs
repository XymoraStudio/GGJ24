using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyActions : MonoBehaviour
{
    public bool active;
    private Animator animatorControl;
    [SerializeField] private string workingParametarName;
    [SerializeField] private string misbehaviourParametarName;
    private Transform startTransform;

    private void Awake() {
        animatorControl = GetComponent<Animator>();
        startTransform = transform;
    } 
    
    private void Update(){
        animatorControl.SetBool(workingParametarName, active);
        animatorControl.SetBool(misbehaviourParametarName, !active);
    }
}
