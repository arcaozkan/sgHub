using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;
using System.IO;
public class PlayerController : MonoBehaviour
{
    //public GameObject asteroidSpawner;
    public float movementSpeed = 5.0f;
    public AudioSource alienCollect;


    public GameObject mainCamera;

    public GameObject greenAlien;
    public GameObject blueAlien;
    public GameObject pinkAlien;
    public GameObject yellowAlien;

    public GameObject secondPart;
    //UI
    public GameObject gameOverScreen1;
    public Text rapor;
    public Text rapor2;

    public Animator friendAnim;

    public GameObject ardacan;
    public GameObject ardacanCarry;
    public GameObject ecesu;
    public GameObject ecesuCarry;

    public GameObject ardacanpng;
    public GameObject ecesupng;
    public bool gotalien = false;
    public double alienTimer=10.0;

    public GameObject spaceship;
    public Vector3 tempalienstartpos;
    public float motionTimer=-2.5f;
    private Vector3[] pointsResult;

    private bool spaceshipentered=false;
    private double spaceshipcounter=0.1;
    void Start()
    {

        if (firstPartData.character == "ecesu")
        {
            ardacan.SetActive(false);
            ecesu.SetActive(true);
        }

        if (firstPartData.character == "ardacan")
        {
            FindObjectOfType<AudioManager>().Play("Merhaba1");
        }
        else if (firstPartData.character == "ecesu")
        {
            FindObjectOfType<AudioManager>().Play("Merhaba1E");
            ardacanpng.SetActive(false);
            ecesupng.SetActive(true);
        }

    }
    IEnumerator ExecuteAfterTime(float time,GameObject alien, GameObject carry)
    {
        yield return new WaitForSeconds(time);

        alien.transform.parent = carry.transform;
        if(firstPartData.character == "ardacan")
            alien.transform.localPosition = new Vector3(0.05f, 0.3f, 0.2f);
        else
            alien.transform.localPosition = new Vector3(0.01f, 0.8f, 0.51f);
    }
    // Update is called once per frame
    void Update()
    {

        if (spaceshipentered)
        {
            spaceshipcounter -= Time.deltaTime;

        }
        if (spaceshipcounter <= 0)
        {
            spaceshipcounter = 0.1;
            spaceshipentered = false;
            alienTimer = 10.0f; //bunu parametre?
            float dratio = 1 - (motionTimer / 13.0f);
            firstPartData.duration_ratios.Add(dratio); //13 sabit bi sayı, bu değişebilir mi olmalı?
            Debug.Log("Duration ratio:");
            Debug.Log(dratio);
            motionTimer = 0;
            Debug.Log("COORDINATES:");
            for (int i = 10; i < 20; i++)
            {
                Debug.Log(HandTracking.allCoordinates[i]);

            }
            Debug.Log("VELOCITIES:");
            for (int i = 10; i < 20;i++)
            {
                Debug.Log(HandTracking.allVelocities[i]);

            }
            Debug.Log("ACCELS:");
            for (int i = 10; i < 20; i++)
            {
                Debug.Log(HandTracking.allAccels[i]);

            }
            Debug.Log("JERKS:");
            for (int i = 10; i < 20; i++)
            {
                Debug.Log(HandTracking.allJerks[i]);

            }
            foreach (var x in HandTracking.allJerks) //CASE 1:Alien is caught
            {
                
                if (x < firstPartData.smoothness_threshold)
                {
                    firstPartData.smooth_motions += 1;
                }
                else
                {
                    //firstPartData.smooth_motions -= 5;
                }
            }
            float sratio = (float)firstPartData.smooth_motions / HandTracking.allJerks.Count;
            firstPartData.smoothness_ratios.Add(sratio);
            firstPartData.completeness_ratios.Add(1.0f);
            Debug.Log("Smoothness ratio:");
            Debug.Log(sratio);
            //Debug.Log("Completeness ratio:");
            //Debug.Log(1.0f);

            pointsResult = new Vector3[HandTracking.allCoordinates.Count];
            //Debug.Log("Spaceship and tempalienstartpos");
            //Debug.Log(spaceship.transform.position);
            //Debug.Log(tempalienstartpos);
            generatePoints(spaceship.transform.position, tempalienstartpos, pointsResult, HandTracking.allCoordinates.Count);
            //for(int i=0;i< HandTracking.allCoordinates.Count; i++)
            //{
            //Debug.Log(HandTracking.allCoordinates[i]);
            //}
            //Debug.Log("distance:");
            float distance = ((spaceship.transform.position - tempalienstartpos) * 12).sqrMagnitude;
            //Debug.Log(distance);
            float eratio = DifferenceBetweenLines(HandTracking.allCoordinates.ToArray(), pointsResult) / distance;
            float eratio2 = DifferenceBetweenLines(pointsResult, HandTracking.allCoordinates.ToArray()) / distance;
            //Debug.Log("eratios:");
            //Debug.Log(eratio);
            //Debug.Log(eratio2);
            if (eratio2 > eratio)
                eratio = eratio2;
            if (eratio > 1)
                eratio = 1;
            //Debug.Log("Steadiness ratio:");
            //Debug.Log(1 - eratio); //400 rastgele bir sayı burayı napalım?
            firstPartData.steadiness_ratios.Add(1 - eratio);
            //Reset for the next motion
            firstPartData.smooth_motions = 0;
            HandTracking.allJerks.Clear();
            HandTracking.allAccels.Clear();
            HandTracking.allVelocities.Clear();
            HandTracking.allCoordinates.Clear();

            List<float> tempStats = new List<float>();
            tempStats.Add(sratio);
            tempStats.Add(1.0f);
            tempStats.Add(dratio);
            tempStats.Add(1 - eratio); 
            firstPartData.firstCaughtAlienStats.Add(tempStats);
            gotalien = false;
            alienCollect.Play(0);
            int randVoiceline = UnityEngine.Random.Range(1, 7);
            if (firstPartData.character == "ardacan" && firstPartData.alienCount != 9)
            {
                ardacan.SetActive(true);
                ardacanCarry.SetActive(false);
                if (firstPartData.alienCount == firstPartData.alienGoalCount - 4)
                    FindObjectOfType<AudioManager>().Play("Az Kaldı");
                else
                    FindObjectOfType<AudioManager>().Play("Encourage" + randVoiceline.ToString());
            }
            else if (firstPartData.character == "ecesu" && firstPartData.alienCount != 9)
            {
                ecesu.SetActive(true);
                ecesuCarry.SetActive(false);
                if (firstPartData.alienCount == firstPartData.alienGoalCount - 4)
                    FindObjectOfType<AudioManager>().Play("Az KaldıE");
                else
                    FindObjectOfType<AudioManager>().Play("Encourage" + randVoiceline.ToString() + "E");
            }
            firstPartData.alienCount++;
            firstPartData.alienColors.Add(gameObject.name);

            if ((firstPartData.endWithCount && firstPartData.alienCount >= firstPartData.alienGoalCount) || firstPartData.endWithCount == false && firstPartData.timer / 60 > firstPartData.timeGoalMinutes)
            {
                if (firstPartData.character == "ardacan")
                {
                    //FindObjectOfType<AudioManager>().Play("Win2");
                }
                else if (firstPartData.character == "ecesu")
                {
                    FindObjectOfType<AudioManager>().Play("Win2E");
                }
                //UI GAME OVER
                gameOverScreen1.SetActive(true);


                int Score = (int)(25 * firstPartData.steadiness_ratios.Average() + 25 * firstPartData.duration_ratios.Average() + 25 * firstPartData.completeness_ratios.Average() + 25 * firstPartData.smoothness_ratios.Average()); ;
                if (Score<0)
                    Score = (int)(33 * firstPartData.steadiness_ratios.Average() + 33 * firstPartData.duration_ratios.Average() + 33 * firstPartData.completeness_ratios.Average());
                rapor.text = firstPartData.alienCount.ToString() + " uzaylı yakaladın.\n";
                rapor2.text = "Skor\n" + Score.ToString();
                //Burda oyunu da durdursak, belki bi animasyon falan olabilir

                /*Debug.Log("Positions");
                foreach (var x in HandTracking.allCoordinates)
                {
                    Debug.Log(x.ToString());
                }

                Debug.Log("Velocities");
                foreach (var x in HandTracking.allVelocities)
                {
                    Debug.Log(x.ToString());
                }

                Debug.Log("Accelerations");
                foreach (var x in HandTracking.allAccels)
                {
                    Debug.Log(x.ToString());
                }*/

                //Debug.Log("Jerks");

                Destroy(gameObject);
                Debug.Log("Smoothness Ratios Average:" + (firstPartData.smoothness_ratios.Average()).ToString());
                //Debug.Log("Average Velocity:" + (HandTracking.allVelocities.Average()).ToString());


                // Debug.Log("DISABLEDDDDDDDDD");
            }
        }

        motionTimer += Time.deltaTime;
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.position = GetMouseAsWorldPoint();
        if(!firstPartData.OnSecondPart )
            firstPartData.timer += Time.deltaTime;
        if (gotalien)
        {
            alienTimer-= Time.deltaTime;

        }


        if (alienTimer <= 0)
        {
            alienTimer = 10.0f; //bunu parametre?
            gotalien = false;
            if (firstPartData.character == "ardacan")
            {
                GameObject alien = ardacanCarry.transform.GetChild(3).gameObject;
                alien.GetComponent<Animator>().Play("FadeOutGame1");
                alien.transform.parent = null;
                ardacanCarry.SetActive(false);
                ardacan.SetActive(true);

                StartCoroutine(ExecuteAfterTime(0.5f,alien,ardacanCarry));
            }
            else
            {
                GameObject alien = ecesuCarry.transform.GetChild(3).gameObject;
                alien.GetComponent<Animator>().Play("FadeOutGame1");
                alien.transform.parent = null;
                ecesuCarry.SetActive(false);
                ecesu.SetActive(true);

                
                StartCoroutine(ExecuteAfterTime(0.5f, alien, ecesuCarry));
            }
            firstPartData.firstMissedAlienPositions.Add(firstPartData.firstCaughtAlienPositions.Last());

            motionTimer = 0;
            foreach (var x in HandTracking.allJerks) //CASE 2:Alien floats away
            {
                if (x < firstPartData.smoothness_threshold)
                {
                    firstPartData.smooth_motions += 1;
                }
            }
            float ratio = (float)firstPartData.smooth_motions / HandTracking.allJerks.Count;
            float cratio = Vector3.Distance(transform.position , spaceship.transform.position) / Vector3.Distance(spaceship.transform.position , tempalienstartpos); //uzaylının z'sini düşünelim
            firstPartData.smoothness_ratios.Add(ratio);
            if (cratio > 1)
                cratio = 1f;
            cratio = 0.5f + (0.5f - (cratio / 2));
            firstPartData.completeness_ratios.Add(cratio);
            Debug.Log("Smoothness ratio:");
            Debug.Log(ratio);
            //Debug.Log("Completeness ratio:");
            //Debug.Log(cratio);

            //Reset for the next motion
            firstPartData.firstCaughtAlienPositions.RemoveAt(firstPartData.firstCaughtAlienPositions.Count - 1);
            firstPartData.smooth_motions = 0;
            HandTracking.allJerks.Clear();
            HandTracking.allAccels.Clear();
            HandTracking.allVelocities.Clear();
            HandTracking.allCoordinates.Clear();

            List<float> tempStats = new List<float>();
            tempStats.Add(ratio);
            tempStats.Add(cratio);
            //tempStats.Add(-1);
            //tempStats.Add(-1);
            firstPartData.firstMissedAlienStats.Add(tempStats);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Alien"&& gameObject.name=="Player" && gotalien == false)
        {

            gotalien = true;
            if (firstPartData.character == "ardacan")
            {
                ardacanCarry.SetActive(true);
                ardacan.SetActive(false);
            }
            else
            {
                ecesuCarry.SetActive(true);
                ecesu.SetActive(false);
            }
            firstPartData.firstCaughtAlienPositions.Add(collision.gameObject.transform.position.ToString());
            tempalienstartpos = collision.gameObject.transform.position;
            tempalienstartpos.x = gameObject.transform.position.x;
        }
        if (collision.gameObject.tag == "Spaceship" && gameObject.name == "Player" && gotalien == true)
        {
            spaceshipentered = true;
            //Debug.Log("PLAYERCOORD:");
            //Debug.Log(transform.position);
            //Debug.Log("SHIPCOORD:");
            //Debug.Log(spaceship.transform.position);
            
        }

        if (collision.gameObject.tag == "Asteroid" && gameObject.name == "Player")
        {
            firstPartData.firstHitAsteroidPositions.Add(collision.gameObject.transform.position.ToString());
            Debug.Log(collision.gameObject.transform.position.ToString());

            int randVoiceline = UnityEngine.Random.Range(1, 4);
            if (firstPartData.character == "ardacan")
            {
                FindObjectOfType<AudioManager>().Play("Asteroid" + randVoiceline.ToString());
            }
            else if (firstPartData.character == "ecesu")
            {
                FindObjectOfType<AudioManager>().Play("Asteroid" + randVoiceline.ToString()+"E");
            }
            FindObjectOfType<AudioManager>().Play("hit");
            if (firstPartData.alienCount >0)
                firstPartData.alienCount--;
            //DAMAGE SOUND AND EFFECT
            firstPartData.alienColors.RemoveAt(firstPartData.alienColors.Count-1);

        }


    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Spaceship")
        {
            spaceshipcounter = 0.1f;
            spaceshipentered = false;
        }
    }

    public void secondPartClicked()
    {
        StartCoroutine(SecondPartExecuteAfterTime(3));
        friendAnim.Play("Dance");
    }

    public void secondPartStart()
    {
        StartCoroutine(SecondPartExecuteAfterTime(0));
    }

    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen
        //mousePoint.z = mousePoint.x;
        mousePoint.z = 2.639097f;
        
        //mousePoint.x = -2.639097f;
        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    IEnumerator SecondPartExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        firstPartData.OnSecondPart = true;
        gameOverScreen1.SetActive(false);
        mainCamera.SetActive(false);
        secondPart.SetActive(true);
        gameObject.SetActive(false);

      
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
