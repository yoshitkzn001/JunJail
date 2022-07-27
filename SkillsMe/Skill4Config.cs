using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4Config : MonoBehaviour
{
    const int STEP_WAIT = 0;
    const int STEP_DRAW_WAIT = 1;
    const int STEP_END = 2;
    const int TIME_DRAW_WAIT = 60;
    const int TIME_END = 5;

    [SerializeField] FuncMove Move;
    [SerializeField] RomDecidedConfig Romdecided;
    [SerializeField] MasBattleConfig Master;
    [SerializeField] Animator ani_jun;

    int step = 0;
    int time = 0;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(step == STEP_WAIT)
        {
            if (Master.step == 8)
            {
                step = STEP_DRAW_WAIT;
                time = TIME_DRAW_WAIT;
            }
        }
        else if(step == STEP_DRAW_WAIT)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 55)
                {
                    Master.voiceSE.Play("voice_skill4");
                }

                if (time == 0)
                {
                    SelectRom();
                    ani_jun.SetInteger("step", 1);
                    Master.step = 6;
                    time = TIME_END;
                    step = STEP_END;
                }
            }
        }
        else if(step == STEP_END)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 0)
                {
                    End();
                }
            }
        }
    }

    private void SelectRom()
    {
        List<int> myskill = new List<int>(Status.myskill);
        myskill.Remove(3);
        if (myskill.Contains(4))
        {
            int use_sp = Status.skills_sp[11 + Status.myskill_level[4]];
            if(Status.sp < use_sp)
            {
                myskill.Remove(4);
            }
        }

        int skill = myskill[Random.Range(0, myskill.Count)];
        int level = Status.myskill_level[skill];
        Move.MoveLocalPos(Romdecided.gameObject, -68, 68);
        Romdecided.SetRomDecided(skill, level, 68, 78, -92, true);
        Master.skill_selected = skill;
        Master.skill_level = level;

        if (skill != 0)
        {
            Master.target_enemy = Status.skill_target[skill];
        }

        if (skill == 2)
        {
            Master.skill3_okureiman.SetActive(true);
        }
    }

    private void End()
    {
        gameObject.SetActive(false);
        ani_jun.SetInteger("skill", 0);
        ani_jun.SetInteger("step", 0);
        step = STEP_WAIT;
    }
}
