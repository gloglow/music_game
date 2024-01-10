using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerNonPlay : MonoBehaviour
{
    [SerializeField] private VisualJudgeLine visualJudgeLine;

    [SerializeField] private GameObject defaultUI;
    [SerializeField] private GameObject optionUI;

    [SerializeField] private GameObject btnCenter;
    [SerializeField] private GameObject btnRight;
    [SerializeField] private GameObject btnLeft;

    [SerializeField] private float musicVolume;
    [SerializeField] private float noteSpeed;
    [SerializeField] private TextMeshProUGUI noteSpeedText;
    [SerializeField] private Slider noteSpeedSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private AudioManager audioManager;

    private Vector3[] linePoints;
    private int lineLength;

    private void Start()
    {
        GetLineInfo();
        SetUI();
        InitializeUserPref();
        noteSpeedSlider.onValueChanged.AddListener(SpeedChanged);
        musicVolumeSlider.onValueChanged.AddListener(VolumeChanged);
    }

    private void InitializeUserPref()
    {
        musicVolumeSlider.value = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        musicVolume = musicVolumeSlider.value;
        noteSpeedSlider.value = PlayerPrefs.HasKey("noteSpeed") ? PlayerPrefs.GetInt("noteSpeed") : 1;
        noteSpeed = noteSpeedSlider.value;
        noteSpeedText.text = GameManager.Instance.speeds[(int)noteSpeedSlider.value].ToString();
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

        btnCenter.transform.position = centerPos;
        btnLeft.transform.position = leftPos;
        btnRight.transform.position = rightPos;
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

    public float SpeedToActualSpeed(float value)
    {
        switch (value)
        {
            case 0:
                return 0.5f;
            case 1:
                return 1f;
            case 2:
                return 2f;
            default:
                return 1f;
        }
    }

    public void SpeedChanged(float value)
    {
        noteSpeedText.text = GameManager.Instance.speeds[(int)value].ToString();
        noteSpeed = value;
    }

    public void ApplyPref()
    {
        GameManager.Instance.SaveOptionData((int)noteSpeed, musicVolume);
    }
}
