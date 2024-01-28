using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    const float CLOCK_START_ANGLE = 450f;
    const float CLOCK_END_ANGLE = 220f;

    [SerializeField] Transform clockHandTransform;
    [SerializeField] Image productivityBar;

    [SerializeField] TMP_Text harassmentText;
    [SerializeField] TMP_Text productivityText;
    Sequence feedbackTextTween;

    private void Start() {
        GameState.StartDay();
        GameState.OnProductivityChange += UpdateProductivityStatus;
        GameState.OnClockChange += UpdateClockStatus;
        CubiclesController.CorrectSlap += AnimateProductivity;
        CubiclesController.WrongSlap += AnimateHarassment;
        GameState.SetAlpha(productivityText, 0f);
        GameState.SetAlpha(harassmentText, 0f);
    }
    private void OnDestroy() {
        GameState.OnProductivityChange -= UpdateProductivityStatus;
        GameState.OnClockChange -= UpdateClockStatus;
    }
    void UpdateProductivityStatus() {
        productivityBar.fillAmount = GameState.Productivity / GameState.MAX_PRODUCTIVITY;
    }
    void UpdateClockStatus() {
        clockHandTransform.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(CLOCK_START_ANGLE, CLOCK_END_ANGLE, (GameState.Clock - GameState.START_TIME) / (GameState.END_TIME - GameState.START_TIME)));
    }

    // FeedbackText
    public void AnimateProductivity() {
        AnimateFeedback(productivityText);
    }
    public void AnimateHarassment() {
        AnimateFeedback(harassmentText);
    }
    void AnimateFeedback(TMP_Text text) {
        GameState.SetAlpha(productivityText, 0f);
        GameState.SetAlpha(harassmentText, 0f);
        productivityText.rectTransform.localEulerAngles = Vector3.zero;
        harassmentText.rectTransform.localEulerAngles = Vector3.zero;

        if(feedbackTextTween.IsActive()) {
            feedbackTextTween.Kill();
        }
        feedbackTextTween = DOTween.Sequence();

        text.rectTransform.anchoredPosition = new Vector2(0f, -100f);
        text.rectTransform.localScale = new Vector2(0.5f, 0.5f);

        feedbackTextTween.Append(text.rectTransform.DOAnchorPosY(-50, 0.4f));
        feedbackTextTween.Join(text.rectTransform.DOScale(1, 0.4f));
        feedbackTextTween.Join(text.DOFade(1, 0.4f));
        feedbackTextTween.Append(text.rectTransform.DOAnchorPosY(-30, 0.3f));
        feedbackTextTween.Append(text.rectTransform.DOAnchorPos(new Vector2(-400f, -200f), 0.4f));
        feedbackTextTween.Join(text.rectTransform.DOScale(0f, 0.2f).SetDelay(0.2f));
        feedbackTextTween.Join(text.DOFade(0f, 0.2f));
        feedbackTextTween.Join(text.rectTransform.DORotate(new Vector3(0f, 0f, 60f), 0.4f).OnComplete(() => GameState.SetAlpha(text, 0f)));
    }
}
