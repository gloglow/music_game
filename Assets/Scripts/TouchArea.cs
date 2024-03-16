using UnityEngine;

public class TouchArea : MonoBehaviour
{
    [SerializeField] private JudgeLine judgeLine;
    private LineRenderer lineRenderer;
    private MeshCollider meshCollider; // インプットを認識するためのコライダー

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void Draw()
    {
        Vector3[] points = judgeLine.GetLinePoints();　//　judge lineの上にtouch areaを描く
        
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }

        //　line rendererで描いたtouch areaがインプットを認識できるように、mesh コライダー
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
    }
}
