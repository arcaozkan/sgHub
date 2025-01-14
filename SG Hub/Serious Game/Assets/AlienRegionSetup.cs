using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienRegionSetup : MonoBehaviour
{
    private Color myColor ;
    private int xcoor;
    private int ycoor;


    void Start()
    {
        GetComponent<Image>().color = Color.white;
    }
    public void onClick()
    {
        if (GetComponent<Image>().color == Color.white)
        {
            GetComponent<Image>().color = Color.green;
            xcoor = gameObject.name[0] - '0' - 1;
            ycoor = gameObject.name[2] - '0' - 1;
            StartGame.AlienSpawnRates[xcoor, ycoor] = 0.08f;
        }

        else if (GetComponent<Image>().color == Color.green)
        {
            GetComponent<Image>().color = Color.red;
            xcoor = gameObject.name[0] - '0' - 1;
            ycoor = gameObject.name[2] - '0' - 1;
            StartGame.AlienSpawnRates[xcoor, ycoor] = 0.02f;
        }

        else if (GetComponent<Image>().color == Color.red)
        {
            GetComponent<Image>().color = Color.black;
            xcoor = gameObject.name[0] - '0'-1;
            ycoor = gameObject.name[2] - '0'-1;
            StartGame.AlienSpawnRates[xcoor, ycoor] = 0;


        }

        else if (GetComponent<Image>().color == Color.black)
        {
            GetComponent<Image>().color = Color.white;
            xcoor = gameObject.name[0] - '0' - 1;
            ycoor = gameObject.name[2] - '0' - 1;
            StartGame.AlienSpawnRates[xcoor, ycoor] = 0.04f;
        }
    }
}
