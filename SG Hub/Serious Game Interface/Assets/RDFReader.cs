using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VDS.RDF;
using VDS.RDF.Parsing;
using System;
using VDS.RDF.Query.Builder;
using VDS.RDF.Query;
using VDS.RDF.Writing.Formatting;

public class RDFReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("File Loading");
        IGraph g = new Graph();
        TurtleParser ttlparser = new TurtleParser();
        ttlparser.Load(g, Application.dataPath + "/RDF.ttl");
        //FileLoader.Load(g, "RDF.xml");
        Debug.Log("File Loaded");
        //HelloWorld(g);
        Debug.Log("Query Answered");
        //Assuming we have some Graph g find all the URI Nodes
        foreach (IUriNode u in g.Nodes.UriNodes())
        {
            //Write the URI to the Console
            //Debug.Log(u.Uri.ToString());
        }

    }

    static void HelloWorld(IGraph g) //https://dotnetrdf.org/docs/stable/user_guide/querying_with_sparql.html
    {
        try
        { //Which children under age 13 have a completeness value greater than 0.5 for at least 1 session of Uzay Macerası Kurtarma Görevi?
            object results = g.ExecuteQuery("PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> SELECT DISTINCT ?id WHERE " +
                "{ ?x <tag:stardog:designer:Serious_Game:model:patient_id> ?id." +
                " ?x <tag:stardog:designer:Serious_Game:model:age> ?age. FILTER(?age < 15)" + //Under age 13
                " ?y <tag:stardog:designer:Serious_Game:model:plays_game> ?session. FILTER regex(xsd:string(?y) , xsd:string(?id))" + //Sessions of the children
                " ?session <tag:stardog:designer:Serious_Game:model:gamePlayed> ?gameName. FILTER regex(?gameName,\"Kurtarma\")" + //Sessions in which Kurtarma Görevi is played
                "?session <tag:stardog:designer:Serious_Game:model:completeness> ?completeness. FILTER (xsd:double(?completeness)>0.2)}"); //Sessions with completeness value is greater than 0.5
                         
            
            /*object results = g.ExecuteQuery("SELECT ?game WHERE " +
                "{?game <tag:stardog:designer:Serious_Game:model:used_for_treating>  \"Dipleji,Hemipleji\" .}");*/
            
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