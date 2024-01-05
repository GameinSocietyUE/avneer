using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayFund : Displayable
{
    public static DisplayFund Instance;

    public TextMeshProUGUI title_Text;
    public TextMeshProUGUI description_Text;
    public LinkButton LinkButton;

    private void Awake() {
        Instance = this;
    }

    public void Display(string title, string description, string url) {
        FadeIn();
        title_Text.text = title;
        description_Text.text = description;
        LinkButton.url = url;
    }
}
