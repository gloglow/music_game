using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TouchArea : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MeshCollider meshCollider;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void Draw(int cnt, Vector3[] posList)
    {
        for (int i = 0; i < cnt; i++)
        {
            lineRenderer.SetPosition(i, posList[i]);
        }

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            Note tmp = other.GetComponent<Note>();
            tmp.Exit();
        }
    }
}
