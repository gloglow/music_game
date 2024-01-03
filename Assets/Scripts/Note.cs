using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float time;
    public float speed;
    public float speedOffset;
    public float userSpeed; // 유저가 설정한 배속
    public Vector3 dirVec;
    public Transform noteSign;

    private int status = 1; // 0 : idle, 1 : initial, 2 : arrive at sign, 3 : moving to destination

    private void Update()
    {
        switch (status)
        {
            case 1:
                transform.position += Vector3.down * speed * userSpeed * Time.deltaTime;
                if(transform.position.y < noteSign.position.y)
                {
                    status = 1;
                }
                break;
            case 2:
                RaycastHit rayHit;
                int layerMask = (1 << 7);
                if (Physics.Raycast(transform.position, dirVec, out rayHit, Mathf.Infinity, layerMask))
                {
                    speed = Vector3.Distance(transform.position, rayHit.transform.position) / speedOffset;
                    transform.position += dirVec * speed * userSpeed * Time.deltaTime;
                    status = 2;
                }
                break;
            case 3:
                transform.position += dirVec * speed * userSpeed * Time.deltaTime;
                break;
        }
    }

    public void Exit()
    {
        status = 0;
        gameObject.SetActive(false);
    }
}
