using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimationScript : MonoBehaviour
{
    public int speed;
    public SpriteRenderer sr;

    public List<Sprite> nomal_chara = new List<Sprite>();
    public List<Sprite> action_chara = new List<Sprite>();

    private int max_t;
    private int t;

    public int action;
    
    // Start is called before the first frame update
    void Start()
    {
        max_t = speed * nomal_chara.Count;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t % speed == 0)
        {
            ChangeSprite();
        }

        t += 1;
    }

    public void ChangeSprite()
    {
        if (t == max_t)
        {
            t = 0;
        }

        if (action == 0)
        {
            sr.sprite = nomal_chara[t / speed];
        }
        else
        {
            sr.sprite = action_chara[t / speed];
        }
    }
}
