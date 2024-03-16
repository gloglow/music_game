using LitJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    [SerializeField] private Sprite[] imageList;　//　音楽のイメージ

    //　音楽情報パネル
    [SerializeField] private TextMeshProUGUI musicInfo_Name;
    [SerializeField] private TextMeshProUGUI musicInfo_Composer;
    [SerializeField] private TextMeshProUGUI textRank;

    [SerializeField] private TextMeshProUGUI musicInfo_CrtScore;
    [SerializeField] private TextMeshProUGUI musicInfo_BestScore;

    [SerializeField] private TextMeshProUGUI musicInfo_CrtMiss;
    [SerializeField] private TextMeshProUGUI musicInfo_BestMiss;

    [SerializeField] private TextMeshProUGUI musicInfo_MaxCombo;

    [SerializeField] private Image musicInfo_Image;

    private MusicInfo musicInfo;

    private void Start()
    {
        ShowMusicInfo();
        if(musicInfo.bestScore < GameManager.Instance.crtScore)
        {
            SaveRecord();
        }
    }

    private void ShowMusicInfo()　//　data　managerを通して音楽情報を持ってくる
    {
        musicInfo = dataManager.GetMusicInfo(GameManager.Instance.crtMusicIndex, GameManager.Instance.crtMusicName);

        musicInfo_Name.text = GameManager.Instance.crtMusicName;
        musicInfo_Composer.text = musicInfo.composer;
        textRank.text = GameManager.Instance.crtRank;

        musicInfo_CrtScore.text = GameManager.Instance.crtScore.ToString();
        musicInfo_BestScore.text = musicInfo.bestScore.ToString();

        musicInfo_CrtMiss.text = GameManager.Instance.crtMiss.ToString();

        if (musicInfo.miss == -1)　//　プレイ記録がない場合
            musicInfo_BestMiss.text = string.Empty;
        else
            musicInfo_BestMiss.text = musicInfo.miss.ToString();

        musicInfo_MaxCombo.text = GameManager.Instance.crtCombo.ToString();
        musicInfo_Image.sprite = imageList[GameManager.Instance.crtMusicIndex];
    }

    private void SaveRecord()　//　プレイ記録が以前のベストデータより良い場合、アップデート
    {
        musicInfo.bestScore = GameManager.Instance.crtScore;
        musicInfo.miss = GameManager.Instance.crtMiss;
        musicInfo.maxCombo = GameManager.Instance.crtCombo;
        
        dataManager.UpdateUserPlayData(musicInfo);
    }

    public void MoveScene(string sceneName)
    {
        GameManager.Instance.MoveScene(sceneName);
    }
}
