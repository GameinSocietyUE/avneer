using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Data;
using UnityEditor.VersionControl;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public class QuestionList
    {
        public QuestionData[] questions;
    }

    public QuestionList chatList = new QuestionList();

    [SerializeField] private NetworkManager _networkManager;

    [SerializeField] private GameObject waitingMessagePrefab;
    [SerializeField] private GameObject botMessagePrefab;
    [SerializeField] private GameObject userMessagePrefab;
    [SerializeField] private GameObject answerMessagePrefab;
    [SerializeField] private GameObject chatContent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject objectPoolContainer;
    [SerializeField] private GameObject emptyLastLine;

    //private List<MessageData> messagesList = new List<MessageData>();
    [SerializeField] private QuestionData[] currentChatList;
    private int chatIndex = 0; 

    private List<AnswerContainer> answerScriptList = new List<AnswerContainer>();

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.MakeMessageList();
        //Debug.Log(messagesList);
        //StartCoroutine(StartChat());

        StartCoroutine(_networkManager.GetChatData());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildChat(String jsonResponse)
    {
        Debug.Log("BuildChat: " + jsonResponse);
        chatList = JsonUtility.FromJson<QuestionList>(jsonResponse);
        currentChatList = chatList.questions;
        //StartCoroutine(StartChat());
        chatIndex = 0;
        SendNextMessage();
    }


    /*private void MakeMessageList()
    {
        string path = Application.persistentDataPath + "/chat.csv";
        Debug.Log(path);
        StreamReader sr = new StreamReader(path);
        sr.ReadLine();
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            messagesList.Add(this.GetMessageData(line));
        }
    }

    private MessageData GetMessageData(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            return null;
        }
        MessageData messageData = new MessageData();

        try
        {
            //Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //string[] values = CSVParser.Split(line);
            string[] values = line.Split(';');
            if (values != null && values.Length >= 4)
            {
                messageData.spawnTime = float.Parse(values[0].ToString());
                messageData.waitingAnimTime = float.Parse(values[1].ToString());
                messageData.messageType = int.Parse(values[2].ToString());
                messageData.messageText = values[3].ToString();
                if (values.Length >= 6 && values[4] != null && values[5] != null)
                {
                    messageData.messageWidth = float.Parse(values[4].ToString());
                    messageData.messageHeight = float.Parse(values[5].ToString());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error importing data from chat.csv line: " + line);
            Debug.Log(e.Message);
        }
        return messageData;
    }*/

    private void StartWaitingAnim()
    {
        waitingMessagePrefab.transform.SetParent(chatContent.transform);
        waitingMessagePrefab.SetActive(true);
        StartCoroutine(ForceScrollDown());
    }

    private void StopWaitingAnim()
    {
        waitingMessagePrefab.SetActive(false);
        waitingMessagePrefab.transform.SetParent(objectPoolContainer.transform);
    }

    private void SendMessageToChat(GameObject messagePrefab, string messageText, float messageWidth, float messageHeight)
    {
        GameObject mess = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        Message messageScript = mess.GetComponent<Message>();
        messageScript.SetMessage(messageText);
        messageScript.SetSize(messageWidth, messageHeight);
        StartCoroutine(ForceScrollDown());
    }

    private void SendAnswerToChat(GameObject messagePrefab, string id, List<QuestionData> answers)
    {
        GameObject mess = Instantiate(answerMessagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        AnswerContainer ansScript = mess.GetComponent<AnswerContainer>();
        answerScriptList.Add(ansScript);
        ansScript.CreateAnswerList(id, answers);
        StartCoroutine(ForceScrollDown());
    }

    /*private IEnumerator StartChat()
    {
        foreach (MessageData message in messagesList)
        {
            if (message != null)
            {
                if (message.spawnTime > 0)
                {
                    yield return new WaitForSeconds(message.spawnTime);
                }
                if (message.messageType == 1)
                {
                    this.StartWaitingAnim();
                    yield return new WaitForSeconds(message.waitingAnimTime);
                    this.StopWaitingAnim();
                    this.SendMessageToChat(botMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
                else if (message.messageType == 2)
                {
                    this.SendMessageToChat(userMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
                else
                {
                    Debug.Log("Wrong message type for Message: " + message.messageText);
                }
            }
        }
        Debug.Log("End of chat");
        //Spawn send result button
        emptyLastLine.transform.SetParent(chatContent.transform);
        StartCoroutine(ForceScrollDown());
    }*/

    public void SelectAnswer(GameObject answerContainer, AnswerContainer answerScript)
    {
        Debug.Log("SelectAnswer: " + answerScript.selectedLabel);
        string messageText = answerScript.selectedLabel;
        answerContainer.SetActive(false);
        this.SendMessageToChat(userMessagePrefab, messageText, 400f, 115f);
        SendNextMessage();
    }

    public void SendNextMessage()
    {
        if (chatIndex < currentChatList.Length - 1)
        {
            StartCoroutine(SendMessage(currentChatList[chatIndex]));
            chatIndex++;
        }
        else
        {
            Debug.Log("End of chat");
            //Spawn send result button
            emptyLastLine.transform.SetParent(chatContent.transform);
            StartCoroutine(ForceScrollDown());
        }  
    }

    private IEnumerator SendMessage(QuestionData message)
    {
        if (message != null)
        {
            if (message.metadata != null)
            {
                //
            }
            //hard coded
            yield return new WaitForSeconds(1f);
            this.StartWaitingAnim();
            //yield return new WaitForSeconds(message.waitingAnimTime);
            yield return new WaitForSeconds(1f);
            this.StopWaitingAnim();
            this.SendMessageToChat(botMessagePrefab, message.label, 620f, 115f);

            if (message.answers != null && message.answers.Count > 0)
            {
                yield return new WaitForSeconds(1f);
                this.SendAnswerToChat(answerMessagePrefab, message.id, message.answers);
            }
            else
            {
                SendNextMessage();
            }
        }
    }

   /* private IEnumerator StartChat()
    {
        foreach (QuestionData message in chatList.questions)
        {
            if (message != null)
            {
                if (message.metadata != null)
                {
                    //
                }
                //hard coded
                yield return new WaitForSeconds(1f);
                this.StartWaitingAnim();
                //yield return new WaitForSeconds(message.waitingAnimTime);
                yield return new WaitForSeconds(1f);
                this.StopWaitingAnim();
                this.SendMessageToChat(botMessagePrefab, message.label, 620f, 115f);

                if (message.answers != null && message.answers.Count > 0)
                {
                    //send user answer
                    //this.SendMessage(userMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
            }
        }
        Debug.Log("End of chat");
        //Spawn send result button
        emptyLastLine.transform.SetParent(chatContent.transform);
        StartCoroutine(ForceScrollDown());
    }*/

    private IEnumerator ForceScrollDown()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}
