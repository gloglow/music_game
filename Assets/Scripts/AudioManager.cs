using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

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
