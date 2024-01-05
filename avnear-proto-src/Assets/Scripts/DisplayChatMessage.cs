using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayChatMessage : Displayable {

    [SerializeField] private TextMeshProUGUI ui_text;
    [SerializeField] private CanvasGroup background_CanvasGroup;
    [SerializeField] private RectTransform background_RectTransform;
    [SerializeField] public float transition_decal = -100f;
    [SerializeField] public float transition_duration = 0.2f;
    [SerializeField] RectTransform rectTransform;

    [SerializeField] public GameObject waiting_Obj;


    public void Display(string message) {
        FadeIn();
        ui_text.text = message;
        background_CanvasGroup.alpha = 0f;
        Canvas.ForceUpdateCanvases();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, background_RectTransform.sizeDelta.y);

        StartCoroutine(DisplayCoroutine());
    }

    IEnumerator DisplayCoroutine() {
        if ( waiting_Obj != null) {
            waiting_Obj.SetActive(true);
            yield return new WaitForSeconds(1f);
            waiting_Obj.SetActive(false);
        }
        DisplayDelay();

    }

    public void StopWaitingAnim()
    {
        if (waiting_Obj != null)
        {
            waiting_Obj.SetActive(false);
            DisplayDelay();
        }
    }

    private void DisplayDelay() {
        background_CanvasGroup.DOFade(1f, transition_duration);
        background_RectTransform.anchoredPosition = Vector3.right * -transition_decal;
        background_RectTransform.DOAnchorPos(Vector2.zero, transition_duration).SetEase(Ease.OutBounce);
    }
}