using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Microlight.MicroUI;
using UnityEngine.SceneManagement;
using Microlight.MicroAudio;

public class MainMenuController : MonoBehaviour {
    [SerializeField] Image titleImage;
    [SerializeField] MicroButton startGameButton;
    [SerializeField] MicroButton exitGameButton;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip soundtrack;
    [SerializeField] AudioClip startGameSound;

    [SerializeField] ParticleSystem cryPS;
    [SerializeField] ParticleSystem laughPS;

    MicroUIShortcutGroup shortcutGroup;

    // DoTween
    Sequence titleTween;

    private void Start() {
        // Title
        //1.5
        titleImage.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        // rotation 11
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
        MicroAudio.PlayOneTrack(startGameSound, false);
    }

    void AnimateTitle() {
        titleTween = DOTween.Sequence();
        MicroAudio.PlayEffectSound(explosionSound, 0.5f);
        cryPS.Play();
        laughPS.Play();
        titleImage.color = new Color(1f, 1f, 1f, 0f);
        titleTween.Append(titleImage.transform.DOScale(1.5f, 1f));
        titleTween.Join(titleImage.transform.DORotate(new Vector3(0f, 0f, 11f), 1f).OnComplete(() => { MicroAudio.PlayOneTrack(soundtrack, true); }));
        titleTween.Join(titleImage.DOFade(1f, 1f));
    }
    void StartGame(MicroInteractable interactable) {
        SceneManager.LoadScene("Level1");
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
        transform.DOScale(1.6f, 0.4f).SetEase(Ease.OutElastic);
    }
    void ScaleDownAnimation(Transform transform) {
        transform.DOScale(1.5f, 0.4f).SetEase(Ease.OutElastic);
    }
    #endregion
}
