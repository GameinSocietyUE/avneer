using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("click parcours: formations count " + DisplayJobInfo.Instance.formations.Count);
        //Dictionary<string, MatchingResponseData.FormationMinRequise> formationsMap = DisplayJobInfo.Instance.formationsMap;
        List<FormationData> formationDatas = DisplayJobInfo.Instance.formationDatas;
        DisplayFormations.Instance.formationDatas = formationDatas;
        DisplayFormations.Instance.jobName = DisplayJobInfo.Instance.jobTitleStr;
        GameManager.Instance.canvasManager.DisplayPage(CanvasManager.Page.Formations);
    }
}
