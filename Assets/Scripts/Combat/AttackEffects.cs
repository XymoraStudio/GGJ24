using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class AttackEffects : MonoBehaviour
{
    public static AttackEffects instance;
    [SerializeField] private int percentageToSlowDownTimeOnHit = 30;
    [SerializeField] private float timeSlowness = 0.5f;
    [SerializeField] private float lenghtOfTimeSlowness = 0.001f;
    private bool timeSlowed;
    private float startTimeScale;
    private float timerForSlowness;

    private void Awake() {
        instance = this;
        timeSlowed = false;
        startTimeScale = Time.timeScale;
    }

    private void Update() {
        if(timeSlowed){
            timerForSlowness -= Time.deltaTime;
            if(timerForSlowness <= 0){
                Debug.Log("Normalno vrijeme");
                timeSlowed = false;
                Time.timeScale = startTimeScale;
            }
        }
    }

    public void SlowingDownTime(){
        int chanceToSlow = Random.Range(0, 100);
        if(chanceToSlow <= percentageToSlowDownTimeOnHit){
            Debug.Log("Usporeno vrijeme");
            timerForSlowness = lenghtOfTimeSlowness;
            Time.timeScale = timeSlowness;
            timeSlowed = true;
        }
    }
}
