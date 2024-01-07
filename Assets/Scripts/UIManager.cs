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

    private float timerForPause;
    public int seconds3Timer;

    float tmpNoteSpeed;
    float tmpVolume;

    private void Start()
    {
        visualJudgeLine.DrawLine();

        if(SceneManager.GetActiveScene().name == "PlayScene")
        {
            touchArea.Draw();
            realJudgeLine.Draw();
        }
        GetLineInfo();
        SetUI();
        InitializeUserPref();
    }

    private void InitializeUserPref()
    {
        noteSpeedSlider.value = GameManager.Instance.noteSpeed;
        noteSpeedText.text = ((int)noteSpeedSlider.value).ToString();
        float volume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        musicVolumeSlider.value = volume;
    }

    private void GetLineInfo()
    {
        linePoints = GameManager.Instance.lineRendererPosArr;
        lineLength = linePoints.Length;
    }

    private void SetUI()
    {
        Vector3 centerPos = Camera.main.WorldToScreenPoint(linePoints[lineLength / 2]);
        Vector3 leftPos = Camera.main.WorldToScreenPoint(linePoints[lineLength / 4]);
        Vector3 rightPos = Camera.main.WorldToScreenPoint(linePoints[lineLength / 4 * 3]);

        if(SceneManager.GetActiveScene().name == "Title" || SceneManager.GetActiveScene().name == "MusicSelect")
        {
            btnCenter.transform.position = centerPos;
            btnLeft.transform.position = leftPos;
            btnRight.transform.position = rightPos;
        }
    }

    public void ShowOption()
    {
        defaultUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void BackToDefault()
    {
        defaultUI.SetActive(true);
        optionUI.SetActive(false);
    }

    private float NoteSpeedModify(Slider slider)
    {
        float valueModified = 1;
        // 0 (0.5x) ~ 1 (1x)
        switch (slider.value)
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
        float valueModified = NoteSpeedModify(slider);
        noteSpeedText.text = valueModified.ToString();
        tmpNoteSpeed = valueModified;
    }

    public void ChangeMusicVolume(Slider slider)
    {
        tmpVolume = slider.value;
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
        Debug.Log(tmpVolume);
        Debug.Log(tmpNoteSpeed);
        GameManager.Instance.ChangeMusicVolume(tmpVolume);
        GameManager.Instance.ChangeNoteSpeed(tmpNoteSpeed);
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
