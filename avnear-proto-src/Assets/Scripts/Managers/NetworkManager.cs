using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private string _apiKey;
    [SerializeField] private string _uriGetChat;
    [SerializeField] private string _uriSendResult;

    private Dictionary<string, string> Endpoints = new Dictionary<string, string>
    {
        {"loginUrl", "https://api.avneer.com/authentication"},
        {"getChatUrl", "https://api.avneer.com/scenarios/"},
        {"postChatData", "https://api.avneer.com/matchings"}
    };

    public IEnumerator GetChatData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_uriGetChat))
        {
            webRequest.SetRequestHeader("X-API-KEY", _apiKey);
            yield return webRequest.SendWebRequest();

            string[] pages = _uriGetChat.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    GameManager.Instance.BuildChat(webRequest.downloadHandler.text);
                    webRequest.Dispose();
                    break;
            }
        }
    }

    public IEnumerator PostChatData()
    {
        string postData = "";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(_uriSendResult, postData, "application/json"))
        {
            webRequest.SetRequestHeader("X-API-KEY", _apiKey);
            yield return webRequest.SendWebRequest();

            string[] pages = _uriGetChat.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    //GameManager.Instance.BuildChat(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    private IEnumerator PostJson(string uri, object jsonData, string responseCall)
    {
        //string postData = JsonUtility.ToJson(jsonData);
        string postData = JsonConvert.SerializeObject(jsonData);
        Debug.Log("URI: " + uri + " POST data: " + postData);
        using UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");
        webRequest.SetRequestHeader("X-API-KEY", _apiKey);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        byte[] rawJsonData = Encoding.UTF8.GetBytes(postData);
        webRequest.uploadHandler = new UploadHandlerRaw(rawJsonData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();        
        string response = webRequest.downloadHandler.text;
        Debug.Log(webRequest.result + " response: " + response);
        if (webRequest.result.Equals(UnityWebRequest.Result.Success))
        {
            HandleResponse(responseCall, response);
        }
        else if (webRequest.result.Equals(UnityWebRequest.Result.ProtocolError))
        {
            HandleError(responseCall, response);
        }
        else
        {
            //retry request
        }
            

        webRequest.Dispose();

        /*switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                string response = webRequest.downloadHandler.text;
                HandleResponse(responseCall, response);
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                HandleError(responseCall, webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                HandleError(responseCall, webRequest.error);
                Debug.LogError("HTTP Error: " + webRequest.error);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }*/
    }

    public void HandleError(string responseCall, string errorData)
    {
        Debug.Log("Handle error for: " + responseCall + " error: " + errorData);
        //ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(errorData);
        ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorData);
        if (responseCall.Equals("LOGIN"))
        {
            if (errorResponse != null && errorResponse.message != null && errorResponse.message.Equals("Identifiants invalides."))
            {
                Debug.Log("Identifiants invalides.");
                //Display error
                //DisplayLogin.Instance.ShowError(errorResponse.message);
            }
        }
        else if (responseCall.Equals("POSTCHAT"))
        {
            Debug.Log("Error with post chat matching: " + errorResponse.code + " " + errorResponse.message);
        }
    }

    public void HandleResponse(string responseCall, string responseData)
    {
        Debug.Log("Handle Response: " + responseData);
        if (responseCall.Equals("LOGIN"))
        {
            string token = JsonUtility.FromJson<UserLoginRespose>(responseData).token;
            Debug.Log(token);
            GameManager.Instance.LoginSuccess(token);
        }
        else if (responseCall.Equals("POSTCHAT"))
        {
            Debug.Log("Response from POSTCHAT");
            Debug.Log(responseData);
            GameManager.Instance.ChatResult();
        }
    }

    public void DoLogin(string username, string password)
    {
        UserLogin loginData = new UserLogin(username, password);
        StartCoroutine(PostJson(Endpoints["loginUrl"], loginData, "LOGIN"));
    }

    public void PostChatData(Dictionary<string, List<string>> interactions)
    {
        MatchingData matchingData = new MatchingData();
        matchingData.scenarioId = "1ee89ec4-1209-6df4-a407-f328a7e43d7c";
        matchingData.interactions = interactions;
        StartCoroutine(PostJson(Endpoints["postChatData"], matchingData, "POSTCHAT"));
    }

    class UserLogin
    {
        public string username;
        public string password;

        public UserLogin(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    class UserLoginRespose
    {
        public string token;
    }

    class ErrorResponse
    {
        public string code;
        public string message;
    }

    class MatchingData
    {
        public string scenarioId;
        public Dictionary<string, List<string>> interactions;
    }

}
