using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Data;
using UnityEditor.VersionControl;
using static MatchingResponseData;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public class QuestionList
    {
        public QuestionData[] questions;
    }

    public class ProfileData
    {
        public string username;
        public int gender;
    }

    public QuestionList chatList = new QuestionList();

    [SerializeField] private NetworkManager _networkManager;

    [SerializeField] private GameObject waitingMessagePrefab;
    [SerializeField] private GameObject botMessagePrefab;
    [SerializeField] private GameObject userMessagePrefab;
    [SerializeField] private GameObject answerMessagePrefab;
    [SerializeField] private GameObject chatContent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject objectPoolContainer;
    [SerializeField] private GameObject emptyLastLine;

    public CanvasManager canvasManager;

    public float messageDuration = 1f;
    public float timer;
    bool timerActive = false;

    //private List<MessageData> messagesList = new List<MessageData>();
    [SerializeField] private QuestionData[] currentChatList;
    private int chatIndex = 0;

    public string loginToken = "";
    private string username;
    private string password;

    public ProfileData profileData = new ProfileData();

    private List<AnswerContainer> answerScriptList = new List<AnswerContainer>();

    private Dictionary<string, List<string>> interactions = new Dictionary<string, List<string>>();

    public bool runChat = true;

    public QuestionData currentMessage = null;

    private bool waitingForAnswer = false;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.MakeMessageList();
        //Debug.Log(messagesList);
        //StartCoroutine(StartChat());

        //StartCoroutine(_networkManager.GetChatData());

        profileData.gender = 0; // male
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive) {
            timer += Time.deltaTime;
        }
    }

    public bool IsConnected()
    {
        return (this.loginToken != null && this.loginToken != "");
    }

    public void Login()
    {
        // _networkManager.DoLogin("+33642424242", "Testpassword42!");
        this.username = DisplayLogin.Instance.GetUserName();
        this.password = DisplayLogin.Instance.GetPassword();
        _networkManager.DoLogin(this.username, this.password);
    }

    public void Reconnect()
    {
        _networkManager.DoLogin(this.username, this.password);
    }

    public void LoginSuccess(string token)
    {
        this.loginToken = token;
        canvasManager.DisplayPage(CanvasManager.Page.Welcome_Connected);
    }

    public void GetTrainingDataSuccess(TrainingsResponseData trainingsResponseData)
    {
        DisplayJobInfo.Instance.UpdateFormationData(trainingsResponseData);
    }

    public void SendChatData()
    {
        _networkManager.PostChatData(interactions);
    }

    public void TestChatResult()
    {
        string resultData = "{\"hash\":\"18ad692881b9789a79de03509786e08b6454b8593dda6dc27b6fe6cb8d2ff032\",\"recommendation\":[{\"score\":0.99,\"identifier\":\"MET.100\",\"name\":\"d\\u00e9veloppeur\\/euse rural\\/e humanitaire\",\"metadata\":{\"resume\":\"D\\u00e9veloppeur rural pour ONG, il \\u0153uvre pour l\\u2019autosuffisance alimentaire durable, optimise les pratiques agricoles et cr\\u00e9e des coop\\u00e9ratives.\",\"romesV3\":{\"romeV3\":\"K1802\"},\"statuts\":{\"statut\":[{\"id\":\"T-ITM.7\",\"libelle\":\"contrat de volontariat\",\"id_ideo1\":\"104442\"},{\"id\":\"T-ITM.9\",\"libelle\":\"salari\\u00e9\",\"id_ideo1\":\"100215\"}]},\"synonymes\":null,\"competences\":\"<h5>\\u00catre p\\u00e9dagogue<\\/h5>\\n            <p>Une exp\\u00e9rience de la gestion de projet aupr\\u00e8s de producteurs est n\\u00e9cessaire pour ce sp\\u00e9cialiste de l'agro-\\u00e9conomie. Dot\\u00e9 d'une forte capacit\\u00e9 d'\\u00e9coute et de diplomatie, le d\\u00e9veloppeur rural humanitaire doit comprendre les besoins et les attentes des b\\u00e9n\\u00e9ficiaires du programme et des autorit\\u00e9s locales. Comprendre la culture du pays d'accueil implique de s'informer sur ses coutumes et sa hi\\u00e9rarchie sociale.<\\/p>\\n            <h5>Savoir g\\u00e9rer son temps et son budget<\\/h5>\\n            <p>Devant assurer une mission de conseil et d'appui technique, le d\\u00e9veloppeur rural choisit les b\\u00e9n\\u00e9ficiaires du programme, r\\u00e9dige les fiches de suivi et d'\\u00e9valuation. Ce gestionnaire de projet veille \\u00e9galement au respect des objectifs et des d\\u00e9lais, et g\\u00e8re le budget affect\\u00e9 \\u00e0 son activit\\u00e9.<\\/p>\\n            <h5>Savoir s'adapter<\\/h5>\\n            <p>Participer \\u00e0 une mission dans une zone recul\\u00e9e d'Afrique ou d'Asie exige une bonne dose d'adaptabilit\\u00e9 et de r\\u00e9sistance au stress. Il faut s'acclimater \\u00e0 la vie en \\u00e9quipe, en milieu isol\\u00e9. Et ne jamais h\\u00e9siter \\u00e0 mettre le pied \\u00e0 la b\\u00eache ou \\u00e0 descendre dans un puits quand cela s'av\\u00e8re n\\u00e9cessaire. Le d\\u00e9veloppeur rural humanitaire doit \\u00eatre organis\\u00e9 et rigoureux, et avoir une bonne capacit\\u00e9 d'analyse.<\\/p>\",\"acces_metier\":\"<p>Les ONG (organisations non gouvernementales) recrutent surtout des ing\\u00e9nieurs agricoles et agronomes, ainsi que quelques v\\u00e9t\\u00e9rinaires, \\u00e9conomistes et sociologues. Des postes d'animateur rural peuvent \\u00eatre accessibles avec un bac + 2. La pratique de l'anglais est obligatoire.<\\/p>\\n            <p>Niveau bac + 2<\\/p>\\n            <p>BTSA d\\u00e9veloppement de l'agriculture des r\\u00e9gions chaudes<\\/p>\\n            <p>Niveau bac + 4<\\/p>\\n            <p>Formation de coordonnateur de projet de solidarit\\u00e9 internationale et locale de l'Ifaid (Institut de formation et d'appui aux initiatives de d\\u00e9veloppement)<\\/p>\\n            <p>Niveau bac + 5<\\/p>\\n            <p>Dipl\\u00f4me d'ing\\u00e9nieur agricoles ou d'agronome<\\/p>\\n            <p>Master Gestion des territoires et d\\u00e9veloppement local<\\/p>\\n            <p>Niveau bac + 6<\\/p>\\n            <p>Mast\\u00e8re de l'Institut des r\\u00e9gions chaudes de SupAgro Montpellier<\\/p>\",\"first_salary\":\"Tr\\u00e8s variable, la r\\u00e9mun\\u00e9ration est fonction du niveau de qualification, de responsabilit\\u00e9 et est bas\\u00e9e sur la grille salariale de l'association.\",\"publications\":null,\"formats_courts\":{\"format_court\":[{\"type\":\"Fiche metier (Documentation)\",\"libelle\":\"d\\u00e9veloppeur\\/euse rural\\/e humanitaire\",\"descriptif\":\"<p>Le d\\u00e9veloppeur rural humanitaire d\\u00e9veloppe des projets de d\\u00e9veloppement humanitaire pour conduire les populations b\\u00e9n\\u00e9ficiaires vers l'autosuffisance alimentaire dans une perspective durable. Pour cela, il dispose de plusieurs moyens d'action. Il am\\u00e9liore les pratiques agricoles, monte des coop\\u00e9ratives agricoles et les aide \\u00e0 trouver des march\\u00e9s locaux. Ses employeurs sont les ONG (organisations non gouvernementales).<\\/p>\"},{\"type\":\"Dico des m\\u00e9tiers\",\"libelle\":\"d\\u00e9veloppeur rural \\/ d\\u00e9veloppeuse rurale humanitaire\",\"descriptif\":\"<p>Le d\\u00e9veloppeur rural humanitaire conseille les populations vuln\\u00e9rables dans un pays en d\\u00e9veloppement. Son objectif\\u00a0: les conduire vers l'autosuffisance alimentaire dans une perspective de d\\u00e9veloppement durable. Les ONG (organisations non gouvernementales) mettent en oeuvre des programmes d'urgence et de d\\u00e9veloppement. Dans ce cadre, le d\\u00e9veloppeur rural est amen\\u00e9 \\u00e0 faire du conseil agricole. Il \\u00e9tablit un diagnostic d\\u00e9taill\\u00e9 de la situation\\u00a0: besoins des populations, productions, march\\u00e9s, et applique ensuite diff\\u00e9rentes solutions\\u00a0: programmes d'introduction et de multiplication de semences, d\\u00e9veloppement de parcelles mara\\u00eech\\u00e8res, cr\\u00e9ation de poulaillers collectifs, etc. Il p\\u00e9rennise cette action en cr\\u00e9ant des coop\\u00e9ratives communautaires, en d\\u00e9veloppant des micro-financements... Il forme aussi cultivateurs et \\u00e9leveurs locaux.<\\/p>\\n                    <p>Le d\\u00e9veloppeur rural humanitaire exerce comme volontaire de la solidarit\\u00e9 internationale. Il b\\u00e9n\\u00e9ficie d'une prise en charge du transport, de l'h\\u00e9bergement et des frais de vie sur place, de la couverture sociale et d'une indemnit\\u00e9 mensuelle. Certaines ONG offrent des postes salari\\u00e9s. Toutes les ONG recrutent des personnes exp\\u00e9riment\\u00e9es\\u00a0: des ing\\u00e9nieurs agricoles et agronomes, ainsi que quelques v\\u00e9t\\u00e9rinaires, des \\u00e9conomistes et des sociologues (bac\\u00a0+\\u00a05). Des postes d'animateur rural peuvent \\u00eatre accessibles avec un bac\\u00a0+\\u00a02.<\\/p>\\n                    <h5>Dur\\u00e9e des \\u00e9tudes<\\/h5>\\n                    <h4>Apr\\u00e8s le bac<\\/h4>\\n                    <p>De bac\\u00a0+\\u00a02 (BTSA d\\u00e9veloppement de l'agriculture des r\\u00e9gions chaudes) \\u00e0 bac\\u00a0+\\u00a03 (formation de coordonnateur de projet de solidarit\\u00e9 internationale et locale), bac\\u00a0+\\u00a05 (dipl\\u00f4me d'ing\\u00e9nieur agricole ou d'agronom, master gestion des territoires et d\\u00e9veloppement local) et bac\\u00a0+\\u00a06 (mast\\u00e8re de l'Institut des r\\u00e9gions chaudes).<\\/p>\"}]},\"nature_travail\":\"<h5>Relancer l'\\u00e9conomie<\\/h5>\\n            <p>Apr\\u00e8s une catastrophe ou un conflit, l'\\u00e9conomie d'une r\\u00e9gion ou d'un pays est endommag\\u00e9e ou an\\u00e9antie. Les ONG (organisations non gouvernementales) mettent alors en oeuvre des programmes d'urgence et de d\\u00e9veloppement. En fonction de sa sp\\u00e9cialit\\u00e9 de base, le d\\u00e9veloppeur rural humanitaire \\u00e9tudie l'impact des projets \\u00e0 long terme pour relancer la production et la commercialisation.<\\/p>\\n            <h5>Conseil agricole<\\/h5>\\n            <p>Le d\\u00e9veloppeur rural humanitaire \\u00e9tablit un diagnostic d\\u00e9taill\\u00e9 de la situation\\u00a0: besoins des populations, productions, march\\u00e9s... Il \\u00e9labore ensuite une strat\\u00e9gie de d\\u00e9veloppement. Selon le contexte, il applique diff\\u00e9rents moyens\\u00a0: programmes d'introduction et de multiplication de semences, d\\u00e9veloppement de parcelles mara\\u00eech\\u00e8res, cr\\u00e9ation de poulaillers collectifs ou de petits \\u00e9levages, installation de fermes de pisciculture, am\\u00e9lioration de l'irrigation, etc. <\\/p>\\n            <h5>D\\u00e9veloppement durable<\\/h5>\\n            <p>Conduire les populations vers l'autosuffisance alimentaire ne suffit pas. Le d\\u00e9veloppeur rural doit p\\u00e9renniser l'action en cr\\u00e9ant des coop\\u00e9ratives communautaires, en d\\u00e9veloppant des march\\u00e9s locaux et des micro-financements... Cela passe \\u00e9galement par la formation de cultivateurs et d'\\u00e9leveurs locaux.<\\/p>\",\"accroche_metier\":\"<p>Le d\\u00e9veloppeur rural humanitaire conseille les populations vuln\\u00e9rables dans les pays en d\\u00e9veloppement. Son objectif : les conduire vers l'autosuffisance alimentaire dans une perspective de d\\u00e9veloppement durable.<\\/p>\",\"centres_interet\":{\"centre_interet\":[{\"id\":\"T-IDEO2.4817\",\"libelle\":\"j'ai le sens du contact\"},{\"id\":\"T-IDEO2.4809\",\"libelle\":\"j'aime bouger\"},{\"id\":\"T-IDEO2.4818\",\"libelle\":\"j'aime les langues\"},{\"id\":\"T-IDEO2.4810\",\"libelle\":\"je r\\u00eave de travailler \\u00e0 l'\\u00e9tranger\"}]},\"libelle_feminin\":\"d\\u00e9veloppeuse rurale humanitaire\",\"libelle_masculin\":\"d\\u00e9veloppeur rural humanitaire\",\"metiers_associes\":null,\"niveau_acces_min\":{\"id\":\"REF.421\",\"libelle\":\"Bac + 3\"},\"condition_travail\":\"<h5>Sur le terrain<\\/h5>\\n            <p>Le d\\u00e9veloppeur rural humanitaire travaille souvent loin de chez lui comme expatri\\u00e9 pour le compte d'une ONG (organisation non gouvernementale), dans un pays o\\u00f9 celle-ci assure un programme de post-urgence (apr\\u00e8s un tsunami ou un tremblement de terre, par exemple) ou de d\\u00e9veloppement. Il est affect\\u00e9 dans une r\\u00e9gion et est appel\\u00e9 \\u00e0 se d\\u00e9placer sur diff\\u00e9rents sites, parfois recul\\u00e9s. Il passe de nombreuses heures sur le terrain et parcourt beaucoup de kilom\\u00e8tres. Il travaille principalement au contact des villageois et des autorit\\u00e9s locales, notamment dans le cadre de formations, et collabore parfois avec d'autres expatri\\u00e9s, sur des projets pr\\u00e9cis.<\\/p>\\n            <h5>Volontariat fr\\u00e9quent<\\/h5>\\n            <p>Les recrutements se font souvent sous statut de volontaire de la solidarit\\u00e9 internationale. Le d\\u00e9veloppeur rural humanitaire b\\u00e9n\\u00e9ficie d'une prise en charge du transport, de l'h\\u00e9bergement et des frais de vie sur place, de la couverture sociale et d'une indemnit\\u00e9 mensuelle. Certaines ONG (comme Acted et Premi\\u00e8re Urgence) offrent des postes salari\\u00e9s.<\\/p>\",\"secteurs_activite\":{\"secteur_activite\":{\"id\":\"T-IDEO2.4856\",\"libelle\":\"Social\"}},\"sources_numeriques\":{\"source\":[{\"valeur\":\"http:\\/\\/www.portail-humanitaire.org\",\"commentaire\":\"Portail francophone de la solidarit\\u00e9 internationale\"},{\"valeur\":\"http:\\/\\/www.coordinationsud.org\",\"commentaire\":\"Site de la coordination nationale des ONG fran\\u00e7aises de solidarit\\u00e9 internationale\"}]},\"vie_professionnelle\":\"<h3>Salaire<\\/h3>\\n            <h5>Salaire du d\\u00e9butant<\\/h5>\\n            <p>Tr\\u00e8s variable, la r\\u00e9mun\\u00e9ration est fonction du niveau de qualification, de responsabilit\\u00e9 et est bas\\u00e9e sur la grille salariale de l'association.<\\/p>\\n            <h3>Int\\u00e9grer le march\\u00e9 du travail<\\/h3>\\n            <h5>Exp\\u00e9rience recommand\\u00e9e<\\/h5>\\n            <p>Le d\\u00e9veloppeur rural humanitaire doit avoir une exp\\u00e9rience professionnelle significative. Les ONG (organisations non gouvernementales) qui assurent des programmes de d\\u00e9veloppement rural recrutent des responsables techniques et des experts en agronomie, des v\\u00e9t\\u00e9rinaires, des sp\\u00e9cialistes en \\u00e9conomie sociale ou en d\\u00e9veloppement, tous exp\\u00e9riment\\u00e9s. Le d\\u00e9veloppement rural repr\\u00e9sente 5\\u00a0% des volontaires en mission. Certaines organisations, comme Premi\\u00e8re Urgence, Acted, ACF (Action contre la faim), l'Association fran\\u00e7aise des volontaires du progr\\u00e8s ou Agronomes et V\\u00e9t\\u00e9rinaires sans fronti\\u00e8res ont une forte implication dans ce domaine.<\\/p>\\n            <h5>Retour en France possible<\\/h5>\\n            <p>Les missions durent entre 6 et 12\\u00a0mois et sont parfois renouvelables. Avec l'exp\\u00e9rience, les humanitaires qui ont d\\u00e9j\\u00e0 \\u00e0 leur actif plusieurs missions peuvent \\u00e9voluer vers des postes de coordination de programmes. Ceux qui souhaitent quitter l'action humanitaire peuvent reprendre une activit\\u00e9 en France en tant que conseiller agricole, d\\u00e9veloppeur rural, v\\u00e9t\\u00e9rinaire...<\\/p>\",\"formations_min_requise\":{\"formation_min_requise\":[{\"id\":\"FOR.396\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'Institut sup\\u00e9rieur d'agriculture Rh\\u00f4ne-Alpes\"},{\"id\":\"FOR.397\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'Institut d'enseignement sup\\u00e9rieur et de recherche en alimentation, sant\\u00e9 animale, sciences agronomiques et de l'environnement\"},{\"id\":\"FOR.398\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'\\u00c9cole nationale sup\\u00e9rieure des sciences agronomiques de Bordeaux Aquitaine\"},{\"id\":\"FOR.591\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'Institut des sciences et industries du vivant et de l'environnement (AgroParisTech)\"},{\"id\":\"FOR.1549\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'\\u00c9cole sup\\u00e9rieure d'agricultures d'Angers\"},{\"id\":\"FOR.4271\",\"libelle\":\"mast\\u00e8re sp\\u00e9. Innovations et politiques pour une alimentation durable (Montpellier SupAgro)\"},{\"id\":\"FOR.4782\",\"libelle\":\"master gestion des territoires et d\\u00e9veloppement local\"},{\"id\":\"FOR.5597\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'\\u00c9cole nationale sup\\u00e9rieure d'agronomie et des industries alimentaires de l'universit\\u00e9 de Lorraine sp\\u00e9cialit\\u00e9 agronomie\"},{\"id\":\"FOR.7807\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'ENS des sc agronomiques, agroalimentaires, horticoles et du paysage (Institut Agro Rennes-Angers) de l'INES pour l'agri, l'alimentation et l'environnement (Institut Agro) sp\\u00e9cialit\\u00e9 agronomie\"},{\"id\":\"FOR.7982\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'Institut Polytechnique UniLaSalle sp\\u00e9cialit\\u00e9 agronomie et agro-industries\"},{\"id\":\"FOR.8052\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'\\u00c9cole d'ing\\u00e9nieurs de Purpan\"},{\"id\":\"FOR.8071\",\"libelle\":\"Coordonnateur de projet de solidarit\\u00e9 internationale et locale\"},{\"id\":\"FOR.8763\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'\\u00c9cole nationale sup\\u00e9rieure agronomique de Toulouse de l'INP de Toulouse\"},{\"id\":\"FOR.8934\",\"libelle\":\"dipl\\u00f4me d'ing\\u00e9nieur de l'Institut sup\\u00e9rieur d'agriculture - JUNIA\"}]}}}],\"metadata\":[]}";
        //string resultData = "{\"hash\":\"18ad692881b9789a79de03509786e08b6454b8593dda6dc27b6fe6cb8d2ff032\",\"recommendation\":[{\"score\":0.99,\"identifier\":\"MET.100\",\"name\":\"d\\u00e9veloppeur\\/euse rural\\/e humanitaire\",\"metadata\":{\"resume\":\"D\\u00e9veloppeur rural pour ONG, il \\u0153uvre pour l\\u2019autosuffisance alimentaire durable, optimise les pratiques agricoles et cr\\u00e9e des coop\\u00e9ratives.\",\"romesV3\":{\"romeV3\":\"K1802\"},\"statuts\":{\"statut\":[{\"id\":\"T-ITM.7\",\"libelle\":\"contrat de volontariat\",\"id_ideo1\":\"104442\"},{\"id\":\"T-ITM.9\",\"libelle\":\"salari\\u00e9\",\"id_ideo1\":\"100215\"}]},\"synonymes\":null,\"competences\":\"<h5>\\u00catre p\\u00e9dagogue<\\/h5>\\n            <p>Une exp\\u00e9rience de la gestion de projet aupr\\u00e8s de producteurs est n\\u00e9cessaire pour ce sp\\u00e9cialiste de l'agro-\\u00e9conomie. Dot\\u00e9 d'une forte capacit\\u00e9 d'\\u00e9coute et de diplomatie, le d\\u00e9veloppeur rural humanitaire doit comprendre les besoins et les attentes des b\\u00e9n\\u00e9ficiaires du programme et des autorit\\u00e9s locales. Comprendre la culture du pays d'accueil implique de s'informer sur ses coutumes et sa hi\\u00e9rarchie sociale.<\\/p>\\n            <h5>Savoir g\\u00e9rer son temps et son budget<\\/h5>\\n            <p>Devant assurer une mission de conseil et d'appui technique, le d\\u00e9veloppeur rural choisit les b\\u00e9n\\u00e9ficiaires du programme, r\\u00e9dige les fiches de suivi et d'\\u00e9valuation. Ce gestionnaire de projet veille \\u00e9galement au respect des objectifs et des d\\u00e9lais, et g\\u00e8re le budget affect\\u00e9 \\u00e0 son activit\\u00e9.<\\/p>\\n            <h5>Savoir s'adapter<\\/h5>\\n            <p>Participer \\u00e0 une mission dans une zone recul\\u00e9e d'Afrique ou d'Asie exige une bonne dose d'adaptabilit\\u00e9 et de r\\u00e9sistance au stress. Il faut s'acclimater \\u00e0 la vie en \\u00e9quipe, en milieu isol\\u00e9. Et ne jamais h\\u00e9siter \\u00e0 mettre le pied \\u00e0 la b\\u00eache ou \\u00e0 descendre dans un puits quand cela s'av\\u00e8re n\\u00e9cessaire. Le d\\u00e9veloppeur rural humanitaire doit \\u00eatre organis\\u00e9 et rigoureux, et avoir une bonne capacit\\u00e9 d'analyse.<\\/p>\",\"acces_metier\":\"<p>Les ONG (organisations non gouvernementales) recrutent surtout des ing\\u00e9nieurs agricoles et agronomes, ainsi que quelques v\\u00e9t\\u00e9rinaires, \\u00e9conomistes et sociologues. Des postes d'animateur rural peuvent \\u00eatre accessibles avec un bac + 2. La pratique de l'anglais est obligatoire.<\\/p>\\n            <p>Niveau bac + 2<\\/p>\\n            <p>BTSA d\\u00e9veloppement de l'agriculture des r\\u00e9gions chaudes<\\/p>\\n            <p>Niveau bac + 4<\\/p>\\n            <p>Formation de coordonnateur de projet de solidarit\\u00e9 internationale et locale de l'Ifaid (Institut de formation et d'appui aux initiatives de d\\u00e9veloppement)<\\/p>\\n            <p>Niveau bac + 5<\\/p>\\n            <p>Dipl\\u00f4me d'ing\\u00e9nieur agricoles ou d'agronome<\\/p>\\n            <p>Master Gestion des territoires et d\\u00e9veloppement local<\\/p>\\n            <p>Niveau bac + 6<\\/p>\\n            <p>Mast\\u00e8re de l'Institut des r\\u00e9gions chaudes de SupAgro Montpellier<\\/p>\",\"first_salary\":\"Tr\\u00e8s variable, la r\\u00e9mun\\u00e9ration est fonction du niveau de qualification, de responsabilit\\u00e9 et est bas\\u00e9e sur la grille salariale de l'association.\",\"publications\":null,\"formats_courts\":{\"format_court\":[{\"type\":\"Fiche metier (Documentation)\",\"libelle\":\"d\\u00e9veloppeur\\/euse rural\\/e humanitaire\",\"descriptif\":\"<p>Le d\\u00e9veloppeur rural humanitaire d\\u00e9veloppe des projets de d\\u00e9veloppement humanitaire pour conduire les populations b\\u00e9n\\u00e9ficiaires vers l'autosuffisance alimentaire dans une perspective durable. Pour cela, il dispose de plusieurs moyens d'action. Il am\\u00e9liore les pratiques agricoles, monte des coop\\u00e9ratives agricoles et les aide \\u00e0 trouver des march\\u00e9s locaux. Ses employeurs sont les ONG (organisations non gouvernementales).<\\/p>\"},{\"type\":\"Dico des m\\u00e9tiers\",\"libelle\":\"d\\u00e9veloppeur rural \\/ d\\u00e9veloppeuse rurale humanitaire\",\"descriptif\":\"<p>Le d\\u00e9veloppeur rural humanitaire conseille les populations vuln\\u00e9rables dans un pays en d\\u00e9veloppement. Son objectif\\u00a0: les conduire vers l'autosuffisance alimentaire dans une perspective de d\\u00e9veloppement durable. Les ONG (organisations non gouvernementales) mettent en oeuvre des programmes d'urgence et de d\\u00e9veloppement. Dans ce cadre, le d\\u00e9veloppeur rural est amen\\u00e9 \\u00e0 faire du conseil agricole. Il \\u00e9tablit un diagnostic d\\u00e9taill\\u00e9 de la situation\\u00a0: besoins des populations, productions, march\\u00e9s, et applique ensuite diff\\u00e9rentes solutions\\u00a0: programmes d'introduction et de multiplication de semences, d\\u00e9veloppement de parcelles mara\\u00eech\\u00e8res, cr\\u00e9ation de poulaillers collectifs, etc. Il p\\u00e9rennise cette action en cr\\u00e9ant des coop\\u00e9ratives communautaires, en d\\u00e9veloppant des micro-financements... Il forme aussi cultivateurs et \\u00e9leveurs locaux.<\\/p>\\n                    <p>Le d\\u00e9veloppeur rural humanitaire exerce comme volontaire de la solidarit\\u00e9 internationale. Il b\\u00e9n\\u00e9ficie d'une prise en charge du transport, de l'h\\u00e9bergement et des frais de vie sur place, de la couverture sociale et d'une indemnit\\u00e9 mensuelle. Certaines ONG offrent des postes salari\\u00e9s. Toutes les ONG recrutent des personnes exp\\u00e9riment\\u00e9es\\u00a0: des ing\\u00e9nieurs agricoles et agronomes, ainsi que quelques v\\u00e9t\\u00e9rinaires, des \\u00e9conomistes et des sociologues (bac\\u00a0+\\u00a05). Des postes d'animateur rural peuvent \\u00eatre accessibles avec un bac\\u00a0+\\u00a02.<\\/p>\\n                    <h5>Dur\\u00e9e des \\u00e9tudes<\\/h5>\\n                    <h4>Apr\\u00e8s le bac<\\/h4>\\n                    <p>De bac\\u00a0+\\u00a02 (BTSA d\\u00e9veloppement de l'agriculture des r\\u00e9gions chaudes) \\u00e0 bac\\u00a0+\\u00a03 (formation de coordonnateur de projet de solidarit\\u00e9 internationale et locale), bac\\u00a0+\\u00a05 (dipl\\u00f4me d'ing\\u00e9nieur agricole ou d'agronom, master gestion des territoires et d\\u00e9veloppement local) et bac\\u00a0+\\u00a06 (mast\\u00e8re de l'Institut des r\\u00e9gions chaudes).<\\/p>\"}]},\"nature_travail\":\"<h5>Relancer l'\\u00e9conomie<\\/h5>\\n            <p>Apr\\u00e8s une catastrophe ou un conflit, l'\\u00e9conomie d'une r\\u00e9gion ou d'un pays est endommag\\u00e9e ou an\\u00e9antie. Les ONG (organisations non gouvernementales) mettent alors en oeuvre des programmes d'urgence et de d\\u00e9veloppement. En fonction de sa sp\\u00e9cialit\\u00e9 de base, le d\\u00e9veloppeur rural humanitaire \\u00e9tudie l'impact des projets \\u00e0 long terme pour relancer la production et la commercialisation.<\\/p>\\n            <h5>Conseil agricole<\\/h5>\\n            <p>Le d\\u00e9veloppeur rural humanitaire \\u00e9tablit un diagnostic d\\u00e9taill\\u00e9 de la situation\\u00a0: besoins des populations, productions, march\\u00e9s... Il \\u00e9labore ensuite une strat\\u00e9gie de d\\u00e9veloppement. Selon le contexte, il applique diff\\u00e9rents moyens\\u00a0: programmes d'introduction et de multiplication de semences, d\\u00e9veloppement de parcelles mara\\u00eech\\u00e8res, cr\\u00e9ation de poulaillers collectifs ou de petits \\u00e9levages, installation de fermes de pisciculture, am\\u00e9lioration de l'irrigation, etc. <\\/p>\\n            <h5>D\\u00e9veloppement durable<\\/h5>\\n            <p>Conduire les populations vers l'autosuffisance alimentaire ne suffit pas. Le d\\u00e9veloppeur rural doit p\\u00e9renniser l'action en cr\\u00e9ant des coop\\u00e9ratives communautaires, en d\\u00e9veloppant des march\\u00e9s locaux et des micro-financements... Cela passe \\u00e9galement par la formation de cultivateurs et d'\\u00e9leveurs locaux.<\\/p>\",\"accroche_metier\":\"<p>Le d\\u00e9veloppeur rural humanitaire conseille les populations vuln\\u00e9rables dans les pays en d\\u00e9veloppement. Son objectif : les conduire vers l'autosuffisance alimentaire dans une perspective de d\\u00e9veloppement durable.<\\/p>\",\"centres_interet\":{\"centre_interet\":[{\"id\":\"T-IDEO2.4817\",\"libelle\":\"j'ai le sens du contact\"},{\"id\":\"T-IDEO2.4809\",\"libelle\":\"j'aime bouger\"},{\"id\":\"T-IDEO2.4818\",\"libelle\":\"j'aime les langues\"},{\"id\":\"T-IDEO2.4810\",\"libelle\":\"je r\\u00eave de travailler \\u00e0 l'\\u00e9tranger\"}]},\"libelle_feminin\":\"d\\u00e9veloppeuse rurale humanitaire\",\"libelle_masculin\":\"d\\u00e9veloppeur rural humanitaire\",\"metiers_associes\":null,\"niveau_acces_min\":{\"id\":\"REF.421\",\"libelle\":\"Bac + 3\"},\"condition_travail\":\"<h5>Sur le terrain<\\/h5>\\n            <p>Le d\\u00e9veloppeur rural humanitaire travaille souvent loin de chez lui comme expatri\\u00e9 pour le compte d'une ONG (organisation non gouvernementale), dans un pays o\\u00f9 celle-ci assure un programme de post-urgence (apr\\u00e8s un tsunami ou un tremblement de terre, par exemple) ou de d\\u00e9veloppement. Il est affect\\u00e9 dans une r\\u00e9gion et est appel\\u00e9 \\u00e0 se d\\u00e9placer sur diff\\u00e9rents sites, parfois recul\\u00e9s. Il passe de nombreuses heures sur le terrain et parcourt beaucoup de kilom\\u00e8tres. Il travaille principalement au contact des villageois et des autorit\\u00e9s locales, notamment dans le cadre de formations, et collabore parfois avec d'autres expatri\\u00e9s, sur des projets pr\\u00e9cis.<\\/p>\\n            <h5>Volontariat fr\\u00e9quent<\\/h5>\\n            <p>Les recrutements se font souvent sous statut de volontaire de la solidarit\\u00e9 internationale. Le d\\u00e9veloppeur rural humanitaire b\\u00e9n\\u00e9ficie d'une prise en charge du transport, de l'h\\u00e9bergement et des frais de vie sur place, de la couverture sociale et d'une indemnit\\u00e9 mensuelle. Certaines ONG (comme Acted et Premi\\u00e8re Urgence) offrent des postes salari\\u00e9s.<\\/p>\",\"secteurs_activite\":{\"secteur_activite\":{\"id\":\"T-IDEO2.4856\",\"libelle\":\"Social\"}},\"sources_numeriques\":{\"source\":[{\"valeur\":\"http:\\/\\/www.portail-humanitaire.org\",\"commentaire\":\"Portail francophone de la solidarit\\u00e9 internationale\"},{\"valeur\":\"http:\\/\\/www.coordinationsud.org\",\"commentaire\":\"Site de la coordination nationale des ONG fran\\u00e7aises de solidarit\\u00e9 internationale\"}]},\"vie_professionnelle\":\"<h3>Salaire<\\/h3>\\n            <h5>Salaire du d\\u00e9butant<\\/h5>\\n            <p>Tr\\u00e8s variable, la r\\u00e9mun\\u00e9ration est fonction du niveau de qualification, de responsabilit\\u00e9 et est bas\\u00e9e sur la grille salariale de l'association.<\\/p>\\n            <h3>Int\\u00e9grer le march\\u00e9 du travail<\\/h3>\\n            <h5>Exp\\u00e9rience recommand\\u00e9e<\\/h5>\\n            <p>Le d\\u00e9veloppeur rural humanitaire doit avoir une exp\\u00e9rience professionnelle significative. Les ONG (organisations non gouvernementales) qui assurent des programmes de d\\u00e9veloppement rural recrutent des responsables techniques et des experts en agronomie, des v\\u00e9t\\u00e9rinaires, des sp\\u00e9cialistes en \\u00e9conomie sociale ou en d\\u00e9veloppement, tous exp\\u00e9riment\\u00e9s. Le d\\u00e9veloppement rural repr\\u00e9sente 5\\u00a0% des volontaires en mission. Certaines organisations, comme Premi\\u00e8re Urgence, Acted, ACF (Action contre la faim), l'Association fran\\u00e7aise des volontaires du progr\\u00e8s ou Agronomes et V\\u00e9t\\u00e9rinaires sans fronti\\u00e8res ont une forte implication dans ce domaine.<\\/p>\\n            <h5>Retour en France possible<\\/h5>\\n            <p>Les missions durent entre 6 et 12\\u00a0mois et sont parfois renouvelables. Avec l'exp\\u00e9rience, les humanitaires qui ont d\\u00e9j\\u00e0 \\u00e0 leur actif plusieurs missions peuvent \\u00e9voluer vers des postes de coordination de programmes. Ceux qui souhaitent quitter l'action humanitaire peuvent reprendre une activit\\u00e9 en France en tant que conseiller agricole, d\\u00e9veloppeur rural, v\\u00e9t\\u00e9rinaire...<\\/p>\",\"formations_min_requise\":{\"formation_min_requise\":[]}}}],\"metadata\":[]}";
        _networkManager.HandleResponse("POSTCHAT", resultData);
    }

    public void ChatResult(MatchingResponseData matchingResponseData)
    {
        Debug.Log("ChatResult");
        //List<MatchingResponseData.Recommendation> recommendations = matchingResponseData.recommendations;
        Debug.Log(matchingResponseData);
        Debug.Log(matchingResponseData.hash);
        Debug.Log(matchingResponseData.recommendation);
        Debug.Log(matchingResponseData.recommendation.Count);
        MatchingResponseData.Recommendation recommendation = matchingResponseData.recommendation[0];
        Debug.Log(recommendation.score + " " + recommendation.identifier + " " + recommendation.metadata.libelle_masculin + recommendation.metadata.formats_courts["format_court"][0].descriptif);
        //(string jobTitle, string jobDesc, string jobDescFull, string jobDurationMin, string jobDurationMax)
        string jobLibelle = profileData.gender == 0 ? recommendation.metadata.libelle_masculin : recommendation.metadata.libelle_feminin;
        List<FormationMinRequise> formations = recommendation.metadata.formations_min_requise["formation_min_requise"];
        DisplayJobInfo.Instance.SetJobInfos(jobLibelle, recommendation.metadata.formats_courts["format_court"][0].descriptif,
            recommendation.metadata.formats_courts["format_court"][1].descriptif, recommendation.metadata.niveau_acces_min.libelle, recommendation.metadata.niveau_acces_min.libelle, formations);
        DisplayMessage.Instance.Hide();
        DisplayChat.Instance.Hide();
        canvasManager.DisplayPage(CanvasManager.Page.JobInfo);
        //pull all formation data from backend
        foreach (FormationMinRequise formation in formations)
        {
            _networkManager.GetTrainingData(formation.id);
        }
    }

    public void BuildChat(String jsonResponse)
    {
        Debug.Log("BuildChat: " + jsonResponse);
        chatList = JsonUtility.FromJson<QuestionList>(jsonResponse);
        currentChatList = chatList.questions;
        chatIndex = 0;
        interactions.Clear();
        StartCoroutine(ChatCoroutine());
    }

    public void PauseChat()
    {
        runChat = false;
    }

    public void RunChat()
    { 
        if (currentMessage != null && currentMessage.answers != null && currentMessage.answers.Count > 0 && waitingForAnswer)
        {
            if (!DisplayChat.Instance.messagesDisplay[currentMessage.id].visible) //Bug here, display the question in chat not working
            {
                DisplayChat.Instance.AddMessage(currentMessage, DisplayChat.Side.Bot);
            }
            Debug.Log("Current message answer, displayAnswer");
            if (DisplayAnswers.Instance.visible) //Fully visible with all questions
            {
                DisplayAnswers.Instance.Hide();
            }
            StartCoroutine(ResumeDisplayAnswer());
        }
        else
        {
            if (currentMessage != null)
            {
                DisplayChat.Instance.StopWaitingAnim(currentMessage.id);
                if (!DisplayChat.Instance.messagesDisplay[currentMessage.id].visible)
                {
                    DisplayChat.Instance.AddMessage(currentMessage, DisplayChat.Side.Bot);
                }
            }
            runChat = true;
        }   
    }

    private IEnumerator ResumeDisplayAnswer()
    {
        yield return new WaitForSeconds(1.5f);
        DisplayAnswers.Instance.Display(currentMessage);
    }

    public void ResetChat()
    {

    }

    public bool IsChatRunning()
    {
        return runChat;
    }


    /*private void MakeMessageList()
    {
        string path = Application.persistentDataPath + "/chat.csv";
        Debug.Log(path);
        StreamReader sr = new StreamReader(path);
        sr.ReadLine();
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            messagesList.Add(this.GetMessageData(line));
        }
    }

    private MessageData GetMessageData(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            return null;
        }
        MessageData messageData = new MessageData();

        try
        {
            //Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //string[] values = CSVParser.Split(line);
            string[] values = line.Split(';');
            if (values != null && values.Length >= 4)
            {
                messageData.spawnTime = float.Parse(values[0].ToString());
                messageData.waitingAnimTime = float.Parse(values[1].ToString());
                messageData.messageType = int.Parse(values[2].ToString());
                messageData.messageText = values[3].ToString();
                if (values.Length >= 6 && values[4] != null && values[5] != null)
                {
                    messageData.messageWidth = float.Parse(values[4].ToString());
                    messageData.messageHeight = float.Parse(values[5].ToString());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error importing data from chat.csv line: " + line);
            Debug.Log(e.Message);
        }
        return messageData;
    }*/

    private void StartWaitingAnim()
    {
        /*waitingMessagePrefab.transform.SetParent(chatContent.transform);
        waitingMessagePrefab.SetActive(true);
        StartCoroutine(ForceScrollDown());*/
    }

    private void StopWaitingAnim()
    {
        /*waitingMessagePrefab.SetActive(false);
        waitingMessagePrefab.transform.SetParent(objectPoolContainer.transform);*/
    }

    private void SendMessageToChat(GameObject messagePrefab, string messageText, float messageWidth, float messageHeight)
    {
        //DisplayChat.Instance.AddMessage(messageText, DisplayChat.Side.Bot);

        /*GameObject mess = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        Message messageScript = mess.GetComponent<Message>();
        messageScript.SetMessage(messageText);
        messageScript.SetSize(messageWidth, messageHeight);
        StartCoroutine(ForceScrollDown());*/
    }

    private void SendAnswerToChat(GameObject messagePrefab, string id, QuestionData question)
    {
        DisplayAnswers.Instance.Display(question);
        /*GameObject mess = Instantiate(answerMessagePrefab, Vector3.zero, Quaternion.identity, chatContent.transform);
        AnswerContainer ansScript = mess.GetComponent<AnswerContainer>();
        answerScriptList.Add(ansScript);
        ansScript.CreateAnswerList(id, answers);
        StartCoroutine(ForceScrollDown());*/
    }

    /*private IEnumerator StartChat()
    {
        foreach (MessageData message in messagesList)
        {
            if (message != null)
            {
                if (message.spawnTime > 0)
                {
                    yield return new WaitForSeconds(message.spawnTime);
                }
                if (message.messageType == 1)
                {
                    this.StartWaitingAnim();
                    yield return new WaitForSeconds(message.waitingAnimTime);
                    this.StopWaitingAnim();
                    this.SendMessageToChat(botMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
                else if (message.messageType == 2)
                {
                    this.SendMessageToChat(userMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
                else
                {
                    Debug.Log("Wrong message type for Message: " + message.messageText);
                }
            }
        }
        Debug.Log("End of chat");
        //Spawn send result button
        emptyLastLine.transform.SetParent(chatContent.transform);
        StartCoroutine(ForceScrollDown());
    }*/
    public void SelectAnswer(string questionId, QuestionData answer) {
        Debug.Log("SELECT ANSWER QuestionId:" + questionId + " answerId: " + answer.id + " " + answer.label + " " + answer.button);
        if (!interactions.ContainsKey(questionId))
        {
            waitingForAnswer = false;
            List<string> answerIdList = new List<string>();
            answerIdList.Add(answer.id);
            interactions.Add(questionId, answerIdList);
            DisplayAnswers.Instance.FadeOut();
            DisplayChat.Instance.AddMessage(answer, DisplayChat.Side.User);
            runChat = true;
        }
    }

    public void SelectAnswer(GameObject answerContainer, AnswerContainer answerScript)
    {
        /*Debug.Log("SelectAnswer: " + answerScript.selectedLabel);
        string messageText = answerScript.selectedLabel;
        answerContainer.SetActive(false);
        this.SendMessageToChat(userMessagePrefab, messageText, 400f, 115f);
        SendNextMessage();*/
    }

    public void SendNextMessage()
    {
        var message = currentChatList[chatIndex];
        DisplayChat.Instance.AddMessage(message, DisplayChat.Side.Bot);
        chatIndex++;
        return;
        /*if (chatIndex < currentChatList.Length - 1)
        {
            StartCoroutine(SendMessage(currentChatList[chatIndex]));
            chatIndex++;
        }
        else
        {
            Debug.Log("End of chat");
            //Spawn send result button
            emptyLastLine.transform.SetParent(chatContent.transform);
            StartCoroutine(ForceScrollDown());
        }*/ 
    }

    private IEnumerator ChatCoroutine() {

        while ( chatIndex < currentChatList.Length - 1) {
            yield return new WaitUntil(() => runChat);

            // display bot message
            var message = currentChatList[chatIndex];
            this.currentMessage = message;
            if (message.answers != null && message.answers.Count > 0)
            {
                waitingForAnswer = true;
            }
            DisplayChat.Instance.AddMessage(message, DisplayChat.Side.Bot);

            yield return new WaitUntil(() => runChat);

            // wait for user answer
            if (message.answers != null && message.answers.Count > 0) {
                yield return new WaitForSeconds(3f);
                DisplayAnswers.Instance.Display(message);
                //while (DisplayAnswers.Instance.visible)
                //    yield return null;
                runChat = false;
                yield return new WaitForSeconds(1f);
            }

            yield return new WaitUntil(() => runChat);

            StartTimer();
            // wait for input or time
            while (timer < messageDuration) {
                //if (Input.GetMouseButtonDown(0))
                //    break;
                yield return null;

                //Debug.Log($"timer : {timer}");
                //Debug.Log($"pressing mouse : {Input.GetMouseButtonDown(0)}");
            }
            yield return new WaitForEndOfFrame();
            EndTimer();
            ++chatIndex;
        }

        if (chatIndex == currentChatList.Length - 1)
        {
            Debug.Log($"finished chat");
            DisplayMessage.Instance.Display("Chat ended. Waiting for result ...");
            this.SendChatData();
        }
    }

    void StartTimer() {
        timer = 0f;
        timerActive = true;
    }
    void EndTimer() {
        timerActive = false;
    }

    private IEnumerator SendMessage(QuestionData message)
    {
        if (message != null)
        {
            if (message.metadata != null)
            {
                //
            }

            //hard coded
            /*yield return new WaitForSeconds(1f);
            this.StartWaitingAnim();
            /yield return new WaitForSeconds(message.waitingAnimTime);
            yield return new WaitForSeconds(1f);
            this.StopWaitingAnim();*/
            yield return new WaitForSeconds(1f);

            this.SendMessageToChat(botMessagePrefab, message.label, 620f, 115f);

            yield return new WaitForSeconds(1f);

            if (message.answers != null && message.answers.Count > 0)
            {
                yield return new WaitForSeconds(1f);
                this.SendAnswerToChat(answerMessagePrefab, message.id, message);
            }
            else
            {
                SendNextMessage();
            }
        }
    }

   /* private IEnumerator StartChat()
    {
        foreach (QuestionData message in chatList.questions)
        {
            if (message != null)
            {
                if (message.metadata != null)
                {
                    //
                }
                //hard coded
                yield return new WaitForSeconds(1f);
                this.StartWaitingAnim();
                //yield return new WaitForSeconds(message.waitingAnimTime);
                yield return new WaitForSeconds(1f);
                this.StopWaitingAnim();
                this.SendMessageToChat(botMessagePrefab, message.label, 620f, 115f);

                if (message.answers != null && message.answers.Count > 0)
                {
                    //send user answer
                    //this.SendMessage(userMessagePrefab, message.messageText, message.messageWidth, message.messageHeight);
                }
            }
        }
        Debug.Log("End of chat");
        //Spawn send result button
        emptyLastLine.transform.SetParent(chatContent.transform);
        StartCoroutine(ForceScrollDown());
    }*/

    private IEnumerator ForceScrollDown()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}
