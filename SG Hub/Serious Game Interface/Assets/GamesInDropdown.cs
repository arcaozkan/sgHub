using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
public class GamesInDropdown : MonoBehaviour
{
    public Dropdown dropdown;
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    private DatabaseAccess databaseAccess;
    public Text parameter1;
    public Text parameter2;
    public Text parameter3;

    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();

        UpdateDropdown();
        UpdateSliders();
    }
    public async void UpdateDropdown()
    {
        var task = databaseAccess.GetGamesFromDatabase();
        var result = await task;
        List<string> outputList = new List<string>();
        List<string> outputList2 = new List<string>();
        foreach (var game in result)
        {
            outputList.Add(game.name);
            outputList2.Add(game.parameters);
        }
        dropdown.options.Clear();
        foreach (string option in outputList)
        {
            dropdown.options.Add(new Dropdown.OptionData(option));
        }
        dropdown.RefreshShownValue();
        string[] str = outputList2[dropdown.value].Split(new[] { ','}, StringSplitOptions.RemoveEmptyEntries);
        parameter1.text = str[0];
        parameter2.text = str[1];
        parameter3.text = str[2];
    }

    public async void UpdateSliders()
    {
        var task = databaseAccess.GetParametersFromDatabase(dropdown.options[dropdown.value].text);
        var result = await task;
        List<double> outputList1 = new List<double>();
        List<double> outputList2 = new List<double>();
        List<double> outputList3 = new List<double>();
        foreach (var param in result)
        {
            outputList1.Add(param.best_param1);
            outputList2.Add(param.best_param2);
            outputList3.Add(param.best_param3);
        }
        
        foreach (double option in outputList1)
        {
            slider1.value = (float)option;
            Debug.Log(slider1.value);
        }
        foreach (double option in outputList2)
        {
            slider2.value = (float)option;
            Debug.Log(slider2.value);
        }
        foreach (double option in outputList3)
        {
            slider3.value = (float)option;
            Debug.Log(slider3.value);
        }

    }

    public void SaveButtonClicked()
    {
        databaseAccess.SaveParametersToDatabase(ChosenPatient.patientID, Convert.ToDouble(slider1.value), Convert.ToDouble(slider2.value), Convert.ToDouble(slider3.value), Convert.ToString(dropdown.options[dropdown.value].text));
    }


}
