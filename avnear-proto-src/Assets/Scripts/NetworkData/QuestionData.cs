using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestionData
{
    public class Metadata
    {
        public float spawnTime;
        public float waitingAnimTime;
        public float messageWidth;
        public float messageHeight;
    }

    public string id;
    public string label;
    public string button;
    public List<Metadata> metadata;
    public List<QuestionData> answers;
}
