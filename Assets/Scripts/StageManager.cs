using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using TMPro;

public class StageManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private AudioManager audioManager;

    private NoteData crtNote;

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
        ReadyToStart();
        dataManager.GetNotes("miles");
    }

    
    private void Update()
    {
        if (!isPause)
        {
            // 以前ビートの時間から1ビートの時間が経ったらビートカウントを増加
            if (AudioSettings.dspTime - lastBeatTime > secondPerBeat)
            {
                beatCnt++;
                lastBeatTime += secondPerBeat;

                // 8ビート以降、準備時間が過ぎたら音楽を再生
                if (beatCnt == musicStartAfterBeats + 1)
                {
                    audioManager.MusicPlay();
                }

                //　次のノーツが生成されるタイミングになったら生成
                while (noteList.Count > index 
                    && (beatCnt - musicStartAfterBeats + speedLogic + 1) == (noteList[index].bar - 1) * 4 + (int)noteList[index].beat - 1)
                {
                    crtNote = noteList[index];
                    float delayTime; // ノーツの生成を遅延させる時間。（1/4音符は０、1/8音符は0.5)
                    if (noteList[index].beat == 1) // 1/4音符
                    {
                        delayTime = 0;
                    }
                    else // 1/8音符, 1/16音符
                    {
                        delayTime = (crtNote.beat - (int)crtNote.beat) * secondPerBeat * 0.5f;
                    }
                    StartCoroutine(MakeNote(delayTime));
                    index++;
                }
            }
        }

        // if there isn't note in noteList, and audio is not playing, show result.
        if(index == noteList.Count && !audioManager.audioSource.isPlaying)
        {
            checkResult();
        }
    }

    private void ReadyToStart()　//　初期化
    {
        startTime = (float)AudioSettings.dspTime;
        lastBeatTime = startTime;
        secondPerBeat = 60 / bpm;

        score = 0;
        combo = 0;
        maxCombo = 0;

        dataManager.GetMusic(GameManager.Instance.crtMusicName);

        switch (GameManager.Instance.crtSpeed)
        {
            case 0: speedLogic = 3; break;
            case 1: speedLogic = 1; break;
            case 2: speedLogic = 0; break;
        }
    }

    public void AddNoteList(NoteData noteData)
    {
        noteList.Add(noteData);
    }

    public void Pause()　//　音楽を一時停止
    {
        isPause = true;
        audioManager.MusicPause();

        // 一時停止された時間を記録
        pauseTimer = (float)AudioSettings.dspTime;

        // 生成された全てのノーツの動作を中止
        for (int i = 0; i < transform.childCount; i++)
        {
            Note note = transform.GetChild(i).GetComponent<Note>();
            note.status = 0;
        }
    }

    public void Resume()
    {
        isPause = false;

        // 停止されていた時間分、基準時間に反映
        lastBeatTime += (float)AudioSettings.dspTime - pauseTimer;
        audioManager.MusicResume();

        // ノーツの動作を再活性化
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

        //　ノーツを生成
        GameObject obj = poolManager.notePool.Get();
        Note note = obj.GetComponent<Note>();
        note.transform.parent = transform;
        note.stageManager = this;

        //　ノーツを活性化
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