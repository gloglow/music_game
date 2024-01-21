using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using TMPro;
using UnityEngine.SceneManagement;

public class MusicSelecting : MonoBehaviour
{
    private string songDataFilePath = Application.streamingAssetsPath + "/Json/songList.json";

    [SerializeField] private GameObject prefab_song;
    [SerializeField] private int songListLength;
    [SerializeField] private GameObject songList;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI composerText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private GameObject unablePlayAlert;
    [SerializeField] private GameObject pointer;

    public int head;
    public int tail;

    public List<SongData> songDataList = new List<SongData>();
    
    private void Start()
    {
        LoadSongData();
        MakeSongList();
        ShowSongInfo(GameManager.Instance.crtSongID);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pointer.transform.Rotate(new Vector3(0, 0, -2f));
        }
        if(Input.GetMouseButtonUp(0))
        {
            pointer.transform.rotation = Quaternion.Euler(0, 0, 72f);
        }
    }

    private void LoadSongData()
    {
        if (File.Exists(songDataFilePath))
        {
            var songDataFile = File.ReadAllText(songDataFilePath);
            JsonData songD = JsonMapper.ToObject(songDataFile.ToString());
            for (int i = 0; i < songD[0].Count; i++)
            {
                SongData songData = new SongData();
                songData.songID = int.Parse(songD[0][i]["songID"].ToString());
                songData.title = songD[0][i]["title"].ToString();
                songData.composer = songD[0][i]["composer"].ToString();
                songData.difficulty = int.Parse(songD[0][i]["difficulty"].ToString());
                songData.bpm = int.Parse(songD[0][i]["bpm"].ToString());
                songDataList.Add(songData);
            }
        }
    }

    private void MakeSongList()
    {
        int centerID = PlayerPrefs.GetInt("selectedSong");
        for (int i = 0; i < songListLength; i++)
        {
            GameObject obj = Instantiate(prefab_song, songList.transform);
            obj.transform.localPosition = new Vector3(0, 1250f - (i * 250f), 0);
            
            SongList song = obj.GetComponent<SongList>();
            song.musicSelecting = this;
            int tmpID = centerID - songListLength / 2 + i;
            if(tmpID < 0)
            {
                tmpID = songDataList.Count + tmpID;
            }
            else if (tmpID >= songDataList.Count)
            {
                tmpID = tmpID - songDataList.Count;
            }
            song.ChangeInfo(tmpID, songDataList[tmpID].title);
            if(i == 0)
            {
                head = tmpID;
            }
            if(i == songListLength -1)
            {
                tail = tmpID;
            }
        }
    }

    public void IndexCalculating(SongList song, bool isTop)
    {
        int tmpID;
        if(isTop)
        {
            head = IndexChanger(head - 1);
            tail = IndexChanger(tail - 1);
            tmpID = head;
        }
        else
        {
            head = IndexChanger(head + 1);
            tail = IndexChanger(tail + 1);
            tmpID = tail;
        }
        song.ChangeInfo(tmpID, songDataList[tmpID].title);
    }

    private int IndexChanger(int index)
    {
        if (index < 0)
        {
            index = songDataList.Count + index;
        }
        else if (index >= songDataList.Count)
        {
            index = index - songDataList.Count;
        }

        return index;
    }

    public void ShowSongInfo(int index)
    {
        titleText.text = songDataList[index].title;
        composerText.text = "- Composer\n" + songDataList[index].composer;
        string str = "- Difficulty\n";
        for(int i = 0; i< songDataList[index].difficulty; i++) 
        {
            str += "O";
        }
        difficultyText.text = str;
    }

    public void StartPlay()
    {
        if(GameManager.Instance.crtSongID == 0)
        {
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            unablePlayAlert.SetActive(true);
        }
    }

    public void CloseAlertPanel()
    {
        unablePlayAlert.SetActive(false);
    }
}
