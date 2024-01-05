using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBottomMenu : Displayable
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float fadeIn_Decal;
    [SerializeField] private float fadeIn_Duration;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            FadeInInstant();
        }
    }

    public override void Show() {
        base.Show();

        rectTransform.anchoredPosition = Vector2.up * fadeIn_Decal;
        rectTransform.DOAnchorPos(Vector2.zero, fade_duration);
    }

    public void TriggerBottomBar(int i) {
        Debug.Log("Bottom button clicked: " + i);
        if (i == 0)
        {
            GameManager.Instance.PauseChat();
            /*DisplayChat.Instance.Hide();
            if (GameManager.Instance.IsConnected())
            {
                GameManager.Instance.canvasManager.DisplayPage(CanvasManager.Page.Welcome_Connected);
            }
            else
            {
                GameManager.Instance.canvasManager.DisplayPage(CanvasManager.Page.Welcome_NoUser);
            }*/
            GameManager.Instance.TestChatResult();
        }
        else if (i == 1)
        {
            GameManager.Instance.RunChat();
            GameManager.Instance.canvasManager.DisplayPage(CanvasManager.Page.Chat);
        }
        else if (i == 2)
        {
            GameManager.Instance.PauseChat();
            DisplayChat.Instance.Hide();
            GameManager.Instance.canvasManager.DisplayPage(CanvasManager.Page.Funds);
        }
        else
        {
            DisplayMessage.Instance.Display($"bottom bar button : {i}");
        }
    }
}
