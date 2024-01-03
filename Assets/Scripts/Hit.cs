using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public int hitCnt;
    public GameObject prefab_hitCollider;
    public GameObject[] hitColliders;
    public int cnt;

    private void Start()
    {
        hitColliders = new GameObject[hitCnt];
        cnt = 0;

        for (int i = 0;  i < hitCnt; i++)
        {
            GameObject tmp = Instantiate(prefab_hitCollider, transform);
            hitColliders[i] = tmp;
        }    
    }

    public void hitting(Vector3 pos)
    {
        if (cnt == hitCnt)
        {
            cnt = 0;
        }
        hitColliders[cnt].SetActive(true);
        hitColliders[cnt].transform.position = pos;
        cnt++;
    }
    /*
    public void Called(Vector3 pos)
    {
        if(cnt == hitCnt)
        {
            cnt = 0;
        }
        hitRanges[cnt].SetActive(true);
        ParticleSystem particleSystem = hitRanges[cnt].GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
        hitRanges[cnt].transform.position = pos;
        cnt++;
    }
    */

}
