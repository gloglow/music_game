using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public IObjectPool<GameObject> notePool { get; private set; }

    public int noteMaxCnt;
    public GameObject prefab_note;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        notePool = new ObjectPool<GameObject>(CreateNote, BringNoteFromPool, ReturnNoteToPool
            , null, true, noteMaxCnt, noteMaxCnt);

        for (int i = 0; i< noteMaxCnt; i++)
        {
            Note note = CreateNote().GetComponent<Note>();
            note.notePool.Release(note.gameObject);
        }
    }

    public GameObject CreateNote()
    {
        GameObject note = Instantiate(prefab_note);
        note.GetComponent<Note>().notePool = notePool;
        return note;
    }

    public void BringNoteFromPool(GameObject note)
    {
        note.SetActive(true);
    }

    public void ReturnNoteToPool (GameObject note)
    {
        note.SetActive(false);
    }
}
