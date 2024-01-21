using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public StageManager stageManager;
    public OnPlayUI onPlayUI;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MusicPlay()
    {
        audioSource.Play();
    }

    public void MusicPause()
    {
        audioSource.Pause();
    }
}
