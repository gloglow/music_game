using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLine : MonoBehaviour
{
    public StageManager stageManager;
    [SerializeField] private ObjectPoolManager poolManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // ノーツのレイヤー
        {
            stageManager.ShowGrade(0); // failで判定
            poolManager.notePool.Release(other.gameObject); // ノーツを解除
        }
    }
}
