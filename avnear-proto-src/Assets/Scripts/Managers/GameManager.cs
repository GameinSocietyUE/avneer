using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using System;
using System.Text.RegularExpressions;

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

    [SerializeField] private GameObject waitingMessagePrefab;
    [SerializeField] private GameObject botMessagePrefab;
    [SerializeField] private GameObject userMessagePrefab;
    [SerializeField] private GameObject chatContent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject objectPoolContainer;
    [SerializeField] private GameObject emptyLastLine;

    private List<MessageData> messagesList = new List<MessageData>();

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.MakeMessageList();
        Debug.Log(messagesList);
        StartCoroutine(StartChat());
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void MakeMessageList()
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
    }

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

    private void SendMessage(GameObject messagePrefab, string messageText, float messageWidth, float messageHeight)
    {
        GameObject mess = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        //mess.transform.SetParent(chatContent.transform, false);
        Message messageScript = mess.GetComponent<Message>();
        messageScript.SetMessage(messageText);
        messageScript.SetSize(messageWidth, messageHeight);
        StartCoroutine(ForceScrollDown());
    }

    private IEnumerator StartChat()
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
                    this.SendMessage(botMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
                else if (message.messageType == 2)
                {
                    this.SendMessage(userMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
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
    }

    private IEnumerator ForceScrollDown()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}
