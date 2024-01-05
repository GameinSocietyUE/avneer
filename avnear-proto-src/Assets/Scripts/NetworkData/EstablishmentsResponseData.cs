using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EstablishmentsResponseData
{
    public class EstablishmentsMetadata
    {
        public string CP;
        public string sigle;
        public string statut;
        public string adresse;
        public string commune;
        //public string téléphone;
        //public string type d'établissement;
    }

    public string identifier;
    public string name;
    public string region;
    public string type;
    public EstablishmentsMetadata metadata;
}
