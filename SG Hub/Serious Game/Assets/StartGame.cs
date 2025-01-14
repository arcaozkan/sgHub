using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;


public class StartGame : MonoBehaviour
{

    public Dropdown gameEndConditionDropdown;

    private DatabaseAccess databaseAccess;
    public Toggle friendMoveToggle;
    public Slider speedSliderL;
    public Slider speedSliderR;
    public Slider sizeSlider;
    public Slider brightnessSlider;
    public Slider volumeSlider;
    public Slider barSlider;
    public Slider timevalSlider;

    public static float[,] AlienSpawnRates;
    private double avgx=0;
    private double avgy=0;
    public Toggle toplamaToggle;
    public Toggle cikarmaToggle;
    public Toggle carpmaToggle;

    public static float speedLval=0.5f; //Force 0.5 ve 20 arasında olsun, defaultu 2.25
    public static float speedRval = 0.5f; //Force 0.5 ve 20 arasında olsun, defaultu 2.25
    public static float sizeval=0.5f; //hallettik
    
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        AlienSpawnRates = new float[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AlienSpawnRates[i, j] = 0.04f;
            }
        }

    }

    public void PlayButtonClicked()
    {
        gameEndCondition();

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int tempi = i - 2; //So the range is between -2 and 2
                int tempj = j - 2;
                avgx+= AlienSpawnRates[i, j]*tempi; //PROBLEM: İki zıt köşeyi seçince sıfır sayıyor. Nasıl çözeriz?
                avgy+= AlienSpawnRates[i, j] * tempj;
            }
        }
        Debug.Log(avgx);
        Debug.Log(avgy);

        DontDestroyOnLoad(gameObject);
        firstPartData.character = char_customization.character;
        SceneManager.LoadScene("Game");

    }

    public void PlayGame2ButtonClicked()
    {
        secondPartData.brightness = 1 - (brightnessSlider.value / 2) - 0.5f;
        secondPartData.volume = volumeSlider.value;
        secondPartData.friendMove = friendMoveToggle.isOn;
        secondPartData.barParam = barSlider.value;
        secondPartData.timeval = timevalSlider.value;
        secondPartData.character = char_customization.character;
        gameEndCondition2();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Game2");

    }

    public void PlayGame3ButtonClicked()
    {
        secondPartData.brightness = 1 - (brightnessSlider.value / 2) - 0.5f;
        secondPartData.volume = volumeSlider.value;
        thirdPartData.toplama = toplamaToggle.isOn;
        thirdPartData.cikarma = cikarmaToggle.isOn;
        thirdPartData.carpma = carpmaToggle.isOn;
        thirdPartData.barParam = barSlider.value;
        thirdPartData.character = char_customization.character;
        firstPartData.OnThirdPart = true;
        gameEndCondition3();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Game3");

    }

    public async void SetupButtonClicked()
    {
        ChosenPatient.chosenGame = "Uzay Macerası:Meteor Yolculuğu";
        var task = databaseAccess.GetParameterFromDatabase();
        var result = await task;
        List<double> outputList = new List<double>(); ;
        foreach (var patient in result)
        {
            outputList.Add(patient.best_param1);
            outputList.Add(patient.best_param2);
            outputList.Add(patient.best_param3);

        }
        if (outputList.Count > 0)
        { 
            speedLval = (float)outputList[outputList.Count - 3];
            speedRval = (float)outputList[outputList.Count - 2];
            sizeval = (float)outputList[outputList.Count-1];
            speedSliderL.value = speedLval;
            speedSliderR.value = speedRval;
            sizeSlider.value = sizeval;
        }
    }

    public async void SetupButtonClicked2()
    {
        ChosenPatient.chosenGame = "Uzay Macerası:Kurtarma Görevi";
        var task = databaseAccess.GetParameterFromDatabase();
        var result = await task;
        List<double> outputList = new List<double>(); ;
        foreach (var patient in result)
        {
            outputList.Add(patient.best_param1);
            outputList.Add(patient.best_param2);
            outputList.Add(patient.best_param3);

        }
        if (outputList.Count > 0)
        {
            secondPartData.barParam = (float)outputList[outputList.Count - 3];
            secondPartData.timeval = (float)outputList[outputList.Count - 2];
            secondPartData.friendMove = (float)outputList[outputList.Count - 1] ==1.0f;
            barSlider.value = secondPartData.barParam;
            timevalSlider.value = secondPartData.timeval;
            friendMoveToggle.isOn = secondPartData.friendMove;
        }
    }

    public async void SetupButtonClicked3()
    {
        ChosenPatient.chosenGame = "Uzay Macerası:Roket Karmaşası";
        var task = databaseAccess.GetParameterFromDatabase();
        var result = await task;
        List<double> outputList = new List<double>(); ;
        foreach (var patient in result)
        {
            outputList.Add(patient.best_param1);
            outputList.Add(patient.best_param2);
            outputList.Add(patient.best_param3);

        }
        if (outputList.Count > 0)
        {
            if ((float)outputList[outputList.Count - 1] == 0.32f)
            {
                toplamaToggle.isOn=true;
                cikarmaToggle.isOn=false;
                carpmaToggle.isOn=false;
            }
            else if ((float)outputList[outputList.Count - 1] == 0.33f)
            {
                toplamaToggle.isOn=false;
                cikarmaToggle.isOn=true;
                carpmaToggle.isOn=false;
            }
            else if ((float)outputList[outputList.Count - 1] == 0.35f)
            {
                toplamaToggle.isOn=false;
                cikarmaToggle.isOn=false;
                carpmaToggle.isOn=true;
            }

            else if ((float)outputList[outputList.Count - 1] == 0.65f)
            {
                toplamaToggle.isOn = true;
                cikarmaToggle.isOn = true;
                carpmaToggle.isOn = false;
            }
            else if ((float)outputList[outputList.Count - 1] == 0.67f)
            {
                toplamaToggle.isOn = true;
                cikarmaToggle.isOn = false;
                carpmaToggle.isOn = true;
            }
            else if ((float)outputList[outputList.Count - 1] == 0.68f)
            {
                toplamaToggle.isOn = false;
                cikarmaToggle.isOn = true;
                carpmaToggle.isOn = true;
            }
            else if ((float)outputList[outputList.Count - 1] == 1.0f)
            {
                toplamaToggle.isOn = true;
                cikarmaToggle.isOn = true;
                carpmaToggle.isOn = true;
            }
            thirdPartData.barParam = (float)outputList[outputList.Count - 3];
            thirdPartData.timeval = (float)outputList[outputList.Count - 2];
            barSlider.value = thirdPartData.barParam;
            timevalSlider.value = thirdPartData.timeval;
        }
    }

    public void gameEndCondition()
    {
        string choice = Convert.ToString(gameEndConditionDropdown.options[gameEndConditionDropdown.value].text);
        if (choice[2] == 'U' || choice[3] == 'U')//Alien Catch condition
        {
            firstPartData.endWithCount = true;
            firstPartData.alienGoalCount = Convert.ToInt32(choice.Substring(0, 2));
            Debug.Log(choice.Substring(0, 2));
        }
        else
        {
            firstPartData.endWithCount = false;
            firstPartData.timeGoalMinutes = Convert.ToInt32(choice.Substring(0, 2));
            Debug.Log(choice.Substring(0, 2));
        }
    }

    public void gameEndCondition2()
    {
        
        string choice = Convert.ToString(gameEndConditionDropdown.options[gameEndConditionDropdown.value].text);
        Debug.Log(choice);
        if (choice[2] == 'U' || choice[3] == 'U')//Alien Catch condition
        {
            secondPartData.endWithCount = true;
            secondPartData.alienGoalCount = Convert.ToInt32(choice.Substring(0, 2));
            Debug.Log(choice.Substring(0, 2));
        }
        else
        {
            secondPartData.endWithCount = false;
            secondPartData.timeGoalMinutes = Convert.ToInt32(choice.Substring(0, 2));
            Debug.Log(choice.Substring(0, 2));
        }
    }

    public void gameEndCondition3()
    {

        string choice = Convert.ToString(gameEndConditionDropdown.options[gameEndConditionDropdown.value].text);
        Debug.Log(choice);
        if (choice[2] == 'U' || choice[3] == 'U')//Alien Catch condition
        {
            thirdPartData.endWithCount = true;
            thirdPartData.alienGoalCount = Convert.ToInt32(choice.Substring(0, 2));
            Debug.Log(choice.Substring(0, 2));
        }
        else
        {
            thirdPartData.endWithCount = false;
            thirdPartData.timeGoalMinutes = Convert.ToInt32(choice.Substring(0, 2));
            Debug.Log(choice.Substring(0, 2));
        }
    }
}
