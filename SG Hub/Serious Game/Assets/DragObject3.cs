using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.IO;


public class DragObject3 : MonoBehaviour

{



    public bool friendMove;
    public string Color;
    private float mZCoord;

    public Slider mySlider;
    public GameObject SliderObject;
    public Text equation;
    public int result;

    public static bool locked = false;
    private GameObject objectCollided;
    public GameObject greenAlien;

    private bool startcounter = false;
    private float counter = thirdPartData.barParam*2;


    public Animator myAnim;
    public Animator rocketAnim;
    public GameObject[] allRockets;
    private GameObject friend;
    private GameObject friendChatBox;
    private Text friendChatText;
    private bool friendSaid = false;

    private GameObject greenPlanet;

    private string islem = "";

    public float motionTimer;
    public Vector3 tempalienstartpos;
    private Vector3[] pointsResult;
    private Vector3[] pointsResult2;

    private float completeness_temp = 0;
    private Vector3 firsthandstartpos;

    void Start()
    {
        motionTimer = 0;
        InvokeRepeating("DestroyAlien", (1 - thirdPartData.timeval) * 25 + 5f, 1.0f);
        InvokeRepeating("FadeOutAnim", (1 - thirdPartData.timeval) * 25 + 4f, 2.0f);
        friend = GameObject.Find("Friend2");
        friendChatBox = GameObject.Find("Friend2ChatBox");
        friendChatText = GameObject.Find("Friend2ChatText").GetComponent<Text>();
        int newCalcNo=0;
        if (thirdPartData.toplama == true && thirdPartData.cikarma == true && thirdPartData.carpma == false)
            newCalcNo = Random.Range(0, 12);
        else if (thirdPartData.toplama == false && thirdPartData.cikarma == true && thirdPartData.carpma == false)
            newCalcNo = Random.Range(7, 12);
        else if (thirdPartData.toplama == true && thirdPartData.cikarma == false && thirdPartData.carpma == false)
            newCalcNo = Random.Range(0, 6);
        else if (thirdPartData.toplama == true && thirdPartData.cikarma == true && thirdPartData.carpma == true)
            newCalcNo = Random.Range(0, 18);
        else if (thirdPartData.toplama == false && thirdPartData.cikarma == true && thirdPartData.carpma == true)
            newCalcNo = Random.Range(7, 18);
        else if (thirdPartData.toplama == true && thirdPartData.cikarma == false && thirdPartData.carpma == true)
        {
            newCalcNo = Random.Range(0, 2);
            if (newCalcNo == 0)
                newCalcNo = Random.Range(0, 6);
            else
                newCalcNo = Random.Range(13, 18);
        }
        else if (thirdPartData.toplama == false && thirdPartData.cikarma == false && thirdPartData.carpma == true)
            newCalcNo= Random.Range(13, 18);

        if (newCalcNo == 0)
            equation.text= "5+3";
        if (newCalcNo == 1)
            equation.text = "4+4";
        if (newCalcNo == 2)
            equation.text = "4+2";
        if (newCalcNo == 3)
            equation.text = "3+3";
        if (newCalcNo == 4)
            equation.text = "5+1";
        if (newCalcNo == 5)
            equation.text = "8+4";
        if (newCalcNo == 6)
            equation.text = "6+6";
        if (newCalcNo == 7)
            equation.text = "6-2";
        if (newCalcNo == 8)
            equation.text = "6-4";
        if (newCalcNo == 9)
            equation.text = "4-2";
        if (newCalcNo == 10)
            equation.text = "8-2";
        if (newCalcNo == 11)
            equation.text = "8-4";
        if (newCalcNo == 12)
            equation.text = "8-6";
        if (newCalcNo == 13)
            equation.text = "2x3";
        if (newCalcNo == 14)
            equation.text = "2x4";
        if (newCalcNo == 15)
            equation.text = "2x5";
        if (newCalcNo == 16)
            equation.text = "3x3";
        if (newCalcNo == 17)
            equation.text = "3x4";
        if (newCalcNo == 18)
            equation.text = "2x2";

        if (equation.text[1] == '+') {
            result= (equation.text[0]-'0') + (equation.text[2]-'0');
            islem = "toplama";
        }
        if (equation.text[1] == '-')
        {
            result = (equation.text[0] - '0') - (equation.text[2] - '0');
            islem = "cikarma";
        }
        if (equation.text[1] == 'x')
        {
            result = (equation.text[0] - '0') * (equation.text[2] - '0');
            islem = "carpma";
        }
        if (thirdPartData.previousMissed)
        {
            allRockets = GameObject.FindGameObjectsWithTag("Rocket");
            GameObject alien = GameObject.FindGameObjectWithTag("Alien");
            int randIdx = Random.Range(0, allRockets.Length - 1);
            for (int i = 0; i < allRockets.Length; i++)
            {
                allRockets[i].GetComponent<RocketNumDisplay>().num = Random.Range(2, 12);
            }

            allRockets[randIdx].GetComponent<RocketNumDisplay>().num = result;
            thirdPartData.previousMissed = false;
        }
        firsthandstartpos = GameObject.FindWithTag("Cursor").transform.position;
        tempalienstartpos = transform.position;


    }
    void FadeOutAnim()
    {
        myAnim.Play("FadeOut");
    }
    void DestroyAlien()
    {
            if (islem == "toplama")
                thirdPartData.totalToplama += 1;
            if (islem == "cikarma")
                thirdPartData.totalCikarma += 1;
            if (islem == "carpma")
                thirdPartData.totalCarpma += 1;
            thirdPartData.previousMissed = true;
            Instantiate(greenAlien, new Vector3(Random.Range(35f, 35f), Random.Range(0, 5f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));
            secondPartData.newAlienIndex++;

            locked = false;
            
            Debug.Log(gameObject.name.Substring(0, 6));
            Debug.Log(gameObject.transform.position.ToString());

            thirdPartData.completeness_ratios.Add(completeness_temp);
            completeness_temp = 0;


        Destroy(gameObject);
    }
    void Update()
    {
        motionTimer += Time.deltaTime;
        thirdPartData.timer += Time.deltaTime;
        if (startcounter)
        {
            counter -= Time.deltaTime;
            mySlider.value = (2f - counter) / 2;

        }
        if (counter <= 0)
        {
            counter = thirdPartData.barParam * 2;
            startcounter = false;
            locked = true;
            SliderObject.SetActive(false);
        }
        if (locked == true)
        {
            transform.position = objectCollided.transform.position + new Vector3(0f, -0.5f, 0f);


            if (friendSaid == false && thirdPartData.character=="ardacan")
            {

                FindObjectOfType<AudioManager>().Play(islem);
                if(islem=="cikarma")
                    Say("Çıkarma işlemi yapalım!");
                if (islem == "carpma")
                    Say("Çarpma işlemi yapalım!");
                if (islem == "toplama")
                    Say("Toplama işlemi yapalım!");
                friendSaid = true;
            }

            else if (friendSaid == false && thirdPartData.character == "ecesu")
            {

                FindObjectOfType<AudioManager>().Play(islem+"E");
                if (islem == "cikarma")
                    Say("Çıkarma işlemi yapalım!");
                if (islem == "carpma")
                    Say("Çarpma işlemi yapalım!");
                if (islem == "toplama")
                    Say("Toplama işlemi yapalım!");
                friendSaid = true;
            }

        }
    }



    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Rocket")
        {
            int rocketNo = collision.gameObject.GetComponent<RocketNumDisplay>().num;
            if ((result == rocketNo))
            {
                /*bunu sil
                foreach (var x in HandTracking.allJerks) //CASE 2:Alien floats away
                {
                    if (x < thirdPartData.smoothness_threshold)
                    {
                        thirdPartData.smooth_motions += 1;
                    }
                }
                float ratio = (float)thirdPartData.smooth_motions / HandTracking.allJerks.Count;
                thirdPartData.smoothness_ratios.Add(ratio);
                thirdPartData.completeness_ratios.Add(1);
                completeness_temp = 0;
                thirdPartData.duration_ratios.Add(1 - (motionTimer / ((1 - thirdPartData.timeval) * 12 + 4f)));

                pointsResult = new Vector3[HandTracking.allCoordinates.Count];
                pointsResult2 = new Vector3[HandTracking.allCoordinates.Count];

                generatePoints(transform.position, tempalienstartpos, pointsResult, HandTracking.allCoordinates.Count); // /2 mi yapsak
                                                                                                                        //generatePoints(firsthandstartpos, tempalienstartpos, pointsResult2, HandTracking.allCoordinates.Count);
                                                                                                                        //Line between the initial alien and planet(after catching)


                float distance = ((transform.position - tempalienstartpos) * 10).sqrMagnitude;
                float eratio = DifferenceBetweenLines(HandTracking.allCoordinates.ToArray(), pointsResult) / distance;
                float eratio2 = DifferenceBetweenLines(pointsResult, HandTracking.allCoordinates.ToArray()) / distance;
                if (eratio2 > eratio)
                    eratio = eratio2;
                if (eratio > 1)
                    eratio = 1;


                Debug.Log(1 - eratio);

                thirdPartData.steadiness_ratios.Add(1 - eratio);


                motionTimer = 0;
                thirdPartData.smooth_motions = 0;
                HandTracking.allJerks.Clear();
                HandTracking.allAccels.Clear();
                HandTracking.allVelocities.Clear();
                HandTracking.allCoordinates.Clear();
                Debug.Log("Smoothness ratio:");
                Debug.Log(ratio);

                bunu sil*/

                locked = false;

                if ((thirdPartData.endWithCount && thirdPartData.newAlienIndex < thirdPartData.alienGoalCount) || (thirdPartData.endWithCount == false && thirdPartData.timer / 60 <= thirdPartData.timeGoalMinutes))
                {
                    if (islem == "toplama")
                    {
                        thirdPartData.totalToplama += 1;
                        thirdPartData.dogruToplama += 1;
                    }
                    if (islem == "cikarma")
                    {
                        thirdPartData.totalCikarma += 1;
                        thirdPartData.dogruCikarma += 1;
                    }
                    if (islem == "carpma")
                    {
                        thirdPartData.totalCarpma += 1;
                        thirdPartData.dogruCarpma += 1;
                    }
                    Instantiate(greenAlien, new Vector3(Random.Range(35f, 35f), Random.Range(0, 5f), Random.Range(75f, 85f)), Quaternion.Euler(0, 90, 0));

                    firstPartData.OnThirdPart = true;
                    thirdPartData.newAlienIndex++;
                    Debug.Log(thirdPartData.newAlienIndex);

                }
                collision.gameObject.GetComponent<RocketNumDisplay>().shouldLaunch=true;
                //secondPartData.secondCaughtAlienPositions.Add(gameObject.transform.position.ToString());
                Destroy(gameObject);
            }
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