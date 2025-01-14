using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondPartData : MonoBehaviour
{
    public static bool friendMove=true;
    public static int alienGoalCount = 10;
    public static float timeGoalMinutes = 3f;
    public static bool endWithCount = true;
    public static int newAlienIndex = 0; //same thing with alien saved
    public static float timer = 0f;
    public static float brightness = 0f;
    public static float volume = 0f;
    public static float barParam = 2f;
    public static float timeval = 0.5f; //2. kýsýmda uzaylý kaybolmasý

    public static List<string> secondCaughtAlienColors = new List<string>();
    public static List<string> secondAllAlienColors = new List<string>();
    public static List<string> secondCaughtAlienPositions = new List<string>();
    public static int correctPlanet;
    public static string character = "ardacan";


    public static List<float> smoothness_ratios = new List<float>();
    public static List<float> completeness_ratios = new List<float>();
    public static List<float> duration_ratios = new List<float>();
    public static List<float> steadiness_ratios = new List<float>();
    public static float smoothness_threshold = 30f;
    public static int smooth_motions = 0;
}
