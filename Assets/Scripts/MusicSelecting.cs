using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using TMPro;
using UnityEngine.SceneManagement;

public class MusicSelecting : MonoBehaviour
{
    // music select scene UI.

    private string songDataFilePath = Application.streamingAssetsPath + "/Json/songList.json"; // song list json file.

    public List<SongData> songDataList = new List<SongData>();

    [SerializeField] private int songListLength; // the number of song object to display. 10 is best.
    [SerializeField] private GameObject songList; // parent of song object.

    // UI.
    [SerializeField] private GameObject prefab_song;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI composerText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private GameObject unablePlayAlert;
    [SerializeField] private GameObject pointer;

    // song index value for display.
    public int head, tail;
    
    private void Start()
    {
        LoadSongData(); // load data from json file.
        MakeSongList(); // make song objects.
        ShowSongInfo(GameManager.Instance.crtSongID); // (left) show song info.
    }

    private void Update()
    {
        // pointer UI.
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
        // load the song player last played.
        int centerID = PlayerPrefs.GetInt("selectedSong");

        // make song objects.
        for (int i = 0; i < songListLength; i++)
        {
            GameObject obj = Instantiate(prefab_song, songList.transform);
            obj.transform.localPosition = new Vector3(0, 1250f - (i * 250f), 0);
            
            SongList song = obj.GetComponent<SongList>();
            song.musicSelecting = this;
            song.songCnt = songDataList.Count;

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
        // when song object moved out of range
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
        GameManager.Instance.crtSongTitle = songDataList[index].title;
        StartCoroutine(AudioManager.Instance.ChangeMusic(songDataList[index].title));
        composerText.text = "- Composer\n" + songDataList[index].composer;
        string str = "- Difficulty\n";
        for(int i = 0; i< songDataList[index].difficulty; i++) 
        {
            str += "O";
        }
        difficultyText.text = str;
    }

    public void StartPlay(string str)
    {
        if(GameManager.Instance.crtSongID == 0)
        {
            GameManager.Instance.MoveScene(str);
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
