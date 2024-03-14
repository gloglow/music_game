using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // «·«ó«°«ë«È«ó

    public AudioMixer audioMixer;

    public int crtSpeed; // 0,1,2 : slider value
    public float[] speeds = {0.5f, 1f, 2f}; // 0.5,1,2 : actual speed = speeds[crtSpeed]

    public string crtMusicName;

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
        crtMusicName = "titleMusic";
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
        // volume less than -40, almost mute.
        if (value == -40)
            value = -80;
        audioMixer.SetFloat("Music", value);
    }

    public void ChangeNoteSpeed(int value)
    {
        crtSpeed = value;
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SaveOptionData(int noteSpeed, float volume)
    {
        // change current game option and save data.
        ChangeNoteSpeed(noteSpeed);
        ChangeMusicVolume(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.SetInt("noteSpeed", noteSpeed);
    }
}