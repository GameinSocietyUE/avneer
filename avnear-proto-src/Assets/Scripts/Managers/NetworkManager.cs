using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private string _apiKey;
    [SerializeField] private string _uri;

    public IEnumerator GetChatData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_uri))
        {
            webRequest.SetRequestHeader("X-API-KEY", _apiKey);
            yield return webRequest.SendWebRequest();

            string[] pages = _uri.Split('/');
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
                    GameManager.Instance.BuildChat(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
