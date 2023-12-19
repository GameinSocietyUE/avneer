using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DisplayAnswers : Displayable {

    public static DisplayAnswers Instance;

    [SerializeField] private DisplayAnswer prefab;
    [SerializeField] private List<DisplayAnswer> pool;
    [SerializeField] private Transform parent;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float fadeIn_Decal;
    [SerializeField] private float fadeIn_Duration;
    private float fadeIn_Init;

    private void Awake() {
        Instance = this;
    }

    public override void Start() {
        base.Start();
        fadeIn_Init = rectTransform.anchoredPosition.y;
    }

    public void Display(List<QuestionData> answers) {
        FadeIn();
        foreach (var da in pool) {
            da.Hide();
        }
        StartCoroutine(DisplayCoroutine(answers));
    }

    IEnumerator DisplayCoroutine(List<QuestionData> answers) {

        FadeIn();
        /*rectTransform.anchoredPosition = Vector2.up * fadeIn_Decal;
        rectTransform.DOAnchorPos(Vector2.up * fadeIn_Init, fade_duration);*/
        yield return new WaitForSeconds(0.5f);
        DisplayChat.Instance.ScrollRect.verticalNormalizedPosition = 0f;

        for (int i = 0; i < answers.Count; i++) {
            if (i >= pool.Count) {
                pool.Add(Instantiate(prefab, parent));
            }

            pool[i].Show();
            pool[i].ui_text.text = answers[i].label;
            pool[i].id = i;
            pool[i].CanvasGroup.alpha = 0f;
        }

        for (int i = 0; i < answers.Count; i++) {
            pool[i].FadeIn();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
