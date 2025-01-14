using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameDropdownHeatmapManager : MonoBehaviour
{
    public Text gamename;
    public GameObject heatmap1;
    public GameObject heatmap2;
    public GameObject heatmap3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void changeVal()
    {
        if (gamename.text == "Uzay Macerası:Meteor Yolculuğu") {
            heatmap1.SetActive(true);
            heatmap2.SetActive(false);
            heatmap3.SetActive(false);
        }
        if (gamename.text == "Uzay Macerası:Kurtarma Görevi")
        {
            heatmap1.SetActive(false);
            heatmap2.SetActive(true);
            heatmap3.SetActive(false);
        }

        if (gamename.text == "Uzay Macerası:Roket Karmaşası") {
            heatmap1.SetActive(false);
            heatmap2.SetActive(false);
            heatmap3.SetActive(true);
        }

    }
}
