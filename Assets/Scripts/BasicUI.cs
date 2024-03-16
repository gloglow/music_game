using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BasicUI : MonoBehaviour
{
    // UI セット
    [SerializeField] private GameObject defaultUI;
    [SerializeField] private GameObject optionUI;

    // オプションバリュー
    [SerializeField] private float noteSpeed, musicVolume;
    [SerializeField] private TextMeshProUGUI noteSpeedText;
    [SerializeField] private Slider noteSpeedSlider, musicVolumeSlider;

    private void Start()
    {
        LoadUserPref();　// ユーザープレファレンスをロード

        if (SceneManager.GetActiveScene().name == "PlayScene")　//　プレイ中にはノーツスピードの調整を不可能にする
        {
            noteSpeedSlider.enabled = false;
        }

        noteSpeedSlider.onValueChanged.AddListener(SpeedChanged);
        musicVolumeSlider.onValueChanged.AddListener(VolumeChanged);
    }

    private void LoadUserPref()
    {
        // プレイヤーの設定があればその通りに
        musicVolumeSlider.value = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        musicVolume = musicVolumeSlider.value;

        noteSpeedSlider.value = PlayerPrefs.HasKey("noteSpeed") ? PlayerPrefs.GetInt("noteSpeed") : 1;
        noteSpeed = noteSpeedSlider.value;
        noteSpeedText.text = GameManager.Instance.speeds[(int)noteSpeedSlider.value].ToString();
    }

    public void OnOffOption(bool onoff)
    {
        optionUI.SetActive(onoff);
    }

    public void OnDefault()
    {
        defaultUI.SetActive(true);
    }

    public void OffDefault()
    {
        defaultUI.SetActive(false);
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

    public void ApplyPref()　//　設定変更を適用
    {
        GameManager.Instance.SaveOptionData((int)noteSpeed, musicVolume);
    }
}
