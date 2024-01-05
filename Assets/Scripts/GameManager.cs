using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // Singleton

    [SerializeField] private VisualJudgeLine visualJudgeLine; // UI
    [SerializeField] private Vector2 idealScreenSize; // Resolution Reference : Apple iPhone 12
    public Vector3 lineStartPos, lineEndPos; // start point & end point of Lines (JudgeLine)

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
        // To make ideal line for every resolution
        float idealLinePoint = Screen.width * idealScreenSize.y / idealScreenSize.x;
        lineStartPos = Camera.main.ScreenToWorldPoint(new Vector2(0, idealLinePoint));
        lineEndPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, idealLinePoint));
        lineStartPos.z = 0;
        lineEndPos.z = 0;

        visualJudgeLine.DrawLine();
    }
}
