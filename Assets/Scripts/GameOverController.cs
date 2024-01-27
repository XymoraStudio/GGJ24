using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour {
    [Header("Title")]
    [SerializeField] TMP_Text titleText;

    [Header("Stats")]
    [SerializeField] TMP_Text wagesText;
    [SerializeField] TMP_Text wagesValueText;
    [SerializeField] TMP_Text expensesText;
    [SerializeField] TMP_Text expensesValueText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] TMP_Text incomeValueText;
    [SerializeField] TMP_Text profitText;
    [SerializeField] TMP_Text profitValueText;

    [Header("Bar")]
    [SerializeField] Image productivityBar;

    void Start() {
        PrepareScreen();
        AnimateGameOver();
    }

    void PrepareScreen() {

    }
    void AnimateGameOver() {

    }
}
