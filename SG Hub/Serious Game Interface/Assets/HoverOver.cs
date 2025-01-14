using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject HoverPanel;
    public Text Params;
    public double param1;
    public double param2;
    public string gamename;
    private string param1name;
    private string param2name;
    private DatabaseAccess databaseAccess;
    public void Awake()
    {
        GetParams();
    }
    public async void GetParams()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        var task = databaseAccess.GetGamesFromDatabase();
        var result = await task;

        foreach (var game in result)
        {
            if (game.name == gamename)
            {
                param1name = game.parameters.Split(',')[0];
                param2name = game.parameters.Split(',')[1];
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverPanel.SetActive(true);
        HoverPanel.transform.position = (gameObject.transform.position);
        HoverPanel.transform.position += new Vector3(0, 30, 0);
        Params = GameObject.Find("HoverParams").GetComponent<Text>();
        Params.text = param1name+":" + param1.ToString("F2") + "\n"+ param2name + ":" + param2.ToString("F2");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverPanel.SetActive(false);
    }
}
