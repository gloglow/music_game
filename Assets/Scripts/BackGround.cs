using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;
    void Update()
    {
        MoveBackgroundImages();
    }

    private void MoveBackgroundImages()
    {
        image1.transform.position -= new Vector3(0.002f, 0, 0);
        image2.transform.position -= new Vector3(0.002f, 0, 0);
        if (image1.transform.position.x < -28)
        {
            image1.transform.position = new Vector3(53.8f, 0, 0);
        }
        if (image2.transform.position.x < -28)
        {
            image2.transform.position = new Vector3(53.8f, 0, 0);
        }
    }
}
