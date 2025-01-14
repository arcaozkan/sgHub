/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Window_Graph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private DatabaseAccess databaseAccess;

    public GameObject heatmap1;
    public GameObject heatmap2;
    public GameObject heatmap3;

    public Text gamename;
    public Text lasthowmany;
    public Text attributeShown;

    public GameObject hoverPanel;
    private bool isRegionalGraph=false;
    public string regionName="";

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        //dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        //dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();



        getScores();

    }

    public async void getScores()
    {
        if (attributeShown.text == "Skor")
        {
            List<double> valueList = new List<double>() { };
            List<double> param1List = new List<double>() { };
            List<double> param2List = new List<double>() { };
            var task = databaseAccess.GetScoresFromDatabase();
            var result = await task;

            foreach (var patient in result)
            {
                if (patient.gamePlayed == gamename.text)
                {
                    valueList.Add(patient.Score);
                    param1List.Add(patient.param1);
                    param2List.Add(patient.param2);
                    if (valueList.Count > int.Parse(lasthowmany.text))
                    {
                        valueList.RemoveAt(0);
                        param1List.RemoveAt(0);
                        param2List.RemoveAt(0);
                    }
                }
            }
            string tempText = "Seans ";
            if (int.Parse(lasthowmany.text) != 5)
                tempText = "";
            ShowGraph(valueList, param1List, param2List, (int _i) => tempText + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
        }
    }

    public async void getTimes()
    {
        if (attributeShown.text == "Zaman")
        {
            List<double> valueList = new List<double>() { };
            List<double> param1List = new List<double>() { };
            List<double> param2List = new List<double>() { };
            var task = databaseAccess.GetScoresFromDatabase();
            var result = await task;

            foreach (var patient in result)
            {
                if (patient.gamePlayed == gamename.text)
                {
                    Debug.Log("Time:");
                    Debug.Log((int)patient.time);
                    valueList.Add((int)patient.time);
                    param1List.Add(patient.param1);
                    param2List.Add(patient.param2);
                    if (valueList.Count > int.Parse(lasthowmany.text))
                    {
                        valueList.RemoveAt(0);
                        param1List.RemoveAt(0);
                        param2List.RemoveAt(0);
                    }
                }
            }
            string tempText = "Seans ";
            if (int.Parse(lasthowmany.text) != 5)
                tempText = "";
            ShowGraph(valueList, param1List, param2List, (int _i) => tempText + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
        }
    }

    public async void getSmoothness()
    {
        if (attributeShown.text == "Smoothness")
        {
            List<double> valueList = new List<double>() { };
            List<double> param1List = new List<double>() { };
            List<double> param2List = new List<double>() { };
            var task = databaseAccess.GetScoresFromDatabase();
            var result = await task;

            foreach (var patient in result)
            {
                if (patient.gamePlayed == gamename.text)
                {
                    //Debug.Log("Smoothness:");
                    //Debug.Log((int)patient.smoothness);
                    if (isRegionalGraph == false)
                        valueList.Add((double)patient.smoothness);
                    else
                    {
                        int x, y;
                        x = regionName[0]-'0'-1;
                        y = regionName[2]-'0'-1;
                        Debug.Log("Smoothness for:");
                        Debug.Log(x);
                        Debug.Log(y);
                        valueList.Add((double)patient.FirstCaughtAlienStats[x,y,0]);
                    }

                    param1List.Add(patient.param1);
                    param2List.Add(patient.param2);
                    if (valueList.Count > int.Parse(lasthowmany.text))
                    {
                        valueList.RemoveAt(0);
                        param1List.RemoveAt(0);
                        param2List.RemoveAt(0);
                    }
                }
            }
            string tempText = "Seans ";
            if (int.Parse(lasthowmany.text) != 5)
                tempText = "";
            ShowGraph(valueList, param1List, param2List, (int _i) => tempText + (_i + 1), (float _f) => "" + _f);
        }
    }
    public async void getCompleteness()
    {
        if (attributeShown.text == "Completeness")
        {
            List<double> valueList = new List<double>() { };
            List<double> param1List = new List<double>() { };
            List<double> param2List = new List<double>() { };
            var task = databaseAccess.GetScoresFromDatabase();
            var result = await task;

            foreach (var patient in result)
            {
                if (patient.gamePlayed == gamename.text)
                {
                    //Debug.Log("Smoothness:");
                    //Debug.Log((int)patient.smoothness);
                    if (isRegionalGraph == false)
                        valueList.Add((double)patient.completeness);
                    else
                    {
                        int x, y;
                        x = regionName[0] - '0' - 1;
                        y = regionName[2] - '0' - 1;
                        Debug.Log("Completeness for:");
                        Debug.Log(x);
                        Debug.Log(y);
                        valueList.Add((double)patient.FirstCaughtAlienStats[x,y,1]);
                    }
                    param1List.Add(patient.param1);
                    param2List.Add(patient.param2);
                    if (valueList.Count > int.Parse(lasthowmany.text))
                    {
                        valueList.RemoveAt(0);
                        param1List.RemoveAt(0);
                        param2List.RemoveAt(0);
                    }
                }
            }
            string tempText = "Seans ";
            if (int.Parse(lasthowmany.text) != 5)
                tempText = "";
            ShowGraph(valueList, param1List, param2List, (int _i) => tempText + (_i + 1), (float _f) => "" + _f);
        }
    }
    public async void getDuration()
    {
        if (attributeShown.text == "Duration")
        {
            List<double> valueList = new List<double>() { };
            List<double> param1List = new List<double>() { };
            List<double> param2List = new List<double>() { };
            var task = databaseAccess.GetScoresFromDatabase();
            var result = await task;

            foreach (var patient in result)
            {
                if (patient.gamePlayed == gamename.text)
                {
                    //Debug.Log("Smoothness:");
                    //Debug.Log((int)patient.smoothness);
                    if (isRegionalGraph == false)
                        valueList.Add((double)patient.duration);
                    else
                    {
                        int x, y;
                        x = regionName[0] - '0' - 1;
                        y = regionName[2] - '0' - 1;
                        Debug.Log("Duration for:");
                        Debug.Log(x);
                        Debug.Log(y);
                        valueList.Add((double)patient.FirstCaughtAlienStats[x,y,2]);
                    }
                    param1List.Add(patient.param1);
                    param2List.Add(patient.param2);
                    if (valueList.Count > int.Parse(lasthowmany.text))
                    {
                        valueList.RemoveAt(0);
                        param1List.RemoveAt(0);
                        param2List.RemoveAt(0);
                    }
                }
            }
            string tempText = "Seans ";
            if (int.Parse(lasthowmany.text) != 5)
                tempText = "";
            ShowGraph(valueList, param1List, param2List, (int _i) => tempText + (_i + 1), (float _f) => "" + _f);
        }
    }
    public async void getSteadiness()
    {
        if (attributeShown.text == "Steadiness")
        {
            List<double> valueList = new List<double>() { };
            List<double> param1List = new List<double>() { };
            List<double> param2List = new List<double>() { };
            var task = databaseAccess.GetScoresFromDatabase();
            var result = await task;

            foreach (var patient in result)
            {
                if (patient.gamePlayed == gamename.text)
                {
                    //Debug.Log("Smoothness:");
                    //Debug.Log((int)patient.smoothness);
                    if (isRegionalGraph == false)
                        valueList.Add((double)patient.steadiness);
                    else
                    {
                        int x, y;
                        x = regionName[0] - '0' - 1;
                        y = regionName[2] - '0' - 1;
                        Debug.Log("Steadiness for:");
                        Debug.Log(x);
                        Debug.Log(y);
                        valueList.Add((double)patient.FirstCaughtAlienStats[x,y,3]);
                    }
                    param1List.Add(patient.param1);
                    param2List.Add(patient.param2);
                    if (valueList.Count > int.Parse(lasthowmany.text))
                    {
                        valueList.RemoveAt(0);
                        param1List.RemoveAt(0);
                        param2List.RemoveAt(0);
                    }
                }
            }
            string tempText = "Seans ";
            if (int.Parse(lasthowmany.text) != 5)
                tempText = "";
            ShowGraph(valueList, param1List, param2List, (int _i) => tempText + (_i + 1), (float _f) => "" + _f);
        }
    }
    public void regionClicked()
    {
        if (regionName == EventSystem.current.currentSelectedGameObject.name && isRegionalGraph == true)
        {
            isRegionalGraph = false;
            deleteGraph();
            getSmoothness();
            getCompleteness();
            getDuration();
            getSteadiness();
        }
        else
        {
            regionName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(regionName);
            isRegionalGraph = true;
            deleteGraph();
            getSmoothness();
            getCompleteness();
            getDuration();
            getSteadiness();
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private GameObject CreateCircle(Vector2 anchoredPosition,double param1, double param2)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.tag = "point";
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(3.5f, 5f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        HoverOver sc = gameObject.AddComponent<HoverOver>();
        sc.HoverPanel = GameObject.Find("HoverPanel");
        sc.param1 = param1;
        sc.param2 = param2;
        sc.gamename = gamename.text;
        return gameObject;
    }
    public void deleteGraph()
    {
        foreach (Transform child in transform.GetChild(2))
        {
            if (child.gameObject.active)
                GameObject.Destroy(child.gameObject);
        }
        if (attributeShown.text == "Skor")
            getScores();
        else if (attributeShown.text == "Zaman")
            getTimes();
        hoverPanel.SetActive(true);
    }
    private void ShowGraph(List<double> valueList, List<double> param1List, List<double> param2List, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return _f.ToString(); };
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 1f;
        if (attributeShown.text == "Skor" || attributeShown.text == "Zaman")
            yMaximum = 200f;
        float xSize=20f;
        if(int.Parse(lasthowmany.text) == 5)
            xSize = 20f;
        if (int.Parse(lasthowmany.text) == 10)
           xSize = 9f;
        if (int.Parse(lasthowmany.text) == 15)
            xSize = 6f;
        if (int.Parse(lasthowmany.text) == 20)
            xSize = 4.5f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize / 4 + (i * xSize);
            float yPosition = ((float)valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), param1List[i], param2List[i]);
            double difficulty = (param1List[i] + param2List[i]) * 127.5;
            //circleGameObject.GetComponent<Image>().color = new Color32((byte)difficulty, (byte)(255 - difficulty), 0, 255);
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition - 45, -50f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);

            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer, false);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, -3f);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = (float)(i * 1f) / separatorCount;
            labelY.anchoredPosition = new Vector2(-50f, (normalizedValue * graphHeight) - 45f);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);

            //RectTransform dashY = Instantiate(dashTemplateY);
            //dashY.SetParent(graphContainer, false);
            //dashY.gameObject.SetActive(true);
            //dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
        }
        hoverPanel.SetActive(false);
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.transform.SetAsFirstSibling();
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 2f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    public void changeHeatmap()
    {
        if (gamename.text == "Uzay Macerası:Kurtarma Görevi")
        {
            
            heatmap1.SetActive(false);
            heatmap2.SetActive(true);
            heatmap3.SetActive(false);
            heatmap2.GetComponent<Heatmap2>().setupHeatmap();
        }

        if (gamename.text == "Uzay Macerası:Meteor Yolculuğu")
        {
            heatmap1.SetActive(true);
            heatmap2.SetActive(false);
            heatmap3.SetActive(false);
            heatmap1.GetComponent<Heatmap>().setupHeatmap();
        }

        if (gamename.text == "Uzay Macerası:Roket Karmaşası")
        {
            heatmap1.SetActive(false);
            heatmap2.SetActive(false);
            heatmap3.SetActive(true);
            heatmap3.GetComponent<Heatmap3>().setupHeatmap();
        }

    }

}