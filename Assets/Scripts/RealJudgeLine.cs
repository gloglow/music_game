using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealJudgeLine : MonoBehaviour
{
    [SerializeField] private GameObject part; // object what actually check note
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
            GameObject tmp = Instantiate(part, transform);

            Vector3 dirVec = lineArr[i + 1] - lineArr[i];
            dirVec.Normalize();

            tmp.transform.position = (lineArr[i] + lineArr[i + 1]) / 2f;
            tmp.transform.rotation = Quaternion.LookRotation(dirVec);
            tmp.transform.Rotate(new Vector3(90, 0, 0));
        }
    }

    /*
    public void Draw(int cnt, Vector3[] posList)
    {
        // initialize size of part object.
        float length = Vector3.Distance(posList[0], posList[1]); // length = distance between two points of line renderer.
        part.transform.localScale = new Vector3(partWidth, length, partWidth);
        
        for (int i = 0; i < cnt-1; i++)
        {
            // instantiate part object and initialize their position and rotation.
            GameObject tmp = Instantiate(part, transform);

            Vector3 dirVec = posList[i+1] - posList[i];
            dirVec.Normalize();

            tmp.transform.position = (posList[i] + posList[i+1]) / 2f;
            tmp.transform.rotation = Quaternion.LookRotation(dirVec);
            tmp.transform.Rotate(new Vector3(90, 0, 0));
        }
    }
    */
}
