using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StageManager : MonoBehaviour
{
    public float bpm;
    
    public ObjectPoolManager poolManager;
    public AudioManager audioManager;
    public Transform[] Spawners;

    public int beatCnt;
    public int barCnt;
    public double secondPerBeat;
    public float startTime;
    public float lastBeatTime;
    bool flag=false;

    private void Start()
    {
        startTime = (float)AudioSettings.dspTime;
        lastBeatTime = startTime;
        secondPerBeat = 60 / bpm;
        MusicPlay();
    }

    private void Update()
    {
        if(AudioSettings.dspTime - lastBeatTime > (float)secondPerBeat)
        {
            beatCnt++;
            lastBeatTime += (float)secondPerBeat;
            if(beatCnt == 5)
            {
                barCnt++;
                beatCnt = 1;
            }
            MakeBeat();
        }
    }

    public void MakeBeat()
    {
        GameObject obj = poolManager.notePool.Get();
        Note note = obj.GetComponent<Note>();
        note.bpm = bpm;
        note.status = 1;
        note.transform.position = Spawners[1].transform.position;
        note.dirVec = Vector3.down;
        Debug.Log(AudioSettings.dspTime);
        //Vector3 randVec = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-1f, 0f), 0);
        //note.transform.position = Spawners[Random.Range(0, 3)].transform.position;
    }

    public void MusicPlay()
    {
        audioManager.MusicPlay();
    }
}
