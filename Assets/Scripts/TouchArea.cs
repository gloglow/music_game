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

    public void Draw(int cnt, Vector3[] posList)
    {
        // draw line with points.
        for (int i = 0; i < cnt; i++)
        {
            lineRenderer.SetPosition(i, posList[i]);
        }

        // make mesh collider to sense input(ray from mouse or touch)
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
    }
}
