using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeLookButton : Displayable {
    public int lookCount = 10;
    public DisplayAvatarCreation.Look lookType;
    public int currentId;
    public TextMeshProUGUI text;

    private void Start() {
        UpdateUI();
    }

    public void Change(int i) {
        Tween.Bounce(GetTransform);
        currentId += i;
        currentId = currentId % lookCount;
        UpdateUI();
        DisplayAvatarCreation.Instance.ChangeLook(lookType, currentId);
    }

    void UpdateUI() {
        string name = "";
        switch (lookType) {
            case DisplayAvatarCreation.Look.Hair:
                name = "Cheveux";
                break;
            case DisplayAvatarCreation.Look.Face:
                name = "Visage";
                break;
            case DisplayAvatarCreation.Look.Skin:
                name = "Peau";
                break;
            default:
                break;
        }
        text.text = $"{name}\n({currentId}/{lookCount - 1})";
    }
}
