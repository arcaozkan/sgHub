using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class LineFitter : MonoBehaviour
{
    [SerializeField] private PointGenerator pointGenerator;
    [SerializeField] private double learningRate;
    [SerializeField] private double normalize;
    [SerializeField] private double epochs;

    public GameObject graphContainer;
    public Dropdown graphDropdown;
    public List<GameObject> Points { get; private set; }
    public GameObject[] PointsArr;
    static public double smoothnessA;
    static public double completenessA;
    static public double durationA;
    static public double steadinessA;


    public void FitLine()
    {
        PointsArr =null;
        Points = new List<GameObject>();
        StopAllCoroutines();
        StartCoroutine(FitLineCoR());
    }

    private IEnumerator FitLineCoR()
    {
        PointsArr = GameObject.FindGameObjectsWithTag("point");
        double a = 0;
        double b = 0;
        double N = PointsArr.Length;//pointGenerator.Points.Count;
        for(int i = 0; i < PointsArr.Length; i++)
        {
            Points.Add(PointsArr[i]);
            Debug.Log(Points.Count);
        }
        var X = Points.Select(p => p.transform.localPosition.x);
        var Y = Points.Select(p => p.transform.localPosition.y);
        for (int i = 0; i < epochs; i++)
        {
            var Y_pred = X.Select(x => x * a + b);
            double D_a = (-2.0/N) * Y
                .Zip(Y_pred, (y, y_pred) => y - y_pred)
                .Zip(X, (y_p, x) => y_p * x )
                .Sum() ;
            double D_b = (-2.0/N) * Y
                .Zip(Y_pred, (y, y_pred) => y - y_pred)
                .Sum();
            a -= learningRate * D_a;
            b -= learningRate * D_b * normalize;
            Debug.Log($"a: {a}, b: {b}");
            SetLine(a, b);
            yield return null;
        }
        if (graphDropdown.value == 2)
        {
            smoothnessA = a;
        }
        if (graphDropdown.value == 3) {
            completenessA = a;
         
        }
        if (graphDropdown.value == 4){
            durationA = a;

        }
        if (graphDropdown.value == 5) {
            steadinessA = a;

        }
    }

    private void SetLine(double a, double b)
    {
        double rad = Math.Atan(a);
        double deg = rad * 180 / Math.PI;
        transform.localPosition = new Vector3(0, (float) b, 0);
        transform.localRotation = Quaternion.Euler(0, 0, (float) deg);
    }
}
