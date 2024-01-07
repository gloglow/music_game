using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManagerNonPlay : MonoBehaviour
{
    [SerializeField] private VisualJudgeLine visualJudgeLine;

    [SerializeField] private GameObject defaultUI;

    [SerializeField] private GameObject btnCenter;
    [SerializeField] private GameObject btnRight;
    [SerializeField] private GameObject btnLeft;

    [SerializeField] private GameObject optionUI;
    [SerializeField] private TextMeshProUGUI noteSpeedText;

    [SerializeField] private Slider noteSpeedSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private AudioManager audioManager;

    private Vector3[] linePoints;
    private int lineLength;

    // variables to save data
    float realNoteSpeed;
    float realVolume;

    // variables for actual change
    float fakeNoteSpeed;
    float fakeVolume; 

    private void Start()
    {
        GetLineInfo();
        SetUI();
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
        if(realVolume == -40f)
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
}
