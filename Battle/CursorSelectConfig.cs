using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorSelectConfig : MonoBehaviour
{
    public int t_range = 50;
    public float alpha_range = 0.35f;
    public float alpha_center = -0.1f;

    [SerializeField] Image img;
    int mode;
    int t;

    private void OnEnable()
    {
        reset(0);
    }

    public void reset(int _mode)
    {
        mode = _mode;
        t = -t_range;
        img.color = new Color(1f, 1f, 1f, alpha_range);
    }

    public void SetColor(float r, float g, float b, float a)
    {
        mode = -1;
        img.color = new Color(r, g, b, a);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(mode == 0) //白透明度上下
        {
            t += 1;
            float alpha = 1.0f - (Mathf.Abs(t) / (float)t_range);

            if (alpha < alpha_range)
            {
                alpha = alpha_range;
            }
            else if(alpha > (1.0f - alpha_range))
            {
                alpha = 1.0f - alpha_range;
            }
            alpha += alpha_center;

            img.color = new Color(1f, 1f, 1f, alpha);

            if(t == t_range)
            {
                t = -t_range;
            }
        }
    }
}
