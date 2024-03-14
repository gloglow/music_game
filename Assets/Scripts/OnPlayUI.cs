using System.Collections;
using TMPro;
using UnityEngine;

public class OnPlayUI : MonoBehaviour
{
    // objects using line renderer
    [SerializeField] private JudgeLine visualJudgeLine;
    [SerializeField] private TouchArea touchArea;

    // ui set.
    [SerializeField] private GameObject defaultUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject resultUI;

    // ui text.
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI result_scoreText;
    [SerializeField] private TextMeshProUGUI result_comboText;
    [SerializeField] private TextMeshProUGUI result_rankText;

    [SerializeField] private StageManager stageManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private TextMeshProUGUI timerUI;

    // unpause timer
    private int seconds3Timer;

    private void Start()
    {
        touchArea.Draw();
        seconds3Timer = 3;
    }

    public void OpenMenu()
    {
        // when menu button pressed. (pause button)
        defaultUI.SetActive(false);
        menuUI.SetActive(true);
        optionUI.SetActive(false);
    }

    public void Pause()
    {
        defaultUI.SetActive(false);
        menuUI.SetActive(true);
        resultUI.SetActive(false);
    }

    public void UnPause()
    { 
        defaultUI.SetActive(false);
        menuUI.SetActive(false);

        // when back button pressed during pausing, restart after 3 seconds. 
        StartCoroutine(PlayBack(3f));

        // display timer.
        timerUI.gameObject.SetActive(true);
        timerUI.text = "3";
        StartCoroutine(TimerUpdate(1f));
        StartCoroutine(TimerUpdate(2f));
        StartCoroutine(TimerUpdate(3f));
    }

    IEnumerator TimerUpdate(float time)
    {
        yield return new WaitForSeconds(time);

        seconds3Timer--;

        timerUI.text = seconds3Timer.ToString();
        if(seconds3Timer == 0)
        {
            // after 3 seconds, initialize timer and turn off.
            seconds3Timer = 3;
            timerUI.gameObject.SetActive(false);
        }
        timerUI.text = seconds3Timer.ToString();
    }

    IEnumerator PlayBack(float time)
    {
        // after 3 seconds, unpause game.
        yield return new WaitForSeconds(time);
        defaultUI.SetActive(true);
    }

    public void updateScore(int score)
    {
        scoreText.text = "Score : " + score.ToString();
    }

    public void updateCombo(int combo)
    {
        comboText.text = combo.ToString() + " COMBO !";
    }

    public void ShowResultUI(int combo, int score, char rank)
    {
        resultUI.SetActive(true);
        defaultUI.SetActive(false);

        result_comboText.text = "COMBO : " + combo.ToString();
        result_scoreText.text = "SCORE : " + score.ToString();
        result_rankText.text = rank.ToString();
    }
}