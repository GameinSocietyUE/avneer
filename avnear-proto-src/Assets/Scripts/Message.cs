using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message : MonoBehaviour
{
    [SerializeField] private TMP_Text messageTmp;
    [SerializeField] private RectTransform rectTransform;

    public void SetMessage(string messageText)
    {
        messageTmp.text = messageText;
    }

    public string GetMessage()
    {
        return messageTmp.text;
    }

    public void SetSize(float width, float height)
    {
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
