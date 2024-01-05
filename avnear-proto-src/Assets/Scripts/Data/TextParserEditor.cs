using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(TextParser), true)]
public class TextParserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TextParser myScript = (TextParser)target;

        if (GUILayout.Button("Download Data"))
        {
            myScript.DownloadCSVs();
        }

        if (GUILayout.Button("Parse Data"))
        {
            myScript.parsingDebug = false;
            myScript.ParseCSV();
        }

        if (GUILayout.Button("Parse Data DEBUG"))
        {
            myScript.parsingDebug = true;
            myScript.ParseCSV();
        }

        DrawDefaultInspector();
    }
}
#endif