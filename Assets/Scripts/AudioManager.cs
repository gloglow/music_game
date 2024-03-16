using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;¡¡//¡¡«·«ó«°«ë«È«ó

    public AudioSource audioSource;

    public AudioClip defaultBGM;
    public AudioClip resultBGM;

    public static AudioManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;

                if (instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
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
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = defaultBGM;
        audioSource.Play();
    }

    public void ChangeMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }

    public void MusicPlay()
    {
        audioSource.Play();
    }

    public void MusicResume()
    {
        audioSource.UnPause();
    }

    public void MusicPause()
    {
        audioSource.Pause();
    }

    public void MusicStop()
    {
        audioSource.Stop();
    }
}
