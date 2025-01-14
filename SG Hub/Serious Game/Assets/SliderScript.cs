using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Text alienText;
    public Slider alienSlider;

    public void SpeedValueChange(string left_right)
    {
        float value = alienSlider.value;
        if (left_right == "right")
            StartGame.speedRval = value;
        if (left_right=="left")
            StartGame.speedLval = value;
        if (value < 0.25)
            alienText.text = "Çok Yavaş";
        else if (value > 0.75)
            alienText.text = "Çok Hızlı";
        else if (value < 0.45)
            alienText.text = "Yavaş";
        else if (value > 0.55)
            alienText.text = "Hızlı";

        else
            alienText.text = "Normal";
    }

    public void DifficultValueChange()
    {
        float value = alienSlider.value;
        StartGame.sizeval = value;
        if (value < 0.25)
            alienText.text = "Çok Kolay";
        else if (value > 0.75)
            alienText.text = "Çok Zor";
        else if (value < 0.45)
            alienText.text = "Kolay";
        else if (value > 0.55)
            alienText.text = "Zor";

        else
            alienText.text = "Normal";
    }

    public void TimeValueChange()
    {
        float value = alienSlider.value;
        if (value < 0.25)
            alienText.text = "Çok Uzun";
        else if (value > 0.75)
            alienText.text = "Çok Kısa";
        else if (value < 0.45)
            alienText.text = "Uzun";
        else if (value > 0.55)
            alienText.text = "Kısa";

        else
            alienText.text = "Normal";
    }

    public void VolumeValueChange()
    {
        float value = alienSlider.value;
        if (value < 0.25)
            alienText.text = "Çok Az";
        else if (value > 0.75)
            alienText.text = "Çok Fazla";
        else if (value < 0.45)
            alienText.text = "Az";
        else if (value > 0.55)
            alienText.text = "Fazla";

        else
            alienText.text = "Normal";
    }
}
