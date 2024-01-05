using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;
using static MatchingResponseData;

public class ChatMessageResult : MonoBehaviour
{
    public static ChatMessageResult Instance;

    public TextMeshProUGUI uiText_FormationName;
    public MatchingResponseData.Recommendation currentRecommendation;

    private List<MatchingResponseData.Recommendation> recommendations;
    private int index;
    private int recommendationsCount;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        index = 0;
    }

    public void SetRecommendations(List<MatchingResponseData.Recommendation> recommendations)
    {
        this.recommendations = recommendations;
        if (this.recommendations != null & this.recommendations.Count > 0)
        {
            recommendationsCount = this.recommendations.Count;
            this.currentRecommendation = this.recommendations[0];
            index = 0;
            this.UpdateFormationData();
        }
    }

    public MatchingResponseData.Recommendation GetCurrentRecommendation()
    {
        return currentRecommendation;
    }

    public void ClickArrow(string arrow)
    {
        if (this.recommendations != null & this.recommendations.Count > 0)
        {
            if (arrow.Equals("left"))
            {
                if (index == 0)
                {
                    index = this.recommendations.Count - 1;
                }
                else
                {
                    index--;
                }
                this.currentRecommendation = this.recommendations[index];
                this.UpdateFormationData();
            }
            else if (arrow.Equals("right"))
            {
                if (index < recommendationsCount - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                this.currentRecommendation = this.recommendations[index];
                this.UpdateFormationData();
            }
        }
    }

    public void ClickInspectFormation()
    {
        GameManager.Instance.InspectRecommendation(this.currentRecommendation);
    }

    private void UpdateFormationData()
    {
        if (this.currentRecommendation != null)
        {
            int gender = GameManager.Instance.GetProfileGender();
            string jobLibelle = gender == 0 ? currentRecommendation.metadata.libelle_masculin : currentRecommendation.metadata.libelle_feminin;

            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            //float score = float.Parse(currentRecommendation.score, NumberStyles.Any, ci);
            int roundScore = Mathf.RoundToInt(float.Parse(currentRecommendation.score, NumberStyles.Any, ci));
            uiText_FormationName.text = jobLibelle + "<br>Compatibilité " + roundScore + "%";
        }
    }
}
