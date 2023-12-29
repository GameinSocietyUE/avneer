using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchingResponseData
{
    public class recommendation
    {
        public string score;
        public string identifier;
        public string name;
    }

    public string hash;
}
