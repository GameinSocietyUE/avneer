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

    public CanvasManager canvasManager;

    public float messageDuration = 1f;
    public float timer;
    bool timerActive = false;

    //private List<MessageData> messagesList = new List<MessageData>();
    [SerializeField] private QuestionData[] currentChatList;
    private int chatIndex = 0;

    public string loginToken = "";
    private string username;
    private string password;

    private List<AnswerContainer> answerScriptList = new List<AnswerContainer>();

    private Dictionary<string, List<string>> interactions = new Dictionary<string, List<string>>();

    public bool runChat = true;

    public QuestionData currentMessage = null;

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

        //StartCoroutine(_networkManager.GetChatData());
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive) {
            timer += Time.deltaTime;
        }
    }

    public bool IsConnected()
    {
        return (this.loginToken != null && this.loginToken != "");
    }

    public void Login()
    {
        // _networkManager.DoLogin("+33642424242", "Testpassword42!");
        this.username = DisplayLogin.Instance.GetUserName();
        this.password = DisplayLogin.Instance.GetPassword();
        _networkManager.DoLogin(this.username, this.password);
    }

    public void Reconnect()
    {
        _networkManager.DoLogin(this.username, this.password);
    }

    public void LoginSuccess(string token)
    {
        this.loginToken = token;
        canvasManager.DisplayPage(CanvasManager.Page.Welcome_Connected);
    }

    public void SendChatData()
    {
        _networkManager.PostChatData(interactions);
    }

    public void ChatResult(MatchingResponseData matchingResponseData)
    {
        Debug.Log("ChatResult");
        //List<MatchingResponseData.Recommendation> recommendations = matchingResponseData.recommendations;
        Debug.Log(matchingResponseData);
        Debug.Log(matchingResponseData.hash);
        Debug.Log(matchingResponseData.recommendation);
        Debug.Log(matchingResponseData.recommendation.Count);
        MatchingResponseData.Recommendation recommendation = matchingResponseData.recommendation[0];
        Debug.Log(recommendation.score + " " + recommendation.identifier + " " + recommendation.metadata.libelle_masculin + recommendation.metadata.formats_courts["format_court"][0].descriptif);
        //(string jobTitle, string jobDesc, string jobDescFull, string jobDurationMin, string jobDurationMax)
        DisplayJobInfo.Instance.SetJobInfos(recommendation.metadata.libelle_masculin, recommendation.metadata.formats_courts["format_court"][0].descriptif,
            recommendation.metadata.formats_courts["format_court"][1].descriptif, recommendation.metadata.niveau_acces_min.libelle, recommendation.metadata.niveau_acces_min.libelle);
        DisplayMessage.Instance.Hide();
        DisplayChat.Instance.Hide();
        canvasManager.DisplayPage(CanvasManager.Page.JobInfo);
    }

    public void BuildChat(String jsonResponse)
    {
        Debug.Log("BuildChat: " + jsonResponse);
        chatList = JsonUtility.FromJson<QuestionList>(jsonResponse);
        currentChatList = chatList.questions;
        chatIndex = 0;
        interactions.Clear();
        StartCoroutine(ChatCoroutine());
    }

    public void PauseChat()
    {
        runChat = false;
    }

    public void RunChat()
    { 
        if (currentMessage.answers != null && currentMessage.answers.Count > 0)
        {
            Debug.Log("Current message answer, displayAnswer");
            if (!DisplayAnswers.Instance.visible) //Fully visible with all questions
            {
                StartCoroutine(ResumeDisplayAnswer());
            }
        }
        else
        {
            DisplayChat.Instance.StopWaitingAnim(currentMessage.id);
            runChat = true;
        }   
    }

    private IEnumerator ResumeDisplayAnswer()
    {
        yield return new WaitForSeconds(1.5f);
        DisplayAnswers.Instance.Display(currentMessage);
    }

    public void ResetChat()
    {

    }

    public bool IsChatRunning()
    {
        return runChat;
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
        /*waitingMessagePrefab.transform.SetParent(chatContent.transform);
        waitingMessagePrefab.SetActive(true);
        StartCoroutine(ForceScrollDown());*/
    }

    private void StopWaitingAnim()
    {
        /*waitingMessagePrefab.SetActive(false);
        waitingMessagePrefab.transform.SetParent(objectPoolContainer.transform);*/
    }

    private void SendMessageToChat(GameObject messagePrefab, string messageText, float messageWidth, float messageHeight)
    {
        //DisplayChat.Instance.AddMessage(messageText, DisplayChat.Side.Bot);

        /*GameObject mess = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        Message messageScript = mess.GetComponent<Message>();
        messageScript.SetMessage(messageText);
        messageScript.SetSize(messageWidth, messageHeight);
        StartCoroutine(ForceScrollDown());*/
    }

    private void SendAnswerToChat(GameObject messagePrefab, string id, QuestionData question)
    {
        DisplayAnswers.Instance.Display(question);
        /*GameObject mess = Instantiate(answerMessagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        AnswerContainer ansScript = mess.GetComponent<AnswerContainer>();
        answerScriptList.Add(ansScript);
        ansScript.CreateAnswerList(id, answers);
        StartCoroutine(ForceScrollDown());*/
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
    public void SelectAnswer(string questionId, QuestionData answer) {
        Debug.Log("SELECT ANSWER QuestionId:" + questionId + " answerId: " + answer.id + " " + answer.label + " " + answer.button);
        if (!interactions.ContainsKey(questionId))
        {
            List<string> answerIdList = new List<string>();
            answerIdList.Add(answer.id);
            interactions.Add(questionId, answerIdList);
            DisplayAnswers.Instance.FadeOut();
            DisplayChat.Instance.AddMessage(answer, DisplayChat.Side.User);
            runChat = true;
        }
    }

    public void SelectAnswer(GameObject answerContainer, AnswerContainer answerScript)
    {
        /*Debug.Log("SelectAnswer: " + answerScript.selectedLabel);
        string messageText = answerScript.selectedLabel;
        answerContainer.SetActive(false);
        this.SendMessageToChat(userMessagePrefab, messageText, 400f, 115f);
        SendNextMessage();*/
    }

    public void SendNextMessage()
    {
        var message = currentChatList[chatIndex];
        DisplayChat.Instance.AddMessage(message, DisplayChat.Side.Bot);
        chatIndex++;
        return;
        /*if (chatIndex < currentChatList.Length - 1)
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
        }*/ 
    }

    private IEnumerator ChatCoroutine() {

        while ( chatIndex < currentChatList.Length - 1) {
            yield return new WaitUntil(() => runChat);

            // display bot message
            var message = currentChatList[chatIndex];
            this.currentMessage = message;
            DisplayChat.Instance.AddMessage(message, DisplayChat.Side.Bot);

            yield return new WaitUntil(() => runChat);

            // wait for user answer
            if (message.answers != null && message.answers.Count > 0) {
                yield return new WaitForSeconds(3f);
                DisplayAnswers.Instance.Display(message);
                //while (DisplayAnswers.Instance.visible)
                //    yield return null;
                runChat = false;
                yield return new WaitForSeconds(1f);
            }

            StartTimer();
            // wait for input or time
            while (timer < messageDuration) {
                //if (Input.GetMouseButtonDown(0))
                //    break;
                yield return null;

                //Debug.Log($"timer : {timer}");
                //Debug.Log($"pressing mouse : {Input.GetMouseButtonDown(0)}");
            }
            yield return new WaitForEndOfFrame();
            EndTimer();
            ++chatIndex;
        }

        if (chatIndex == currentChatList.Length - 1)
        {
            Debug.Log($"finished chat");
            DisplayMessage.Instance.Display("Chat ended. Waiting for result ...");
            this.SendChatData();
        }
    }

    void StartTimer() {
        timer = 0f;
        timerActive = true;
    }
    void EndTimer() {
        timerActive = false;
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
            /*yield return new WaitForSeconds(1f);
            this.StartWaitingAnim();
            /yield return new WaitForSeconds(message.waitingAnimTime);
            yield return new WaitForSeconds(1f);
            this.StopWaitingAnim();*/
            yield return new WaitForSeconds(1f);

            this.SendMessageToChat(botMessagePrefab, message.label, 620f, 115f);

            yield return new WaitForSeconds(1f);

            if (message.answers != null && message.answers.Count > 0)
            {
                yield return new WaitForSeconds(1f);
                this.SendAnswerToChat(answerMessagePrefab, message.id, message);
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
