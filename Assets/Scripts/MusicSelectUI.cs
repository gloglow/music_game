using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MusicSelectUI : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    public List<string> musicList;　//　音楽の名前のリスト
    [SerializeField] private Sprite[] imageList;　//　音楽のイメージ

    //　音楽情報パネル
    [SerializeField] private TextMeshProUGUI musicInfo_Name;
    [SerializeField] private TextMeshProUGUI musicInfo_Composer;
    [SerializeField] private TextMeshProUGUI musicInfo_BestScore;
    [SerializeField] private TextMeshProUGUI musicInfo_MaxCombo;
    [SerializeField] private Image musicInfo_Image;

    //　音楽選択
    [SerializeField] private TextMeshProUGUI musicList_Name;

    //　現在選択されている音楽のインデックス
    private int crtMusicIndex;

    private void Start()
    {
        LoadMusicList();
        crtMusicIndex = 0;
        musicList_Name.text = musicList[crtMusicIndex];
        ShowMusicInfo(crtMusicIndex);
        SelectMusic(crtMusicIndex);
    }

    private void LoadMusicList()　//　data managerを通して音楽リストを作る
    {
        musicList = dataManager.GetMusicList();
    }

    private void ShowMusicInfo(int index)　//　data　managerを通して音楽情報を持ってくる
    {
        MusicInfo musicInfo = dataManager.GetMusicInfo(index, musicList[crtMusicIndex]);
        musicInfo_Name.text = musicList[index];
        musicInfo_Composer.text = musicInfo.composer;
        musicInfo_BestScore.text = musicInfo.bestScore.ToString();
        musicInfo_MaxCombo.text = musicInfo.maxCombo.ToString();
        musicInfo_Image.sprite = imageList[index];
    }

    private void SelectMusic(int index)
    {
        crtMusicIndex = index;
        StartCoroutine(dataManager.GetMusic(musicList[crtMusicIndex]));
        AudioManager.instance.MusicPlay();
    }

    private void ChangeMusic()
    {

    }

    public void PlaySelectedMusic()
    {
        GameManager.Instance.crtMusicIndex = crtMusicIndex;
        GameManager.Instance.crtMusicName = musicList[crtMusicIndex];
    }
}
