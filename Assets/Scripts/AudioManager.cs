using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    // manage audio play and stop

    private static AudioManager instance; // singleton.

    private string songFolderPath = Application.streamingAssetsPath + "/Sounds/Music/"; // music file path. 
    
    public AudioSource audioSource;

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
    }

    public void MusicPlay()
    {
        audioSource.Play();
    }

    public IEnumerator ChangeMusic(string songTitle)
    {
        //change music and play. (if fail to find music, music stop.)
        string songFilePath = songFolderPath + songTitle + ".mp3";
        using(UnityWebRequest www = 
            UnityWebRequestMultimedia.GetAudioClip(songFilePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if(www.error == null)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
            
        }
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
