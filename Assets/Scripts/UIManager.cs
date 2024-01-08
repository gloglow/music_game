using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class UIManager : MonoBehaviour
{
    // objects using line renderer
    [SerializeField] private VisualJudgeLine visualJudgeLine;
    [SerializeField] private TouchArea touchArea;
    [SerializeField] private RealJudgeLine realJudgeLine;

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

    // ui used in option.
    [SerializeField] private float noteSpeed;
    [SerializeField] private float musicVolume;
    [SerializeField] private TextMeshProUGUI noteSpeedText;
    [SerializeField] private Slider noteSpeedSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private StageManager stageManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private TextMeshProUGUI timerUI;

    // use for line rendering.
    private Vector3[] linePoints;
    private int lineLength;

    // unpause timer
    private int seconds3Timer;

    private void Start()
    {
        touchArea.Draw();
        realJudgeLine.Draw();
        seconds3Timer = 3;
        
        // 
        GetLineInfo();
        InitializeUserPref();

        // in playing, only music volume can adjusted.
        musicVolumeSlider.onValueChanged.AddListener(VolumeChanged);
    }

    private void InitializeUserPref()
    {
        // if there isn't data, music volume = -20f, notespeed = 1
        musicVolumeSlider.value = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        musicVolume = musicVolumeSlider.value;
        noteSpeedSlider.value = PlayerPrefs.HasKey("noteSpeed") ? PlayerPrefs.GetFloat("noteSpeed") : 1;
        noteSpeed = noteSpeedSlider.value;
        noteSpeedText.text = noteSpeedSlider.value.ToString();
    }

    private void GetLineInfo()
    {
        linePoints = GameManager.Instance.lineRendererPosArr;
        lineLength = linePoints.Length;
    }

    public void ShowOption()
    {
        // when option button pressed.
        defaultUI.SetActive(false);
        menuUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void OpenMenu()
    {
        // when menu button pressed. (pause button)
        defaultUI.SetActive(false);
        menuUI.SetActive(true);
        optionUI.SetActive(false);
    }

    public void BackToDefault()
    {
        // default.
        defaultUI.SetActive(false);
        menuUI.SetActive(false);
        optionUI.SetActive(false);
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void VolumeChanged(float value)
    {
        musicVolume = musicVolumeSlider.value;
    }

    public void ApplyPref()
    {
        GameManager.Instance.SaveOptionData(noteSpeed, musicVolume);
    }

    public void Pause()
    {
        defaultUI.SetActive(false);
        menuUI.SetActive(true);
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
        stageManager.PlayBack();
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