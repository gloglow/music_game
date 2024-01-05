using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Note : MonoBehaviour
{
    public IObjectPool<GameObject> notePool {  get; set; }

    public float speed;
    public Vector3 dirVec;
    public float bpm;
    Vector3 destination;
    float dist;
    Vector3 initialPos;
    float initialTime;
    public float offset;

    public int status; // 0 : idle, 1 : Check Destination 2 : Move to Destination

    private void Update()
    {
        switch (status)
        {
            case 0:
                break;
            case 1:
                initialPos = transform.position;
                RaycastHit rayHit;
                initialTime = (float)AudioSettings.dspTime;
                int layerMask = (1 << 7);
                
                if (Physics.Raycast(transform.position, dirVec, out rayHit, Mathf.Infinity, layerMask))
                {
                    destination = rayHit.point;
                    dist = Vector3.Distance(transform.position, destination);
                    //speed = dist * bpm / 60f * 0.0008f;
                }
                float tmp = (((float)AudioSettings.dspTime - initialTime) / StageManager.Instance.secondPerBeat) * dist;
                transform.position = initialPos + dirVec * tmp * 0.5f;
                //transform.Translate(dirVec * speed);
                status = 2;
                break;
            case 2:
                tmp = (((float)AudioSettings.dspTime - initialTime) / StageManager.Instance.secondPerBeat) * dist;
                transform.position = initialPos + dirVec * tmp *0.5f;
                //transform.Translate(dirVec * speed);
                if(transform.position.y < Camera.main.ScreenToWorldPoint(new Vector2(0, -1 * Screen.height)).y)
                {
                    StageManager.Instance.ShowGrade(0);
                    Exit();
                }
                break;
        }
    }

    public int Grading()
    {
        dist = Vector3.Distance(transform.position, destination);
        if (dist < 0.5f)
        {
            return 4;
        }
        else if(dist < 0.85f)
        {
            return 3;
        }
        else
        {
            return 1;
        }
    }

    public void Exit()
    {
        status = 0;
        notePool.Release(gameObject);
    }
}
