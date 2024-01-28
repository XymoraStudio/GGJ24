using System;
using UnityEngine;
using TMPro;

public static class GameState {
    // Productivity
    public static event Action OnProductivityChange;
    public static event Action OnGameOver;

    static float _productivity = MAX_PRODUCTIVITY;
    public static float Productivity {
        get => _productivity;
        set {
            if(_productivity == value) return;

            value = Mathf.Clamp(value, 0f, MAX_PRODUCTIVITY);
            _productivity = value;
            OnProductivityChange?.Invoke();
            if(_productivity <= 0) OnGameOver?.Invoke();
        }
    }

    public const float MAX_PRODUCTIVITY = 100f;
    public const float PRODUCTIVITY_LOST_PER_SEC = 1f;    
    public const float INCOME_PER_PRODUCTIVITY = 100;
    public const float PRODUCTIVITY_BOOST = 10f;
    public const float PRODUCTIVITY_PENALTY = 100f;
    public static float Wages { get; set; }
    public static float Expenses { get; set; }
    public const float EXPENSE_PER_SLAP = 100f;

    // Time
    public static event Action OnDayEnd;
    public static event Action OnClockChange;

    public const float START_TIME = 9;
    public const float END_TIME = 17;
    const float DAY_DURATION = 120;
    static float TIME_PASS_PER_SEC = (END_TIME - START_TIME) / DAY_DURATION;

    static float _clock;
    public static float Clock {
        get => _clock;
        private set {
            if(_clock > END_TIME && value > END_TIME) return;   // Lets clock to update be run but not trigger end day
            _clock = value;
            OnClockChange?.Invoke();
            if(_clock >= END_TIME) {
                OnDayEnd?.Invoke();
            }
        }
    }
    public static void StartDay() {
        Clock = START_TIME;
    }
    public static void UpdateClock() {
        Clock += TIME_PASS_PER_SEC * Time.deltaTime;        
    }

    // Levels
    public static string NextScene = "Level1";

    #region Utility
    public static void SetAlpha(TMP_Text text, float alpha) {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
    #endregion
}