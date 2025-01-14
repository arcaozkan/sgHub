using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEditor;
using System.IO;
public class heatmap : MonoBehaviour
{
    // Start is called before the first frame update
    public static int[,] heatmapArray;
    public static int[,] heatmapArrayM;
    public static float[,] ratioHeatmapArray;
    public static float[,,] heatmapArrayStats;
    public static float[,,] heatmapArrayMStats;
    public Image[] heatmapTileColors;
    private DatabaseAccess databaseAccess;
    void Start()
    {
        SaveSystem.Init();
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        heatmapArray = new int[5, 5];
        heatmapArrayM = new int[5, 5];
        ratioHeatmapArray = new float[5, 5];
        heatmapArrayStats = new float[5,5,4];
        heatmapArrayMStats = new float[5, 5, 2];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                heatmapArray[i,j] = 0;
                heatmapArrayM[i, j] = 0;
                ratioHeatmapArray[i, j] = 0f;
            }
        }

        for (int i=0; i < firstPartData.firstCaughtAlienPositions.Count; i++) {
            string currentAlien = firstPartData.firstCaughtAlienPositions[i];
            if (currentAlien.Substring(7, 3) == "0.4") //Down
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArray[4, 0] += 1;
                    heatmapArrayStats[4, 0, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[4, 0, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[4, 0, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[4, 0, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArray[4, 1] += 1;
                    heatmapArrayStats[4, 1, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[4, 1, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[4, 1, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[4, 1, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArray[4, 2] += 1;
                    heatmapArrayStats[4, 2, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[4, 2, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[4, 2, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[4, 2, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArray[4, 3] += 1;
                    heatmapArrayStats[4, 3, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[4, 3, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[4, 3, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[4, 3, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArray[4, 4] += 1;
                    heatmapArrayStats[4, 4, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[4, 4, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[4, 4, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[4, 4, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
            }

            else if (currentAlien.Substring(7, 3) == "0.6") //MDown
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArray[3, 0] += 1;
                    heatmapArrayStats[3, 0, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[3, 0, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[3, 0, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[3, 0, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArray[3, 1] += 1;
                    heatmapArrayStats[3, 1, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[3, 1, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[3, 1, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[3, 1, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArray[3, 2] += 1;
                    heatmapArrayStats[3, 2, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[3, 2, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[3, 2, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[3, 2, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArray[3, 3] += 1;
                    heatmapArrayStats[3, 3, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[3, 3, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[3, 3, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[3, 3, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArray[3, 4] += 1;
                    heatmapArrayStats[3, 4, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[3, 4, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[3, 4, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[3, 4, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
            }
            else if (currentAlien.Substring(7, 3) == "0.9") //Mid
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArray[2, 0] += 1;
                    heatmapArrayStats[2, 0, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[2, 0, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[2, 0, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[2, 0, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArray[2, 1] += 1;
                    heatmapArrayStats[2, 1, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[2, 1, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[2, 1, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[2, 1, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArray[2, 2] += 1;
                    heatmapArrayStats[2, 2, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[2, 2, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[2, 2, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[2, 2, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArray[2, 3] += 1;
                    heatmapArrayStats[2, 3, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[2, 3, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[2, 3, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[2, 3, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArray[2, 4] += 1;
                    heatmapArrayStats[2, 4, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[2, 4, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[2, 4, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[2, 4, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
            }
            else if (currentAlien.Substring(7, 3) == "1.1") //MUp
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArray[1, 0] += 1;
                    heatmapArrayStats[1, 0, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[1, 0, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[1, 0, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[1, 0, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArray[1, 1] += 1;
                    heatmapArrayStats[1, 1, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[1, 1, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[1, 1, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[1, 1, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArray[1, 2] += 1;
                    heatmapArrayStats[1, 2, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[1, 2, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[1, 2, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[1, 2, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArray[1, 3] += 1;
                    heatmapArrayStats[1, 3, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[1, 3, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[1, 3, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[1, 3, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArray[1, 4] += 1;
                    heatmapArrayStats[1, 4, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[1, 4, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[1, 4, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[1, 4, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
            }
            else if (currentAlien.Substring(7, 3) == "1.4") //Up
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArray[0, 0] += 1;
                    heatmapArrayStats[0, 0, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[0, 0, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[0, 0, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[0, 0, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArray[0, 1] += 1;
                    heatmapArrayStats[0, 1, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[0, 1, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[0, 1, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[0, 1, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArray[0, 2] += 1;
                    heatmapArrayStats[0, 2, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[0, 2, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[0, 2, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[0, 2, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArray[0, 3] += 1;
                    heatmapArrayStats[0, 3, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[0, 3, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[0, 3, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[0, 3, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArray[0, 4] += 1;
                    heatmapArrayStats[0, 4, 0] += firstPartData.firstCaughtAlienStats[i][0];
                    heatmapArrayStats[0, 4, 1] += firstPartData.firstCaughtAlienStats[i][1];
                    heatmapArrayStats[0, 4, 2] += firstPartData.firstCaughtAlienStats[i][2];
                    heatmapArrayStats[0, 4, 3] += firstPartData.firstCaughtAlienStats[i][3];
                }
            }
            
        }
        Debug.Log("Starting missed");
        Debug.Log(firstPartData.firstMissedAlienPositions.Count);
        Debug.Log(firstPartData.firstMissedAlienStats.Count);
        for (int i = 0; i < firstPartData.firstMissedAlienPositions.Count; i++)
        {
            string currentAlien = firstPartData.firstMissedAlienPositions[i];
            if (currentAlien.Substring(7, 3) == "0.4") //Down
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArrayM[4, 0] += 1;
                    heatmapArrayMStats[4, 0, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[4, 0, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArrayM[4, 1] += 1;
                    heatmapArrayMStats[4, 1, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[4, 1, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArrayM[4, 2] += 1;
                    heatmapArrayMStats[4, 2, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[4, 2, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArrayM[4, 3] += 1;
                    heatmapArrayMStats[4, 3, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[4, 3, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArrayM[4, 4] += 1;
                    heatmapArrayMStats[4, 4, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[4, 4, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
            }

            else if (currentAlien.Substring(7, 3) == "0.6") //MDown
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArrayM[3, 0] += 1;
                    heatmapArrayMStats[3, 0, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[3, 0, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArrayM[3, 1] += 1;
                    heatmapArrayMStats[3, 1, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[3, 1, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArrayM[3, 2] += 1;
                    heatmapArrayMStats[3, 2, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[3, 2, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArrayM[3, 3] += 1;
                    heatmapArrayMStats[3, 3, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[3, 3, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArrayM[3, 4] += 1;
                    heatmapArrayMStats[3, 4, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[3, 4, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
            }
            else if (currentAlien.Substring(7, 3) == "0.9") //Mid
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArrayM[2, 0] += 1;
                    heatmapArrayMStats[2, 0, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[2, 0, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArrayM[2, 1] += 1;
                    heatmapArrayMStats[2, 1, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[2, 1, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArrayM[2, 2] += 1;
                    heatmapArrayMStats[2, 2, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[2, 2, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArrayM[2, 3] += 1;
                    heatmapArrayMStats[2, 3, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[2, 3, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArrayM[2, 4] += 1;
                    heatmapArrayMStats[2, 4, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[2, 4, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
            }
            else if (currentAlien.Substring(7, 3) == "1.1") //MUp
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArrayM[1, 0] += 1;
                    heatmapArrayMStats[1, 0, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[1, 0, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArrayM[1, 1] += 1;
                    heatmapArrayMStats[1, 1, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[1, 1, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArrayM[1, 2] += 1;
                    heatmapArrayMStats[1, 2, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[1, 2, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArrayM[1, 3] += 1;
                    heatmapArrayMStats[1, 3, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[1, 3, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArrayM[1, 4] += 1;
                    heatmapArrayMStats[1, 4, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[1, 4, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
            }
            else if (currentAlien.Substring(7, 3) == "1.4") //Up
            {
                if (currentAlien.Substring(12, currentAlien.Length - 13) == "-2.0") //Left
                {
                    heatmapArrayM[0, 0] += 1;
                    heatmapArrayMStats[0, 0, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[0, 0, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "-1.0") //MLeft
                {
                    heatmapArrayM[0, 1] += 1;
                    heatmapArrayMStats[0, 1, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[0, 1, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "0.0") //Mid
                {
                    heatmapArrayM[0, 2] += 1;
                    heatmapArrayMStats[0, 2, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[0, 2, 1] += firstPartData.firstMissedAlienStats[i][1];
                }

                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "1.0") //Right
                {
                    heatmapArrayM[0, 3] += 1;
                    heatmapArrayMStats[0, 3, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[0, 3, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
                else if (currentAlien.Substring(12, currentAlien.Length - 13) == "2.0") //Right
                {
                    heatmapArrayM[0, 4] += 1;
                    heatmapArrayMStats[0, 4, 0] += firstPartData.firstMissedAlienStats[i][0];
                    heatmapArrayMStats[0, 4, 1] += firstPartData.firstMissedAlienStats[i][1];
                }
            }

        }
        Debug.Log("Ending missed");
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if ((heatmapArray[i, j] + heatmapArrayM[i, j]) != 0)
                    ratioHeatmapArray[i, j] = (float)heatmapArray[i, j] / (heatmapArray[i, j] + heatmapArrayM[i, j]);
                else
                    ratioHeatmapArray[i, j] = -1; //Bunu silersek NaN olarak tutuyor, onu da yapabiliriz

                heatmapArrayStats[i,j,0]= (heatmapArrayMStats[i, j, 0] + heatmapArrayStats[i, j, 0]) / (heatmapArray[i, j]+ heatmapArrayM[i, j]); //Average Smoothness
                heatmapArrayStats[i, j, 1] = (heatmapArrayMStats[i, j, 1] + heatmapArrayStats[i, j, 1]) / (heatmapArray[i, j]+ heatmapArrayM[i, j]); //Average Completeness

                heatmapArrayStats[i, j, 2] = heatmapArrayStats[i, j, 2] / heatmapArray[i, j]; //Average Duration
                heatmapArrayStats[i, j, 3] = heatmapArrayStats[i, j, 3] / heatmapArray[i, j]; //Average Steadiness

                heatmapArrayMStats[i, j, 0] = heatmapArrayMStats[i, j, 0] / heatmapArrayM[i, j];//Average Missed Smoothness
                heatmapArrayMStats[i, j, 1] = heatmapArrayMStats[i, j, 1] / heatmapArrayM[i, j];//Average Missed Completeness
            }
        }
        Debug.Log("Starting Update");
        UpdateHeatmapTile();
        Debug.Log("Ending Update");

        //Gerekirse bu 3 satýrý disablelayýp gameliked sistemini enablela
        int Score = (int)(25 * firstPartData.steadiness_ratios.Average() + 25 * firstPartData.duration_ratios.Average() + 25 * firstPartData.completeness_ratios.Average() + 25 * firstPartData.smoothness_ratios.Average());
        DateTime date = DateTime.UtcNow;
        databaseAccess.SaveScoreToDataBase(ChosenPatient.patientID,  Score,
            StartGame.speedLval, StartGame.speedRval, StartGame.sizeval, firstPartData.timer, 
            firstPartData.smoothness_ratios.Average(), firstPartData.completeness_ratios.Average(), 
            firstPartData.duration_ratios.Average(), firstPartData.steadiness_ratios.Average(), 
            ratioHeatmapArray, heatmapArray, heatmapArrayStats, 
            heatmapArrayMStats, 
            heatmapArrayM,true, date.ToString(), "Uzay Macerası:Meteor Yolculuğu");
        Debug.Log("Saved to database");
        //Save();
        //Debug.Log("Saved to json");

    }
    /*
    private void Save()
    {

        DateTime date = DateTime.UtcNow;
        SaveObject saveObject = new SaveObject
        {
            ID = ChosenPatient.patientID,
            patientName = ChosenPatient.patientName,
            Score = 100,
            Date = date.ToString(),
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

    }
    private class SaveObject
    {
        public int ID;
        public string patientName;
        public int Score;
        public string Date;
    }*/

    /*
    void UpdateHeatmapTile()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (heatmapArray[x, y] == 0)
                    heatmapTileColors[x * 5 + y].color = Color.blue;
                else if (heatmapArray[x, y] == 1)
                    heatmapTileColors[x * 5 + y].color = Color.green;
                else if (heatmapArray[x, y] == 2)
                    heatmapTileColors[x * 5 + y].color = Color.yellow;
                else if (heatmapArray[x, y] == 3)
                    heatmapTileColors[x * 5 + y].color = new Color(0.2F, 0.3F, 0.4F);
                else if (heatmapArray[x, y] >= 4)
                    heatmapTileColors[x * 5 + y].color = Color.red;
                
            }
        }
    }
    */
    void UpdateHeatmapTile()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (ratioHeatmapArray[x, y]  == -1f)
                    heatmapTileColors[x * 5 + y].color = Color.blue;
                else if (ratioHeatmapArray[x, y] <= 0.2f)
                    heatmapTileColors[x * 5 + y].color = Color.red;
                else if (ratioHeatmapArray[x, y] <= 0.4f)
                    heatmapTileColors[x * 5 + y].color = new Color(1F, 0.56F, 0F); 
                else if (ratioHeatmapArray[x, y] <= 0.6f)
                    heatmapTileColors[x * 5 + y].color = new Color(1F, 0.84F, 0F);
                else if (ratioHeatmapArray[x, y] <= 0.8f)
                    heatmapTileColors[x * 5 + y].color = new Color(0.8F, 1F, 0F);
                else if (ratioHeatmapArray[x, y] <= 1f)
                    heatmapTileColors[x * 5 + y].color = Color.green;

            }
        }
    }
}
