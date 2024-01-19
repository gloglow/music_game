using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class MusicSelecting : MonoBehaviour
{
    private string noteDataFilePath = Application.streamingAssetsPath + "/Json/songList.json";

    [SerializeField] GameObject music0;
    [SerializeField] GameObject music1;
    [SerializeField] GameObject music2;
    [SerializeField] GameObject music3;
    [SerializeField] GameObject music4;

    private Vector3 mousePosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            
        }
    }
}
