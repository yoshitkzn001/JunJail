using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionConfig : MonoBehaviour
{
    [SerializeField] JunConfig junC;
    public int action_num;
    [SerializeField] GameObject action_sub_obj;
    public SpriteRenderer sr;
    public Sprite[] sprite;

    [SerializeField] CharaAnimationScript[] anims;

    public void ChangeSprite(int n)
    {
        if(action_num != 5)
        {
            sr.sprite = sprite[n];
        }
        else
        {
            foreach(CharaAnimationScript anim in anims)
            {
                anim.action = n;
                anim.ChangeSprite();
            }
        }
    }

    public void Action()
    {
        if(action_num == 1)  //door
        {
            junC.Master.nomalSE.Play("action3");
            action_sub_obj.GetComponent<ActionCageDoorConfig>().MoveDoor();
            junC.t_action = 20;
        }
        else if(action_num == 2)  //box
        {
            action_sub_obj.GetComponent<ActionBoxConfig>().OpneBox();
            junC.t_action = -1;
        }
        else if(action_num == 3)  //stair
        {
            junC.Master.nomalSE.Play("stair1");
            junC.canmove = false;
            junC.UseStair();
        }
        else if(action_num == 4)  //orb
        {
            action_sub_obj.GetComponent<ActionOrbConfig>().Open();
            junC.t_action = -1;
        }
        else if (action_num == 5)  //talk
        {
            junC.Master.nomalSE.Play("message1");
            GetComponent<ActionTalkConfig>().Open(this);
            ChangeSprite(0);
            junC.t_action = -1;
        }
    }
}
