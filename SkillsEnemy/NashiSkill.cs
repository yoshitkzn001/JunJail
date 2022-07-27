using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NashiSkill : MonoBehaviour
{
    const int STEP_WALK = 1;
    const int STEP_ATTACK_READY = 2;
    const int STEP_ATTACK_JUMP = 3;
    const int STEP_ATTACK = 4;
    const int STEP_SLIDE = 5;
    const int STEP_SLIDE_RETURN = 6;

    const int TIME_SLIDE = 25;
    const int TIME_SLIDE_RETURN = 35;
    const int TIME_ATTACK = 25;
    const int TIME_ATTACK_READY = 10;
    const int TIME_JUMP = 28;

    BattleEnemySingleConfig Parent;
    BattleEnemyConfig bec;
    Animator ani;
    public bool load;

    private Vector2 first_pos;
    public bool attack;
    private int attack_pos;
    private int time;
    private int step;
    private int attack_count_max;
    private int attack_count;

    // Start is called before the first frame update
    public void OnStart()
    {
        if (load == false)
        {
            Parent = gameObject.GetComponent<BattleEnemySingleConfig>();
            bec = Parent.bec;
            ani = bec.battle_enemy_ani[Parent.my_num];
            load = true;
        }

        first_pos = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);

        int lv = Parent.level;
        if (lv == 0)
        {
            attack_count_max = 1;
        }
        else if (lv == 1)
        {
            attack_count_max = 1;
        }
        else if (lv == 2)
        {
            attack_count_max = 2;
        }
        else
        {
            attack_count_max = 3;
        }

        attack_count = 0;
        time = 0;
        step = STEP_WALK;
        attack = false;
        ani.SetBool("attack_ready", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(step == STEP_WALK)
        {
            bool doneX = false;
            bool doneY = false;

            Transform transform = gameObject.transform;
            Vector3 pos = transform.localPosition;

            if(pos.x > -17f)
            {
                pos.x -= 1f;
            }
            else
            {
                doneX = true;
            }

            if(pos.y >= 20.5f)
            {
                pos.y -= 1;
            }
            else if(pos.y > 19.5f)
            {
                doneY = true;
            }
            else
            {
                pos.y += 1;
            }

            if(doneX == true & doneY == true)
            {
                ani.SetInteger("attack_step", 1);
                time = TIME_ATTACK_READY;
                step = STEP_ATTACK_READY;
            }
            else
            {
                transform.localPosition = pos;
            }

            Parent.SetPositionZ();
        }

        else if(step == STEP_ATTACK_READY)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 0)
                {
                    Jump();
                }
            }
        }

        else if (step == STEP_ATTACK_JUMP)
        {
            if(time > 0)
            {
                time -= 1;

                if(time % 4 == 0)
                {
                    bec.Move.DeccelMoveY(time, TIME_JUMP, 45f, 60f, gameObject);
                }
            }
        }

        else if(step == STEP_ATTACK)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == TIME_ATTACK - 6)
                {
                    bec.Move.MoveLocalPosPlusY(bec.stage, 6f);
                }
                else if(time == TIME_ATTACK - 12)
                {
                    bec.Move.MoveLocalPosPlusY(bec.stage, -2f);
                }
                else if(time == 0)
                {
                    attack_count += 1;

                    if(attack_count == attack_count_max)
                    {
                        step = STEP_SLIDE_RETURN;
                        time = TIME_SLIDE_RETURN + 1;
                    }
                    else
                    {
                        step = STEP_SLIDE;
                        time = TIME_SLIDE + 1;
                        attack = false;
                    }
                    ani.SetInteger("attack_step", 3);
                    bec.Move.MoveLocalPosZ(Parent.shadow, -1f, -Parent.height, 0.1f);
                    Parent.SetPositionZ();
                }

            }
        }

        if(step == STEP_SLIDE_RETURN)
        {
            if (time > 0)
            {
                time -= 1;

                if (time % 2 == 0)
                {
                    bec.Move.DeccelMoveX(time, TIME_SLIDE_RETURN, -75.5f, first_pos.x, gameObject);
                    bec.Move.DeccelMoveY(time, TIME_SLIDE_RETURN, 20f - attack_pos * 17f + 8f, first_pos.y, gameObject);

                    Parent.SetPositionZ();

                    if (time == 0)
                    {
                        ani.SetInteger("attack_step", 0);
                        EndAttack();
                    }
                }
            }
        }

        if(step == STEP_SLIDE)
        {
            if (time > 0)
            {
                time -= 1;

                bec.Move.DeccelMoveX(time, TIME_SLIDE_RETURN, -75.5f, -17f, gameObject);
                bec.Move.DeccelMoveY(time, TIME_SLIDE_RETURN, 20f - attack_pos * 17f + 8f, 20f, gameObject);

                Parent.SetPositionZ();

                if (time == 0)
                {
                    ani.SetInteger("attack_step", 1);
                    time = TIME_ATTACK_READY;
                    step = STEP_ATTACK_READY;
                }
            }
        }
    }

    private void Jump()
    {
        Parent.shadow.transform.parent = bec.stage.transform;

        ani.SetInteger("attack_step", 2);
        step = STEP_ATTACK_JUMP;
        time = TIME_JUMP;
        bec.Move.MoveLocalPosPlusY(gameObject, 25f);

        attack_pos = Random.Range(0, 2);
        for (int y = attack_pos; y < attack_pos + 2; y++)
        {
            bec.SetAttackReady_outArea(Parent.my_num, (int)gameObject.transform.localPosition.x - 15, y);
        }
        bec.nomalSE.Play("emerge2");
    }

    public void Attack()
    {
        time = 0;
        step = STEP_ATTACK;
        time = TIME_ATTACK;
        bec.SetAttackReady_inArea(Parent.my_num, attack_pos, 0, true, -1, -1, Parent.atk);
        bec.SetAttackReady_inArea(Parent.my_num, attack_pos + 3, 0, true, -1, -1, Parent.atk);
        bec.SetAttackReady_inArea(Parent.my_num, (attack_pos + 1), 0, true, -1, -1, Parent.atk);
        bec.SetAttackReady_inArea(Parent.my_num, (attack_pos + 1) + 3, 0, true, -1, -1, Parent.atk);
        attack = true;
        bec.Move.MoveLocalPos(gameObject, -75.5f, 20f - attack_pos * 17f);
        bec.Move.MoveLocalPosPlusY(bec.stage, -4f);

        Parent.shadow.transform.parent = gameObject.transform;
        bec.Move.MoveLocalPosZ(Parent.shadow, -1f, -6.5f, 0.1f);
        Parent.SetPositionZ();
        bec.nomalSE.Play("attack3");
    }

    private void EndAttack()
    {
        ani.SetBool("attack_ready", false);
        bec.battle_enemy_attacked += 1;
        this.enabled = false;
    }
}
