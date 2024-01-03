using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private int grade;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("dd");
        if (other.gameObject.layer == 8)
        {
            float tmp = Vector3.Distance(transform.position, other.transform.position);
            grade = Grading(tmp);
            other.gameObject.SetActive(false);
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("dd");
        if(collision.gameObject.layer == 8)
        {
            float tmp = Vector3.Distance(transform.position, collision.transform.position);
            grade = Grading(tmp);
            collision.gameObject.SetActive(false);
        }
    }*/

    private int Grading (float dist)
    {
        if (dist < 0.2f)
            return 4;
        else if (dist < 0.3f)
            return 3;
        else if (dist < 0.4f)
            return 2;
        else
            return 1;
    }
}
