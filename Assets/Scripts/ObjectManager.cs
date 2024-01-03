using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject prefab_note;
    public GameObject notes;

    private void Start()
    {
        CreateNotes();
    }

    private void CreateNotes()
    {
        
        for(int i = 0; i < 3; i++)
        {
            //Instantiate(prefab_note, notes.transform);
        }
        
    }

    public void MoveNotes()
    {

    }
}
