using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualJudgeLine : MonoBehaviour 
{
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine()
    {
        Vector3[] lineArr = GameManager.Instance.lineRendererPosArr;
        // draw parabola with linerenderer and Slerp. 
        for (int i = 0; i < lineArr.Length; i++)
        {
            lineRenderer.SetPosition(i, lineArr[i]);
        }
    }
}
