using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private float mZCoord=5f;
    public GameObject ardacan;
    public GameObject ecesu;

    public GameObject ardacanpng;
    public GameObject ecesupng;
    // Start is called before the first frame update
    void Start()
    {
        if (secondPartData.character == "ecesu")
        {
            ardacan.SetActive(false);
            ecesu.SetActive(true);
        }
        else if (thirdPartData.character == "ecesu")
        {
            ardacan.SetActive(false);
            ecesu.SetActive(true);
        }

        if (secondPartData.character == "ardacan")
        {
            FindObjectOfType<AudioManager>().Play("Merhaba2");
        }
        else if (secondPartData.character == "ecesu")
        {
            FindObjectOfType<AudioManager>().Play("Merhaba2E");
            ardacanpng.SetActive(false);
            ecesupng.SetActive(true);
        }

        if (thirdPartData.character == "ardacan")
        {
            FindObjectOfType<AudioManager>().Play("Merhaba3");
        }
        else if (thirdPartData.character == "ecesu")
        {
            FindObjectOfType<AudioManager>().Play("Merhaba3E");
            ardacanpng.SetActive(false);
            ecesupng.SetActive(true);
        }

    }
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel")>0f) // forward
        {
            mZCoord += 0.2f;
        }
        if (Input.GetAxis("Mouse ScrollWheel")<0f) // backwards
        {
            mZCoord -= 0.2f;
        }

        //transform.position = GetMouseAsWorldPoint(); //UNCOMMENT THIS LINE FOR MOUSE MOVEMENT
    }

    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
}
