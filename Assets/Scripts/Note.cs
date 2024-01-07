using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Note : MonoBehaviour
{
    public IObjectPool<GameObject> notePool {  get; set; }

    public Vector3 dirVec; // direction vector to move.

    // variables to calculate move speed.
    private Vector3 initialPos; // position when note is activated (spawner's position)
    public float initialTime; // time when note is activated
    private Vector3 destination; // on judge line.
    private float dist; // distance between initial position and destination.

    // range of perfect and great grade
    [SerializeField] private float perfectRange;
    [SerializeField] private float greatRange;
    public float index;

    StageManager stageManager;
    public int status; // 0 : idle, 1 : set destination and speed, 2 : move

    private void Update()
    {
        switch (status)
        {
            case 0:
                break;

            case 1: // when activated by stage manager
                stageManager = transform.GetComponentInParent<StageManager>();
                initialPos = transform.position; // initial position (position of spawner)
                initialTime = (float)AudioSettings.dspTime; // activated timing
                
                RaycastHit rayHit;
                int layerMask = (1 << 7); // layer of real judge line
                
                if (Physics.Raycast(transform.position, dirVec, out rayHit, Mathf.Infinity, layerMask))
                {
                    destination = rayHit.point; // set destination on judge line
                    dist = Vector3.Distance(transform.position, destination); // distance to move.
                }

                // move.
                // calculate the position where this note should be at current timing.

                // speed = (current time / beat) * distance.
                // -> time taken for move initial position to destination is ONE BEAT. 
                float defaultSpeed = (((float)AudioSettings.dspTime - initialTime) / stageManager.secondPerBeat) * dist;
                
                // position = initial position + direction * speed * useroffset
                transform.position = initialPos + dirVec * defaultSpeed * stageManager.userSpeed;
                status = 2; // change status into 2 (don't need to calculate distance)
                break;

            case 2:
                // moving mechanism is same.
                defaultSpeed= (((float)AudioSettings.dspTime - initialTime) / stageManager.secondPerBeat) * dist;
                transform.position = initialPos + dirVec * defaultSpeed * stageManager.userSpeed;

                // if arrive on destroy line, destroy
                if(transform.position.y < Camera.main.ScreenToWorldPoint(new Vector2(0, -1 * Screen.height)).y)
                {
                    // grading this note MISS
                    stageManager.ShowGrade(0);
                    Exit();
                }
                break;
        }
    }

    public int Grading()
    {
        // grading by calculating distance between note and judgeline.
        // the closer, the higher grade
        dist = Vector3.Distance(transform.position, destination);

        if (dist < perfectRange) // set 0.5
        {
            return 3;
        }
        else if(dist <greatRange) // set 0.85
        {
            return 2;
        }
        else // bad grade.
        {
            return 1;
        }
    }

    public void Exit()
    {
        // set note status 0 and return to note object pool.
        status = 0;
        notePool.Release(gameObject);
    }
}
