using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;


public class UI : MonoBehaviour
{

    public GameObject canvasFilter;
    public Text alien_count;
    public GameObject gameOverScreen;
    public Text rapor;
    public Text rapor2;
    public Text rapor3;
    private DatabaseAccess databaseAccess;
    bool didEnd = false;
    private bool gameliked=false;
    private int Score;

    public Image image;
    public GameObject heatmap2Obj;

    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();

        image = canvasFilter.GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = secondPartData.brightness;
        image.color = tempColor;

        AudioListener.volume = secondPartData.volume;
    }
    void Update()
    {
        if (firstPartData.OnThirdPart == true)
            alien_count.text = thirdPartData.newAlienIndex.ToString();
        else if (firstPartData.OnSecondPart ==false)
            alien_count.text = firstPartData.alienCount.ToString();

        else
            alien_count.text = secondPartData.newAlienIndex.ToString();

        if ((((secondPartData.alienGoalCount== secondPartData.newAlienIndex && secondPartData.endWithCount) || (secondPartData.timer >= secondPartData.timeGoalMinutes *60) && secondPartData.endWithCount==false)) 
            && firstPartData.OnSecondPart && didEnd == false)
        {
            if (secondPartData.character == "ardacan")
            {
                FindObjectOfType<AudioManager>().Play("Win1");
            }
            if (secondPartData.character == "ecesu")
            {
                FindObjectOfType<AudioManager>().Play("Win1E");
            }
            Destroy(GameObject.FindWithTag("Cursor"));
            Debug.Log("Game Over");
            gameOverScreen.SetActive(true);
            heatmap2Obj.GetComponent<heatmap2>().calculateHeatmap();
            Score = (int)(25 * secondPartData.steadiness_ratios.Average() + 25 * secondPartData.duration_ratios.Average() + 25 * secondPartData.completeness_ratios.Average() + 25 * secondPartData.smoothness_ratios.Average());
            if (Score < 0)
                Score = (int)(33 * secondPartData.steadiness_ratios.Average() + 33 * secondPartData.duration_ratios.Average() + 33 * secondPartData.completeness_ratios.Average());
            rapor.text = ((int)secondPartData.timer).ToString() + " saniyede"; 
            rapor2.text = secondPartData.newAlienIndex.ToString() + " uzaylı kurtardın.\n";
            rapor3.text = "Skor\n" + Score.ToString();
            Debug.Log("secondpartend started");
            secondPartEnd();
            Debug.Log("secondpartend end");
            didEnd = true;

            /*
                        var eng = IronPython.Hosting.Python.CreateEngine();
                        var scope = eng.CreateScope();
                        eng.Execute(@"if True:
                def greetings(name):
                    return 'Hello ' + name.title() + '!'
            ", scope);
                        dynamic greetings = scope.GetVariable("greetings");
                        System.Console.WriteLine(greetings("world"));
                      */
            //engine.ExecuteFile("bo_examples.py");
        }

        if ((((thirdPartData.alienGoalCount == thirdPartData.newAlienIndex && thirdPartData.endWithCount) || (thirdPartData.timer >= thirdPartData.timeGoalMinutes * 60) && thirdPartData.endWithCount == false))
    && firstPartData.OnThirdPart && didEnd == false)
        {
            if (thirdPartData.character == "ardacan")
            {
                FindObjectOfType<AudioManager>().Play("Win1");
            }
            if (thirdPartData.character == "ecesu")
            {
                FindObjectOfType<AudioManager>().Play("Win1E");
            }
            Destroy(GameObject.FindWithTag("Cursor"));
            Debug.Log("Game Over");
            gameOverScreen.SetActive(true);
            thirdPartEnd();
            Score = (int)(25 * thirdPartData.steadiness_ratios.Average() + 25 * thirdPartData.duration_ratios.Average() + 25 * thirdPartData.completeness_ratios.Average() + 25 * thirdPartData.smoothness_ratios.Average());
            if (Score < 0)
                Score = (int)(33 * thirdPartData.steadiness_ratios.Average() + 33 * thirdPartData.duration_ratios.Average() + 33 * thirdPartData.completeness_ratios.Average());
            rapor.text = "Toplama: " + (thirdPartData.dogruToplama).ToString() + "/"+ (thirdPartData.totalToplama).ToString()
                + "\nÇıkarma: " + (thirdPartData.dogruCikarma).ToString() + "/" + (thirdPartData.totalCikarma).ToString()
                + "\nÇarpma: " + (thirdPartData.dogruCarpma).ToString() + "/" + (thirdPartData.totalCarpma).ToString();
            rapor2.text = (thirdPartData.newAlienIndex).ToString() + " uzaylı kurtardın.";
            rapor3.text = "Skor\n" + Score.ToString();

            didEnd = true;

        }
    }
    /*public void gameLikeButtonClicked()
    {
        gameliked = true;
        DateTime date = DateTime.UtcNow;
        Score = (int)(Score *1.5f);
        databaseAccess.SaveScoreToDataBase(ChosenPatient.patientID, ChosenPatient.patientName, Score, StartGame.speedLval, StartGame.speedRval, StartGame.sizeval, firstPartData.timer, heatmap.ratioHeatmapArray, gameliked,date.ToString(), "Uzay Macerası:Kurtarma Görevi");
        //String.Join("-", PlayerController.firstMissedAlienPositions), gameliked);
    }

    public void gameDislikeButtonClicked()
    {
        gameliked = false;
        DateTime date = DateTime.UtcNow;
        databaseAccess.SaveScoreToDataBase(ChosenPatient.patientID, ChosenPatient.patientName, Score, StartGame.speedLval, StartGame.speedRval, StartGame.sizeval, firstPartData.timer, heatmap.ratioHeatmapArray, gameliked,date.ToString(), "Uzay Macerası:Kurtarma Görevi");
    }

    public void firstPartEnd()
    {
        Score = 150 - (int)firstPartData.timer - (firstPartData.firstMissedAlienPositions.Count * 5) - firstPartData.smoothness_exceptions *3;
        DateTime date = DateTime.UtcNow;
        gameliked = true;
        databaseAccess.SaveScoreToDataBase(ChosenPatient.patientID, ChosenPatient.patientName, Score, StartGame.speedLval, StartGame.speedRval, StartGame.sizeval, firstPartData.timer, heatmap.ratioHeatmapArray, gameliked, date.ToString(), "Uzay Macerası:Meteor Yolculuğu");
        //SceneManager.LoadScene("Menu");

        for (int i = 0; i < 20; i++)
            Debug.Log(HandTracking.allCoordinates[i]);
    }*/

    public void secondPartEnd()
    {
        Score = (int)(25 * secondPartData.steadiness_ratios.Average()+ 25 * secondPartData.duration_ratios.Average() + 25 * secondPartData.completeness_ratios.Average() + 25 * secondPartData.smoothness_ratios.Average() );
        DateTime date = DateTime.UtcNow;
        gameliked = true;
        databaseAccess.SaveScoreToDataBaseG2(ChosenPatient.patientID,  Score, secondPartData.barParam, secondPartData.timeval, (float)Convert.ToInt32(secondPartData.friendMove), 
            secondPartData.timer,heatmap2.heatmapArray, gameliked, date.ToString(), "Uzay Macerası:Kurtarma Görevi", secondPartData.smoothness_ratios.Average(), 
            secondPartData.completeness_ratios.Average(), secondPartData.duration_ratios.Average(), secondPartData.steadiness_ratios.Average());
        //SceneManager.LoadScene("Menu");

        //for (int i = 0; i < 20; i++)
            //Debug.Log(HandTracking.allCoordinates[i]);
    }


    public void thirdPartEnd()
    {
        Score = (int)(25 * thirdPartData.steadiness_ratios.Average() + 25 * thirdPartData.duration_ratios.Average() + 25 * thirdPartData.completeness_ratios.Average() + 25 * thirdPartData.smoothness_ratios.Average());
        DateTime date = DateTime.UtcNow;
        gameliked = true;
        float param1 = 0f;
        if (thirdPartData.toplama)
            param1 += 0.32f;
        if (thirdPartData.cikarma)
            param1 += 0.33f;
        if (thirdPartData.carpma)
            param1 += 0.35f;

        float[] heatmapArray = new float[3];
        if (thirdPartData.totalToplama != 0)
            heatmapArray[0] = (float)thirdPartData.dogruToplama / thirdPartData.totalToplama;
        else
            heatmapArray[0] = -1;
        if (thirdPartData.totalCikarma != 0)
            heatmapArray[1] = (float)thirdPartData.dogruCikarma / thirdPartData.totalCikarma;
        else
            heatmapArray[1] = -1;
        if (thirdPartData.totalCarpma != 0)
            heatmapArray[2] = (float)thirdPartData.dogruCarpma / thirdPartData.totalCarpma;
        else
            heatmapArray[2] = -1;


        databaseAccess.SaveScoreToDataBaseG3(ChosenPatient.patientID,  Score, thirdPartData.barParam, thirdPartData.timeval, param1, thirdPartData.timer, heatmapArray, gameliked, date.ToString(), "Uzay Macerası:Roket Karmaşası", thirdPartData.smoothness_ratios.Average(),
            thirdPartData.completeness_ratios.Average(), thirdPartData.duration_ratios.Average(), thirdPartData.steadiness_ratios.Average());
        //SceneManager.LoadScene("Menu");

    }
}
