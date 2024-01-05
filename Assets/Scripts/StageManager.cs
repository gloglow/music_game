using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using LitJson;
using Unity.VisualScripting;

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

    public static List<NoteData> noteList = new List<NoteData>();
    JsonData noteJson;
    public string title;
    public string composer;
    public int index = 0;
    public int crtIndex = 0;
    float musicStartTime;

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
        LoadNoteData();
    }

    private void LoadNoteData()
    {
        if(File.Exists(Application.dataPath+ "/Resources/JSON/miles.json"))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/JSON/miles.json");
            JsonData noteD = JsonMapper.ToObject(jsonStr);

            title = noteD[0].ToString();
            composer = noteD[1].ToString();

            for (int i=0; i<12; i++)
            {
                
                NoteData noteData = new NoteData();
                noteData.beat = float.Parse(noteD[2][i]["beat"].ToString());
                noteData.x = float.Parse(noteD[2][i]["x"].ToString());
                noteData.y = float.Parse(noteD[2][i]["y"].ToString());
                noteList.Add(noteData);
            }
        }
        
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
                musicStartTime = (float)AudioSettings.dspTime;
                flag = true;
            }

            if (flag)
            {    
                while (beatCnt - musicStartAfterBeats == (int)noteList[index].beat)
                {
                    if(noteList[index].beat - (beatCnt - musicStartAfterBeats) == 0)
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

    public void MakeNote()
    {
        // bring note from note object pool.
        GameObject obj = ObjectPoolManager.Instance.notePool.Get();
        Note note = obj.GetComponent<Note>();

        // activate note.
        note.status = 1;
        note.transform.position = Spawners[1].transform.position;
        note.dirVec = new Vector3(noteList[crtIndex].x, noteList[crtIndex].y, 0);
        crtIndex++;
        //note.dirVec = Vector3.down;
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
