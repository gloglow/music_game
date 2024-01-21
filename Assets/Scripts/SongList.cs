using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongList : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public int songID;
    private Vector3 mousePosition;
    [SerializeField] private float mouseOffset;
    private float songListHeight = 1250f;
    public MusicSelecting musicSelecting;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            float ypos = (mousePosition.y - Input.mousePosition.y) * mouseOffset;
            transform.position -= new Vector3(0, ypos, 0);
            if (Mathf.Abs(transform.localPosition.y) < 100 && GameManager.Instance.crtSongID != songID)
            {
                GameManager.Instance.crtSongID = songID;
                musicSelecting.ShowSongInfo(songID);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 tmp = transform.localPosition;
            transform.localPosition = new Vector3(tmp.x, 250f * Mathf.RoundToInt(tmp.y / 250f), tmp.z);
        }

        Vector3 crtLocalPos = transform.localPosition;
        if(transform.localPosition.y > songListHeight)
        {
            transform.localPosition = new Vector3(crtLocalPos.x, -songListHeight, crtLocalPos.z);
            musicSelecting.IndexCalculating(this, false);
        }
        if (transform.localPosition.y < -songListHeight)
        {
            transform.localPosition = new Vector3(crtLocalPos.x, songListHeight, crtLocalPos.z);
            musicSelecting.IndexCalculating(this, true);
        }
        if (transform.localPosition.y <= 250f && transform.localPosition.y >= -250f)
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