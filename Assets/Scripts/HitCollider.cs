using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public StageManager stageManager;
    private void OnTriggerEnter(Collider other)
    {
        // when collide with note
        if (other.gameObject.layer == 8) // layer of note
        {
            Note note = other.gameObject.GetComponent<Note>();
            stageManager.ShowGrade(note.Grading()); // grade the note.
            ObjectPoolManager.Instance.notePool.Release(other.gameObject); // return note back to object pool.
        }
    }
}
