using UnityEngine;

public class JudgeLine : MonoBehaviour 
{
    private LineRenderer lineRenderer;
    [SerializeField] private Vector2 idealScreenSize; // Resolution Reference : Apple iPhone 12
    [SerializeField] private int lineRendererPosCnt;
    private Vector3 lineStartPos;
    private Vector3 lineEndPos;
    [SerializeField] private float lineOffset;
    public StageManager stageManager;

    [SerializeField] private GameObject part; // object what actually check note
    [SerializeField] private GameObject dPart;
    [SerializeField] private float partWidth; // width of object. best is 0.03.

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        ReadyToDrawLine();
        GetLinePoint();
    }

    private void ReadyToDrawLine()
    {
        // To make ideal line for every resolution
        float idealLinePoint = Screen.width * idealScreenSize.y / idealScreenSize.x;
        lineStartPos = Camera.main.ScreenToWorldPoint(new Vector2(0, idealLinePoint));
        lineEndPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, idealLinePoint));
        lineStartPos.z = 0;
        lineEndPos.z = 0;
    }

    private void GetLinePoint()
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
            lineRenderer.SetPosition(i, point);
        }

        Draw();
    }

    public void Draw()
    {
        // initialize size of part object.
        float length = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)); // length = distance between two points of line renderer.
        part.transform.localScale = new Vector3(partWidth, length, partWidth);

        for (int i = 0; i < lineRendererPosCnt - 1; i++)
        {
            // instantiate part object and initialize their position and rotation.
            GameObject judgePart = Instantiate(part, transform);
            GameObject judgePartDestroy = Instantiate(dPart, transform);

            Vector3 dirVec = lineRenderer.GetPosition(i+1) - lineRenderer.GetPosition(i);
            dirVec.Normalize();

            judgePart.transform.position = (lineRenderer.GetPosition(i) + lineRenderer.GetPosition(i+1)) / 2f;
            judgePart.transform.rotation = Quaternion.LookRotation(dirVec);
            judgePart.transform.Rotate(new Vector3(90, 0, 0));

            judgePartDestroy.transform.position = judgePart.transform.position + new Vector3(0, -3f, 0);
            judgePartDestroy.transform.rotation = judgePart.transform.rotation;

            DestroyLine dpart = judgePartDestroy.GetComponent<DestroyLine>();
            dpart.stageManager = stageManager;
        }
    }

    public Vector3[] GetLinePoints()
    {
        Vector3[] arr = new Vector3[lineRendererPosCnt];
        int tmp = lineRenderer.GetPositions(arr);
        return arr;
    }
}
