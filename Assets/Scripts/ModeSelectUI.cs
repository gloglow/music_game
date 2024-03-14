using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeSelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] modeTextList;

    public void PointerEnter(int index)
    {
        modeTextList[index].gameObject.SetActive(true);
    }

    public void PointerExit(int index)
    {
        modeTextList[index].gameObject.SetActive(false);
    }
}
