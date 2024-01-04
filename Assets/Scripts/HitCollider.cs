using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class HitCollider : MonoBehaviour
{
    public Text gradeText;
    private int grade; // 4 : great, 3 : good, 2 : bad, 1 : miss

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            float tmp = Vector3.Distance(transform.position, other.transform.position);
            grade = Grading(tmp);
            other.gameObject.SetActive(false);
        }
    }

    private int Grading (float dist)
    {
        if (dist < 0.2f)
        {
            gradeText.text = "Perfect";
            return 4;
        }
        else if (dist < 0.3f)
        {
            gradeText.text = "Great";
            return 3;
        }
        else if (dist < 0.4f)
        {
            gradeText.text = "Good";
            return 2;
        }
        else
        {
            gradeText.text = "Bad";
            return 1;
        }
    }
}
