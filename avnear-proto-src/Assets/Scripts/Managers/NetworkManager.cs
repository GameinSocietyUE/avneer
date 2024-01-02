using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private string _apiKey;
    [SerializeField] private string _uriGetChat;

    private Dictionary<string, string> Endpoints = new Dictionary<string, string>
    {
        {"loginUrl", "https://api.avneer.com/authentication"},
        {"getChatUrl", "https://api.avneer.com/scenarios/"},
        {"postChatData", "https://api.avneer.com/matchings"},
        {"getTraining", "https://api.avneer.com/trainings/"}
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

    private IEnumerator GetRequest(string uri, string responseCall)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("X-API-KEY", _apiKey);
            yield return webRequest.SendWebRequest();
            string response = webRequest.downloadHandler.text;
            Debug.Log(webRequest.result + " response: " + response);
            if (webRequest.result.Equals(UnityWebRequest.Result.Success))
            {
                HandleResponse(responseCall, response);
            }
            else if (webRequest.result.Equals(UnityWebRequest.Result.ProtocolError) || webRequest.result.Equals(UnityWebRequest.Result.DataProcessingError))
            {
                HandleError(responseCall, response);
            }
            else
            {
                //retry request
            }


            webRequest.Dispose();
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
        else if (webRequest.result.Equals(UnityWebRequest.Result.ProtocolError) || webRequest.result.Equals(UnityWebRequest.Result.DataProcessingError))
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
        ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(errorData);
        //ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorData);
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
        else if (responseCall.Equals("GETCHAT"))
        {
            GameManager.Instance.BuildChat(responseData);
        }
        else if (responseCall.Equals("POSTCHAT"))
        {
            Debug.Log("Response from POSTCHAT");
            Debug.Log(responseData);
            responseData = responseData.Replace("\"formations_min_requise\":null", "\"formations_min_requise\":{\"formation_min_requise\":[]}");
            Debug.Log(responseData);
            //Bug with formations_min_requise when its not a list (only one formation)
            MatchingResponseData matchingResponseData = JsonConvert.DeserializeObject<MatchingResponseData>(responseData, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            GameManager.Instance.ChatResult(matchingResponseData);
        }
        else if (responseCall.Equals("GETTRAINING"))
        {
            Debug.Log("Response from GETTRAINING: " + responseData);
            TrainingsResponseData trainingsResponseData = JsonConvert.DeserializeObject<TrainingsResponseData>(responseData, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            GameManager.Instance.GetTrainingDataSuccess(trainingsResponseData);
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

    public void GetChatScenario(string id)
    {
        StartCoroutine(GetRequest(Endpoints["getChatUrl"] + id, "GETCHAT"));
    }

    public void GetTrainingData(string id)
    {
        StartCoroutine(GetRequest(Endpoints["getTraining"] + id, "GETTRAINING"));
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
