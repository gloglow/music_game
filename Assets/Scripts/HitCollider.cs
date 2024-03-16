using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public StageManager stageManager;
    [SerializeField] ObjectPoolManager poolManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // ノーツのレイヤー
        {
            Note note = other.gameObject.GetComponent<Note>();
            note.Exit(false);
        }
    }
}
