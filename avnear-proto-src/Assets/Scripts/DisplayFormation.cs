using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayFormation : Displayable
{
    public RectTransform rectTransform;
    public GridFreeLayout GridFreeLayout;
    float minY = 0f;

    //public TextMeshProUGUI uiText_SchoolName;
    //public TextMeshProUGUI uiText_SchoolLocation;
    public TextMeshProUGUI uiText_FormationName;
    public TextMeshProUGUI uiText_sup;
    public TextMeshProUGUI uiText_apprentice;
    public TextMeshProUGUI uiText_duration;
    public TextMeshProUGUI uiText_establishment_count;

    public void Display(FormationData data) {
        FadeIn();
        minY = rectTransform.sizeDelta.y;
        //uiText_SchoolName.text = data.schoolName;
        //uiText_SchoolLocation.text = data.schoolLocation;
        uiText_FormationName.text = data.formationName;
        float h = 0f;
        /*if (data.informations != null && data.informations.Count > 0) {
            GridFreeLayout.DisplayInfos(data.informations, out h);
        }*/
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minY + h);
        uiText_sup.text = data.sup ? "parcous sup : oui" : "parcous sup : non";
        uiText_apprentice.text = data.apprentice ? "apprenti : oui" : "apprenti : non";
        uiText_duration.text = data.duration;
        uiText_establishment_count.text = data.establishmentCount.ToString();
    }
}

[System.Serializable]
public class FormationData {
    public FormationData(string formationId, string formationName, bool sup, bool apprentice, string duration, int establishmentCount) {
        this.formationId = formationId;
        this.formationName = formationName;
        this.sup = sup;
        this.apprentice = apprentice;
        this.duration = duration;
        this.establishmentCount = establishmentCount;
        
    }
    public string formationId;
    public string formationName;
    public bool sup;
    public bool apprentice;
    public string duration;
    public int establishmentCount;
}