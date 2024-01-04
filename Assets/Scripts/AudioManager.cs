using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public bool flag = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MusicPlay()
    {
        flag = true;
        audioSource.Play();
    }
}
