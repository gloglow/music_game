using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject prefab_destroyEffect;
    public GameObject[] destroyEffects;
    public int cntDestroyEffect;

    private void Start()
    {
        destroyEffects = new GameObject[cntDestroyEffect];
        for (int i = 0; i < destroyEffects.Length; i++)
        {
            GameObject tmp;
            tmp = Instantiate(prefab_destroyEffect, transform);
            destroyEffects[i] = tmp;
        }
    }
}
