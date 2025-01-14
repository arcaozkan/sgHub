using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
public class PatientsInDropdown : MonoBehaviour
{
    public Dropdown dropdown;
    private DatabaseAccess databaseAccess;

    public Text name;
    public Text age;
    public Text special_need;
    public Text other_info;

    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();

        UpdateDropdown();
        
    }

    [Serializable]
    public class Patient
    {
        public string patientName;
        public int ID;
    }

    public async void UpdateDropdown()
    {
        //Get patients from local database
        Patient myPatient = new Patient();
        List<string> patient_infos;
        patient_infos=SaveSystem.LoadAll();
        dropdown.options.Clear();
        foreach (string patient_info in patient_infos)
        {
            myPatient = JsonUtility.FromJson<Patient>(patient_info);
            dropdown.options.Add(new Dropdown.OptionData(myPatient.patientName));
            dropdown.RefreshShownValue();
        }

            /*var task = databaseAccess.GetPatientsFromDatabase();
            var result = await task;
            List<string> outputList = new List<string>(); ;
            foreach (var patient in result)
            {
                outputList.Add(patient.patient_name);
            }
            dropdown.options.Clear();
            foreach (string option in outputList)
            {
                dropdown.options.Add(new Dropdown.OptionData(option));
            }
            */

    }

    public void choosePatient()
    {
        ChosenPatient.patientName = dropdown.options[dropdown.value].text;
        ChosenPatient.patientID = dropdown.value + 1;
    }

    public void addPatient()
    {
        Save();
        UpdateDropdown();
    }

    private void Save()
    {

        //DateTime date = DateTime.UtcNow;
        SaveObject saveObject = new SaveObject
        {
            ID = dropdown.options.Count+1,
            patientName = name.text,
            age = Convert.ToInt32(age.text),
            specialNeed=special_need.text,
            otherInfo=other_info.text,
            //Date = date.ToString(),
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

    }
    private class SaveObject
    {
        public int ID;
        public string patientName;
        public int age;
        public string specialNeed;
        public string otherInfo;
    }
}
