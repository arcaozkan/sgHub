using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondPartStart : MonoBehaviour
{
    public GameObject Player;
    public GameObject pinkAlien;
    public GameObject greenAlien;
    public GameObject yellowAlien;
    public GameObject blueAlien;

    // Start is called before the first frame update
    void Start()
    {
        firstPartData.OnSecondPart = true;
        int rannum = UnityEngine.Random.Range(0, 4);
        secondPartData.correctPlanet = rannum;
        if (rannum == 0)
            Instantiate(pinkAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
        else if (rannum == 1)
            Instantiate(greenAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
        else if (rannum == 2)
            Instantiate(yellowAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
        else if (rannum == 3)
            Instantiate(blueAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
