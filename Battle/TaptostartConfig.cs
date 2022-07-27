using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaptostartConfig : MonoBehaviour
{
    private const int ANIM_SPEED = 6;

    [SerializeField] private Sprite[] sprites;
    private Image img;
    private int t;

    private bool tapped;

    public void SetStart(float x, float y)
    {
        t = 0;
        tapped = false;
        gameObject.SetActive(true);
        Transform transform = gameObject.transform;
        Vector3 pos = transform.localPosition;
        pos.x = x;
        pos.y = y;
        transform.localPosition = pos;
        img = gameObject.GetComponent<Image>();
        img.sprite = sprites[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(tapped == false)
        {
            t += 1;

            if (t == ANIM_SPEED)
            {
                img.sprite = sprites[1];
            }

            if (t == ANIM_SPEED * 2)
            {
                img.sprite = sprites[2];
                t = 0;
            }
        }
        else
        {
            t += 1;

            if(t == ANIM_SPEED)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Tapped()
    {
        tapped = true;
        t = 0;
        img.sprite = sprites[3];
    }
}
