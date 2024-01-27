using System;

public static class GameState {
    public static Action OnProductivityChange;
    public static float Productivity {
        get => Productivity;
        set {
            Productivity = value;
            OnProductivityChange?.Invoke();
        }
    }
}
