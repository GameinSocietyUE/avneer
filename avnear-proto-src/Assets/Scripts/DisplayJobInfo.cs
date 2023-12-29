using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [SerializeField] private DisplayJobDescription displayJobDescription;
    [SerializeField] private TextMeshProUGUI jobTitle;
    [SerializeField] private TextMeshProUGUI jobDurationMin;
    [SerializeField] private TextMeshProUGUI jobDurationMax;
    private string jobDesc;
    private string jobDescFull;

    public void Display() {

    }

    public void SetJobInfos(string jobTitle, string jobDesc, string jobDescFull, string jobDurationMin, string jobDurationMax)
    {
        this.jobTitle.text = jobTitle;
        this.jobDurationMin.text = jobDurationMin;
        this.jobDurationMax.text = jobDurationMax;
        displayJobDescription.SetDesc(jobDesc, jobDescFull);
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
