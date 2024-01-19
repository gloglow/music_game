using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLine : MonoBehaviour
{
    public StageManager stageManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {

        }
    }
}
