using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayFormation : Displayable
{
    public RectTransform rectTransform;
    public GridFreeLayout GridFreeLayout;
    float minY = 0f;

    public TextMeshProUGUI uiText_SchoolName;
    public TextMeshProUGUI uiText_SchoolLocation;
    public TextMeshProUGUI uiText_FormationName;
    public TextMeshProUGUI uiText_sup;

    public void Display(FormationData data) {
        FadeIn();
        minY = rectTransform.sizeDelta.y;
        uiText_SchoolName.text = data.schoolName;
        uiText_SchoolLocation.text = data.schoolLocation;
        uiText_FormationName.text = data.formationName;
        float h = 0f;
        if (data.informations != null && data.informations.Count > 0) {
            GridFreeLayout.DisplayInfos(data.informations, out h);
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minY + h);
        uiText_sup.text = data.sup ? "parcous sup : oui" : "parcous sup : non";
    }
}

[System.Serializable]
public class FormationData {
    public FormationData(string formationId, string schoolName, string schoolLocation, string formationName, List<string> informations, bool sup) {
        this.formationId = formationId;
        this.schoolName = schoolName;
        this.schoolLocation = schoolLocation;
        this.formationName = formationName;
        this.informations = informations;
        this.sup = sup;
    }
    public string formationId;
    public string schoolName;
    public string schoolLocation;
    public string formationName;
    public List<string> informations;
    public bool sup;
}