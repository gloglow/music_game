using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TouchArea : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MeshCollider meshCollider; // to sense input(mouse or touch)

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void Draw()
    {
        Vector3[] lineArr = GameManager.Instance.lineRendererPosArr;
        // draw line with points.
        for (int i = 0; i < lineArr.Length; i++)
        {
            lineRenderer.SetPosition(i, lineArr[i]);
        }

        // make mesh collider to sense input(ray from mouse or touch)
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
    }
}
