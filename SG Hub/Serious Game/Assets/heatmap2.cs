using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heatmap2 : MonoBehaviour
{
    private int pinkCount=0;
    private int greenCount=0;
    private int blueCount=0;
    private int yellowCount=0;
    private int redCount = 0;
    private int purpleCount = 0;

    private int pinkCountA = 0;
    private int greenCountA = 0;
    private int blueCountA = 0;
    private int yellowCountA = 0;
    private int redCountA = 0;
    private int purpleCountA = 0;

    public Text pinkText;
    public Text greenText;
    public Text blueText;
    public Text yellowText;
    public Text redText;
    public Text purpleText;

    public static float[,] heatmapArray;

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    public void calculateHeatmap()
    {
        Debug.Log("heatmap2 started");
        for (int i = 0; i < secondPartData.secondCaughtAlienColors.Count; i++)
        {
            if (secondPartData.secondCaughtAlienColors[i].Substring(0, 7) == "Drag_pi")
            {
                pinkCount++;
            }
            else if (secondPartData.secondCaughtAlienColors[i].Substring(0, 6) == "Drag_g")
            {
                greenCount++;
            }
            else if (secondPartData.secondCaughtAlienColors[i].Substring(0, 6) == "Drag_y")
            {
                yellowCount++;
            }
            else if (secondPartData.secondCaughtAlienColors[i].Substring(0, 6) == "Drag_b")
            {
                blueCount++;
            }
            else if (secondPartData.secondCaughtAlienColors[i].Substring(0, 6) == "Drag_r")
            {
                redCount++;
            }
            else if (secondPartData.secondCaughtAlienColors[i].Substring(0, 7) == "Drag_pu")
            {
                purpleCount++;
            }
        }

        for (int i = 0; i < secondPartData.secondAllAlienColors.Count; i++)
        {
            if (secondPartData.secondAllAlienColors[i].Substring(0, 7) == "Drag_pi")
            {
                pinkCountA++;
            }
            else if (secondPartData.secondAllAlienColors[i].Substring(0, 6) == "Drag_g")
            {
                greenCountA++;
            }
            else if (secondPartData.secondAllAlienColors[i].Substring(0, 6) == "Drag_y")
            {
                yellowCountA++;
            }
            else if (secondPartData.secondAllAlienColors[i].Substring(0, 6) == "Drag_b")
            {
                blueCountA++;
            }
            else if (secondPartData.secondAllAlienColors[i].Substring(0, 6) == "Drag_r")
            {
                redCountA++;
            }
            else if (secondPartData.secondAllAlienColors[i].Substring(0, 7) == "Drag_pu")
            {
                purpleCountA++;
            }
        }

        pinkText.text = pinkCount.ToString() + "/" + pinkCountA.ToString();
        yellowText.text = yellowCount.ToString() + "/" + yellowCountA.ToString();
        greenText.text = greenCount.ToString() + "/" + greenCountA.ToString();
        blueText.text = blueCount.ToString() + "/" + blueCountA.ToString();
        redText.text = redCount.ToString() + "/" + redCountA.ToString();
        purpleText.text = purpleCount.ToString() + "/" + purpleCountA.ToString();

        heatmapArray = new float[2, 3];
        if (pinkCountA != 0)
            heatmapArray[0, 0] = (float)pinkCount / pinkCountA;
        else
            heatmapArray[0, 0] = -1;
        if (greenCountA != 0)
            heatmapArray[0, 1] = (float)greenCount / greenCountA;
        else
            heatmapArray[0, 1] = -1;
        if (redCountA != 0)
            heatmapArray[0, 2] = (float)redCount / redCountA;
        else
            heatmapArray[0, 2] = -1;
        if (yellowCountA != 0)
            heatmapArray[1, 0] = (float)yellowCount / yellowCountA;
        else
            heatmapArray[1, 0] = -1;
        if (blueCountA != 0)
            heatmapArray[1, 1] = (float)blueCount / blueCountA;
        else
            heatmapArray[1, 1] = -1;
        if (purpleCountA != 0)
            heatmapArray[1, 2] = (float)purpleCount / purpleCountA;
        else
            heatmapArray[1, 2] = -1;
    }
}
