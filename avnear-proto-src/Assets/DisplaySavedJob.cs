using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplaySavedJob : Displayable
{
    public TextMeshProUGUI uiText;

    public void Display( string jobName) {
        FadeIn();
        uiText.text = jobName;
    }
}
