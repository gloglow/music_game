using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    // manage user information
    private static GameManager instance; // Singleton

    [SerializeField] private Vector2 idealScreenSize; // Resolution Reference : Apple iPhone 12
    public Vector3 lineStartPos, lineEndPos; // start point & end point of Lines (JudgeLine)
    [SerializeField] private int lineRendererPosCnt;
    [SerializeField] private float lineOffset;
    public Vector3[] lineRendererPosArr; // objects that use line renderer draw line from this array

    public AudioMixer audioMixer;

    public int crtSpeed; // 0,1,2 : slider value
    public float[] speeds = {0.5f, 1f, 2f}; // 0.5,1,2 : actual speed = speeds[crtSpeed]

    public int crtSongID;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if(instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        LoadPlayerData();
        ReadyToDrawLine();
        GetLinePoint();
    }

    private void LoadPlayerData()
    {
        // when game starts, load player data (music volume, note speed, etc)
        float volume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : -20f;
        audioMixer.SetFloat("Music", volume);
        crtSpeed = PlayerPrefs.HasKey("noteSpeed") ? PlayerPrefs.GetInt("noteSpeed") : 1;

        if (!PlayerPrefs.HasKey("selectedSong"))
        {
            PlayerPrefs.SetInt("selectedSong", 0);
        }
        crtSongID = PlayerPrefs.GetInt("selectedSong");
    }

    public void ReadyToDrawLine()
    {
        lineRendererPosArr = new Vector3[lineRendererPosCnt];

        // To make ideal line for every resolution
        float idealLinePoint = Screen.width * idealScreenSize.y / idealScreenSize.x;
        lineStartPos = Camera.main.ScreenToWorldPoint(new Vector2(0, idealLinePoint));
        lineEndPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, idealLinePoint));
        lineStartPos.z = 0;
        lineEndPos.z = 0;
    }

    public void GetLinePoint()
    {
        // variables for drawing line
        Vector3 stPos, edPos, center;

        // draw parabola with linerenderer and Slerp. 
        for (int i = 0; i < lineRendererPosCnt; i++)
        {
            stPos = lineStartPos;
            edPos = lineEndPos;
            center = (stPos + edPos) * 0.5f;
            center.y += lineOffset;
            stPos = stPos - center;
            edPos = edPos - center;
            Vector3 point = Vector3.Slerp(stPos, edPos, i / (float)(lineRendererPosCnt - 1));
            point += center;
            lineRendererPosArr[i] = point;
        }
    }

    public void ChangeMusicVolume(float value)
    {
        // volume less than -40, almost mute.
        if (value == -40)
            value = -80;
        audioMixer.SetFloat("Music", value);
    }

    public void ChangeNoteSpeed(int value)
    {
        crtSpeed = value;
    }

    public void SaveOptionData(int noteSpeed, float volume)
    {
        // change current game option and save data.
        ChangeNoteSpeed(noteSpeed);
        ChangeMusicVolume(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.SetInt("noteSpeed", noteSpeed);
    }
}
