using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RocketNumDisplay : MonoBehaviour
{
    public int num;
    public Text solution;
    public Animator anim;
    public GameObject fire;
    public GameObject newRocket;
    public bool shouldLaunch = false;
    private bool launched = false;
    public GameObject[] allRockets;
    public Color color;

    public GameObject RocketBody;
    public Material Green;
    public Material Blue;
    public Material Orange;

    void Start()
    {
        //num=Random.Range(0, 12);
        fire.SetActive(false);
        shouldLaunch = false;
        launched = false;
        gameObject.tag = "Rocket";
        updateRockets();
    }

    // Update is called once per frame
    void Update()
    {
        solution.text = num.ToString();
        if (shouldLaunch==true)
            Launch();
    }

    public void Launch()
    {
        if (launched == false)
        {
            gameObject.tag = "Untagged";
            Vector3 tempLoc = gameObject.transform.position;
            anim.Play("Launch");
            fire.SetActive(true);
            solution.text = "";
            launched = true;
            StartCoroutine(makenew(tempLoc));


            Destroy(gameObject, 1.1f);
        }

    }

    private IEnumerator makenew(Vector3 tempLoc)
    {

            yield return new WaitForSeconds(1f);
            Instantiate(newRocket, tempLoc, Quaternion.Euler(0, 90, 0));


    }

    public void updateRockets()
    {
        allRockets = GameObject.FindGameObjectsWithTag("Rocket");
        GameObject alien = GameObject.FindGameObjectWithTag("Alien");
        int randIdx = Random.Range(0, allRockets.Length-1);
        int colorcounter = 0;
        for (int i = 0; i < allRockets.Length; i++)
        {
            
            allRockets[i].GetComponent<RocketNumDisplay>().num = Random.Range(2, 12);
            if (colorcounter == 0)
            {
                allRockets[i].GetComponent<RocketNumDisplay>().RocketBody.GetComponent<Renderer>().material = Orange;
            }
            if (colorcounter == 1)
                allRockets[i].GetComponent<RocketNumDisplay>().RocketBody.GetComponent<Renderer>().material = Blue;
            if (colorcounter == 2)
                allRockets[i].GetComponent<RocketNumDisplay>().RocketBody.GetComponent<Renderer>().material = Green;

            colorcounter++;
        }

        allRockets[randIdx].GetComponent<RocketNumDisplay>().num = alien.GetComponent<DragObject3>().result;
    }
}
