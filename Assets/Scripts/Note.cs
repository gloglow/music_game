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
    public Vector3 destination;
    public Rigidbody rigid;

    public int status; // 0 : idle, 1 : initial, 2 : arrive at sign, 3 : moving to destination

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        switch (status)
        {
            case 0:
                speed = Vector3.Distance(transform.position, destination) * bpm / 60f;
                status = 1;
                break;
            case 1:
                //transform.Translate(Vector3.down * speed * Time.deltaTime);
                if(transform.position.y == destination.y)
                {
                    RaycastHit rayHit;
                    int layerMask = (1 << 7);
                    if (Physics.Raycast(transform.position, dirVec, out rayHit, Mathf.Infinity, layerMask))
                    {
                        destination = rayHit.point;
                        speed = Vector3.Distance(transform.position, rayHit.transform.position) * bpm / 60f;
                        transform.Translate(dirVec * speed * Time.deltaTime);
                    }
                    status = 2;
                }
                break;
            case 2:
                transform.Translate(dirVec * speed * Time.deltaTime);
                if(transform.position.y < destination.y)
                {
                    status = 3;
                }
                break;
            case 3:
                Exit();
                break;
        }
    }

    public void Exit()
    {
        status = 0;
        notePool.Release(gameObject);
    }
}
