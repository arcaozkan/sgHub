using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public bool isSpawner = false;
    public GameObject asteroid;
    public GameObject[] aliens;
    private GameObject currentAsteroid;
    private GameObject currentAlien;
    public float leftAsteroidSpawnRate=0.33f;
    public float rightAsteroidSpawnRate = 0.33f;
    public float upAsteroidSpawnRate = 0.5f;
    private float maxvalsum=0;
    public static float[,] AlienSpawnRates;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Asteroid" && isSpawner==false)
        {
            Vector3 temp = new Vector3(17.0f, 0, 0);

            collision.gameObject.transform.position -= temp;
        }


    }
    void Start()
    {
        float spawnRepeatRate = (1-(StartGame.speedLval) + 1-(StartGame.speedRval)) +1;

        //InvokeRepeating("SpawnAsteroid",1.0f, spawnRepeatRate);
        InvokeRepeating("SpawnAlien", 2.5f, spawnRepeatRate);
    }
    void SpawnAsteroid()
    {
        if (isSpawner)
        {
            float randNum = Random.Range(0f, 1.0f);
            float z;
            float y;
            if (randNum <= leftAsteroidSpawnRate)
            {
                z = -2f; //-2,0,2
            }
            else if (randNum <= leftAsteroidSpawnRate + rightAsteroidSpawnRate)
            {
                z = 2f; //-2,0,2
            }
            else
            {
                z = 0f;
            }

            randNum = Random.Range(0f, 1.0f);
            if (randNum <= upAsteroidSpawnRate)
            {
                y = 0f; //-2,0,2
            }

            else
            {
                y = -1f;
            }


            Vector3 spawnPos = gameObject.transform.position + new Vector3(0, y, z);
            currentAsteroid =Instantiate(asteroid, spawnPos, Quaternion.identity);
            currentAsteroid.transform.localScale= new Vector3(0.2f, 0.2f, 0.2f);
            ConstantForce cachedConstantForce; cachedConstantForce = currentAsteroid.GetComponent<ConstantForce>();
            if (z <= 0)
            {
                if (StartGame.speedLval >= 0.5)
                    cachedConstantForce.force = new Vector3((33 * (StartGame.speedLval * StartGame.speedLval) - 13.5f * StartGame.speedLval + 0.5f), 0.0f, 0.0f); //32x^2-12.5x +0.5    0->0.5 0.5->2 1->20
                else
                    cachedConstantForce.force = new Vector3((3f * StartGame.speedLval + 0.5f), 0.0f, 0.0f);
            }
            if (z > 0)
            {
                if (StartGame.speedRval >= 0.5)
                    cachedConstantForce.force = new Vector3((33 * (StartGame.speedRval * StartGame.speedRval) - 13.5f * StartGame.speedRval + 0.5f), 0.0f, 0.0f); //32x^2-12.5x +0.5    0->0.5 0.5->2 1->20
                else
                    cachedConstantForce.force = new Vector3((3f * StartGame.speedLval + 0.5f), 0.0f, 0.0f);
            }
        }
    }

    void SpawnAlien()
    {
        if (isSpawner)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    maxvalsum+=StartGame.AlienSpawnRates[i, j];
                }
            }
            float randNum = Random.Range(0f, maxvalsum);
            maxvalsum = 0;
            float z=0;
            float y=0;
            
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    randNum -= StartGame.AlienSpawnRates[i, j];
                    if (randNum <= 0)
                    {
                        z = j - 2; //-2,-1,0,1,2
                        y = i / 4.0f; //0.4,0.7,0.9,0.11,0.14
                        break;
                    }

                }

                if (randNum <= 0)
                {
                    break;
                }
            }


            Vector3 spawnPos = gameObject.transform.position + new Vector3(0, -y-0.2f, z); //
            currentAlien = Instantiate(aliens[Random.Range(0,4)], spawnPos, Quaternion.identity);
            currentAlien.transform.Rotate(0, 90, 0);
            currentAlien.transform.localScale = new Vector3(1f-(StartGame.sizeval)+0.8f, 1f-(StartGame.sizeval)+0.8f, 1f-(StartGame.sizeval)+0.8f);
            ConstantForce cachedConstantForce; cachedConstantForce = currentAlien.GetComponent<ConstantForce>();

            if (z <= 0)
            {
                if(StartGame.speedLval >=0.5)
                    cachedConstantForce.force = new Vector3((33 * (StartGame.speedLval * StartGame.speedLval) - 13.5f * StartGame.speedLval + 0.5f), 0.0f, 0.0f); //32x^2-12.5x +0.5    0->0.5 0.5->2 1->20
                else
                    cachedConstantForce.force = new Vector3((3f * StartGame.speedLval + 0.5f), 0.0f, 0.0f);
            }
            if (z > 0)
            {
                StartGame.speedRval = StartGame.speedLval;
                if (StartGame.speedRval >= 0.5)
                    cachedConstantForce.force = new Vector3((33 * (StartGame.speedRval * StartGame.speedRval) - 13.5f * StartGame.speedRval + 0.5f), 0.0f, 0.0f); //32x^2-12.5x +0.5    0->0.5 0.5->2 1->20
                else
                    cachedConstantForce.force = new Vector3((3f * StartGame.speedLval + 0.5f), 0.0f, 0.0f);
            }
        }
    }

}
