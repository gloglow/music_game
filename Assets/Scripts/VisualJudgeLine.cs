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
    public void DrawLine(int posCnt, Vector3[] posArr)
    {
        // draw parabola with linerenderer and Slerp. 
        for (int i = 0; i < posCnt; i++)
        {
            lineRenderer.SetPosition(i, posArr[i]);
        }

        // draw touch area and real judge line.
        DrawOtherLines();
    }
   
    public void DrawOtherLines()
    {
        // get all position of line.
        Vector3[] posArr = new Vector3[lineRenderer.positionCount];
        int pos = lineRenderer.GetPositions(posArr);

        // draw touch area and real judge line.
        touchArea.Draw(pos, posArr);
        realJudgeLine.Draw(pos, posArr);
    }
}
