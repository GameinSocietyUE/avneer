using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BounceOnClick : Displayable, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Tween.Bounce(GetTransform);
    }

}