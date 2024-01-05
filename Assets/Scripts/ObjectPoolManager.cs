using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    // manager of object pooling. singleton.

    private static ObjectPoolManager instance;
    public IObjectPool<GameObject> notePool { get; private set; } // object pool of notes.

    [SerializeField] private int noteMaxCnt; // the default number of notes. 
    [SerializeField] private GameObject prefab_note; // prefab of note.

    public static ObjectPoolManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(ObjectPoolManager)) as ObjectPoolManager;

                if (instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Initialize(); // create object pool and objects.
    }

    public void Initialize()
    {
        // create note object pool.
        notePool = new ObjectPool<GameObject>(CreateNote, BringNoteFromPool, ReturnNoteToPool
            , notdestroybuttmp, true, noteMaxCnt, noteMaxCnt);

        // create notes and let them in pool.
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

    public void notdestroybuttmp(GameObject note)
    {

    }
}
