using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // when collide with note
        if (other.gameObject.layer == 8) // layer of note
        {
            Note note = other.gameObject.GetComponent<Note>();
            StageManager.Instance.ShowGrade(note.Grading()); // grade the note.
            ObjectPoolManager.Instance.notePool.Release(other.gameObject); // return note back to object pool.
        }
    }
}
