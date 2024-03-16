using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    //　背景イメージを移動させる
    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;

    [SerializeField] private float movingSpeed; // 移動速度。0.002で設定
    [SerializeField] private float posStandard; // 位置を変える基準位置。-30で設定
    [SerializeField] private float posMove; // 位置を変える時に移動する位置。51.8で設定

    void Update()
    {
        MoveBackgroundImages();
    }

    private void MoveBackgroundImages()
    {
        image1.transform.position -= new Vector3(movingSpeed, 0, 0);
        image2.transform.position -= new Vector3(movingSpeed, 0, 0);
        if (image1.transform.position.x < posStandard)
        {
            image1.transform.position = new Vector3(posMove, 0, 0);
        }
        if (image2.transform.position.x < posStandard)
        {
            image2.transform.position = new Vector3(posMove, 0, 0);
        }
    }
}
