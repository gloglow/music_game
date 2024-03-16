using System.Collections;
using TMPro;
using UnityEngine;

public class OnPlayUI : MonoBehaviour
{
    // UIセット
    [SerializeField] private BasicUI basicUI;
    [SerializeField] private GameObject menuUI;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI gradeText;

    [SerializeField] private TextMeshProUGUI timerUI;　//　一時中止を解除した時のタイマー

    public void OnOffMenu(bool onoff)
    {
        menuUI.SetActive(onoff);
    }

    public void TimerUpdate(int time)
    {
        timerUI.text = time.ToString();
    }

    public void OnOffTimer(bool onoff)
    {
        timerUI.gameObject.SetActive(onoff);
    }

    public void ChangeGradeText(int grade) //　ノーツの判定
    {
        switch (grade)
        {
            case -1:
                gradeText.text = "Miss!";
                break;
            case 0:
                gradeText.text = "Bad!";
                break;
            case 1:
                gradeText.text = "Great!";
                break;
            case 2:
                gradeText.text = "Perfect!";
                break;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score.ToString();
    }

    public void UpdateCombo(int combo)
    {
        comboText.text = combo.ToString() + " COMBO !";
    }
}