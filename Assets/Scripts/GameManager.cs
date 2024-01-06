using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // Singleton

    [SerializeField] private UIManager uiManager;
    public AudioMixer audioMixer;
    public Slider musicVolumeSlider;
    public Slider noteSpeedSlider;

    public float noteSpeed;

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

    private void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("Title")
            || SceneManager.GetActiveScene().name.Equals("MusicSelect"))
        {
            Vector3 pos;
            if (Input.GetMouseButtonDown(0))
            {
                pos = Input.mousePosition;
            }
            else if (Input.touchCount > 0)
            {
                pos = Input.GetTouch(0).position;
            }
            else pos = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                string name = rayHit.collider.name;
                uiManager.TitleBtnClicked(name);
            }
        }
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowOptionUI()
    {
        uiManager.ShowOption();
    }

    public void BackToDefaultUI()
    {
        uiManager.BackToDefault();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeMusicVolume()
    {
        // control music volume by slider.
        // value range : -40 ~ 0
        float value = musicVolumeSlider.value;

        if (value == -40f)
        {
            // ~-40f : almost can not hear
            audioMixer.SetFloat("Music", -80);
        }
        else
        {
            audioMixer.SetFloat("Music", value);
        }
    }

    public void ChangeNoteSpeed()
    {
        // control note speed by slider.
        // 0 (0.5x) ~ 1 (1x)
        float value = noteSpeedSlider.value;

        switch (value)
        {
            case 0:
                noteSpeed = 0.5f;
                break;
            case 1:
                noteSpeed = 1f;
                break;
        }

        uiManager.ChangeNoteSpeedText(noteSpeed);
    }
}
