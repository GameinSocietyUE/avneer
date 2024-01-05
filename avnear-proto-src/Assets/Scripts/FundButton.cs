using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FundButton : Displayable, IPointerClickHandler {

    public string buttonName;
    public TextMeshProUGUI buttonName_Text;
    public string title;
    public string description;
    public string url;
    public int id;

    public override void Start() {
        base.Start();
        buttonName_Text.text = LinkLoader.datas[id].buttonName;
        title = LinkLoader.datas[id].title;
        description = LinkLoader.datas[id].description;
        url = LinkLoader.datas[id].url;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);
        DisplayFund.Instance.Display(title, description, url);
    }
}
