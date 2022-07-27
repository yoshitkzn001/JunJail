using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill5Config : MonoBehaviour
{
    const int STEP_WAIT = 0;
    const int STEP_LIFEUP_WAIT = 1;
    const int TIME_LIFEUP_WAIT = 120;
    const int STEP_STAY = 2;
    const int TIME_STAY = 120;

    [SerializeField] MasBattleConfig Master;
    [SerializeField] Animator ani_jun;

    int step = 0;
    int time = 0;

    int[] lifeup_point = new int[3] { 20, 40, 60 };

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (step == STEP_WAIT)
        {
            if (Master.step == 8)
            {
                step = STEP_LIFEUP_WAIT;
                time = TIME_LIFEUP_WAIT;
            }
        }
        else if(step == STEP_LIFEUP_WAIT)
        {
            if (time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    Master.nomalSE.Play("select5");
                    Master.ItemEffect(1, Master.jun_effect, Master.jun_effect_ani, 9.5f, -15f);
                    Master.ChangeHPdisplay(lifeup_point[Master.skill_level - 1], 55, 9f, -6f);
                    step = STEP_STAY;
                    time = TIME_STAY;
                }
            }
        }
        else if(step == STEP_STAY)
        {
            if (time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    End();
                }
            }
        }
    }

    private void End()
    {
        gameObject.SetActive(false);
        ani_jun.SetInteger("skill", 0);
        step = STEP_WAIT;
        Master.EndAttack();
    }
}
