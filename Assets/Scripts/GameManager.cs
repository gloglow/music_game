using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // シングルトン

    public AudioMixer audioMixer;

    public int crtSpeed; // スライダーのバリュー：0,1,2
    public float[] speeds = {0.5f, 1f, 2f}; // 

    public int crtMusicIndex;
    public string crtMusicName;
    public int crtScore;
    public int crtMiss;
    public int crtCombo;
    public string crtRank;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if(instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        crtMusicName = "TitleMusic";
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        float volume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        audioMixer.SetFloat("Music", volume);
        crtSpeed = PlayerPrefs.HasKey("noteSpeed") ? PlayerPrefs.GetInt("noteSpeed") : 1;

    }

    public void ChangeMusicVolume(float value)
    {
        // -40以下はほぼ聞こえないので、ミュートにする
        if (value <= -40)
            value = -80;
        audioMixer.SetFloat("Music", value);
    }

    public void ChangeNoteSpeed(int value)
    {
        crtSpeed = value;
    }

    public void MoveScene(string sceneName)
    {
        switch (sceneName)
        {
            case "ModeSelect":
                if(AudioManager.instance.audioSource.clip != AudioManager.instance.defaultBGM)
                {
                    AudioManager.instance.audioSource.clip = AudioManager.instance.defaultBGM;
                    AudioManager.instance.audioSource.Play();
                }
                break;
            case "Result":
                AudioManager.instance.audioSource.clip = AudioManager.instance.resultBGM;
                AudioManager.instance.audioSource.Play();
                break;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void SaveOptionData(int noteSpeed, float volume)
    {
        // 設定変更を適用
        ChangeNoteSpeed(noteSpeed);
        ChangeMusicVolume(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.SetInt("noteSpeed", noteSpeed);
    }
}