using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private OnPlayUI onPlayUI;

    private NoteData crtNote;

    [SerializeField] private float spawnYPos; // ノーツが生成される位置のy座標

    [SerializeField] private float bpm; // 音楽の速さ
    [SerializeField] private int musicStartAfterBeats; // 音楽が始まる前の時間

    [SerializeField] private int beatCnt; // ビートのカウンター
    private float startTime; // audio systemが始まる時間
    private float lastBeatTime; // 最後のビートの時間

    public float secondPerBeat; //　1ビート当たりの時間。bpmで計算
    private int speedLogic;

    private static List<NoteData> noteList = new List<NoteData>();　// ノーツデータ

    private int index = 0;　
    private int crtIndex = 0;

    public bool isPause;
    private float pauseTimer; // 停止された時間を記録
    private int second3Timer;　//　停止を解除して再開するためのタイマー

    // プレイデータ
    private int score;
    private int combo;
    private int maxCombo;
    private int missCnt;

    [SerializeField] private float perfectRange, greatRange; // 各判定の基準。perfect：0.75, great：0.83
    [SerializeField] private int[] scoreList; // 各判定の点数。0:bad, １：great, ２：perfect
    [SerializeField] private int rankSStandard, rankAStandard, rankBStandard;　//　成績の基準。

    private void Start()
    {
        ReadyToStart();　//　初期化
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
                    AudioManager.instance.MusicPlay();
                }

                //　次のノーツが生成されるタイミングになったら生成
                while (noteList.Count > index 
                    && (beatCnt - musicStartAfterBeats + 1) == (noteList[index].bar - 1) * 4 + (int)noteList[index].beat - 1)
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
                    StartCoroutine(MakeNote(delayTime + secondPerBeat * 0.5f));
                    index++;
                }
            }
        }

        // 音楽が終わったら
        if(!isPause && !AudioManager.instance.audioSource.isPlaying && index >= noteList.Count)
        {
            GameManager.Instance.crtScore = score;
            GameManager.Instance.crtMiss = missCnt;
            GameManager.Instance.crtCombo = maxCombo;
            GameManager.Instance.crtRank = WhatisRank();
            GameManager.Instance.MoveScene("Result");
        }
    }

    private void ReadyToStart()　//　初期化
    {
        startTime = (float)AudioSettings.dspTime;　//　音楽システムが始まった時間を記録
        lastBeatTime = startTime;
        secondPerBeat = 60 / bpm;

        //　初期化
        score = 0;
        combo = 0;
        maxCombo = 0;
        missCnt = 0;
        second3Timer = 3;

        AudioManager.instance.MusicStop();
        AudioManager.instance.audioSource.loop = false;

        //　後に修正すべきなロジック
        switch (GameManager.Instance.crtSpeed)
        {
            case 0: speedLogic = 3; break;
            case 1: speedLogic = 1; break;
            case 2: speedLogic = 0; break;
        }

        dataManager.GetNotes("miles");　//　ノーツデータをロード
    }

    public void AddNoteList(NoteData noteData)
    {
        noteList.Add(noteData);
    }

    public void Pause()　//　音楽を停止
    {
        isPause = true;
        AudioManager.instance.MusicPause();

        // 停止された時間を記録
        pauseTimer = (float)AudioSettings.dspTime;

        // 生成された全てのノーツの動作を中止
        for (int i = 0; i < transform.childCount; i++)
        {
            Note note = transform.GetChild(i).GetComponent<Note>();
            note.status = 0;
        }
    }

    public void UnPause()　//　停止を解除
    {
        onPlayUI.OnOffTimer(true);
        for(int i=0; i<4; i++)
        {
            StartCoroutine(TimerUpdate(i));
        }
    }

    IEnumerator TimerUpdate(int time)　//　3秒後に再開
    {
        yield return new WaitForSeconds(time);

        onPlayUI.TimerUpdate(3 - time);
        second3Timer--;

        if (second3Timer < 0)
        {
            Resume();
            onPlayUI.OnOffTimer(false);
            second3Timer = 3;
        }
    }

    public void Resume()　//　停止を解除
    {
        isPause = false;

        // 停止されていた時間分、基準時間に反映
        lastBeatTime += (float)AudioSettings.dspTime - pauseTimer;
        AudioManager.instance.MusicResume();

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

    public void Grading(float distance)　//　ノーツと判定線間の距離を使い、成績を決める
    {
        if (distance == -1)　//　miss判定の場合
        {
            maxCombo = combo;
            combo = 0;
            missCnt++;
            onPlayUI.UpdateCombo(combo);
            onPlayUI.ChangeGradeText((int)distance);
            return;
        }

        int grade = DistanceToGrade(distance);
        onPlayUI.ChangeGradeText(grade);

        score += scoreList[grade];
        combo++;

        //　UIアップデート
        onPlayUI.UpdateScore(score);
        onPlayUI.UpdateCombo(combo);
    }

    private int DistanceToGrade(float distance)　//　距離
    {
        if (distance < perfectRange) // perfectで判定
        {
            return 2;
        }
        else if (distance < greatRange) // greatで判定
        {
            return 1;
        }
        else // badで判定
        {
            return 0;
        }
    }

    private string WhatisRank() //　プレイを評価
    {
        float rate = score / (scoreList[2] * noteList.Count);
        if (rate >= rankSStandard)
            return "S";
        else if (rate >= rankAStandard)
            return "A";
        else if (rate >= rankBStandard)
            return "B";
        else
            return "C";
    }
}