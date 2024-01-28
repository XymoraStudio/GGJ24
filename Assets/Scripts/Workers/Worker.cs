using Microlight.MicroAudio;
using System;
using UnityEngine;

public class Worker : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem laughingParticle;
    [SerializeField] MicroSoundGroup slapSoundGroup;

    public static event Action<Worker> OnSlapped;

    private void Start() {
        animator.SetTrigger("Work");
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
    }
    #endregion
}