using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroAudio;

public class CubiclesController : MonoBehaviour {
    [Header("Cubicles")]
    [SerializeField] List<Cubicle> allCubicles;
    List<Cubicle> laughCubicles = new List<Cubicle>();

    [Header("Sounds")]
    [SerializeField] MicroInfinitySoundGroup laughSound;
    [SerializeField] MicroInfinitySoundGroup workSound;
    MicroInfinityInstance activeSoundInstance;

    int maxActiveCubicles = 1;

    const float CUBICLE_LAUGH_TIMER = 2f;
    float laughCubicleTimer = CUBICLE_LAUGH_TIMER;

    private void Start() {
        Cubicle.OnCubicleSlapped += SlappedCubicle;
        activeSoundInstance = MicroAudio.PlayInfinityEffectSound(workSound);
        foreach(Cubicle x in allCubicles) {
            x.StopLaughing();
        }
    }
    private void OnDestroy() {
        Cubicle.OnCubicleSlapped -= SlappedCubicle;
    }
    private void Update() {
        laughCubicleTimer -= Time.deltaTime;
        if(laughCubicleTimer <= 0 && laughCubicles.Count < maxActiveCubicles) {
            LaughCubicle();
        }
    }

    // Laughing in cubicles
    #region Laughing
    void LaughCubicle() {
        while(true) {
            Cubicle cubicle = allCubicles[Random.Range(0, allCubicles.Count -1)];
            if(laughCubicles.Contains(cubicle)) {
                continue;
            }
            cubicle.Laugh();
            laughCubicles.Add(cubicle);

            // If work sound was before
            if(laughCubicles.Count == 1) {
                activeSoundInstance.Cancel();
                activeSoundInstance = MicroAudio.PlayInfinityEffectSound(laughSound);
            }            
            Debug.Log("Making cubicle laugh.");
            break;
        }

    }
    #endregion

    // When cubicle is slapped
    #region Slapping
    void SlappedCubicle(Cubicle cubicle) {
        if(laughCubicles.Contains(cubicle)) {
            SlappedCorrectCubicle(cubicle);
        }
        else {
            SlappedWrongCubicle(cubicle);
        }
    }
    void SlappedCorrectCubicle(Cubicle cubicle) {
        GameState.Productivity += 20f;
        cubicle.StopLaughing();
        laughCubicles.Remove(cubicle);
        laughCubicleTimer = CUBICLE_LAUGH_TIMER;

        // If there is no more laughing cubicles
        if(laughCubicles.Count == 0) {
            activeSoundInstance.Cancel();
            activeSoundInstance = MicroAudio.PlayInfinityEffectSound(workSound);
        }
    }
    void SlappedWrongCubicle(Cubicle cubicle) {
        GameState.Productivity -= 20f;
    }
    #endregion
}
