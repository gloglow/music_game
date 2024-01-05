using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Note note = other.gameObject.GetComponent<Note>();
            StageManager.Instance.ShowGrade(note.Grading());
            ObjectPoolManager.Instance.notePool.Release(other.gameObject);
        }
    }
}
