using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Heatmap2 : MonoBehaviour
{
    // Start is called before the first frame update
    public List<double[,]> heatmapArrays = new List<double[,]>();
    private DatabaseAccess databaseAccess;
    public Dropdown dateDropdown;
    private int lastHowManySessions;
    public Dropdown sessionNoDropdown;

    public Text pinkText;
    public Text greenText;
    public Text redText;
    public Text yellowText;
    public Text blueText;
    public Text purpleText;

    public static int sessionCount;

    public int startingPoint;
    public int endingPoint;
    public GameObject minmaxSlider;
    public Text gamename;
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
                heatmapArrays.Add(result[i].SecondHeatMap);
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
        if (gamename.text == "Uzay Macerası:Kurtarma Görevi")
        {
            int i = dateDropdown.value - 1;
            
            if (i == -1)
            {

                double kır_sum = 0;
                double sar_sum = 0;
                double mav_sum = 0;
                double yes_sum = 0;
                double mor_sum = 0;
                double pem_sum = 0;
                int kır_count = 0;
                int sar_count = 0;
                int mav_count = 0;
                int yes_count = 0;
                int mor_count = 0;
                int pem_count = 0;
                startingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().minValue - 1;
                endingPoint = (int)minmaxSlider.GetComponent<Min_Max_Slider2.MinMaxSlider2>().maxValue;
                for (int x = startingPoint ; x < endingPoint; x++)
                {
                    if (heatmapArrays[x][0,0] != -1)
                    {
                        pem_sum += heatmapArrays[x][0,0];
                        pem_count++;
                    }
                    if (heatmapArrays[x][0,1] != -1)
                    {
                        yes_sum += heatmapArrays[x][0,1];
                        yes_count++;
                    }
                    if (heatmapArrays[x][0,2] != -1)
                    {
                        kır_sum += heatmapArrays[x][0,2];
                        kır_count++;
                    }
                    if (heatmapArrays[x][1, 0] != -1)
                    {
                        sar_sum += heatmapArrays[x][1,0];
                        sar_count++;
                    }
                    if (heatmapArrays[x][1, 1] != -1)
                    {
                        mav_sum += heatmapArrays[x][1,1];
                        mav_count++;
                    }
                    if (heatmapArrays[x][1, 2] != -1)
                    {
                        mor_sum += heatmapArrays[x][1,2];
                        mor_count++;
                    }


                }
                pem_sum = pem_sum / pem_count;
                yes_sum = yes_sum / yes_count;
                mav_sum = mav_sum / mav_count;
                sar_sum = sar_sum / sar_count;
                kır_sum = kır_sum / kır_count;
                mor_sum = mor_sum / mor_count;

                pinkText.text = (pem_sum * 100).ToString();
                greenText.text = (yes_sum * 100).ToString();
                redText.text = (kır_sum * 100).ToString();
                yellowText.text = (sar_sum * 100).ToString();
                blueText.text = (mav_sum * 100).ToString();
                purpleText.text = (mor_sum * 100).ToString();

            }
            else
            {
                double[,] heatmapArray = heatmapArrays[i];
                pinkText.text = (heatmapArray[0, 0]*100).ToString();
                greenText.text = (heatmapArray[0, 1] * 100).ToString();
                redText.text = (heatmapArray[0, 2] * 100).ToString();
                yellowText.text = (heatmapArray[1, 0] * 100).ToString();
                blueText.text = (heatmapArray[1, 1] * 100).ToString();
                purpleText.text = (heatmapArray[1, 2] * 100).ToString();
                if (pinkText.text == "-100")
                    pinkText.text = "NA";
                if (greenText.text == "-100")
                    greenText.text = "NA";
                if (redText.text == "-100")
                    redText.text = "NA";
                if (yellowText.text == "-100")
                    yellowText.text = "NA";
                if (blueText.text == "-100")
                    blueText.text = "NA";
                if (purpleText.text == "-100")
                    purpleText.text = "NA";
            }
        }
    }
    
}
