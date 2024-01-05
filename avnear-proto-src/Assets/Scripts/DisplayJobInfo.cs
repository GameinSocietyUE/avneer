using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MatchingResponseData;

public class DisplayJobInfo : Displayable
{
    public static DisplayJobInfo Instance;

    private void Awake() {
        Instance = this;
    }

    public GameObject employement_Obj;
    public GameObject salary_Obj;
    public GameObject save_Obj;
    public Vector2Int duration;

    public GameObject formationButton;

    public List<FormationMinRequise> formations;
    public Dictionary<string, FormationMinRequise> formationsMap = new Dictionary<string, FormationMinRequise>();
    public List<FormationData> formationDatas = new List<FormationData>();
    public Dictionary<string, FormationData> formationDataMap = new Dictionary<string, FormationData>();

    [SerializeField] private DisplayJobDescription displayJobDescription;
    [SerializeField] private TextMeshProUGUI jobTitle;
    [SerializeField] private TextMeshProUGUI jobDurationMin;
    [SerializeField] private TextMeshProUGUI jobDurationMax;

    public string jobTitleStr;

    private int formationUpdateIndex = 0;

    public void Display() {

    }

    public void SetJobInfos(string jobTitle, string jobDesc, string jobDescFull, string jobDurationMin, string jobDurationMax, List<FormationMinRequise> formations)
    {
        formationButton.SetActive(false);
        formationsMap.Clear();
        formationDatas.Clear();
        formationDataMap.Clear();
        formationUpdateIndex = 0;
        DisplayFormations.Instance.formationDatas.Clear();
        DisplayFormations.Instance.displayFormations.Clear();
        //Todo: optimize, quick fix. Make a pool of gameobject to reuse instead of instantiate/destroy;
        foreach (Transform child in DisplayFormations.Instance.formationParent.transform)
        {
            Destroy(child.gameObject);
        }
        this.jobTitleStr = jobTitle;
        this.jobTitle.text = jobTitle;
        this.jobDurationMin.text = jobDurationMin;
        this.jobDurationMax.text = jobDurationMax;
        displayJobDescription.SetDesc(jobDesc, jobDescFull);
        this.formations = formations;
        foreach (FormationMinRequise formation in formations)
        {
            if (!formationsMap.ContainsKey(formation.id))
            {
                formationsMap.Add(formation.id, formation);
            }
        }
    }

    public void UpdateFormationData(TrainingsResponseData trainingsResponseData)
    {
        FormationMinRequise formation = formationsMap[trainingsResponseData.identifier];
        FormationData formationData = new FormationData(formation.id, formation.libelle, trainingsResponseData.parcoursup, trainingsResponseData.apprenticeship, trainingsResponseData.metadata.duree_formation, trainingsResponseData.establishments);
        formationDataMap.Add(formation.id, formationData);
        formationUpdateIndex++;
        if (formationUpdateIndex == formations.Count)
        {
            foreach (FormationMinRequise formationMinRequise in formations)
            {
                formationDatas.Add(formationDataMap[formationMinRequise.id]);
            }
            formationButton.SetActive(true);
        }
    }

    public bool AllFormationDataUpdated()
    {
        if (formationUpdateIndex == formations.Count - 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void SetJobTitle(string jobTitle)
    {
        this.jobTitle.text = jobTitle; 
    }

    public void SetJobDurationMin(string jobDurationMin)
    {
        this.jobDurationMin.text = jobDurationMin;
    }

    public void SetJobDurationMax(string jobDurationMax)
    {
        this.jobDurationMax.text = jobDurationMax;
    }

}
