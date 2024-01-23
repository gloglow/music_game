using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLine : MonoBehaviour
{
    public StageManager stageManager;
    private void OnTriggerEnter(Collider other)
    {
        // when collide with note
        if (other.gameObject.layer == 8) // layer of note
        {
            stageManager.ShowGrade(0); // grade the note.
            ObjectPoolManager.Instance.notePool.Release(other.gameObject); // return note back to object pool.
        }
    }
}
