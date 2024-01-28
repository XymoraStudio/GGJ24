using UnityEngine;
using TMPro;
using DG.Tweening;
using Microlight.MicroUI;

public class MainMenuController : MonoBehaviour {
    [SerializeField] TMP_Text titleText;
    [SerializeField] MicroButton startGameButton;
    [SerializeField] MicroButton exitGameButton;

    MicroUIShortcutGroup shortcutGroup;

    // DoTween
    Sequence titleTween;

    private void Start() {
        // Title
        titleText.text = "Laugh Shooter";
        AnimateTitle();

        // Buttons
        startGameButton.OnClick += StartGame;
        startGameButton.OnHoverChange += HoverChange;
        exitGameButton.OnClick += ExitGame;
        exitGameButton.OnHoverChange += HoverChange;
        shortcutGroup = new MicroUIShortcutGroup(startGameButton, exitGameButton, MicroUIShortcutController.DEFAULT_PRIORITY);
        MicroUIShortcutController.Instance.AddGroup(shortcutGroup);
    }
    private void OnDestroy() {
        MicroUIShortcutController.Instance.RemoveGroup(shortcutGroup);
        if(titleTween.IsActive()) {
            titleTween.Kill();
        }
    }

    void AnimateTitle() {
        titleTween = DOTween.Sequence();

        titleTween.Append(titleText.transform.DORotate(new Vector3(0f, 0f, -10f), 1f).SetEase(Ease.Unset));
        titleTween.Append(titleText.transform.DORotate(new Vector3(0f, 0f, 10f), 1f).SetEase(Ease.Unset));
        titleTween.SetLoops(-1);
    }
    void StartGame(MicroInteractable interactable) {
        Debug.Log("Start game");
    }
    void ExitGame(MicroInteractable interactable) {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    #region Animation
    void HoverChange(MicroInteractable interactable) {
        if(interactable.IsHovered) ScaleUpAnimation(interactable.transform);
        else ScaleDownAnimation(interactable.transform);
    }
    void ScaleUpAnimation(Transform transform) {
        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutElastic);
    }
    void ScaleDownAnimation(Transform transform) {
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutElastic);
    }
    #endregion
}
