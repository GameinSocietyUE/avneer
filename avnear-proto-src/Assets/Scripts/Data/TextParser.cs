using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class TextParser : MonoBehaviour
{
    public string linkReplace = "export?format=csv";

    public string url;
    public UnityEngine.Object targetAsset;

    public bool parsingDebug = false;

    public virtual void Awake()
    {
        parsingDebug = false;

        ParseCSV();
    }

    #region parse
    public virtual void ParseCSV()
    {
        string fileName = targetAsset.name + ".csv";

        TextAsset textAsset = Resources.Load<TextAsset>("TextAssets/" + targetAsset.name);
        fgCSVReader.LoadFromString(textAsset.text, new fgCSVReader.ReadLineDelegate(GetCell));
    }

    public virtual void GetCell(int row_index, List<string> cells)
    {
        
    }
    #endregion


#if UNITY_EDITOR
    public void DownloadCSVs()
    {
        StartCoroutine(DownloadCSVsCoroutine());
    }
    IEnumerator DownloadCSVsCoroutine()
    {
        yield return DownloadCSV(url, targetAsset);

        yield return new WaitForEndOfFrame();
    }
    IEnumerator DownloadCSV(string tmpUrl, UnityEngine.Object asset)
    {
        tmpUrl = tmpUrl.Replace("edit", linkReplace);

        Debug.Log("Sending request fir " +asset.name + "...");

        float timeOut = Time.realtimeSinceStartup + 10f;

        UnityWebRequest www = UnityWebRequest.Get(tmpUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error when requesting CSV file (responseCode:" + www.responseCode + ")");
            Debug.LogError(www.error);
        }
        else
        {
            string filepath = AssetDatabase.GetAssetPath(asset);
            System.IO.File.WriteAllText(filepath, www.downloadHandler.text);
            Undo.RecordObject(asset, "Downloaded CSV from distant file");

            Debug.Log("Finished downloading : " + asset.name);

            AssetDatabase.ImportAsset(filepath);
        }

    }
#endif
}
