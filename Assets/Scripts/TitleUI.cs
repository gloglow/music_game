using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUI: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startBtn;
    [SerializeField] private float alphaChange;
    private int switchAlphaUp = -1;

    private void Update()
    {
        if(startBtn.color.a <= 0)
            switchAlphaUp = 1;
        else if (startBtn.color.a >= 1)
            switchAlphaUp = -1;

        startBtn.color += new Color(0, 0, 0, switchAlphaUp * alphaChange); 
    }
}