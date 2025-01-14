using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Heatmap : MonoBehaviour
{
    // Start is called before the first frame update
    public List<double[,]> heatmapArrays = new List<double[,]>();
    public List<int[,]> CaughtHeatmapArrays = new List<int[,]>();
    public List<int[,]> MissedHeatmapArrays = new List<int[,]>();

    public List<float[,]> CaughtAlienSmoothness = new List<float[,]>();
    public List<float[,]> CaughtAlienCompleteness = new List<float[,]>();
    public List<float[,]> CaughtAlienDuration = new List<float[,]>();
    public List<float[,]> CaughtAlienSteadiness = new List<float[,]>();

    public Image[] heatmapTileColors;
    private DatabaseAccess databaseAccess;
    public Dropdown dateDropdown;
    public Dropdown heatmapDropdown;

    public static int sessionCount;

    public int startingPoint;
    public int endingPoint;
    public GameObject minmaxSlider;

    public Text gamename;

    public GameObject lejand;
    public GameObject lejandSum;
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        
        setupHeatmap();
    }

    public async void setupHeatmap()
    {

        var task = databaseAccess.GetScoresFromDatabase();
        var result = await task;
        dateDropdown.ClearOptions();
        heatmapArrays = new List<double[,]>();
        CaughtHeatmapArrays = new List<int[,]>();
        CaughtAlienSmoothness = new List<float[,]>();
        dateDropdown.options.Add(new Dropdown.OptionData() { text = "Özet" });
        for (int i=0;i<result.Count;i++)
        {
            Debug.Log(result[i].gamePlayed);
            if (result[i].gamePlayed == gamename.text)
            {
                dateDropdown.options.Add(new Dropdown.OptionData(result[i].Date));
                heatmapArrays.Add(result[i].FirstHeatMap);
                CaughtHeatmapArrays.Add(result[i].FirstCaughtHeatMap); 
                MissedHeatmapArrays.Add(result[i].FirstMissedHeatMap);

                float[,] tempSm=new float[5,5];
                float[,] tempCo = new float[5, 5];
                float[,] tempDu = new float[5, 5];
                float[,] tempSt = new float[5, 5];
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        tempSm[x,y]=result[i].FirstCaughtAlienStats[x,y,0];
                        tempCo[x, y] = result[i].FirstCaughtAlienStats[x, y, 1];
                        tempDu[x, y] = result[i].FirstCaughtAlienStats[x, y, 2];
                        tempSt[x, y] = result[i].FirstCaughtAlienStats[x, y, 3];
                    }
                }
                CaughtAlienSmoothness.Add(tempSm);
                CaughtAlienCompleteness.Add(tempCo);
                CaughtAlienDuration.Add(tempDu);
                CaughtAlienSteadiness.Add(tempSt);
                //Debug.Log(heatmapArrays[i]);
            }

        }
        dateDropdown.RefreshShownValue();
        Debug.Log("heatmap setup");
        sessionCount = heatmapArrays.Count;
        UpdateHeatmapTile();
    }



    public void UpdateHeatmapTile()
    {
        if (gamename.text == "Uzay Macerası:Meteor Yolculuğu")
        {
            int i = dateDropdown.value - 1;
            
            if (i == -1)
            {
                lejand.SetActive(false);
                lejandSum.SetActive(true);
                //StartCoroutine(printHeatmap()); //Change comment for animation or difference 
                //DifferenceSummaryUpdate();
                LinearRegression();

            }
            else
            {
                lejand.SetActive(true);
                lejandSum.SetActive(false);

                if (heatmapDropdown.options[heatmapDropdown.value].text == "Uzaylı")
                {
                    double[,] heatmapArray = heatmapArrays[i];
                    int[,] CaughtHeatmapArray = CaughtHeatmapArrays[i];
                    int[,] MissedHeatmapArray = MissedHeatmapArrays[i];
                    for (int x = 0; x < 5; x++)
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            if (heatmapArray[x, y] == -1f)
                                heatmapTileColors[x * 5 + y].color = Color.blue;
                            else if (heatmapArray[x, y] <= 0.2f)
                                heatmapTileColors[x * 5 + y].color = Color.red;
                            else if (heatmapArray[x, y] <= 0.4f)
                                heatmapTileColors[x * 5 + y].color = new Color(1F, 0.56F, 0F);
                            else if (heatmapArray[x, y] <= 0.6f)
                                heatmapTileColors[x * 5 + y].color = new Color(1F, 0.84F, 0F);
                            else if (heatmapArray[x, y] <= 0.8f)
                                heatmapTileColors[x * 5 + y].color = new Color(0.8F, 1F, 0F);
                            else if (heatmapArray[x, y] <= 1f)
                                heatmapTileColors[x * 5 + y].color = Color.green;
                            heatmapTileColors[x * 5 + y].GetComponentInChildren<Text>().text = CaughtHeatmapArray[x, y].ToString() + "/" + (CaughtHeatmapArray[x, y] + MissedHeatmapArray[x, y]).ToString();

                        }
                    }
                }
                else{
                    float[,] heatmapArray = CaughtAlienSmoothness[i];
                    if (heatmapDropdown.options[heatmapDropdown.value].text == "Smoothness")
                    {
                        heatmapArray = CaughtAlienSmoothness[i];
                    }
                    else if (heatmapDropdown.options[heatmapDropdown.value].text == "Completeness")
                    {
                        heatmapArray = CaughtAlienCompleteness[i];
                    }
                    else if (heatmapDropdown.options[heatmapDropdown.value].text == "Duration")
                    {
                        heatmapArray = CaughtAlienDuration[i];
                    }
                    else if (heatmapDropdown.options[heatmapDropdown.value].text == "Steadiness")
                    {
                        heatmapArray = CaughtAlienSteadiness[i];
                    }

                    for (int x = 0; x < 5; x++)
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            if (heatmapArray[x, y] == -1f || float.IsNaN(heatmapArray[x, y]))
                                heatmapTileColors[x * 5 + y].color = Color.blue;
                            else if (heatmapArray[x, y] <= 0.2f)
                                heatmapTileColors[x * 5 + y].color = Color.red;
                            else if (heatmapArray[x, y] <= 0.4f)
                                heatmapTileColors[x * 5 + y].color = new Color(1F, 0.56F, 0F);
                            else if (heatmapArray[x, y] <= 0.6f)
                                heatmapTileColors[x * 5 + y].color = new Color(1F, 0.84F, 0F);
                            else if (heatmapArray[x, y] <= 0.8f)
                                heatmapTileColors[x * 5 + y].color = new Color(0.8F, 1F, 0F);
                            else if (heatmapArray[x, y] <= 1f)
                                heatmapTileColors[x * 5 + y].color = Color.green;
                            heatmapTileColors[x * 5 + y].GetComponentInChildren<Text>().text = (heatmapArray[x, y]).ToString();

                        }
                    }
                }
                
            }
        }
    }
    public void DifferenceSummaryUpdate()
    {
        
        
        int length = heatmapArrays.Count;
        startingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().minValue-1;
        endingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().maxValue;
        double[,] prevHeatmapArray = heatmapArrays[startingPoint];
        double[,] improvementHeatmapArray = new double[5, 5];
        for (int i = startingPoint+1; i < endingPoint; i++)
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                { //Hepsini bir öncekinden değil de hepsini ilkinden çıkarıyor, 
                    //bir öncekinden istersek prevHeatmapi güncellemek lazım
                    improvementHeatmapArray[x,y]+=(heatmapArrays[i][x, y] - prevHeatmapArray[x, y]);

                }
            }
        }

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (improvementHeatmapArray[x, y] == 0)
                    heatmapTileColors[x * 5 + y].color = Color.blue;
                else if (improvementHeatmapArray[x, y] >= 3)
                    heatmapTileColors[x * 5 + y].color = Color.green;
                else if (improvementHeatmapArray[x, y] == 2)
                    heatmapTileColors[x * 5 + y].color = Color.yellow;
                else if (improvementHeatmapArray[x, y] == 1)
                    heatmapTileColors[x * 5 + y].color = new Color(1.0f, 0.64f, 0.0f);
                else if (improvementHeatmapArray[x, y] < 0)
                    heatmapTileColors[x * 5 + y].color = Color.black;

            }
        }
    }
    public List<double> Points { get; private set; }
    public List<double> Indices { get; private set; }
    public List<double> Slopes { get; private set; }
    public List<List<double>> AllPoints { get; private set; }
    public List<List<double>> AllIndices { get; private set; }
    private double learningRate= 1e-05;
    private double normalize=1000;
    private double epochs=500;
    double a = 0;
    double b = 0;

    public double LinearRegression() //To find the slope of every heatmap location for all sessions
    {
        startingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().minValue - 1;
        endingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().maxValue;

        AllPoints = new List<List<double>>();
        AllIndices = new List<List<double>>();
        Slopes = new List<double>();
        for (int x = 0; x < 5; x++) //Structuring the points by storing the points at the same location together
        {
            for (int y = 0; y < 5; y++)
            {
                Points = new List<double>();
                Indices = new List<double>();
                for (int i = startingPoint; i < endingPoint; i++)
                {
                    if (heatmapDropdown.options[heatmapDropdown.value].text == "Uzaylı")
                    {
                        Points.Add(heatmapArrays[i][x, y]);
                        Indices.Add(i);
                    }
                    if (heatmapDropdown.options[heatmapDropdown.value].text == "Smoothness" && float.IsNaN((float)CaughtAlienSmoothness[i][x, y]) == false)
                    {
                        Points.Add(CaughtAlienSmoothness[i][x, y]);
                        Indices.Add(i);
                    }
                    if (heatmapDropdown.options[heatmapDropdown.value].text == "Completeness" && float.IsNaN((float)CaughtAlienCompleteness[i][x, y]) == false)
                    {
                        Points.Add(CaughtAlienCompleteness[i][x, y]);
                        Indices.Add(i);
                    }
                    if (heatmapDropdown.options[heatmapDropdown.value].text == "Duration" && float.IsNaN((float)CaughtAlienDuration[i][x, y]) == false)
                    {
                        Points.Add(CaughtAlienDuration[i][x, y]);
                        Indices.Add(i);
                    }
                    if (heatmapDropdown.options[heatmapDropdown.value].text == "Steadiness" && float.IsNaN((float)CaughtAlienSteadiness[i][x, y]) == false)
                    {
                        Points.Add(CaughtAlienSteadiness[i][x, y]);
                        Indices.Add(i);
                    }
                    
                }
                AllPoints.Add(Points);
                AllIndices.Add(Indices);
            }
        }

        for (int j = 0; j < AllPoints.Count; j++) //Linear Regression for all locations
        {
            Points=AllPoints[j];
            Indices = AllIndices[j];
            a = 0;
            b = 0;

            double N = Points.Count;//pointGenerator.Points.Count;

            var X = Indices.ToArray();
            var Y = Points.ToArray();
            var Y_pred = Indices.ToArray();
            for (int i = 0; i < epochs; i++)
            {
                for(int k = 0; k < X.Length; k++)
                {
                    Y_pred[k] = X[k] * a + b;

                }
                //multiply each index by a and add b to predict a score for that index

                //Derive D_a and D_b values based on how close the prediction was
                double D_a = (-2.0 / N) * Y                    
                    .Zip(Y_pred, (y, y_pred) => y - y_pred)
                    .Zip(X, (y_p, x) => y_p * x)
                    .Sum();
                double D_b = (-2.0 / N) * Y
                    .Zip(Y_pred, (y, y_pred) => y - y_pred)
                    .Sum();
                a -= learningRate * D_a;
                b -= learningRate * D_b * normalize;
                //Change a and b according to the learning rate at each iteration to be closer to the line that fits best           
            }
            //Debug.Log($"{j} a: {a}, b: {b}");
            Slopes.Add(a);
            //We have all the slopes for each region in the heatmap, now let's display it here

        }
        for (int x = 0; x < 25; x++)
        {


            if (Slopes[x] >= 0.01)
                heatmapTileColors[x].color = Color.green;
            else if (Slopes[x] >= 0.005)
                heatmapTileColors[x].color = Color.yellow;
            else if (Slopes[x] >= 0.001)
                heatmapTileColors[x].color = new Color(1.0f, 0.64f, 0.0f);
            else if (Slopes[x] < -0.001)
                heatmapTileColors[x].color = Color.black;
            else if (Slopes[x] < 0.001 || float.IsNaN((float)Slopes[x]))
                heatmapTileColors[x].color = Color.blue;
            heatmapTileColors[x].GetComponentInChildren<Text>().text = Slopes[x].ToString();
        }
        return 0;
    }


    IEnumerator printHeatmap()
    {
        sessionCount = heatmapArrays.Count;
        int startingPoint = 0;//burası değişmeli çalışacaksa
        //if (startingPoint < 0)
            startingPoint = 0;
        Debug.Log("Starting point:");
        Debug.Log(startingPoint);
        int counter = 1;
        double[,] avgHeatmapArray = new double[5, 5];
        /*
        4 eleman varsa->1,1,1,1
        5->2,1,1,1
        6->2,2,1,1
        7->2,2,2,1
        8->2,2,2,2*/

        for (int z = startingPoint; z < sessionCount; z++) //For every heatmap
        {
            double[,] heatmapArray = heatmapArrays[z];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Debug.Log(heatmapArray[x, y]);
                    avgHeatmapArray[x, y] += heatmapArray[x, y];
                }
            }

            if ((z + 1) % Mathf.Ceil(sessionCount/ 4f) == 0)
            { //if this heatmap is the one that should be printed

                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (avgHeatmapArray[x, y] == 0)
                            heatmapTileColors[x * 5 + y].color = Color.blue;
                        else if (avgHeatmapArray[x, y] == 1)
                            heatmapTileColors[x * 5 + y].color = Color.green;
                        else if (avgHeatmapArray[x, y] == 2)
                            heatmapTileColors[x * 5 + y].color = Color.yellow;
                        else if (avgHeatmapArray[x, y] == 3)
                            heatmapTileColors[x * 5 + y].color = new Color(1.0f, 0.64f, 0.0f);
                        else if (avgHeatmapArray[x, y] > 4)
                            heatmapTileColors[x * 5 + y].color = Color.red;

                    }

                }
                avgHeatmapArray = new double[5, 5];
                yield return new WaitForSeconds(1);
            }



        }

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (avgHeatmapArray[x, y] == 0)
                    heatmapTileColors[x * 5 + y].color = Color.blue;
                else if (avgHeatmapArray[x, y] == 1)
                    heatmapTileColors[x * 5 + y].color = Color.green;
                else if (avgHeatmapArray[x, y] == 2)
                    heatmapTileColors[x * 5 + y].color = Color.yellow;
                else if (avgHeatmapArray[x, y] == 3)
                    heatmapTileColors[x * 5 + y].color = new Color(1.0f, 0.64f, 0.0f);
                else if (avgHeatmapArray[x, y] > 4)
                    heatmapTileColors[x * 5 + y].color = Color.red;

            }

        }

    }
}
