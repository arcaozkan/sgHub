using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
public class AddUser : MonoBehaviour
{
    private DatabaseAccess databaseAccess;
    public Text patient_name,age,gender,height,weight,special_need,therapy_type,therapy_start_date,additional_notes;
    public Dropdown patientsdropdown;
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
    }

    public void AddUserButtonClicked()
    {
        databaseAccess.SavePatientToDataBase(LogInTherapist.username_verified, 1, patient_name.text.ToString(), Convert.ToInt32(age.text.ToString()), gender.text.ToString(), Convert.ToDouble(height.text.ToString()), Convert.ToDouble(weight.text.ToString()), special_need.text.ToString(), therapy_type.text.ToString(), therapy_start_date.text.ToString(), additional_notes.text.ToString());
        patientsdropdown.GetComponent<PatientsInDropdown>().UpdateDropdown();
    }
}
