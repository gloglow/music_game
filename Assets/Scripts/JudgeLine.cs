using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
    public TouchArea touchArea;
    public RealJudgeLine realJudgeLine;
    public Transform noteSign;
    public Transform spawner;

    private LineRenderer lineRenderer;
    public float lineOffset;
    public Vector2 idealScreen;
    public Vector3 startPos, endPos;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        float screenWeight = Screen.height - (Screen.width * idealScreen.y / idealScreen.x);
        startPos = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height - screenWeight));
        endPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height - screenWeight));

        startPos.z = 0;
        endPos.z = 0;
        Vector3 stPos, edPos, center;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            stPos = startPos;
            edPos = endPos;
            center = (stPos + edPos) * 0.5f;
            center.y += lineOffset;
            stPos = stPos - center;
            edPos = edPos - center;
            Vector3 point = Vector3.Slerp(stPos, edPos, i / (float)(lineRenderer.positionCount - 1));
            point += center;
            lineRenderer.SetPosition(i, point);
        }

        int tmp;
        Vector3[] tmpArr = new Vector3[lineRenderer.positionCount];
        tmp = lineRenderer.GetPositions(tmpArr);
        DrawTouchArea(tmp, tmpArr);

        spawner.position = new Vector3(0, startPos.y * 2.4f, 0);
        noteSign.position = new Vector3 (0, startPos.y * 0.8f, 0);
    }
   
    void DrawTouchArea(int cnt, Vector3[] posList)
    {
        touchArea.Draw(cnt, posList);
        realJudgeLine.Draw(cnt, posList);
    }
}
