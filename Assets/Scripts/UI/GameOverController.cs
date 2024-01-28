using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Microlight.MicroAudio;

public class GameOverController : MonoBehaviour {
    [Header("Title")]
    [SerializeField] TMP_Text titleText;

    [Header("Labels")]
    [SerializeField] TMP_Text wagesText;
    [SerializeField] TMP_Text expensesText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] TMP_Text profitsText;

    [Header("Values")]
    [SerializeField] TMP_Text wagesValueText;
    [SerializeField] TMP_Text expensesValueText;
    [SerializeField] TMP_Text incomeValueText;
    [SerializeField] TMP_Text profitsValueText;

    [Header("Bar")]
    [SerializeField] Image productivityBar;

    [Header("Lights")]
    [SerializeField] Light pcLight;

    [Header("Sounds")]
    [SerializeField] AudioClip cashTallySound;
    [SerializeField] AudioClip happyReactionSound;
    [SerializeField] AudioClip grumpyReactionSound;

    [Header("Eyes")]
    [SerializeField] GameObject tiredEyes;
    [SerializeField] GameObject happyEyes;

    float _wages;
    float Wages {
        get => _wages;
        set {
            _wages = value;
            if(_wages == 0) {
                wagesValueText.text = $"<color=white>{_wages:N0}$</color>";
            }
            else {
                wagesValueText.text = $"<color=red>{_wages:N0}$</color>";
            }            
            Profits = 0;
        }
    }
    float _expenses;
    float Expenses {
        get => _expenses;
        set {
            _expenses = value;
            if(_expenses == 0) {
                expensesValueText.text = $"<color=white>{_expenses:N0}$</color>";
            }
            else {
                expensesValueText.text = $"<color=red>{_expenses:N0}$</color>";
            }
            Profits = 0;
        }
    }
    float _income;
    float Income {
        get => _income;
        set {
            _income = value;
            if(_income == 0) {
                incomeValueText.text = $"<color=white>{_income:N0}$</color>";
            }
            else {
                incomeValueText.text = $"<color=green>{_income:N0}$</color>";
            }
            Profits = 0;
        }
    }
    float _profits;
    float Profits {
        get => _profits;
        set {
            _profits = Income - Wages - Expenses;
            if(_profits > 0) {
                profitsValueText.text = $"<color=green>{_profits:N0}$</color>";
            }
            else if (_profits == 0){
                profitsValueText.text = $"<color=white>{_profits:N0}$</color>";
            }
            else {
                profitsValueText.text = $"<color=red>{_profits:N0}$</color>";
            }
        }
    }

    // DOTween
    Sequence uiSequence;
    Sequence pcLightSequence;

    void Start() {
        // TEST
        GameState.Productivity = 50;
        GameState.Wages = 2500;
        GameState.Expenses = 1000;
        PrepareScreen();
        AnimateGameOver();
        AnimatePCLight();
    }
    private void OnDestroy() {
        if(pcLightSequence.IsActive()) {
            pcLightSequence.Kill();
        }
        if(uiSequence.IsActive()) {
            uiSequence.Kill();
        }
    }
    void PrepareScreen() {
        // Scales
        titleText.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        wagesText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        expensesText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        incomeText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        profitsText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        productivityBar.transform.localScale = new Vector3(1f, 0f, 1f);

        // Alpha
        GameState.SetAlpha(wagesText, 0);
        GameState.SetAlpha(wagesValueText, 0);
        GameState.SetAlpha(expensesText, 0);
        GameState.SetAlpha(expensesValueText, 0);
        GameState.SetAlpha(incomeText, 0);
        GameState.SetAlpha(incomeValueText, 0);
        GameState.SetAlpha(profitsText, 0);
        GameState.SetAlpha(profitsValueText, 0);

        // Values
        Wages = 0;
        Expenses = 0;
        Income = 0;

        // Eyes
        tiredEyes.SetActive(true);
        happyEyes.SetActive(false);
    }
    void AnimateGameOver() {
        uiSequence = DOTween.Sequence();

        // Animate game appearing
        uiSequence.Append(titleText.transform.DOScale(1f, 0.5f));
        uiSequence.Append(productivityBar.transform.DOScaleY(GameState.Productivity / GameState.MAX_PRODUCTIVITY, 0.6f));
        uiSequence.Join(wagesText.transform.DOScale(1f, 0.3f));        
        uiSequence.Join(wagesText.DOFade(1f, 0.15f));
        uiSequence.Join(wagesValueText.DOFade(1f, 0.15f));
        uiSequence.Join(expensesText.transform.DOScale(1f, 0.3f).SetDelay(0.15f));
        uiSequence.Join(expensesText.DOFade(1f, 0.15f));
        uiSequence.Join(expensesValueText.DOFade(1f, 0.15f));
        uiSequence.Join(incomeText.transform.DOScale(1f, 0.3f).SetDelay(0.15f));
        uiSequence.Join(incomeText.DOFade(1f, 0.15f));
        uiSequence.Join(incomeValueText.DOFade(1f, 0.15f));
        uiSequence.Join(profitsText.transform.DOScale(1f, 0.3f).SetDelay(0.15f));
        uiSequence.Join(profitsText.DOFade(1f, 0.15f));
        uiSequence.Join(profitsValueText.DOFade(1f, 0.15f));

        uiSequence.Append(DOTween.To(() => Wages, x => Wages = x, GameState.Wages, 1f).SetDelay(0.5f).SetEase(Ease.OutCubic).OnStart(PlayCashTally));
        uiSequence.Append(DOTween.To(() => Expenses, x => Expenses = x, GameState.Expenses, 1f).SetDelay(0.5f).SetEase(Ease.OutCubic).OnStart(PlayCashTally));
        uiSequence.Append(DOTween.To(() => Income, x => Income = x, GameState.INCOME_PER_PRODUCTIVITY * GameState.Productivity, 2f).SetDelay(0.5f).SetEase(Ease.OutCubic).OnStart(PlayCashTally).OnUpdate(SetEyes).OnComplete(BossReaction));
        uiSequence.Join(productivityBar.transform.DOScaleY(0, 2f).SetEase(Ease.OutCubic));
    }
    void AnimatePCLight() {
        pcLightSequence = DOTween.Sequence();
        pcLightSequence.Append(pcLight.DOIntensity(6f, 2f));
        pcLightSequence.Append(pcLight.DOIntensity(1.5f, 2f));
        pcLightSequence.SetLoops(-1);
    }
    void PlayCashTally() {
        MicroAudio.PlayEffectSound(cashTallySound);
    }
    void SetEyes() {
        if(Profits > 0) {
            happyEyes.SetActive(true);
            tiredEyes.SetActive(false);
        }
        else {
            tiredEyes.SetActive(true);
            happyEyes.SetActive(false);
        }
    }
    void BossReaction() {
        if(Profits > 0) {
            MicroAudio.PlayEffectSound(happyReactionSound);
        }
        else {
            MicroAudio.PlayEffectSound(grumpyReactionSound);
        }
    }
}
