using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongList : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public int songID;
    public int songCnt;
    private Vector3 mousePosition;
    [SerializeField] private float mouseOffset;
    private float songListHeight = 1250f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            // drag -> move song list
            float ypos = (mousePosition.y - Input.mousePosition.y) * mouseOffset;
            transform.position -= new Vector3(0, ypos, 0);
            if (Mathf.Abs(transform.localPosition.y) < 100)
            {

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 tmpPos = transform.localPosition;
            transform.localPosition = new Vector3(tmpPos.x, 250f * Mathf.RoundToInt(tmpPos.y / 250f), tmpPos.z);
        }

        Vector3 crtLocalPos = transform.localPosition;
        if(transform.localPosition.y > songListHeight)
        {
            transform.localPosition = new Vector3(crtLocalPos.x, -songListHeight, crtLocalPos.z);
        }
        else if (transform.localPosition.y < -songListHeight)
        {
            transform.localPosition = new Vector3(crtLocalPos.x, songListHeight, crtLocalPos.z);
        }
        else if (transform.localPosition.y <= 250f && transform.localPosition.y >= -250f)
        {
            float tmp = 1.2f - ((float)Mathf.Abs(transform.localPosition.y) / 1250f);
            transform.localScale = new Vector3(tmp, tmp, tmp);
            transform.localPosition = new Vector3(-100 + 0.4f*Mathf.Abs(transform.localPosition.y), transform.localPosition.y, transform.transform.localPosition.z);
        }
    }

    public void ChangeInfo(int ID, string title)
    {
        songID = ID;
        titleText.text = title;
    }
}