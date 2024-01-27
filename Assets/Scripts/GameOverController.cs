using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    float _wages;
    float Wages {
        get => _wages;
        set {
            _wages = value;
            wagesValueText.text = $"{_wages:N0}$";
            Profits = 0;
        }
    }
    float _expenses;
    float Expenses {
        get => _expenses;
        set {
            _expenses = value;
            expensesValueText.text = $"{_expenses:0}$";
            Profits = 0;
        }
    }
    float _income;
    float Income {
        get => _income;
        set {
            _income = value;
            incomeValueText.text = $"{_income:0}$";
            Profits = 0;
        }
    }
    float _profits;
    float Profits {
        get => _profits;
        set {
            _profits = Income - Wages - Expenses;
            profitsValueText.text = $"{_profits:0}$";
        }
    }

    void Start() {
        // TEST
        GameState.Productivity = 50;
        GameState.Wages = 2500;
        GameState.Expenses = 1000;
        PrepareScreen();
        AnimateGameOver();
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
    }
    void AnimateGameOver() {
        Sequence screenAnimation = DOTween.Sequence();

        // Animate game appearing
        screenAnimation.Append(titleText.transform.DOScale(1f, 0.5f));
        screenAnimation.Append(productivityBar.transform.DOScaleY(GameState.Productivity / GameState.MaxProductivity, 0.6f));
        screenAnimation.Join(wagesText.transform.DOScale(1f, 0.3f));        
        screenAnimation.Join(wagesText.DOFade(1f, 0.15f));
        screenAnimation.Join(wagesValueText.DOFade(1f, 0.15f));
        screenAnimation.Join(expensesText.transform.DOScale(1f, 0.3f).SetDelay(0.15f));
        screenAnimation.Join(expensesText.DOFade(1f, 0.15f));
        screenAnimation.Join(expensesValueText.DOFade(1f, 0.15f));
        screenAnimation.Join(incomeText.transform.DOScale(1f, 0.3f).SetDelay(0.15f));
        screenAnimation.Join(incomeText.DOFade(1f, 0.15f));
        screenAnimation.Join(incomeValueText.DOFade(1f, 0.15f));
        screenAnimation.Join(profitsText.transform.DOScale(1f, 0.3f).SetDelay(0.15f));
        screenAnimation.Join(profitsText.DOFade(1f, 0.15f));
        screenAnimation.Join(profitsValueText.DOFade(1f, 0.15f));

        screenAnimation.Append(DOTween.To(() => Wages, x => Wages = x, GameState.Wages, 1f).SetDelay(0.5f).SetEase(Ease.OutCubic));
        screenAnimation.Append(DOTween.To(() => Expenses, x => Expenses = x, GameState.Expenses, 1f).SetDelay(0.5f).SetEase(Ease.OutCubic));
        screenAnimation.Append(DOTween.To(() => Income, x => Income = x, GameState.IncomePerProductivity * GameState.Productivity, 2f).SetDelay(0.5f).SetEase(Ease.OutCubic));
        screenAnimation.Join(productivityBar.transform.DOScaleY(0, 2f).SetEase(Ease.OutCubic));
    }
}
