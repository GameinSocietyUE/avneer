using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayFormations : Displayable {
    public static DisplayFormations Instance;

    public Transform filterParent;
    public FilterButton prefab;
    public List<FilterButton> filterButtons;
    public List<FormationData> formationDatas = new List<FormationData>();

    public TextMeshProUGUI uiText_Title;

    private void Awake() {
        Instance = this;
    }

    public void Display(string name, List<FormationData> datas, List<string> filterNames) {
        FadeIn();
        uiText_Title.text = name;
        for (int i = 0; i < filterNames.Count; i++) {
            if ( i >= filterButtons.Count )
                Instantiate(prefab, filterParent);
            filterButtons[i].Display(filterNames[i]);
        }
        SelectFilter(filterButtons[0], filterNames[0]);
    }

    public void SelectFilter(FilterButton filterButton, string name) {
        foreach (var button in filterButtons)
            button.Deselect();
        filterButton.Select();
        Debug.Log($"selected filter : {name}");
    }
}
    
