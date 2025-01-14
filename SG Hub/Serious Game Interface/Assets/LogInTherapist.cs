using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogInTherapist : MonoBehaviour
{
    private DatabaseAccess databaseAccess;
    public Text Username_field;
    public Text Password_field;
    private string username;
    private string password;
    public static string username_verified;
    public GameObject therapistPlayerMenu;
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        
    }
    public async void Buttonclicked()
    {
        username = Username_field.text.ToString();
        password = Password_field.text.ToString();
        var task = databaseAccess.GetTherapistFromDatabase(username,password);
        var result = await task;
        List<string> outputList = new List<string>(); ;
        foreach (var patient in result)
        {
            outputList.Add(patient.therapist_username);
        }
        if (outputList.Count != 0)
        {
            username_verified = outputList[0];
            Debug.Log(username_verified + " has successfuly logged in!");
            gameObject.SetActive(false);
            therapistPlayerMenu.SetActive(true);
        }
    }
}
