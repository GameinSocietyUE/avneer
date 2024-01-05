using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayAnswer : Displayable, IPointerClickHandler {
    public string id;
    public TextMeshProUGUI ui_text;
    public QuestionData answer;
    public string questionId;

    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);
        GameManager.Instance.SelectAnswer(questionId, answer);
    }
}
