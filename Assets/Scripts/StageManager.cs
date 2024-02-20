using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using TMPro;

public class StageManager : MonoBehaviour
{
    private string noteDataFilePath = Application.streamingAssetsPath + "/Json/miles.json";

    // managing a stageUI, note.
    [SerializeField] private OnPlayUI onPlayUI;
    [SerializeField] private float spawnYPos; // positionY where notes are activated.
    [SerializeField] private TextMeshProUGUI gradeText; // UI text of grade.

    [SerializeField] private float bpm; // music bpm.
    [SerializeField] private int musicStartAfterBeats; // the number of initial beats. set 8.
    public bool isPause;

    [SerializeField] private int beatCnt; // counting beat.
    private float startTime; // start timing of audio system. 
    private float lastBeatTime; // timing of last beat.

    public float secondPerBeat; // second per beat. calculated by bpm.
    private int speedLogic;

    // note data.
    private static List<NoteData> noteList = new List<NoteData>();
    private string title;
    private string composer;
    private int index = 0;
    private int crtIndex = 0;

    private float pauseTimer; // save the time when pause button is pressed.

    // play data.
    private int score;
    private int combo;
    private int maxCombo;

    // standards of grading.
    [SerializeField] private int perfectScore, greatScore, badScore, missScore;
    [SerializeField] private int rankSStandard, rankAStandard, rankBStandard;

    private void Start()
    {
        startTime = (float)AudioSettings.dspTime;
        lastBeatTime = startTime;
        secondPerBeat = 60 / bpm;
        
        // load note data from json file.
        LoadNoteData();

        // initialize play data.
        score = 0;
        combo = 0;
        maxCombo = 0;

        switch(GameManager.Instance.crtSpeed)
        {
            case 0: speedLogic = 3; break;
            case 1: speedLogic = 1; break;
            case 2: speedLogic = 0; break;
        }
    }

    private void LoadNoteData()
    {
        // load json file
        // json file [title, composer, note count, notedata[startpoint, directionx, directiony]

        if (File.Exists(noteDataFilePath))
        {
            var noteDataFile = File.ReadAllText(noteDataFilePath);
            JsonData noteD = JsonMapper.ToObject(noteDataFile.ToString());
            for (int i = 0; i < noteD.Count; i++)
            {
                // load notedata and add in notedata list.
                NoteData noteData = new NoteData();
                noteData.xPos = float.Parse(noteD[i]["xPos"].ToString());
                noteData.bar = int.Parse(noteD[i]["bar"].ToString());
                noteData.beat = float.Parse(noteD[i]["beat"].ToString());
                noteData.unitVecX = float.Parse(noteD[i]["unitVecX"].ToString());
                noteData.unitVecY = float.Parse(noteD[i]["unitVecY"].ToString());
                noteList.Add(noteData);
            }
        }
        else Debug.Log("????");
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

                // after first 8 beats, music start.
                if (beatCnt == musicStartAfterBeats + 1)
                {
                    AudioManager.Instance.MusicPlay();
                }

                // there is any note data && current beat == note beat, make note.
                while (noteList.Count > index && (beatCnt - musicStartAfterBeats + speedLogic + 1) == (int)noteList[index].beat)
                {
                    float f; // the waiting time of note. (if note is 1/4 note, not wait.)
                    if (noteList[index].beat == (beatCnt - musicStartAfterBeats + speedLogic)) // 1/4 note.
                    {
                        f = 0;
                    }
                    else // 1/8 note, 1/16 note, etc
                    {
                        // if beat of a note is 6.5, at 6th beat, wait 0.5beat time and be made.
                        f = (noteList[index].beat - (beatCnt - musicStartAfterBeats + speedLogic)) * secondPerBeat * GameManager.Instance.speeds[GameManager.Instance.crtSpeed] * 0.5f;
                    }
                    StartCoroutine(MakeNote(f));
                    index++;
                }
            }
        }

        // if there isn't note in noteList, and audio is not playing, show result.
        if(index == noteList.Count && !AudioManager.Instance.audioSource.isPlaying)
        {
            checkResult();
        }
    }
    
    public void Pause() // when pause button is pressed in playing time.
    {
        isPause = true;
        AudioManager.Instance.MusicPause();

        // record the time when pause button is pressed.
        pauseTimer = (float)AudioSettings.dspTime;

        // all of notes stop when pause button is pressed.
        for (int i = 0; i < transform.childCount; i++)
        {
            Note note = transform.GetChild(i).GetComponent<Note>();
            note.status = 0;
        }
    }

    public void PlayBack()
    {
        // unpause.
        isPause = false;

        // add the time passed during pause to lastBeatTime.
        lastBeatTime += (float)AudioSettings.dspTime - pauseTimer;
        AudioManager.Instance.MusicPlay();

        // reactivate notes.
        int childCnt = transform.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Note note = transform.GetChild(i).GetComponent<Note>();
            note.initialTime += (float)AudioSettings.dspTime - pauseTimer;
            note.status = 2;
        }
    }

    IEnumerator MakeNote(float time)
    {
        yield return new WaitForSeconds(time);  
        // bring note from note object pool.
        GameObject obj = ObjectPoolManager.Instance.notePool.Get();
        Note note = obj.GetComponent<Note>();
        note.transform.parent = transform;
        note.stageManager = this;

        // activate note.
        note.status = 1;
        note.transform.position = new Vector3(noteList[crtIndex].xPos, spawnYPos, 0);
        note.dirVec = new Vector3(noteList[crtIndex].unitVecX, noteList[crtIndex].unitVecY, 0);
        crtIndex++;
        if (isPause) note.status = 0;
    }

    public void ShowGrade(int grade)
    {
        // display grade and combo.
        switch (grade)
        {
            case 0:
                gradeText.text = "Miss";
                maxCombo = combo;
                combo = 0;
                onPlayUI.updateCombo(combo);
                grade = missScore;
                return;
            case 1:
                gradeText.text = "Bad";
                grade = badScore;
                break;
            case 2:
                gradeText.text = "Great";
                grade = greatScore;
                break;
            case 3:
                gradeText.text = "Perfect";
                grade = perfectScore;
                break;
        }
        score += grade;
        combo++;
        onPlayUI.updateScore(score);
        onPlayUI.updateCombo(combo);
    }

    private void checkResult()
    {
        // show result (rank, maxcombo, score)
        char rank;
        float rate = (score / 5) / noteList.Count;
        
        if (rate > rankSStandard) rank = 'S';
        else if (rate > rankAStandard) rank = 'A';
        else if (rate > rankBStandard) rank = 'B';
        else rank = 'C';

        onPlayUI.ShowResultUI(maxCombo, score, rank);
        isPause = true;
    }
}