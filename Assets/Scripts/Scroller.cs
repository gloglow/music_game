using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float tempo;
    public bool startFlag;

    private void Start()
    {
        tempo = tempo / 60f;
    }

    private void Update()
    {
        if (!startFlag)
        {
            if (Input.anyKeyDown)
            {
                startFlag = true;
            }
        }
        else
        {
            transform.position -= new Vector3(0f, tempo * Time.deltaTime, 0f);
        }
    }
}
