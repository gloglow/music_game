using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualJudgeLine : MonoBehaviour
{
    public TouchArea touchArea;
    public RealJudgeLine realJudgeLine;
    public Transform noteSign;
    public Transform spawner;

    private LineRenderer lineRenderer;
    public float lineOffset;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void DrawLine()
    {
        Vector3 stPos, edPos, center;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            stPos = GameManager.Instance.lineStartPos;
            edPos = GameManager.Instance.lineEndPos;
            center = (stPos + edPos) * 0.5f;
            center.y += lineOffset;
            stPos = stPos - center;
            edPos = edPos - center;
            Vector3 point = Vector3.Slerp(stPos, edPos, i / (float)(lineRenderer.positionCount - 1));
            point += center;
            lineRenderer.SetPosition(i, point);
        }
        DrawOtherLines();

        spawner.position = new Vector3(0, GameManager.Instance.lineStartPos.y * 2.4f, 0);
        noteSign.position = new Vector3 (0, GameManager.Instance.lineStartPos.y * 0.8f, 0);
    }
   
    void DrawOtherLines()
    {
        int tmp;
        Vector3[] tmpArr = new Vector3[lineRenderer.positionCount];
        tmp = lineRenderer.GetPositions(tmpArr);

        touchArea.Draw(tmp, tmpArr);
        realJudgeLine.Draw(tmp, tmpArr);
    }
}
