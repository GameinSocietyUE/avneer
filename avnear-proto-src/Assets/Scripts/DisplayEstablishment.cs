using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayEstablishment : Displayable
{
    public RectTransform rectTransform;
    public GridFreeLayout GridFreeLayout;
    float minY = 0f;

    public TextMeshProUGUI uiText_establishment_name;
    public TextMeshProUGUI uiText_establishment_sigle;
    public TextMeshProUGUI uiText_establishment_type;
    public TextMeshProUGUI uiText_establishment_statut;
    public TextMeshProUGUI uiText_establishment_commune;
    public TextMeshProUGUI uiText_establishment_adresse;

    public void Display(EstablishmentData data)
    {
        FadeIn();
        minY = rectTransform.sizeDelta.y;
        uiText_establishment_name.text = data.name;
        uiText_establishment_sigle.text = data.sigle;
        uiText_establishment_type.text = data.type;
        uiText_establishment_statut.text = data.statut;
        uiText_establishment_commune.text = data.commune;
        uiText_establishment_adresse.text = data.adresse + " " + data.CP;
        float h = 0f;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minY + h);
    }
}

[System.Serializable]
public class EstablishmentData
{
    public EstablishmentData(string id, string name, string type, string CP, string sigle, string statut, string adresse, string commune)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.CP = CP;
        this.sigle = sigle;
        this.statut = statut;
        this.adresse = adresse;
        this.commune = commune;
    }

    public string id;
    public string name;
    public string type;
    public string CP;
    public string sigle;
    public string statut;
    public string adresse;
    public string commune;
}
