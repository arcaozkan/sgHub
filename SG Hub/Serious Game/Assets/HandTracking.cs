using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    public static List<Vector3> allCoordinates = new List<Vector3>();
    public static List<float> allVelocities = new List<float>();
    public static List<float> allAccels = new List<float>();
    public static List<float> allJerks = new List<float>();
    float elapsed = 0f;
    Vector3 previous;
    float previous_velocity, previous_accel,previous_jerk, current_velocity, current_accel, current_jerk;
    Vector3 current;

    //FILTER
    float C; //Filter aggressiveness
    float dt;
    float alpha;

    float y;

    void Start()
    {
        previous = new Vector3(-2.639f, -0.5f, -0.304f);
        previous_velocity = 0;
        previous_accel = 0;
        previous_jerk = 0;

        C = 0.2f; //Filter aggressiveness
        dt = 0.02f;
        alpha = dt / (C + dt);
    }


    void Update()
    {
        string data = udpReceive.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        //print(data);
        string[] points = data.Split(',');
        //print(points[0]);

        elapsed += Time.deltaTime;

        //0        1*3      2*3
        //x1,y1,z1,x2,y2,z2,x3,y3,z3


            float x = 7-float.Parse(points[0])/100;
            float y = float.Parse(points[1]) / 100;
            float z = -2.639f;
        if (firstPartData.OnSecondPart)
        {
            //Debug.Log("Im here");
            x = 88- float.Parse(points[0])*1.25f / 100; //77-87
            y = (float.Parse(points[1]) * 1.25f / 100)-2; //-2,4
            /*z = 45+float.Parse(points[5]) /10 ; //42.4f; 30-46
            if (z <= 42f)
                z = 39f;
            else if (z > 42f)
                z = 42f;
            */
            z = 39f;

        }
        else if (firstPartData.OnThirdPart)
        {
            x = 92 - float.Parse(points[0]) / 50; //77-87
            y = (float.Parse(points[1]) / 50) - 2; //-2,4
            z = 35f;
        }

        current = new Vector3(z, y, x);
        handPoints[0].transform.localPosition = current;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        //2 low pass fikri:
        //1) 1/60. frame ve 2/60. frame arasında çok distance farkı var, bi insan bu kadar distance yapamaz. Atıyorum 3 birimden fazla varsa bunu yoksayalım.
        //2) 1/60. frame ve 2/60. frame arasında çok fark var, bu yüzden 1/60 ve 10/60a bakıp aradakileri yoksayalım.


        //50 fps
            
            current_velocity = (current.magnitude - previous.magnitude) / Time.deltaTime;
            current_velocity=lowPass(current_velocity, previous_velocity);

            current_accel = (current_velocity - previous_velocity) / Time.deltaTime;
            current_accel = lowPass(current_accel, previous_accel);

            current_jerk = (current_accel - previous_accel) / Time.deltaTime;

            current_jerk = lowPass(current_jerk, previous_jerk);


            previous = current;
            previous_velocity = current_velocity;
            previous_accel = current_accel;
            previous_jerk = current_jerk;


            allVelocities.Add(Mathf.Abs(current_velocity)); // Absolute value
            allAccels.Add(Mathf.Abs(current_accel));
            allJerks.Add(Mathf.Abs(current_jerk));
            allCoordinates.Add(current);

    }

    float lowPass(float data,float prevdata)
    {

        if (prevdata == 0)
            y = alpha * data;
        else
            y = alpha * data + (1 - alpha) * prevdata;

        return y;
    }
}