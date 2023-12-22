using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMessage : Displayable
{
    public static DisplayMessage Instance;

    public TextMeshProUGUI ui_Text;

    private void Awake() {
        Instance = this;
    }

    public void Display (string message) {
        FadeIn();
        ui_Text.text = message;
    }
}
