using LitJson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEditor.Experimental.GraphView;

public class DataManager : MonoBehaviour
{
    private string musicListPath = Application.streamingAssetsPath + "/Json/";
    private string musicInfoPath = Application.streamingAssetsPath + "/Json/MusicInfo/";
    private string noteDataPath = Application.streamingAssetsPath + "/Json/NoteData/";
    private string musicPath = Application.streamingAssetsPath + "/Sounds/Music/";

    [SerializeField] private StageManager stageManager;
    [SerializeField] private AudioManager audioManager;

    private JsonData jsonData;


    public void ReadJsonData(string filePathAndName)
    {
        filePathAndName = filePathAndName + ".json";
        if (File.Exists(filePathAndName))
        {
            var dataFile = File.ReadAllText(filePathAndName);
            jsonData = JsonMapper.ToObject(dataFile.ToString());
        }
        else Debug.Log(filePathAndName);
    }

    public List<string> GetMusicList()
    {
        ReadJsonData(musicListPath + "MusicList");
        List<string> musicList = new List<string>();
        for (int i = 0; i < jsonData.Count; i++)
        {
            MusicInfo musicInfo = new MusicInfo();
            musicInfo.name = jsonData[i]["name"].ToString();
            musicList.Add(musicInfo.name);
        }
        return musicList;
    }

    public MusicInfo GetMusicInfo(int musicID, string musicName)
    {
        ReadJsonData(musicInfoPath + musicName);
        MusicInfo musicData = new MusicInfo();
        musicData.ID = musicID;
        musicData.composer = jsonData[0]["composer"].ToString();
        musicData.difficulty = int.Parse(jsonData[0]["difficulty"].ToString());
        musicData.bpm = int.Parse(jsonData[0]["bpm"].ToString());
        musicData.bestScore = int.Parse(jsonData[0]["bestScore"].ToString());
        musicData.maxCombo = int.Parse(jsonData[0]["maxCombo"].ToString());
        return musicData;
    }

    public void GetNotes(string songName)
    {
        ReadJsonData(noteDataPath + songName);
        for (int i = 0; i < jsonData.Count; i++)
        {
            NoteData noteData = new NoteData();
            noteData.xPos = float.Parse(jsonData[i]["xPos"].ToString());
            noteData.bar = int.Parse(jsonData[i]["bar"].ToString());
            noteData.beat = float.Parse(jsonData[i]["beat"].ToString());
            noteData.unitVecX = float.Parse(jsonData[i]["unitVecX"].ToString());
            noteData.unitVecY = float.Parse(jsonData[i]["unitVecY"].ToString());

            stageManager.AddNoteList(noteData);
        }
    }

    public IEnumerator GetMusic(string songTitle)
    {
        //change music and play. (if fail to find music, music stop.)
        string songFilePath = musicPath + songTitle + ".mp3";
        using (UnityWebRequest www =
            UnityWebRequestMultimedia.GetAudioClip(songFilePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                audioManager.ChangeMusic(audioClip);
            }
            else
            {
                
            }

        }
    }
}
