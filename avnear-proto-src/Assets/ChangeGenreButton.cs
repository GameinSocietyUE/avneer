using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeGenreButton : Displayable, IPointerClickHandler {
    public DisplayAvatarCreation.Genre genre;

    public Image image;
    public Color selectedColor;
    public Color idleColor;

    public ChangeGenreButton other;

    private void Start() {
        if ( genre == DisplayAvatarCreation.Genre.Female) {
            Select();
        } else {
            Deselect();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);
        DisplayAvatarCreation.Instance.ChangeGenre((int)genre);
        Select();
    }
    public void Select() {
        other.Deselect();
        image.color = selectedColor;
    }
    public void Deselect() {
        image.color = idleColor;
    }
}
