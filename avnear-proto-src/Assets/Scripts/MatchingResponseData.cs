using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchingResponseData
{
    public class FormatCourt
    {
        public string type;
        public string libelle;
        public string descriptif;
    }

    public class NiveauAccesMin
    {
        public string id;
        public string libelle;
    }

    public class FormationMinRequise
    {
        public string id;
        public string libelle;
    }

    public class MatchMetadata
    {
        public string resume;
        //public string synonymes;
        public string competences;
        public string first_salary;
        public Dictionary<string, List<FormatCourt>> formats_courts;
        public string nature_travail;
        public string libelle_feminin;
        public string libelle_masculin;
        public NiveauAccesMin niveau_acces_min;
        //public Dictionary<string, List<FormationMinRequise>> formations_min_requise;
    }

    public class Recommendation
    {
        public string score;
        public string identifier;
        public string name;
        public MatchMetadata metadata;
    }

    public string hash;
    public List<Recommendation> recommendation;
}
