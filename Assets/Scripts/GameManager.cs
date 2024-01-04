using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public AudioManager audioManager;
    public VisualJudgeLine visualJudgeLine;

    public Vector2 idealScreenSize;
    public Vector3 lineStartPos, lineEndPos;

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
    }

    private void Start()
    {
        float screenWeight = Screen.height - (Screen.width * idealScreenSize.y / idealScreenSize.x);
        lineStartPos = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height - screenWeight));
        lineEndPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height - screenWeight));
        lineStartPos.z = 0;
        lineEndPos.z = 0;

        visualJudgeLine.DrawLine();
    }



    private void StartUIFinish()
    {
        //audioManager.MusicPlay();
    }
}
