using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    private AnswerContainer answerContainer;

    private string id;
    private string text;
    public bool selected = false;

    [SerializeField] private TMP_Text messageTmp;

    // Start is called before the first frame update
    void Start()
    {
        answerContainer = this.gameObject.transform.parent.parent.GetComponent<AnswerContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetId(string id)
    {
        this.id = id;
    }

    public string GetId()
    {
        return this.id;
    }

    public void SetText(string text)
    {
        this.text = text;
    }

    public string GetText()
    {
        return this.text;
    }

    public void SetMessage(string messageText)
    {
        messageTmp.text = messageText;
        this.text = messageText;
    }

    public void OnClick()
    {
        Debug.Log("OnClick: " + this.id + " " + this.text);
        this.selected = true;
        answerContainer.OnAnswerChange(this.id, this.text);
    }
}
