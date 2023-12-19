using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayAnswer : Displayable, IPointerClickHandler {
    public int id;
    public TextMeshProUGUI ui_text;

    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);
        GameManager.Instance.SelectAnswer(ui_text.text);
    }
}
