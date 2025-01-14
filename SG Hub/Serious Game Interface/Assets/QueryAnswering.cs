using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using System;
using VDS.RDF.Query.Builder;
using VDS.RDF.Query;
using VDS.RDF.Writing.Formatting;
using System.Globalization;

public class QueryAnswering : MonoBehaviour
{
    public Text cevap;
    private DatabaseAccess databaseAccess;
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public Dropdown dropdown3;
    public Dropdown dropdown4;
    public Dropdown dropdown5;
    public Dropdown dropdown6;
    public Dropdown dropdown7;
    public Dropdown dropdown8;
    public Dropdown dropdown9;
    public Dropdown dropdown10;
    public Dropdown dropdown11;
    public Dropdown dropdown12;
    public Dropdown dropdown13;
    public Dropdown dropdown14;
    public Dropdown dropdown15;
    public GameObject DisabilityQuestions;
    public GameObject PatientQuestions;
    public GameObject GameQuestions;
    public Dropdown questionDropdown;

    public ToggleGroup toggles1;
    public ToggleGroup toggles2;
    public ToggleGroup toggles3;
    private Toggle selectedToggle;
    // Start is called before the first frame update
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void Query1()
    {
        string game = dropdown1.options[dropdown1.value].text;
        bool lessmore = true;
        if (dropdown2.options[dropdown2.value].text == "en az")
            lessmore = false;
        int score = Convert.ToInt32(dropdown3.options[dropdown3.value].text);
        var task = databaseAccess.Query1(game, lessmore, score);
        var result = await task;
        List<string> outputList = new List<string>(); ;
        foreach (var patient in result)
        {
            outputList.Add(patient.UserName);
        }
        if (outputList.Count != 0)
        {
            foreach (var patient in outputList)
            {
                Debug.Log(patient);
            }
        }
    }

    public async void Query2()
    {
        bool lessmore = true;
        if (dropdown4.options[dropdown4.value].text == "En az")
            lessmore = false;
        int session = Convert.ToInt32(dropdown5.options[dropdown5.value].text);
        var task = databaseAccess.GetScoresFromDatabase();
        var result = await task;
        List<string> outputList = new List<string>(); ;
        Dictionary<string, int> occuranceCounter = new Dictionary<string, int>();
        int value = 0;
        foreach (var patient in result)
        {
            if (occuranceCounter.TryGetValue(patient.UserName, out value))
            {
                occuranceCounter[patient.UserName] += 1;
            }
            else
                occuranceCounter[patient.UserName] = 1;
        }

        if (occuranceCounter.Count != 0)
        {
            foreach (var patient in occuranceCounter)
            {
                if (lessmore == false)
                {
                    if (occuranceCounter[patient.Key] >= session) //en az
                        Debug.Log(patient.Key);
                }
                else
                {
                    if (occuranceCounter[patient.Key] <= session) //en çok
                        Debug.Log(patient.Key);
                }
            }
        }
    }

    public async void Query3()
    {
        int age = Convert.ToInt32(dropdown6.options[dropdown6.value].text);
        string greater = dropdown7.options[dropdown7.value].text;
        bool lessmore = true;
        if (dropdown8.options[dropdown8.value].text == "en az")
            lessmore = false;
        int year = Convert.ToInt32(dropdown9.options[dropdown9.value].text);
        var task = databaseAccess.Query3(age, greater, lessmore, year);
        var result = await task;
        List<string> outputList = new List<string>(); ;
        foreach (var patient in result)
        {
            outputList.Add(patient.patient_name);
        }
        if (outputList.Count != 0)
        {
            foreach (var patient in outputList)
            {
                Debug.Log(patient);
            }
        }
    }

    public async void Query4()
    {
        int age = Convert.ToInt32(dropdown13.options[dropdown13.value].text);
        string score = dropdown11.options[dropdown11.value].text;
        string game = dropdown10.options[dropdown10.value].text;
        string metric = dropdown12.options[dropdown12.value].text;

        if (metric == "tamamlama")
        {
            metric = "completeness";
        }
        else if (metric == "yumuşaklık")
        {
            metric = "smoothness";
        }
        else if (metric == "süre")
        {
            metric = "duration";
        }
        else if (metric == "sabitlik")
        {
            metric = "steadiness";
        }
        Debug.Log("File Loading");
        IGraph g = new Graph();
        TurtleParser ttlparser = new TurtleParser();
        ttlparser.Load(g, Application.dataPath + "/RDF.ttl");
        //FileLoader.Load(g, "RDF.xml");
        Debug.Log("File Loaded");
        cevap.text="Cevap: "+Q4(g,age,score,game,metric);
        Debug.Log("Query Answered");
        //Assuming we have some Graph g find all the URI Nodes
        foreach (IUriNode u in g.Nodes.UriNodes())
        {
            //Write the URI to the Console
            //Debug.Log(u.Uri.ToString());
        }

    }

