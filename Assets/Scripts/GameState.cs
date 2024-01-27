using System;
using UnityEngine;
using TMPro;

public static class GameState {
    // Productivity
    public static Action OnProductivityChange;
    public static Action OnGameOver;
    static float _productivity;
    public static float Productivity {
        get => _productivity;
        set {
            if(_productivity == value) return;

            value = Mathf.Clamp(value, 0f, MaxProductivity);
            _productivity = value;
            OnProductivityChange?.Invoke();
            if(_productivity <= 0) OnGameOver?.Invoke();
        }
    }
    public const float MaxProductivity = 100f;
    public const float ProductivityLostPerSec = 1;    
    public const float IncomePerProductivity = 100;
    public static float Wages { get; set; }
    public static float Expenses { get; set; }

    // Time
    public static Action OnDayEnd;
    const float StartTime = 9;
    const float EndTime = 17;
    const float DayDurationSec = 120;
    static float TimePerSec = (EndTime - StartTime) / DayDurationSec;
    static bool DayActive = false;
    static float _clock;
    public static float Clock {
        get => _clock;
        private set {
            _clock = value;
            if(_clock >= EndTime) {
                DayActive = false;
                OnDayEnd?.Invoke();
            }
        }
    }
    public static void StartDay() {
        Clock = StartTime;
        DayActive = true;
    }
    public static void UpdateTime() {
        if(!DayActive) {
            return;
        }
        Clock += TimePerSec * Time.deltaTime;        
    }

    public static void SetAlpha(TMP_Text text, float alpha) {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
