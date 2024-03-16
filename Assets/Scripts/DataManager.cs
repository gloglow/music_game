using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    //　ファイルアドレス
    private static string mainPath = Application.streamingAssetsPath;
    private string musicListPath = mainPath + "/Json/";
    private string musicInfoPath = mainPath + "/Json/MusicInfo/";
    private string musicImagePath = mainPath + "/Images/";
    private string noteDataPath = mainPath + "/Json/NoteData/";
    private string musicPath = mainPath + "/Sounds/Music/";

    [SerializeField] private StageManager stageManager;
    public Image musicImage;

    private JsonData jsonData;　//　読み込んだjsonファイルのデータをを臨時に入れる変数

    public void ReadJsonData(string filePathAndName)　//　渡されたアドレスのjsonファイルを読み込む
    {
        filePathAndName = filePathAndName + ".json";
        if (File.Exists(filePathAndName))
        {
            var dataFile = File.ReadAllText(filePathAndName);
            jsonData = JsonMapper.ToObject(dataFile.ToString());
        }
        else Debug.Log(filePathAndName);
    }

    public List<string> GetMusicList()　//　音楽リストのデータを読み込む
    {
        ReadJsonData(musicListPath + "MusicList");
        List<string> musicList = new List<string>();
        for (int i = 0; i < jsonData.Count; i++)
        {
            MusicInfo musicInfo = new MusicInfo();
            musicList.Add(jsonData[i]["name"].ToString());
        }
        return musicList;
    }

    public MusicInfo GetMusicInfo(int musicID, string musicName)　//　音楽情報のデータを読み込む
    {
        ReadJsonData(musicInfoPath + musicName);
        MusicInfo musicData = new MusicInfo();
        musicData.ID = musicID;
        
        musicData.composer = jsonData["composer"].ToString();
        musicData.difficulty = int.Parse(jsonData["difficulty"].ToString());
        musicData.bpm = int.Parse(jsonData["bpm"].ToString());
        musicData.bestScore = int.Parse(jsonData["bestScore"].ToString());
        musicData.miss = int.Parse(jsonData["miss"].ToString());
        musicData.maxCombo = int.Parse(jsonData["maxCombo"].ToString());
        return musicData;
    }

    public void UpdateUserPlayData(MusicInfo musicInfo) //　プレイデータをアップデート
    {
        jsonData = JsonMapper.ToJson(musicInfo);
        File.WriteAllText(musicInfoPath + GameManager.Instance.crtMusicName + ".json", jsonData.ToString());
    }

    public void GetNotes(string songName)　//　ノーツデータを読み込む
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

    public IEnumerator GetMusic(string songTitle)　//　音楽ファイルを持ってくる
    {
        string songFilePath = musicPath + songTitle + ".mp3";
        using (UnityWebRequest www =
            UnityWebRequestMultimedia.GetAudioClip(songFilePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                AudioManager.instance.ChangeMusic(audioClip);
                AudioManager.instance.MusicPlay();
            }
            else
            {

            }

        }
    }
    /*
    public IEnumerator GetMusicImage(string songTitle)　//　音楽イメージを持ってくる
    {
        Texture texture = null;
        string songImagePath = musicImagePath + songTitle + ".jpg";
        using (UnityWebRequest www =
            UnityWebRequestTexture.GetTexture(songImagePath))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                texture = DownloadHandlerTexture.GetContent(www);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                musicImage.sprite = Sprite.Create((Texture2D)texture, rect, new Vector2(0.5f, 0.5f));
            }
            else
            {

            }

        }
    }*/
}
