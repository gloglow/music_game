using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealJudgeLine : MonoBehaviour
{
    public GameObject part;
    public float partWidth;

    public void Draw(int cnt, Vector3[] posList)
    {
        float length = Vector3.Distance(posList[0], posList[1]);
        part.transform.localScale = new Vector3(partWidth, length, partWidth);
        
        for (int i = 0; i < cnt-1; i++)
        {
            GameObject tmp = Instantiate(part, transform);
            Vector3 dirVec = posList[i+1] - posList[i];
            dirVec.Normalize();
            tmp.transform.position = (posList[i] + posList[i+1]) / 2f;
            tmp.transform.rotation = Quaternion.LookRotation(dirVec);
            tmp.transform.Rotate(new Vector3(90, 0, 0));
        }
    }
}
