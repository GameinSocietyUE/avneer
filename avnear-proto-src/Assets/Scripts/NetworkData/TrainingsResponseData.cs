using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingsResponseData
{
    public class NiveauEtudes
    {
        public string id;
        public string libelle;
    }

    public class TrainingMetadata
    {
        public string url_teaching;
        public NiveauEtudes niveau_etudes;
        public string duree_formation;
    }

    public class Establishment
    {
        public string identifier;
        public string name;
    }

    public string identifier;
    public string name;
    public string studyLevel;
    public string certificationLevel;
    public bool parcoursup;
    public bool apprenticeship;
    public TrainingMetadata metadata;
    public List<Establishment> establishments;
    public int bookmarksCount;
}
