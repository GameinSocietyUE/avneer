using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayButton : Displayable, IPointerClickHandler
{
    public string buttonName;
    public Displayable showTarget;
    public Displayable hideTarget;

    public bool locked = false;

    public bool lockOnEnable = false;

    void OnEnable()
    {
        if (lockOnEnable)
        {
            Lock();
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (locked)
        {
            return;
        }
        if (buttonName != null && buttonName.Equals("login"))
        {
            Debug.Log("Test login click");
            GameManager.Instance.Login();
            return;
        }
        else if (buttonName != null && buttonName.Equals("chat"))
        {
            Debug.Log("Click chat button, reset and show");
            hideTarget?.FadeOut();
            DisplayChat.Instance.ResetChatAndShow();
            return;
        }
        Trigger();   
    }
    
    public void Trigger()
    {
        Tween.Bounce(GetTransform);

        showTarget?.FadeIn();
        hideTarget?.FadeOut();
    }

    public void Lock()
    {
        CanvasGroup.alpha = 0.5f;
        locked = true;
    }

    public void Unlock()
    {
        CanvasGroup.alpha = 1f;

        locked = false;
    }
}
