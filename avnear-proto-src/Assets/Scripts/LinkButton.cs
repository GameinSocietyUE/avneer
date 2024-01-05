using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkButton : Displayable, IPointerClickHandler {

    public string url;
    
    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);

        DisplayFund.Instance.FadeOut();
        Application.OpenURL(url);
    }
}