    public async void Query5()
    {

        string disability = dropdown14.options[dropdown14.value].text;
        string benefit = dropdown15.options[dropdown15.value].text;

        Debug.Log("File Loading");
        IGraph g = new Graph();
        TurtleParser ttlparser = new TurtleParser();
        ttlparser.Load(g, Application.dataPath + "/RDF.ttl");
        //FileLoader.Load(g, "RDF.xml");
        Debug.Log("File Loaded");
        Q5(g, disability,benefit);
        Debug.Log("Query Answered");
        //Assuming we have some Graph g find all the URI Nodes
        foreach (IUriNode u in g.Nodes.UriNodes())
        {
            //Write the URI to the Console
            //Debug.Log(u.Uri.ToString());
        }

    }
    public void SorguButon()
    {
        if (PatientQuestions.active)
        {
            selectedToggle = toggles1.ActiveToggles().FirstOrDefault();
            if (selectedToggle.gameObject.name == "Toggle1")
                Query1();
            if (selectedToggle.gameObject.name == "Toggle2")
                Query2();
            if (selectedToggle.gameObject.name == "Toggle3")
                Query3();
            if (selectedToggle.gameObject.name == "Toggle4")
                Query4();
            if (selectedToggle.gameObject.name == "Toggle5")
                Query5();
        }
        if (GameQuestions.active)
        {
            selectedToggle = toggles2.ActiveToggles().FirstOrDefault();
            if (selectedToggle.gameObject.name == "Toggle1")
                Query5();
        }
    }

    public void DropdownChanged()
    {
        int no = questionDropdown.value;
        PatientQuestions.SetActive(false);
        DisabilityQuestions.SetActive(false);
        GameQuestions.SetActive(false);
        if (no == 0)
        {
            PatientQuestions.SetActive(true);
        }
        if (no == 1)
        {
            GameQuestions.SetActive(true);
        }
        if (no == 2)
        {
            DisabilityQuestions.SetActive(true);
        }
    }

    static string Q4(IGraph g,int age,string score,string game,string metric) //https://dotnetrdf.org/docs/stable/user_guide/querying_with_sparql.html
    {
        try
        { //Which children under age 13 have a completeness value greater than 0.5 for at least 1 session of Uzay Macerası Kurtarma Görevi?
            object results = g.ExecuteQuery("PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> SELECT DISTINCT ?id WHERE " +
                "{ ?x <tag:stardog:designer:Serious_Game:model:patient_id> ?id." +
                " ?x <tag:stardog:designer:Serious_Game:model:age> ?age. FILTER(?age < "+age+")" + //Under age 13
                " ?y <tag:stardog:designer:Serious_Game:model:plays_game> ?session. FILTER regex(xsd:string(?y) , xsd:string(?id))" + //Sessions of the children
                " ?session <tag:stardog:designer:Serious_Game:model:gamePlayed> ?gameName. FILTER regex(?gameName,\"" + game + "\")" + //Sessions in which Kurtarma Görevi is played
                "?session <tag:stardog:designer:Serious_Game:model:"+metric+"> ?"+metric+". FILTER (xsd:double(?"+metric+")>"+score+")}"); //Sessions with completeness value is greater than 0.5
            /*             object results = g.ExecuteQuery("SELECT ?time WHERE " +
                "{ ?obj_0 a <tag:stardog:designer:Serious_Games:data:PatientGameDB.PatientGameCollection:11.02.2024%2006%3A40%3A00>." +
                "  ?obj_0 <tag:stardog:designer:Serious_Games:model:time> ?time. }");
            */
            if (results is SparqlResultSet)
            {
                //SELECT/ASK queries give a SparqlResultSet
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (SparqlResult r in rset)
                {
                    Debug.Log(r.ToString());  
                    return r.ToString().Split('^')[0];
                }
            }
            else if (results is IGraph)
            {
                //CONSTRUCT/DESCRIBE queries give a IGraph
                IGraph resGraph = (IGraph)results;
                foreach (Triple t in resGraph.Triples)
                {
                    //Do whatever you want with each Triple
                }
                return "";
            }
            else
            {
                //If you don't get a SparqlResutlSet or IGraph something went wrong 
                //but didn't throw an exception so you should handle it here
                Debug.Log("ERROR");
                return "";
            }
            return "Yok";
        }
        catch (RdfQueryException queryEx)
        {
            //There was an error executing the query so handle it here
            Debug.Log(queryEx.Message);
            return "";
        }
    }

    static void Q5(IGraph g, string disability,string benefit) //https://dotnetrdf.org/docs/stable/user_guide/querying_with_sparql.html
    {
        try
        { //Games used for treating Hemipleji and Dipleji patients which also include aritmhetics?
            object results = g.ExecuteQuery("SELECT ?game WHERE " +
             "{?game <tag:stardog:designer:Serious_Game:model:used_for_treating> ?disability. FILTER regex(?disability,\"" + disability + "\")" +
             "?game <tag:stardog:designer:Serious_Game:model:reaching> ?benefits. FILTER regex(?benefits,\"" + benefit + "\") }");

            if (results is SparqlResultSet)
            {
                //SELECT/ASK queries give a SparqlResultSet
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (SparqlResult r in rset)
                {
                    Debug.Log(r.ToString());
                }
            }
            else if (results is IGraph)
            {
                //CONSTRUCT/DESCRIBE queries give a IGraph
                IGraph resGraph = (IGraph)results;
                foreach (Triple t in resGraph.Triples)
                {
                    //Do whatever you want with each Triple
                }
            }
            else
            {
                //If you don't get a SparqlResutlSet or IGraph something went wrong 
                //but didn't throw an exception so you should handle it here
                Debug.Log("ERROR");
            }
        }
        catch (RdfQueryException queryEx)
        {
            //There was an error executing the query so handle it here
            Debug.Log(queryEx.Message);
        }
    }

}
