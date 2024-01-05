using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayJobDescription : MonoBehaviour
{
    private Vector2 initScale;
    private Vector2 initPos;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform fullDescriptionAnchor;
    [SerializeField] private float transition_duration = 0.5f;
    private bool zoomed = false;

    [SerializeField] private Displayable zoomButton;
    [SerializeField] private Displayable unzoomButton;

    private string desc;
    private string descFull;
    [SerializeField] private TextMeshProUGUI jobDesc;


    // Start is called before the first frame update
    void Start()
    {
        initPos = rectTransform.anchoredPosition;
        initScale = rectTransform.sizeDelta;
        jobDesc.text = desc;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.M) ) {
            if (zoomed) {
                Unzoom();
            } else {
                Zoom();
            }
        }
    }

    public void SetDesc(string desc, string descFull)
    {
        this.desc = desc;
        this.descFull = descFull;
    }

    public void Zoom() {
        zoomButton.FadeOut();
        unzoomButton.FadeIn();
        Tween.Bounce(unzoomButton.GetTransform);
        zoomed = true;
        rectTransform.DOSizeDelta(fullDescriptionAnchor.rect.size, transition_duration);
        rectTransform.DOLocalMove(fullDescriptionAnchor.localPosition, transition_duration);
        jobDesc.text = descFull;
    }
    public void Unzoom() {
        zoomButton.FadeIn();
        unzoomButton.FadeOut();
        rectTransform.DOSizeDelta(initScale, transition_duration);
        rectTransform.DOAnchorPos(initPos, transition_duration);
        zoomed = true;
        jobDesc.text = desc;
    }


}
