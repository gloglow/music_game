using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VisualJudgeLine visualJudgeLine;
    [SerializeField] private TouchArea touchArea;
    [SerializeField] private RealJudgeLine realJudgeLine;

    [SerializeField] private GameObject defaultUI;
    [SerializeField] private GameObject menuUI;

    [SerializeField] private GameObject btnCenter;
    [SerializeField] private GameObject btnRight;
    [SerializeField] private GameObject btnLeft;

    [SerializeField] private GameObject optionUI;
    [SerializeField] private TextMeshProUGUI noteSpeedText;

    [SerializeField] private Slider noteSpeedSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private StageManager stageManager;
    [SerializeField] private AudioManager audioManager;

    public TextMeshProUGUI timerUI;
    private float timerForRestart;

    private Vector3[] linePoints;
    private int lineLength;

    // variables to save data
    float realNoteSpeed;
    float realVolume;

    // variables for actual change
    float fakeNoteSpeed;
    float fakeVolume;

    int seconds3Timer;

    private void Start()
    {
        touchArea.Draw();
        realJudgeLine.Draw();
        seconds3Timer = 3;
        
        GetLineInfo();
        InitializeUserPref();
    }

    private void InitializeUserPref()
    {
        noteSpeedSlider.value = GameManager.Instance.noteSpeed;
        noteSpeedText.text = ((noteSpeedSlider.value == 0f) ? 0.5f : 1f).ToString();
        float volume = PlayerPrefs.HasKey("musicVolume") ? (PlayerPrefs.GetFloat("musicVolume") + 40) * 100 / 40 : -20f;
        musicVolumeSlider.value = volume;
    }

    private void GetLineInfo()
    {
        linePoints = GameManager.Instance.lineRendererPosArr;
        lineLength = linePoints.Length;
    }

    public void ShowOption()
    {
        defaultUI.SetActive(false);
        menuUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void OpenMenu()
    {
        defaultUI.SetActive(false);
        menuUI.SetActive(true);
        optionUI.SetActive(false);
    }

    public void BackToDefault()
    {
        defaultUI.SetActive(true);
        menuUI.SetActive(false);
        optionUI.SetActive(false);
    }

    private float NoteSpeedModify(float tmpSpeed)
    {
        // change 0 into 0.5

        float valueModified = 1;
        // 0 (0.5x) ~ 1 (1x)
        switch (tmpSpeed)
        {
            case 0:
                valueModified = 0.5f;
                break;
            case 1:
                valueModified = 1f;
                break;
        }
        return valueModified;
    }

    public void ChangeNoteSpeed(Slider slider)
    {
        // control note speed by slider.
        realNoteSpeed = slider.value; // 0 or 1
        float valueModified = NoteSpeedModify(realNoteSpeed); // 0.5 or 1
        noteSpeedText.text = valueModified.ToString();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        fakeVolume = slider.value; // 0 ~ 100
        realVolume = fakeVolume * 40 / 100 - 40;
        if (realVolume == -40f)
        {
            realVolume = -80f;
        }
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ApplyPref()
    {
        GameManager.Instance.ChangeMusicVolume(realVolume); // -40 ~ 0
        GameManager.Instance.ChangeNoteSpeed(realNoteSpeed);
    }

    public void Pause()
    {
        defaultUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void UnPause()
    {
        defaultUI.SetActive(true);
        menuUI.SetActive(false);
        Invoke("PlayBack", 3f);
        timerUI.gameObject.SetActive(true);
        timerUI.text = "3";
        Invoke("TimerUpdate", 1f);
        Invoke("TimerUpdate", 2f);
        Invoke("TimerUpdate", 3f);
    }

    private void TimerUpdate()
    {
        seconds3Timer--;
        timerUI.text = seconds3Timer.ToString();
        if(seconds3Timer == 0)
        {
            seconds3Timer = 3;
            timerUI.gameObject.SetActive(false);
        }
        timerUI.text = seconds3Timer.ToString();
    }

    public void PlayBack()
    {
        stageManager.PlayBack();
    }
}
