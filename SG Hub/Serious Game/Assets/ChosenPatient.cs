using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenPatient : MonoBehaviour
{
    public static string patientName;
    public static string chosenGame;
    public static int patientID;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = "Seçilen Kullanıcı: " + patientName;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = "Seçilen Kullanıcı: " + patientName;
    }
}
