using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private static StageManager instance; 
    public float bpm;
    
    public AudioManager audioManager;
    public Transform[] Spawners;

    public int beatCnt;
    public int barCnt;
    public float secondPerBeat;
    public float startTime;
    public float lastBeatTime;
    bool flag=false;

    public Text gradeText;

    public static StageManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(StageManager)) as StageManager;

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
        flag = false;
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
            if (barCnt == 1 && beatCnt == 4)
                flag = true;
            if (flag)
                MakeBeat();
        }
    }

    public void MakeBeat()
    {
        GameObject obj = ObjectPoolManager.Instance.notePool.Get();
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

    public void ShowGrade(int grade)
    {
        switch (grade)
        {
            case 0:
                gradeText.text = "Miss"; break;
            case 1:
                gradeText.text = "Bad"; break;
            case 2:
                gradeText.text = "Good"; break;
            case 3:
                gradeText.text = "Great"; break;
            case 4:
                gradeText.text = "Perfect"; break;
        }
    }
}
