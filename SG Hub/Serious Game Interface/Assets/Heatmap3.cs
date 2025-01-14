using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Heatmap3 : MonoBehaviour
{
    // Start is called before the first frame update
    public List<double[]> heatmapArrays = new List<double[]>();
    private DatabaseAccess databaseAccess;
    public Dropdown dateDropdown;
    private int lastHowManySessions;
    public Dropdown sessionNoDropdown;

    public Text toplamaText;
    public Text cikarmaText;
    public Text carpmaText;

    public Text gamename;

    public static int sessionCount;

    public int startingPoint;
    public int endingPoint;
    public GameObject minmaxSlider;
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        dateDropdown.options.Add(new Dropdown.OptionData() { text = "Özet" });
        setupHeatmap();
    }

    public async void setupHeatmap()
    {

        var task = databaseAccess.GetScoresFromDatabase();
        var result = await task;
        dateDropdown.ClearOptions();
        dateDropdown.options.Add(new Dropdown.OptionData() { text = "Özet" });
        for (int i = 0; i < result.Count; i++)
        {
            Debug.Log(result[i].gamePlayed);
            if (result[i].gamePlayed == gamename.text)
            {
                dateDropdown.options.Add(new Dropdown.OptionData(result[i].Date));
                heatmapArrays.Add(result[i].ThirdHeatMap);
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
        if (gamename.text == "Uzay Macerası:Roket Karmaşası")
        {
            int i = dateDropdown.value - 1;

            if (i == -1)
            {
                double toplama_sum = 0;
                double cikarma_sum = 0;
                double carpma_sum = 0;
                int top_count = 0;
                int cik_count = 0;
                int car_count = 0;
                startingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().minValue - 1;
                endingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().maxValue;
                for (int x = startingPoint+1; x <= endingPoint; x++)
                {
                    if (heatmapArrays[x][0] != -1)
                    {
                        toplama_sum += heatmapArrays[x][0];
                        top_count++;
                    }
                    if (heatmapArrays[x][1] != -1)
                    {
                        cikarma_sum += heatmapArrays[x][1];
                        cik_count++;
                    }
                    if (heatmapArrays[x][2] != -1)
                    {
                        carpma_sum += heatmapArrays[x][2];
                        car_count++;
                    }
                    
                    
                }
                toplama_sum = toplama_sum / top_count;
                cikarma_sum = cikarma_sum / cik_count;
                carpma_sum = carpma_sum / car_count ;
                toplamaText.text = (toplama_sum*100).ToString();
                cikarmaText.text = (cikarma_sum * 100).ToString();
                carpmaText.text = (carpma_sum * 100).ToString();

            }
            else
            {
                double[] heatmapArray = heatmapArrays[i];
                toplamaText.text = (heatmapArray[0]*100).ToString();
                cikarmaText.text = (heatmapArray[1] * 100).ToString();
                carpmaText.text = (heatmapArray[2] * 100).ToString();
                if (toplamaText.text == "-100")
                    toplamaText.text = "NA";
                if (cikarmaText.text == "-100")
                    cikarmaText.text = "NA";
                if (carpmaText.text == "-100")
                    carpmaText.text = "NA";

            }
        }
    }

}
