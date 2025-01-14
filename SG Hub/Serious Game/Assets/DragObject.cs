using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using UnityEditor;
using System.IO;

public class DragObject : MonoBehaviour

{

    

    public bool friendMove;
    public string Color;
    private float mZCoord;

    public Slider mySlider;
    public GameObject SliderObject;

    public static bool locked=false;
    private GameObject objectCollided;
    public GameObject greenAlien;
    public GameObject blueAlien;
    public GameObject pinkAlien;
    public GameObject yellowAlien;
    public GameObject redAlien;
    public GameObject purpleAlien;

    private bool startcounter = false;
    private float counter = secondPartData.barParam*2;


    public Animator myAnim;

    private GameObject friend;
    private GameObject friendChatBox;
    private Text friendChatText;
    private bool friendSaid = false;

    private GameObject greenPlanet;
    private GameObject bluePlanet;
    private GameObject yellowPlanet;
    private GameObject pinkPlanet;
    private GameObject purplePlanet;
    private GameObject redPlanet;

    public float motionTimer;
    public Vector3 tempalienstartpos;
    private Vector3[] pointsResult;
    private Vector3[] pointsResult2;

    private float completeness_temp=0;
    private Vector3 firsthandstartpos;

    void Start()
    {
        firsthandstartpos= GameObject.FindWithTag("Cursor").transform.position;
        motionTimer = 0;
        InvokeRepeating("DestroyAlien", (1 - secondPartData.timeval) * 25 + 5f,1.0f);
        InvokeRepeating("FadeOutAnim", (1- secondPartData.timeval)*25+4f, 2.0f);
        friend = GameObject.Find("Friend2");
        friendChatBox = GameObject.Find("Friend2ChatBox");
        friendChatText= GameObject.Find("Friend2ChatText").GetComponent<Text>();
        int newColorNo = Random.Range(0, 6);
        greenPlanet = GameObject.FindWithTag("Green");
        bluePlanet = GameObject.FindWithTag("Blue");
        yellowPlanet = GameObject.FindWithTag("Yellow");
        pinkPlanet = GameObject.FindWithTag("Pink");
        purplePlanet = GameObject.FindWithTag("Purple");
        redPlanet = GameObject.FindWithTag("Red");
        tempalienstartpos = transform.position;
    }
    void FadeOutAnim()
    {
        myAnim.Play("FadeOut");
    }
    void DestroyAlien()
    {

        //PlayerController.alienCount--;
        //PlayerController.alienColors.Count - PlayerController.alienCount;
        int newColorNo = Random.Range(0, 6);
        if ((secondPartData.endWithCount && secondPartData.newAlienIndex < secondPartData.alienGoalCount) || (secondPartData.endWithCount == false && secondPartData.timer / 60 <= secondPartData.timeGoalMinutes))
        {
            pinkPlanet.transform.position = new Vector3(0f, 0f, 0f);

            bluePlanet.transform.position = new Vector3(0f, 0f, 0f);

            yellowPlanet.transform.position = new Vector3(0f, 0f, 0f);

            greenPlanet.transform.position = new Vector3(0f, 0f, 0f);

            redPlanet.transform.position = new Vector3(0f, 0f, 0f);

            purplePlanet.transform.position = new Vector3(0f, 0f, 0f);

            secondPartData.correctPlanet = Random.Range(0, 4);
            Vector3 coors = new Vector3(0f, 0f, 0f);
            if (secondPartData.correctPlanet == 0)
                coors = new Vector3(39f, 3f, 75f);
            if (secondPartData.correctPlanet == 1)
                coors = new Vector3(39f, 3f, 87f);
            if (secondPartData.correctPlanet == 2)
                coors = new Vector3(39f, -2f, 75f);
            if (secondPartData.correctPlanet == 3)
                coors = new Vector3(39f, -2f, 87f);

            List<int> usedColors = new List<int>();

            usedColors.Add(newColorNo);
            int newWrongColorNo = Random.Range(0, 6);

            for (int i = 0; i < 3; i++)
            {
                while (usedColors.Contains(newWrongColorNo) == true) //Create 3 other random colors
                {
                    newWrongColorNo = Random.Range(0, 6);
                }
                usedColors.Add(newWrongColorNo);

            }
            int j = 1;
            for (int i = 0; i < 4; i++)
            {

                if (i == 0 && secondPartData.correctPlanet != 0)
                {
                    if (usedColors[j] == 0)
                    {
                        pinkPlanet.transform.position = new Vector3(39f, 3f, 75f);
                    }
                    if (usedColors[j] == 1)
                    {
                        bluePlanet.transform.position = new Vector3(39f, 3f, 75f);
                    }
                    if (usedColors[j] == 2)
                    {
                        yellowPlanet.transform.position = new Vector3(39f, 3f, 75f);
                    }
                    if (usedColors[j] == 3)
                    {
                        greenPlanet.transform.position = new Vector3(39f, 3f, 75f);
                    }
                    if (usedColors[j] == 4)
                    {
                        redPlanet.transform.position = new Vector3(39f, 3f, 75f);
                    }
                    if (usedColors[j] == 5)
                    {
                        purplePlanet.transform.position = new Vector3(39f, 3f, 75f);
                    }
                    j++;
                }

                else if (i == 1 && secondPartData.correctPlanet != 1)
                {
                    if (usedColors[j] == 0)
                    {
                        pinkPlanet.transform.position = new Vector3(39f, 3f, 87f);
                    }
                    if (usedColors[j] == 1)
                    {
                        bluePlanet.transform.position = new Vector3(39f, 3f, 87f);
                    }
                    if (usedColors[j] == 2)
                    {
                        yellowPlanet.transform.position = new Vector3(39f, 3f, 87f);
                    }
                    if (usedColors[j] == 3)
                    {
                        greenPlanet.transform.position = new Vector3(39f, 3f, 87f);
                    }
                    if (usedColors[j] == 4)
                    {
                        redPlanet.transform.position = new Vector3(39f, 3f, 87f);
                    }
                    if (usedColors[j] == 5)
                    {
                        purplePlanet.transform.position = new Vector3(39f, 3f, 87f);
                    }
                    j++;
                }

                else if (i == 2 && secondPartData.correctPlanet != 2)
                {
                    if (usedColors[j] == 0)
                    {
                        pinkPlanet.transform.position = new Vector3(39f, -2f, 75f);
                    }
                    if (usedColors[j] == 1)
                    {
                        bluePlanet.transform.position = new Vector3(39f, -2f, 75f);
                    }
                    if (usedColors[j] == 2)
                    {
                        yellowPlanet.transform.position = new Vector3(39f, -2f, 75f);
                    }
                    if (usedColors[j] == 3)
                    {
                        greenPlanet.transform.position = new Vector3(39f, -2f, 75f);
                    }
                    if (usedColors[j] == 4)
                    {
                        redPlanet.transform.position = new Vector3(39f, -2f, 75f);
                    }
                    if (usedColors[j] == 5)
                    {
                        purplePlanet.transform.position = new Vector3(39f, -2f, 75f);
                    }
                    j++;
                }
                else if (i == 3 && secondPartData.correctPlanet != 3)
                {
                    if (usedColors[j] == 0)
                    {
                        pinkPlanet.transform.position = new Vector3(39f, -2f, 87f);
                    }
                    if (usedColors[j] == 1)
                    {
                        bluePlanet.transform.position = new Vector3(39f, -2f, 87f);
                    }
                    if (usedColors[j] == 2)
                    {
                        yellowPlanet.transform.position = new Vector3(39f, -2f, 87f);
                    }
                    if (usedColors[j] == 3)
                    {
                        greenPlanet.transform.position = new Vector3(39f, -2f, 87f);
                    }
                    if (usedColors[j] == 4)
                    {
                        redPlanet.transform.position = new Vector3(39f, -2f, 87f);
                    }
                    if (usedColors[j] == 5)
                    {
                        purplePlanet.transform.position = new Vector3(39f, -2f, 87f);
                    }
                    j++;
                }
            }





            if (newColorNo == 0)
            { //Pink
                Instantiate(pinkAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                pinkPlanet.transform.position = coors;
            }
            else if (newColorNo == 1)
            { //Blue
                Instantiate(blueAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                bluePlanet.transform.position = coors;
            }
            else if (newColorNo == 2)
            { //Yellow
                Instantiate(yellowAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                yellowPlanet.transform.position = coors;
            }
            else if (newColorNo == 3)
            { //Green
                Instantiate(greenAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                greenPlanet.transform.position = coors;
            }
            else if (newColorNo == 4)
            { //Red
                Instantiate(redAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                redPlanet.transform.position = coors;
            }
            else if (newColorNo == 5)
            { //Purple
                Instantiate(purpleAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                purplePlanet.transform.position = coors;
            }

            //secondPartData.newAlienIndex++;

        }
        locked = false;
        secondPartData.secondAllAlienColors.Add(gameObject.name);
        Debug.Log(gameObject.name.Substring(0, 6));
        Debug.Log(gameObject.transform.position.ToString());
        secondPartData.completeness_ratios.Add(completeness_temp);
        completeness_temp = 0;
        Destroy(gameObject);
    }
    void Update()
    {
        motionTimer += Time.deltaTime;
        secondPartData.timer += Time.deltaTime;
        if (startcounter)
        {
            counter -= Time.deltaTime;
            mySlider.value = (2f - counter) / 2;
            
        }
        if (counter <= 0)
        {
            counter = secondPartData.barParam*2;
            startcounter = false;
            locked = true;
            SliderObject.SetActive(false);
            completeness_temp = 0.5f;
        }
        if (locked == true)
        {
            transform.position = objectCollided.transform.position + new Vector3(0f, -0.5f, 0f);

            Vector3 friendTarget = new Vector3(40, -2, 84);
            if (secondPartData.correctPlanet == 3)
                friendTarget = new Vector3(40, -2, 84);
            else if (secondPartData.correctPlanet == 2)
                friendTarget = new Vector3(40, -2, 78);
            else if (secondPartData.correctPlanet == 1)
                friendTarget = new Vector3(40, 3, 84);
            else if (secondPartData.correctPlanet==0)
                friendTarget = new Vector3(40, 3, 78);
            

            if(secondPartData.friendMove)
                friend.transform.position = Vector3.MoveTowards(friend.transform.position, friendTarget, Time.deltaTime*2);
            if (friendSaid == false)
            {
                if (Color == "Blue")
                {
                    Say("Mavi gezegene getir!");
                    
                }
                else if (Color == "Pink")
                {
                    Say("Pembe gezegene getir!");
                }
                else if (Color == "Green")
                {
                    Say("Yeşil gezegene getir!");
                }
                else if (Color == "Yellow")
                {
                    Say("Sarı gezegene getir!");
                }
                else if (Color == "Red")
                {
                    Say("Kırmızı gezegene getir!");
                }
                else if (Color == "Purple")
                {
                    Say("Mor gezegene getir!");
                }
                if (secondPartData.character == "ardacan")
                    FindObjectOfType<AudioManager>().Play("Planet" + Color);
                if(secondPartData.character=="ecesu")
                    FindObjectOfType<AudioManager>().Play("Planet" + Color+"E");
                friendSaid = true;
            }

        }
    }


    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == Color)
        {
            //Play sound effect
            //aliencount++

            //PlayerController.alienCount--;

            /*bunu sil
            foreach (var x in HandTracking.allJerks) //CASE 2:Alien floats away
            {
                if (x < secondPartData.smoothness_threshold)
                {
                    secondPartData.smooth_motions += 1;
                }
            }
            float ratio = (float)secondPartData.smooth_motions / HandTracking.allJerks.Count;
            secondPartData.smoothness_ratios.Add(ratio);
            secondPartData.completeness_ratios.Add(1);
            completeness_temp = 0;
            secondPartData.duration_ratios.Add( 1-(motionTimer  /((1 - secondPartData.timeval) * 12 + 4f)));

            pointsResult = new Vector3[HandTracking.allCoordinates.Count];
            pointsResult2 = new Vector3[HandTracking.allCoordinates.Count];
            //Debug.Log("Spaceship and tempalienstartpos");
            //Debug.Log(spaceship.transform.position);
            //Debug.Log(tempalienstartpos);
            generatePoints(transform.position, tempalienstartpos, pointsResult, HandTracking.allCoordinates.Count); // /2 mi yapsak
            //generatePoints(firsthandstartpos, tempalienstartpos, pointsResult2, HandTracking.allCoordinates.Count);
            //Line between the initial alien and planet(after catching)

            //for(int i=0;i< HandTracking.allCoordinates.Count; i++)
            //{
            //Debug.Log(HandTracking.allCoordinates[i]);
            //}
            //Debug.Log("distance:");
            float distance = ((transform.position - tempalienstartpos) * 10).sqrMagnitude;
            //Debug.Log(distance);
            float eratio = DifferenceBetweenLines(HandTracking.allCoordinates.ToArray(), pointsResult) / distance;
            float eratio2 = DifferenceBetweenLines(pointsResult, HandTracking.allCoordinates.ToArray()) / distance;
            if (eratio2 > eratio)
                eratio = eratio2;
            if (eratio > 1)
                eratio = 1;

            /*float eratio3 = DifferenceBetweenLines(HandTracking.allCoordinates.ToArray(), pointsResult2) / distance;
            float eratio4 = DifferenceBetweenLines(pointsResult2, HandTracking.allCoordinates.ToArray()) / distance;
            if (eratio4 > eratio3)
                eratio3 = eratio4;
            if (eratio3 > 1)
                eratio3 = 1;
            Debug.Log("Steadiness ratio:");
            Debug.Log(1 - (eratio3+eratio)/2); 
            
            secondPartData.steadiness_ratios.Add(1 - (eratio3 + eratio) / 2);
            *//*bunun birini sil
            Debug.Log(1 - eratio);

            secondPartData.steadiness_ratios.Add(1 - eratio);



            motionTimer = 0;
            secondPartData.smooth_motions = 0;
            HandTracking.allJerks.Clear();
            HandTracking.allAccels.Clear();
            HandTracking.allVelocities.Clear();
            HandTracking.allCoordinates.Clear();
            Debug.Log("Smoothness ratio:");
            Debug.Log(ratio);

            locked = false;
            bunu sil*/
            int newColorNo = Random.Range(0, 6);
            if ((secondPartData.endWithCount && secondPartData.newAlienIndex < secondPartData.alienGoalCount) || (secondPartData.endWithCount == false && secondPartData.timer / 60 <= secondPartData.timeGoalMinutes))
            {
                /*if (PlayerController.alienColors[newAlienIndex] == "Alien_pink(Clone)")
                    Instantiate(pinkAlien, new Vector3(Random.Range(41f, 43f), Random.Range(-3, 3f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));
                else if (PlayerController.alienColors[newAlienIndex] == "Alien_blue(Clone)")
                    Instantiate(blueAlien, new Vector3(Random.Range(41f, 43f), Random.Range(-3, 3f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));
                else if (PlayerController.alienColors[newAlienIndex] == "Alien_yellow(Clone)")
                    Instantiate(yellowAlien, new Vector3(Random.Range(41f, 43f), Random.Range(-3, 3f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));
                else if (PlayerController.alienColors[newAlienIndex] == "Alien_green(Clone)")
                    Instantiate(greenAlien, new Vector3(Random.Range(41f, 43f), Random.Range(-3, 3f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));
                else if (PlayerController.alienColors[newAlienIndex] == "Alien_red(Clone)")
                    Instantiate(redAlien, new Vector3(Random.Range(41f, 43f), Random.Range(-3, 3f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));
                else if (PlayerController.alienColors[newAlienIndex] == "Alien_purple(Clone)")
                    Instantiate(purpleAlien, new Vector3(Random.Range(41f, 43f), Random.Range(-3, 3f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));*/

                //4 GEZEGEN RENGİNİ BURDA KARALIM

                pinkPlanet.transform.position = new Vector3(0f, 0f, 0f);

                bluePlanet.transform.position = new Vector3(0f, 0f, 0f);

                yellowPlanet.transform.position = new Vector3(0f, 0f, 0f);

                greenPlanet.transform.position = new Vector3(0f, 0f, 0f);

                redPlanet.transform.position = new Vector3(0f, 0f, 0f);

                purplePlanet.transform.position = new Vector3(0f, 0f, 0f);

                secondPartData.correctPlanet = Random.Range(0, 4);
                Vector3 coors = new Vector3(0f, 0f, 0f);
                if (secondPartData.correctPlanet == 0)
                    coors = new Vector3(39f, 3f, 75f);
                if (secondPartData.correctPlanet == 1)
                    coors = new Vector3(39f, 3f, 87f);
                if (secondPartData.correctPlanet == 2)
                    coors = new Vector3(39f, -2f, 75f);
                if (secondPartData.correctPlanet == 3)
                    coors = new Vector3(39f, -2f, 87f);

                List<int> usedColors = new List<int>();

                usedColors.Add(newColorNo);
                int newWrongColorNo = Random.Range(0, 6);

                for (int i=0; i < 3; i++)
                {
                    while (usedColors.Contains(newWrongColorNo) == true) //Create 3 other random colors
                    {
                        newWrongColorNo = Random.Range(0, 6);
                    }
                    usedColors.Add(newWrongColorNo);

                }
                int j = 1;
                for (int i=0; i < 4; i++)
                {

                        if (i == 0 && secondPartData.correctPlanet != 0)
                        {
                            if (usedColors[j] == 0)
                            {
                                pinkPlanet.transform.position = new Vector3(39f, 3f, 75f);
                            }
                            if (usedColors[j] == 1)
                            {
                                bluePlanet.transform.position = new Vector3(39f, 3f, 75f);
                            }
                            if (usedColors[j] == 2)
                            {
                                yellowPlanet.transform.position = new Vector3(39f, 3f, 75f);
                            }
                            if (usedColors[j] == 3)
                            {
                                greenPlanet.transform.position = new Vector3(39f, 3f, 75f);
                            }
                            if (usedColors[j] == 4)
                            {
                                redPlanet.transform.position = new Vector3(39f, 3f, 75f);
                            }
                            if (usedColors[j] == 5)
                            {
                                purplePlanet.transform.position = new Vector3(39f, 3f, 75f);
                            }
                            j++;
                        }

                        else if (i == 1 && secondPartData.correctPlanet != 1)
                        {
                            if (usedColors[j] == 0)
                            {
                                pinkPlanet.transform.position = new Vector3(39f, 3f, 87f);
                            }
                            if (usedColors[j] == 1)
                            {
                                bluePlanet.transform.position = new Vector3(39f, 3f, 87f);
                            }
                            if (usedColors[j] == 2)
                            {
                                yellowPlanet.transform.position = new Vector3(39f, 3f, 87f);
                            }
                            if (usedColors[j] == 3)
                            {
                                greenPlanet.transform.position = new Vector3(39f, 3f, 87f);
                            }
                            if (usedColors[j] == 4)
                            {
                                redPlanet.transform.position = new Vector3(39f, 3f, 87f);
                            }
                            if (usedColors[j] == 5)
                            {
                                purplePlanet.transform.position = new Vector3(39f, 3f, 87f);
                            }
                            j++;
                        }

                        else if (i == 2 && secondPartData.correctPlanet != 2)
                        {
                            if (usedColors[j] == 0)
                            {
                                pinkPlanet.transform.position = new Vector3(39f, -2f, 75f);
                            }
                            if (usedColors[j] == 1)
                            {
                                bluePlanet.transform.position = new Vector3(39f, -2f, 75f);
                            }
                            if (usedColors[j] == 2)
                            {
                                yellowPlanet.transform.position = new Vector3(39f, -2f, 75f);
                            }
                            if (usedColors[j] == 3)
                            {
                                greenPlanet.transform.position = new Vector3(39f, -2f, 75f);
                            }
                            if (usedColors[j] == 4)
                            {
                                redPlanet.transform.position = new Vector3(39f, -2f, 75f);
                            }
                            if (usedColors[j] == 5)
                            {
                                purplePlanet.transform.position = new Vector3(39f, -2f, 75f);
                            }
                            j++;
                        }
                        else if (i == 3 && secondPartData.correctPlanet != 3)
                        {
                            if (usedColors[j] == 0)
                            {
                                pinkPlanet.transform.position = new Vector3(39f, -2f, 87f);
                            }
                            if (usedColors[j] == 1)
                            {
                                bluePlanet.transform.position = new Vector3(39f, -2f, 87f);
                            }
                            if (usedColors[j] == 2)
                            {
                                yellowPlanet.transform.position = new Vector3(39f, -2f, 87f);
                            }
                            if (usedColors[j] == 3)
                            {
                                greenPlanet.transform.position = new Vector3(39f, -2f, 87f);
                            }
                            if (usedColors[j] == 4)
                            {
                                redPlanet.transform.position = new Vector3(39f, -2f, 87f);
                            }
                            if (usedColors[j] == 5)
                            {
                                purplePlanet.transform.position = new Vector3(39f, -2f, 87f);
                            }
                            j++;
                        }
                }

                



                if (newColorNo == 0)
                { //Pink
                    Instantiate(pinkAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                    pinkPlanet.transform.position = coors;
                }
                else if (newColorNo == 1)
                { //Blue
                    Instantiate(blueAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                    bluePlanet.transform.position = coors;
                }
                else if (newColorNo == 2)
                { //Yellow
                    Instantiate(yellowAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                    yellowPlanet.transform.position = coors;
                }
                else if (newColorNo == 3)
                { //Green
                    Instantiate(greenAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                    greenPlanet.transform.position = coors;
                }
                else if (newColorNo == 4)
                { //Red
                    Instantiate(redAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                    redPlanet.transform.position = coors;
                }
                else if (newColorNo == 5)
                { //Purple
                    Instantiate(purpleAlien, new Vector3(39f, 0f, 81f), Quaternion.Euler(0, 90, 0));
                    purplePlanet.transform.position = coors;
                }

                    secondPartData.newAlienIndex++;
                    Debug.Log(secondPartData.newAlienIndex);

                }
            secondPartData.secondAllAlienColors.Add(gameObject.name);
            secondPartData.secondCaughtAlienColors.Add(gameObject.name);
            secondPartData.secondCaughtAlienPositions.Add(gameObject.transform.position.ToString());
            Destroy(gameObject);
                }

        if (collision.gameObject.tag == "Cursor")
        {

                startcounter = true;
                SliderObject.SetActive(true);
                objectCollided = collision.gameObject;

        }

    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Cursor")
        {
            counter = 2f;
            startcounter = false;
            SliderObject.SetActive(false);
            mySlider.value = 0f;
        }
    }

    void Say(string text)
    {

        friendChatBox.GetComponent<Image>().enabled = true;
        friendChatText.text = text;
        friendChatBox.GetComponent<Animator>().Rebind();
        friendChatBox.GetComponent<Animator>().Update(0f);
        friendChatText.GetComponent<Animator>().Rebind();
        friendChatText.GetComponent<Animator>().Update(0f);
    }


    float DifferenceBetweenLines(Vector3[] drawn, Vector3[] toMatch)
    {
        float sqrDistAcc = 0f;
        float length = 0f;

        Vector3 prevPoint = toMatch[0];

        foreach (var toMatchPoint in WalkAlongLine(toMatch))
        {
            sqrDistAcc += SqrDistanceToLine(drawn, toMatchPoint);
            length += Vector3.Distance(toMatchPoint, prevPoint);

            prevPoint = toMatchPoint;
        }

        return sqrDistAcc / length;
    }

    /// <summary>
    /// Move a point from the beginning of the line to its end using a maximum step, yielding the point at each step.
    /// </summary>
    IEnumerable<Vector3> WalkAlongLine(IEnumerable<Vector3> line, float maxStep = .01f)
    {
        using (var lineEnum = line.GetEnumerator())
        {
            if (!lineEnum.MoveNext())
                yield break;

            var pos = lineEnum.Current;

            while (lineEnum.MoveNext())
            {
                //Debug.Log(lineEnum.Current);
                var target = lineEnum.Current;
                while (pos != target)
                {
                    yield return pos = Vector3.MoveTowards(pos, target, maxStep);
                }
            }
        }
    }

    static float SqrDistanceToLine(Vector3[] line, Vector3 point)
    {
        return ListSegments(line)
            .Select(seg => SqrDistanceToSegment(seg.a, seg.b, point))
            .Min();
    }

    static float SqrDistanceToSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        var projected = ProjectPointOnLineSegment(linePoint1, linePoint1, point);
        return (projected - point).sqrMagnitude;
    }

    /// <summary>
    /// Outputs each position of the line (but the last) and the consecutive one wrapped in a Segment.
    /// Example: a, b, c, d --> (a, b), (b, c), (c, d)
    /// </summary>
    static IEnumerable<Segment> ListSegments(IEnumerable<Vector3> line)
    {
        using (var pt1 = line.GetEnumerator())
        using (var pt2 = line.GetEnumerator())
        {
            pt2.MoveNext();

            while (pt2.MoveNext())
            {
                pt1.MoveNext();

                yield return new Segment { a = pt1.Current, b = pt2.Current };
            }
        }
    }
    struct Segment
    {
        public Vector3 a;
        public Vector3 b;
    }

    //This function finds out on which side of a line segment the point is located.
    //The point is assumed to be on a line created by linePoint1 and linePoint2. If the point is not on
    //the line segment, project it on the line using ProjectPointOnLine() first.
    //Returns 0 if point is on the line segment.
    //Returns 1 if point is outside of the line segment and located on the side of linePoint1.
    //Returns 2 if point is outside of the line segment and located on the side of linePoint2.
    static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        Vector3 lineVec = linePoint2 - linePoint1;
        Vector3 pointVec = point - linePoint1;

        if (Vector3.Dot(pointVec, lineVec) > 0)
        {
            return pointVec.magnitude <= lineVec.magnitude ? 0 : 2;
        }
        else
        {
            return 1;
        }
    }

    //This function returns a point which is a projection from a point to a line.
    //The line is regarded infinite. If the line is finite, use ProjectPointOnLineSegment() instead.
    static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
    {
        //get vector from point on line to point in space
        Vector3 linePointToPoint = point - linePoint;
        float t = Vector3.Dot(linePointToPoint, lineVec);
        return linePoint + lineVec * t;
    }

    //This function returns a point which is a projection from a point to a line segment.
    //If the projected point lies outside of the line segment, the projected point will
    //be clamped to the appropriate line edge.
    //If the line is infinite instead of a segment, use ProjectPointOnLine() instead.
    static Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        Vector3 vector = linePoint2 - linePoint1;
        Vector3 projectedPoint = ProjectPointOnLine(linePoint1, vector.normalized, point);

        switch (PointOnWhichSideOfLineSegment(linePoint1, linePoint2, projectedPoint))
        {
            case 0:
                return projectedPoint;
            case 1:
                return linePoint1;
            case 2:
                return linePoint2;
            default:
                //output is invalid
                return Vector3.zero;
        }
    }


    void generatePoints(Vector3 from, Vector3 to, Vector3[] result, int chunkAmount)
    {
        //divider must be between 0 and 1
        float divider = 1f / chunkAmount;
        float linear = 0f;

        if (chunkAmount == 0)
        {
            Debug.LogError("chunkAmount Distance must be > 0 instead of " + chunkAmount);
            return;
        }

        if (chunkAmount == 1)
        {
            result[0] = Vector3.Lerp(from, to, 0.5f); //Return half/middle point
            return;
        }

        for (int i = 0; i < chunkAmount; i++)
        {
            if (i == 0)
            {
                linear = divider / 2;
            }
            else
            {
                linear += divider; //Add the divider to it to get the next distance
            }
            // Debug.Log("Loop " + i + ", is " + linear);
            result[i] = Vector3.Lerp(from, to, linear);
        }
    }



}