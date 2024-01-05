using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayEstablishments : Displayable
{
    public static DisplayEstablishments Instance;

    public Transform filterParent;
    public Transform establishmentParent;
    public FilterButton filterPrefab;
    public DisplayEstablishment establishmentPrefab;
    public List<FilterButton> filterButtons;
    public List<EstablishmentData> establishmentDatas = new List<EstablishmentData>();
    public List<DisplayEstablishment> displayEstablishment = new List<DisplayEstablishment>();

    public TextMeshProUGUI uiText_Title;

    private List<TrainingsResponseData.Establishment> establishments = new List<TrainingsResponseData.Establishment>();
    private Dictionary<string, EstablishmentsResponseData> establishmentsMap = new Dictionary<string, EstablishmentsResponseData>();
    private string formationName;
    private int indexUpdate = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SetData(string formationName, List<TrainingsResponseData.Establishment> establishments)
    {
        this.Clear();
        this.formationName = formationName;
        uiText_Title.text = "Chargement...";
        this.establishments = establishments;
        if (establishments != null && establishments.Count > 0)
        {
            foreach (TrainingsResponseData.Establishment establishment in establishments)
            {
                establishmentsMap.Add(establishment.identifier, null);
            }
        }
        GameManager.Instance.InspectEstablishments(establishments);
    }

    public void UpdateEstablishmentData(EstablishmentsResponseData establishmentsResponseData)
    {
        if (establishmentsMap.ContainsKey(establishmentsResponseData.identifier))
        {
            establishmentsMap.Remove(establishmentsResponseData.identifier);
            establishmentsMap.Add(establishmentsResponseData.identifier, establishmentsResponseData);
            indexUpdate++;
        }
        if (indexUpdate == establishments.Count)
        {
            AllDataCollected();
            uiText_Title.text = this.formationName;
        }
    }

    private void AllDataCollected()
    {
        foreach (EstablishmentsResponseData establishment in establishmentsMap.Values)
        {
            EstablishmentData establishmentData = new EstablishmentData(establishment.identifier, establishment.name, "", establishment.metadata.CP.ToString(), establishment.metadata.sigle, establishment.metadata.statut, establishment.metadata.adresse, establishment.metadata.commune);
            establishmentDatas.Add(establishmentData);
        }
        this.Display(establishmentDatas, new List<string> { "public", "privé", });
    }

    public override void Show()
    {
        base.Show();
    }

    public void Display(List<EstablishmentData> datas, List<string> filterNames)
    {
        for (int i = 0; i < filterNames.Count; i++)
        {
            if (i >= filterButtons.Count)
                filterButtons.Add(Instantiate(filterPrefab, filterParent));
            filterButtons[i].Display(filterNames[i]);
        }
        SelectFilter(filterButtons[0], filterNames[0]);

        for (int i = 0; i < datas.Count; i++)
        {
            if (i >= displayEstablishment.Count)
            {
                displayEstablishment.Add(Instantiate(establishmentPrefab, establishmentParent));
            }
            displayEstablishment[i].Display(datas[i]);
        }
    }

    public void Clear()
    {
        indexUpdate = 0;
        establishmentDatas.Clear();
        displayEstablishment.Clear();
        establishments.Clear();
        establishmentsMap.Clear();
        //Todo: optimize, quick fix. Make a pool of gameobject to reuse instead of instantiate/destroy;
        foreach (Transform child in establishmentParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectFilter(FilterButton filterButton, string name)
    {
        foreach (var button in filterButtons)
            button.Deselect();
        filterButton.Select();
        Debug.Log($"selected filter : {name}");
    }
}
