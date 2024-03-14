using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    // manage audio play and stop

    private string musicPath = Application.streamingAssetsPath + "/Sounds/Music/"; // music file path. 
    
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
