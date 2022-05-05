using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarController : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Color color;
    public TextMeshProUGUI text;

    public void SetMaxAmount(float amount)
    {
        slider.maxValue = amount;
        slider.value = amount;
        text.text = amount.ToString();

        if (gradient != null)
        {
            fill.color = gradient.Evaluate(1f);
        }
        else
        {
            fill.color = color;
        }
    }

    public void SetAmount(float amount)
    {
        slider.value = amount;
        text.text = amount.ToString("F0");
        if(gradient != null)
        {
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }

}
