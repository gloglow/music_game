using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using LitJson;
using Unity.VisualScripting;
using System;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class StageManager : MonoBehaviour
{
    // managing a stageUI, music, note.


    [SerializeField] private AudioManager audioManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Transform[] Spawners; // positions where notes are activated.
    [SerializeField] private TextMeshProUGUI gradeText; // UI text of grade.

    [SerializeField] private float bpm; // music bpm.
    public float userSpeed; // speed set by user.
    [SerializeField] private int musicStartAfterBeats; // the number of initial beats. set 8.
    public bool flag; // music play start flag.
    public bool isPause;

    [SerializeField] private int beatCnt; // counting beat.
    private float startTime; // start timing of audio system. 
    public float lastBeatTime; // timing of last beat.

    public float secondPerBeat; // second per beat. calculated by bpm.

    // note data.
    private static List<NoteData> noteList = new List<NoteData>();
    private string title;
    private string composer;
    private int index = 0;
    private int crtIndex = 0;

    private float pauseTimer;


    private void Start()
    {
        startTime = (float)AudioSettings.dspTime;
        lastBeatTime = startTime;
        secondPerBeat = 60 / bpm;
        LoadNoteData();
    }

    private void LoadNoteData()
    {
        // load json file that have note data.
        if(File.Exists(Application.dataPath+ "/Resources/JSON/miles.json"))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/JSON/miles.json");
            JsonData noteD = JsonMapper.ToObject(jsonStr);

            title = noteD[0].ToString();
            composer = noteD[1].ToString();
            for (int i=0; i < int.Parse(noteD[2].ToString()); i++)
            {
                // load notedata and add in notedata list.
                NoteData noteData = new NoteData();
                noteData.beat = float.Parse(noteD[3][i]["beat"].ToString());
                noteData.x = float.Parse(noteD[3][i]["x"].ToString());
                noteData.y = float.Parse(noteD[3][i]["y"].ToString());
                noteList.Add(noteData);
            }
        }
        
    }

    private void Update()
    {
        if (!isPause)
        {
            // count beat because note should be activated and move with beat timing.
            if (AudioSettings.dspTime - lastBeatTime > secondPerBeat) // if over one beat time passed from last beat time, count beat.
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
                {
                    // if there is note, and it is the time when note should be created,
                    while (noteList.Count > index && beatCnt - musicStartAfterBeats == (int)noteList[index].beat)
                    {
                        if (noteList[index].beat - (beatCnt - musicStartAfterBeats) == 0)
                        {
                            MakeNote();
                        }
                        else
                        {
                            Invoke("MakeNote", (noteList[index].beat - (beatCnt - musicStartAfterBeats)) * secondPerBeat);
                        }
                        index++;
                    }
                }
            }
        }
    }

    public void Pause()
    {
        isPause = true;
        audioManager.MusicPause();
        pauseTimer = (float)AudioSettings.dspTime;
        int childCnt = transform.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Note note = transform.GetChild(i).GetComponent<Note>();
            note.status = 0;
        }
    }

    public void PlayBack()
    {
        isPause = false;
        lastBeatTime += (float)AudioSettings.dspTime - pauseTimer;
        audioManager.MusicPlay();
        int childCnt = transform.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Note note = transform.GetChild(i).GetComponent<Note>();
            note.initialTime += (float)AudioSettings.dspTime - pauseTimer;
            note.status = 2;
        }
    }
    
    public void MakeNote()
    {
        // bring note from note object pool.
        GameObject obj = ObjectPoolManager.Instance.notePool.Get();
        Note note = obj.GetComponent<Note>();
        note.transform.parent = transform;

        // activate note.
        note.status = 1;
        note.transform.position = Spawners[1].transform.position;
        note.dirVec = new Vector3(noteList[crtIndex].x, noteList[crtIndex].y, 0);
        crtIndex++;
        if (isPause) note.status = 0;
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
