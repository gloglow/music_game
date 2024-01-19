using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class BasicUI : MonoBehaviour
{
    [SerializeField] private GameObject defaultUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject menuUI;

    [SerializeField] private float musicVolume;
    [SerializeField] private float noteSpeed;
    [SerializeField] private TextMeshProUGUI noteSpeedText;
    [SerializeField] private Slider noteSpeedSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
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

    public void ShowOption()
    {
        defaultUI.SetActive(false);
        menuUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void BackToDefault()
    {
        defaultUI.SetActive(true);
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
