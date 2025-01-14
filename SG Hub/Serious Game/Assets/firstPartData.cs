using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstPartData : MonoBehaviour
{
    public static int alienGoalCount = 10;
    public static float timeGoalMinutes = 3f;
    public static bool endWithCount = true;
    public static float brightness = 0f;
    public static float volume = 0f;

    public static int alienCount = 0;
    public static List<string> alienColors = new List<string>();

    public static List<string> firstMissedAlienPositions = new List<string>();
    public static List<string> firstCaughtAlienPositions = new List<string>();
    public static List<List<float>> firstCaughtAlienStats = new List<List<float>>();
    public static List<List<float>> firstMissedAlienStats = new List<List<float>>();
    public static List<string> firstHitAsteroidPositions = new List<string>();
    public static List<float> smoothness_ratios = new List<float>();
    public static List<float> completeness_ratios = new List<float>();
    public static List<float> duration_ratios = new List<float>();
    public static List<float> steadiness_ratios = new List<float>();
    public static bool OnSecondPart = false;
    public static bool OnThirdPart = false;

    public static float smoothness_threshold = 8f;
    public static int smooth_motions = 0;
    public static float timer = 0;
    public static string character = "ardacan";

}
