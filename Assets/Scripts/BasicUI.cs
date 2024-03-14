using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BasicUI : MonoBehaviour
{
    // UI set.
    [SerializeField] private GameObject defaultUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject menuUI;

    // Option value
    [SerializeField] private float noteSpeed, musicVolume;
    [SerializeField] private TextMeshProUGUI noteSpeedText;
    [SerializeField] private Slider noteSpeedSlider, musicVolumeSlider;

    private void Start()
    {
        // Load user preference
        LoadUserPref();

        if(SceneManager.GetActiveScene().name == "PlayScene")
        {
            noteSpeedSlider.enabled = false;
        }
        noteSpeedSlider.onValueChanged.AddListener(SpeedChanged);
        musicVolumeSlider.onValueChanged.AddListener(VolumeChanged);
    }

    private void LoadUserPref()
    {
        // if there is player data, set ui value.
        musicVolumeSlider.value = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        musicVolume = musicVolumeSlider.value;

        noteSpeedSlider.value = PlayerPrefs.HasKey("noteSpeed") ? PlayerPrefs.GetInt("noteSpeed") : 1;
        noteSpeed = noteSpeedSlider.value;
        noteSpeedText.text = GameManager.Instance.speeds[(int)noteSpeedSlider.value].ToString();
    }

    public void OnDefault()
    {
        defaultUI.SetActive(true);
    }

    public void OffDefault()
    {
        defaultUI.SetActive(false);
    }

    public void OnOption()
    {
        optionUI.SetActive(true);
    }

    public void OffOption()
    {
        optionUI.SetActive(false);
    }

    public void ShowOption() // When option btn pressed
    {
        defaultUI.SetActive(false);
        optionUI.SetActive(true);
        if(SceneManager.GetActiveScene().name == "PlayScene")
            menuUI.SetActive(false);
    }

    public void BackToDefault() // When back btn pressed
    {
        defaultUI.SetActive(true);
        optionUI.SetActive(false);
        if (SceneManager.GetActiveScene().name == "PlayScene")
            menuUI.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MoveScene(string sceneName)
    {
        GameManager.Instance.MoveScene(sceneName);
    }
    
    public void VolumeChanged(float value)
    {
        musicVolume = musicVolumeSlider.value;
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
