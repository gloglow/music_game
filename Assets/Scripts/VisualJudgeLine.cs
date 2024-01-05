using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualJudgeLine : MonoBehaviour 
{
    // Draw Visual Judge Line(UI) -> reference for other lines (real judge line & toucharea)

    [SerializeField] private TouchArea touchArea;
    [SerializeField] private RealJudgeLine realJudgeLine;

    private LineRenderer lineRenderer;
    [SerializeField] private float lineOffset; // decide the shape of line. best is 2.

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void DrawLine()
    {
        // variables for drawing line
        Vector3 stPos, edPos, center;

        // draw parabola with linerenderer and Slerp. 
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

        // draw touch area and real judge line.
        DrawOtherLines();
    }
   
    void DrawOtherLines()
    {
        // get all position of line.
        Vector3[] posArr = new Vector3[lineRenderer.positionCount];
        int pos = lineRenderer.GetPositions(posArr);

        // draw touch area and real judge line.
        touchArea.Draw(pos, posArr);
        realJudgeLine.Draw(pos, posArr);
    }
}
