using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncBar : MonoBehaviour
{
    public void SetBar(int child, int parent, int width, GameObject bar)
    {
        RectTransform recttransform = bar.GetComponent<RectTransform>();
        Vector2 size = recttransform.sizeDelta;
        size.x = (child / (float)parent) * width;
        recttransform.sizeDelta = size;
    }

    public void SetBarInt(int child, int parent, int width, GameObject bar)
    {
        RectTransform recttransform = bar.GetComponent<RectTransform>();
        Vector2 size = recttransform.sizeDelta;
        size.x = (int)((child / (float)parent) * width);
        recttransform.sizeDelta = size;
    }

    public void DecBar(int t, int t_max, int value, int value_dec, float value_max, int width, RectTransform transform)
    {
        float now_value = value + value_dec * (t / (float)t_max);
        Vector2 size = transform.sizeDelta;
        size.x = (now_value / value_max) * width;
        transform.sizeDelta = size;
    }
}
