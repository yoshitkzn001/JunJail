using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTalkConfig : MonoBehaviour
{
    [SerializeField] MessageConfig message;

    [SerializeField] GameObject jun;
    [SerializeField] GameObject chara;

    public List<List<(int, int, int, string)>> talk_info = new List<List<(int, int, int, string)>>();

    public bool flip;
    private int talk_step = 0;

    [SerializeField] bool boss = false;
    [SerializeField] GameObject boss_obj = null;

    public void Open(ActionConfig ac)
    {
        Transform juntransform = jun.transform;
        Vector2 jun_pos = juntransform.position;
        Transform charatransform = gameObject.transform;
        Vector2 chara_pos = charatransform.position;

        SpriteRenderer jun_sr = jun.GetComponent<SpriteRenderer>();
        SpriteRenderer chara_sr = chara.GetComponent<SpriteRenderer>();

        if(jun_pos.x > chara_pos.x)
        {
            jun_sr.flipX = true;
            if(flip == true)
            {
                chara_sr.flipX = true;
            }
        }
        else
        {
            jun_sr.flipX = false;
            if (flip == true)
            {
                chara_sr.flipX = false;
            }
        }

        message.message_info = talk_info[talk_step];

        if(talk_step != talk_info.Count - 1)
        {
            talk_step += 1;
        }
        message.ac = ac;
        message.Open(boss, boss_obj);
    }
}
