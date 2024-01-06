using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance; // Singleton

    [SerializeField] private Vector2 idealScreenSize; // Resolution Reference : Apple iPhone 12
    [SerializeField] private VisualJudgeLine visualJudgeLine;
    [SerializeField] private int lineRendererPosCnt;
    [SerializeField] private float lineOffset;
    private Vector3[] lineRendererPosArr;

    [SerializeField] private GameObject defaultUI;

    [SerializeField] private GameObject titleStart;
    [SerializeField] private GameObject titleOption;
    [SerializeField] private GameObject titleExit;
    [SerializeField] private GameObject btnStart;
    [SerializeField] private GameObject btnOption;
    [SerializeField] private GameObject btnExit;
    [SerializeField] private Image titleIcon;
    [SerializeField] private Sprite[] titleIcons;

    [SerializeField] private GameObject optionUI;
    [SerializeField] private TextMeshProUGUI noteSpeedText;

    public Vector3 lineStartPos, lineEndPos; // start point & end point of Lines (JudgeLine)

    public static UIManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
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
        ReadyToDrawLine();
        DrawLineAndPlaceUI();
        
    }

    public void ReadyToDrawLine()
    {
        // To make ideal line for every resolution
        float idealLinePoint = Screen.width * idealScreenSize.y / idealScreenSize.x;
        lineStartPos = Camera.main.ScreenToWorldPoint(new Vector2(0, idealLinePoint));
        lineEndPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, idealLinePoint));
        lineStartPos.z = 0;
        lineEndPos.z = 0;
        lineRendererPosArr = new Vector3[lineRendererPosCnt];
    }

    public void DrawLineAndPlaceUI()
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
        PlaceUI();

        visualJudgeLine.DrawLine(lineRendererPosCnt, lineRendererPosArr);
    }

    private void PlaceUI()
    {
        btnStart.transform.position = lineRendererPosArr[lineRendererPosCnt / 2];
        btnOption.transform.position = lineRendererPosArr[lineRendererPosCnt / 4 * 3];
        btnExit.transform.position = lineRendererPosArr[lineRendererPosCnt / 4];
        titleStart.transform.position = Camera.main.WorldToScreenPoint(lineRendererPosArr[lineRendererPosCnt / 2]);
        titleOption.transform.position = Camera.main.WorldToScreenPoint(lineRendererPosArr[lineRendererPosCnt / 4 * 3]);
        titleExit.transform.position = Camera.main.WorldToScreenPoint(lineRendererPosArr[lineRendererPosCnt / 4]);
    }

    public void TitleBtnClicked(string str)
    {
        switch (str)
        {
            case "StartBtn":
                GameManager.Instance.MoveScene("MusicSelect");
                break;
            case "OptionBtn":
                ShowOption();
                break;
            case "ExitBtn":
                GameManager.Instance.ExitGame();
                break;
        }
    }

    public void DrawOtherLine()
    {
        visualJudgeLine.DrawOtherLines();
    }

    public void ShowOption()
    {
        defaultUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void BackToDefault()
    {
        defaultUI.SetActive(true);
        optionUI.SetActive(false);
    }

    public void ChangeNoteSpeedText(float value)
    {
        noteSpeedText.text = value.ToString();
    }
}
