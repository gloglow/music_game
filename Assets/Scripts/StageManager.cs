using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StageManager : MonoBehaviour
{
    public int bpm;
    
    public ObjectPoolManager poolManager;
    public AudioManager audioManager;
    public Transform spawner;
    public Transform noteSign;
    public int j=0;

    public int beatCnt;
    public int barCnt;
    public double secondPerBeat;


    private void Start()
    {
        secondPerBeat = (double)60 / (double)bpm;
        Invoke("PlayStart", (float)secondPerBeat * 4f);
        StartCoroutine("myCoroutine", secondPerBeat);
    }

    public IEnumerator myCoroutine(double secondPerBeat)
    {
        WaitForSeconds waitForSec = new WaitForSeconds((float)secondPerBeat);

        while (true)
        {
            GameObject obj = poolManager.notePool.Get();
            Note note = obj.GetComponent<Note>();
            note.bpm = bpm;
            note.status = 1;
            note.destination = noteSign.transform.position;
            note.dirVec = new Vector3(0, -1, 0);
            note.transform.position = noteSign.transform.position;
            yield return waitForSec;
        }
        
            
    }

    private void PlayStart()
    {
        audioManager.MusicPlay();
    }
}
