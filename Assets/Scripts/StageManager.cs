using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    // managing a stageUI, music, note.
    // singleton.

    private static StageManager instance;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Transform[] Spawners; // positions where notes are activated.
    [SerializeField] private Text gradeText; // UI text of grade.

    [SerializeField] private float bpm; // music bpm.
    public float userSpeed; // speed set by user.
    [SerializeField] private int musicStartAfterBeats; // the number of initial beats. set 8.
    [SerializeField] private bool flag; // music play start flag.

    [SerializeField] private int beatCnt; // counting beat.
    private float startTime; // start timing of audio system. 
    private float lastBeatTime; // timing of last beat.

    public float secondPerBeat; // second per beat. calculated by bpm.

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
        startTime = (float)AudioSettings.dspTime;
        lastBeatTime = startTime;
        secondPerBeat = 60 / bpm;
        //Invoke("MusicPlay", secondPerBeat * musicStartAfterBeats);
    }

    private void Update()
    {
        // count beat because note should be activated and move with beat timing.
        if(AudioSettings.dspTime - lastBeatTime > secondPerBeat) // if over one beat time passed from last beat time, count beat.
        {
            beatCnt++;
            lastBeatTime += secondPerBeat;

            // after initial beats, music start and make note.
            if (beatCnt == musicStartAfterBeats + 1)
            {
                MusicPlay();
                flag = true;
            }

            if (flag)
                MakeNote();
        }
    }

    public void MakeNote()
    {
        // bring note from note object pool.
        GameObject obj = ObjectPoolManager.Instance.notePool.Get();
        Note note = obj.GetComponent<Note>();

        // activate note.
        note.status = 1;
        note.transform.position = Spawners[1].transform.position;
        note.dirVec = Vector3.down;
    }

    public void MusicPlay()
    {
        audioManager.MusicPlay();
    }

    public void ShowGrade(int grade)
    {
        // display grade.
        switch (grade)
        {
            case 0:
                gradeText.text = "Miss"; break;
            case 1:
                gradeText.text = "Bad"; break;
            case 2:
                gradeText.text = "Great"; break;
            case 3:
                gradeText.text = "Perfect"; break;
        }
    }
}
