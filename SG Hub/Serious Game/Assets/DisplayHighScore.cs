using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayHighScore : MonoBehaviour
{
    private DatabaseAccess databaseAccess;

    public Text highScoreOutput;
    // Start is called before the first frame update
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        Invoke("DisplayHighScoreInText", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void DisplayHighScoreInText()
    {
        var task = databaseAccess.GetScoresFromDatabase();
        var result = await task;
        var output = "";
        foreach(var score in result)
        {
            //output += score.UserName + " Score: " + score.Score;
        }
        highScoreOutput.text = output;
    }
}
