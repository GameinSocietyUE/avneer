using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FilterButton : Displayable, IPointerClickHandler {
    public TextMeshProUGUI ui_text;
    public Image image;
    public Sprite sprite_Selected;
    public Sprite sprite_Unselected;
    public Color color_Selected;
    public Color color_Unselected;

    string _name;

    public void Display(string name) {
        FadeIn();
        _name = name;
        ui_text.text = name;
    }

    public void Select() {
        ui_text.color = color_Selected;
        image.sprite = sprite_Selected;
    }

    public void Deselect() {
        ui_text.color = color_Unselected;
        image.sprite = sprite_Unselected;
    }

    public void OnPointerClick(PointerEventData eventData) {
        DisplayFormations.Instance.SelectFilter(this, _name);
    }
}
