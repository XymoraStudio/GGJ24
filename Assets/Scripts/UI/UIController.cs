using UnityEngine;

public class UIController : MonoBehaviour {
    const float CLOCK_START_ANGLE = 450f;
    const float CLOCK_END_ANGLE = 220f;

    [SerializeField] Transform clockHandTransform;
    [SerializeField] Transform productivityBar;

    private void Start() {
        GameState.StartDay();
        GameState.OnProductivityChange += UpdateProductivityStatus;
        GameState.OnClockChange += UpdateClockStatus;
    }
    private void OnDestroy() {
        GameState.OnProductivityChange -= UpdateProductivityStatus;
        GameState.OnClockChange -= UpdateClockStatus;
    }
    void UpdateProductivityStatus() {
        productivityBar.localScale = new Vector3(1f, GameState.Productivity / GameState.MAX_PRODUCTIVITY, 1f);
    }
    void UpdateClockStatus() {
        clockHandTransform.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(CLOCK_START_ANGLE, CLOCK_END_ANGLE, (GameState.Clock - GameState.START_TIME) / (GameState.END_TIME - GameState.START_TIME)));
    }
}
