using UnityEngine;

public class RealJudgeLine : MonoBehaviour
{
    [SerializeField] private GameObject part; // object what actually check note
    [SerializeField] private GameObject dPart;
    [SerializeField] private float partWidth; // width of object. best is 0.03.

    public void Draw()
    {
        Vector3[] lineArr = GameManager.Instance.lineRendererPosArr;

        // initialize size of part object.
        float length = Vector3.Distance(lineArr[0], lineArr[1]); // length = distance between two points of line renderer.
        part.transform.localScale = new Vector3(partWidth, length, partWidth);

        for (int i = 0; i < lineArr.Length - 1; i++)
        {
            // instantiate part object and initialize their position and rotation.
            GameObject judgePart = Instantiate(part, transform);
            GameObject judgePartDestroy = Instantiate(dPart, transform); 

            Vector3 dirVec = lineArr[i + 1] - lineArr[i];
            dirVec.Normalize();

            judgePart.transform.position = (lineArr[i] + lineArr[i + 1]) / 2f;
            judgePartDestroy.transform.position = (lineArr[i] + lineArr[i + 1]) / 2f + new Vector3(0, -3f, 0);
            judgePart.transform.rotation = Quaternion.LookRotation(dirVec);
            judgePartDestroy.transform.rotation = Quaternion.LookRotation(dirVec);
            judgePart.transform.Rotate(new Vector3(90, 0, 0));
            judgePartDestroy.transform.Rotate(new Vector3(90, 0, 0));
        }
    }
}
