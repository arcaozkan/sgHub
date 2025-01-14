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
        patient_infos = SaveSystem.LoadAll();
        dropdown.options.Clear();
        foreach (string patient_info in patient_infos)
        {
            myPatient = JsonUtility.FromJson<Patient>(patient_info);
            dropdown.options.Add(new Dropdown.OptionData(myPatient.patientName));
            dropdown.RefreshShownValue();
        }
    }

        public void choosePatient()
    {
        ChosenPatient.patientName = dropdown.options[dropdown.value].text;
        ChosenPatient.patientID = dropdown.value + 1;
    }
}
