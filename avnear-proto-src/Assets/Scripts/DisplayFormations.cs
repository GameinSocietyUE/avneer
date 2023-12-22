using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DisplayFormations : Displayable {
    public static DisplayFormations Instance;

    public Transform filterParent;
    public Transform formationParent;
    public FilterButton filterPrefab;
    public DisplayFormation formationPrefab;
    public List<FilterButton> filterButtons;
    public List<FormationData> formationDatas = new List<FormationData>();
    public List<DisplayFormation> displayFormations = new List<DisplayFormation>();

    public TextMeshProUGUI uiText_Title;

    private void Awake() {
        Instance = this;
    }

    public override void Show() {
        base.Show();
        Display("formations", formationDatas, new List<string> { "filtre 1", "filtre 2", });
    }

    public void Display(string name, List<FormationData> datas, List<string> filterNames) {
        uiText_Title.text = name;
        for (int i = 0; i < filterNames.Count; i++) {
            if (i >= filterButtons.Count)
                filterButtons.Add(Instantiate(filterPrefab, filterParent));
            filterButtons[i].Display(filterNames[i]);
        }
        SelectFilter(filterButtons[0], filterNames[0]);

        for (int i = 0;i < datas.Count;i++) {
            if (i >= displayFormations.Count) {
                displayFormations.Add(Instantiate(formationPrefab, formationParent));
            }
            displayFormations[i].Display(datas[i]);
        }
    }

    public void SelectFilter(FilterButton filterButton, string name) {
        foreach (var button in filterButtons)
            button.Deselect();
        filterButton.Select();
        Debug.Log($"selected filter : {name}");
    }
}
    
