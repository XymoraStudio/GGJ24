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
    private ParticleSystem laughingPS;

    private void Awake() {
        animatorControl = GetComponent<Animator>();
        laughingPS = GetComponentInChildren<ParticleSystem>();
    } 
    
    private void Update() {
        animatorControl.SetBool(workingParametarName, !active);
        animatorControl.SetBool(misbehaviourParametarName, active);

        laughingPS.gameObject.SetActive(active);
    }
}