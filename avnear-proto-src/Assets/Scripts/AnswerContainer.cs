using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerContainer : MonoBehaviour
{
    public string id;
    public string selectedId;
    public string selectedLabel;
    public GameObject answerButtonPrefab;
    public GameObject answerButtonPanel;
    public List<GameObject> answerButtons = new List<GameObject>();
    private List<AnswerButton> answerButtonsScript = new List<AnswerButton>();
    private List<QuestionData> answersList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAnswerList(string id, List<QuestionData> answers)
    {
        this.id = id;
        this.answersList = answers;
        foreach (QuestionData message in answersList)
        {
            GameObject ansButton = Instantiate(answerButtonPrefab, Vector3.zero, Quaternion.identity, answerButtonPanel.transform);
            AnswerButton ansScript = ansButton.GetComponent<AnswerButton>();
            ansScript.SetId(message.id);
            ansScript.SetMessage(message.label);
            answerButtons.Add(ansButton);
            answerButtonsScript.Add(ansScript);
        }
    }

    public void OnAnswerChange(string id, string label)
    {
        Debug.Log("OnAnswerChange: " + selectedId + " " + id + " " + label);
        if (selectedId == null || selectedId == "")
        {
            this.selectedId = id;
            this.selectedLabel = label;
            GameManager.Instance.SelectAnswer(this.gameObject, this);
        }
        else
        {
            foreach (AnswerButton ansScript in answerButtonsScript)
            {
                if (ansScript.GetId().Equals(id))
                {
                    ansScript.selected = true;
                }
                else
                {
                    ansScript.selected = false;
                }
            }
            this.selectedId = id;
            this.selectedLabel= label;
        }
    }
}
