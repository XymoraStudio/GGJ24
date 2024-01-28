using Microlight.MicroAudio;
using System;
using UnityEngine;

public class Worker : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem laughingParticle;
    [SerializeField] MicroSoundGroup slapSoundGroup;
    [SerializeField] private float timeAfterSlapToReturnToStartPosition = 1f;
    private float timerForReturnToStartingPosition;
    private Vector3 startPosition;
    private Quaternion startRotation;
    public static event Action<Worker> OnSlapped;

    private void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        animator.SetTrigger("Work");
    }

    private void Update() {
    }

    #region Laugh
    public void Laugh() {
        animator.SetTrigger("Laugh");
        laughingParticle.Play();
    }
    public void StopLaughing() {
        animator.SetTrigger("Work");
        laughingParticle.Stop();
    }
    #endregion

    #region Slap
    public void SlapWorker() {
        MicroAudio.PlayEffectSound(slapSoundGroup.GetRandomClip, 0.3f);
        GameState.Expenses += GameState.EXPENSE_PER_SLAP;
        OnSlapped?.Invoke(this);
        timerForReturnToStartingPosition = timeAfterSlapToReturnToStartPosition;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        Invoke(nameof(ReturningToStartPosition), timeAfterSlapToReturnToStartPosition);
    }
    #endregion
    private void ReturningToStartPosition(){
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}