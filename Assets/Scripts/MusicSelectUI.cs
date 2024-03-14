using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MusicSelectUI : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    private List<string> musicList;
    [SerializeField] private Sprite[] imageList;

    [SerializeField] private TextMeshProUGUI musicInfo_Name;
    [SerializeField] private TextMeshProUGUI musicInfo_Composer;
    [SerializeField] private TextMeshProUGUI musicInfo_BestScore;
    [SerializeField] private TextMeshProUGUI musicInfo_MaxCombo;
    [SerializeField] private Image musicInfo_Image;

    [SerializeField] private TextMeshProUGUI musicList_Name;

    private int crtMusicIndex;

    private void Start()
    {
        LoadMusicList();
        musicList_Name.text = musicList[crtMusicIndex];
        ShowMusicInfo(0);
    }

    private void Update()
    {
        
    }

    private void LoadMusicList()
    {
        musicList = dataManager.GetMusicList();
    }

    private void ShowMusicInfo(int index)
    {
        MusicInfo musicInfo = dataManager.GetMusicInfo(index, musicList[crtMusicIndex]);
        musicInfo_Name.text = musicList[index];
        musicInfo_Composer.text = musicInfo.composer;
        musicInfo_BestScore.text = musicInfo.bestScore.ToString();
        musicInfo_MaxCombo.text = musicInfo.maxCombo.ToString();
        musicInfo_Image.sprite = imageList[index];
    }
}
