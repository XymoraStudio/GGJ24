using System;
using UnityEngine;
using TMPro;

public static class GameState {
    public static Action OnProductivityChange;
    static float _productivity;
    public static float Productivity {
        get => _productivity;
        set {
            if(_productivity == value) return;

            value = Mathf.Clamp(value, 0f, 100f);
            _productivity = value;
            OnProductivityChange?.Invoke();
        }
    }
    public const float MaxProductivity = 100f;
    public const float ProductivityLostPerSec = 1;
    public const float DayDurationSec = 120;
    public const float IncomePerProductivity = 100;
    public static float Wages { get; set; }
    public static float Expenses { get; set; }

    public static void SetAlpha(TMP_Text text, float alpha) {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
