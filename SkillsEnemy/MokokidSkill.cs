using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MokokidSkill : MonoBehaviour
{
    const int STEP_WALK = 1;
    const int STEP_ATTACK_READY = 2;
    const int STEP_ATTACK_OUT = 3;
    const int STEP_ATTACK_IN = 4;
    const int STEP_ATTACK_EXTEND = 5;

    const int TIME_END = 40;
    const int TIME_DAIPAN = 25;
    const int TIME_ANGRY = 50;
    const int TIME_ATTACK = 20;

    BattleEnemySingleConfig Parent;
    BattleEnemyConfig bec;
    Animator ani;
    public bool load;

    private int my_num_pos;
    private int level;

    private Vector2 first_pos;
    private int attack_kind;
    private int return_position;
    private int t_end_attack;
    private int t_daipan;
    private int time;
    private int step;
    private int attack_count;
    private int attack_extend_count;
    public int attacked_count;
    public int attacked_count_max;
    public bool attack;

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

        first_pos = new Vector2(gameObject.transform.localPosition.x - 0.5f, gameObject.transform.localPosition.y);

        int enemy_count = bec.battle_active_enemy_id.Count;
        if(enemy_count == 1)
        {
            my_num_pos = 1;
        }
        else if(enemy_count == 2)
        {
            if(Parent.my_num == 0)
            {
                my_num_pos = 0;
            }
            else if (Parent.my_num == 1)
            {
                my_num_pos = 2;
            }
        }
        else
        {
            my_num_pos = Parent.my_num;
        }

        level = Parent.level;

        attack_count = 0;
        attack_extend_count = 0;
        attack_kind = -1;
        attacked_count = 0;
        attacked_count_max = -1;
        return_position = 0;
        t_end_attack = 0;
        t_daipan = 0;
        attack = false;
        time = 0;
        step = STEP_WALK;
        ani.SetBool("attack_ready", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_daipan > 0)
        {
            t_daipan -= 1;

            if (t_daipan == 0)
            {
                ani.SetInteger("attack_step", 0);
                return_position = 1;
            }
        }
        if(return_position == 1)
        {
            Transform transform = gameObject.transform;
            Vector3 pos = transform.localPosition;

            if (pos.x < first_pos.x)
            {
                pos.x += 1f;
            }
            else
            {
                return_position = 2;
                ani.SetBool("attack_ready", false);
            }

            transform.localPosition = pos;
        }
        if(return_position == 2 & t_end_attack > 0)
        {
            t_end_attack -= 1;

            if(t_end_attack == 0)
            {
                EndAttack();
            }
        }

        if(step == STEP_WALK)
        {
            Transform transform = gameObject.transform;
            Vector3 pos = transform.localPosition;

            if (pos.x > 36f)
            {
                pos.x -= 1f;
            }
            else
            {
                step = STEP_ATTACK_READY;
                time = TIME_ANGRY;
                ani.SetInteger("attack_step", 1);
            }

            transform.localPosition = pos;
        }
        else if(step == STEP_ATTACK_READY)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 0)
                {
                    ani.SetInteger("attack_step", 2);
                    bec.SetAttackReady_inArea(Parent.my_num, -1, 1, false, 2, my_num_pos, Parent.atk);
                    bec.nomalSE.Play("emerge1");
                    attack_count = 1;
                    step = STEP_ATTACK_OUT;
                    time = TIME_ATTACK;
                }
            }
        }
        else if (step == STEP_ATTACK_OUT)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 0)
                {
                    attack_count += 1;
                    if(attack_count != 4)
                    {
                        bec.SetAttackReady_inArea(Parent.my_num, -1, 1, false, 3 - attack_count, my_num_pos, Parent.atk);
                    }
                    else
                    {
                        bec.SetAttackReady_inArea(Parent.my_num, my_num_pos, 1, false, -1, -1, Parent.atk);
                    }
                    bec.nomalSE.Play("emerge1");

                    time = TIME_ATTACK;
                    if (attack_count == 4)
                    {
                        step = STEP_ATTACK_IN;
                    }
                }
            }
        }
        else if(step == STEP_ATTACK_IN)
        {
            if (time > 0)
            {
                time -= 1;

                if(time == 0)
                {
                    SelectAttackPos();
                }
            }
        }
        else if (step == STEP_ATTACK_EXTEND)
        {
            if (time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    SelectExtendAttackPos();
                }
            }
        }
    }

    public void Attack()
    {
        attack = true;
        ani.SetInteger("attack_step", 3);
        t_daipan = TIME_DAIPAN;
    }

    private void SelectAttackPos()
    {
        if (attack_count == 4)
        {
            int jun_pos = bec.master.walk_box_pos;
            if (jun_pos == 0 | jun_pos == 1 | jun_pos == 2)
            {
                attack_kind = my_num_pos;
                attacked_count_max = attack_count + 2;
            }
            else
            {
                bec.SetAttackReady_inArea(Parent.my_num, my_num_pos + 3, 1, false, -1, -1, Parent.atk);
                time = TIME_ATTACK;
                bec.nomalSE.Play("emerge1");
            }
            attack_count += 1;
        }
        else if(attack_count == 5)
        {
            if(attack_kind == -1)
            {
                attack_kind = my_num_pos;
                attacked_count_max = attack_count + 2;
                attack_count += 1;
            }
        }

        if(attack_kind != -1)
        {
            int posX = (attack_count - 5) * 3;

            if (attack_kind == 0 | attack_kind == 3)
            {
                bec.SetAttackReady_inArea(Parent.my_num, attack_kind / 3 + 1 + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                if (attack_kind != 3)
                {
                    time = TIME_ATTACK;
                    attack_kind += 3;
                }
                else
                {
                    if (Parent.level > 0)
                    {
                        step = STEP_ATTACK_EXTEND;
                        time = TIME_ATTACK;
                    }
                }
            }
            else if(attack_kind == 1)
            {
                bec.SetAttackReady_inArea(Parent.my_num, 0 + posX, 1, false, -1, -1, Parent.atk);
                bec.SetAttackReady_inArea(Parent.my_num, 2 + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");

                if (Parent.level > 0)
                {
                    step = STEP_ATTACK_EXTEND;
                    time = TIME_ATTACK;
                }
            }
            else if(attack_kind == 2 | attack_kind == 5)
            {
                bec.SetAttackReady_inArea(Parent.my_num, (2 - (attack_kind + 1) / 3) + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                if (attack_kind != 5)
                {
                    time = TIME_ATTACK;
                    attack_kind += 3;
                }
                else
                {
                    if (Parent.level > 0)
                    {
                        step = STEP_ATTACK_EXTEND;
                        time = TIME_ATTACK;
                    }
                }
            }
        }
    }

    private void SelectExtendAttackPos()
    {
        int posX = ((attack_count - 5) * 3 + 3) % 6;

        if (attack_extend_count == 0)
        {
            if (attack_kind == 1)
            {
                if(attack_extend_count == 0)
                {
                    if (level == 1)
                    {
                        bec.SetAttackReady_inArea(Parent.my_num, Random.Range(0, 2) * 2 + posX, 1, false, -1, -1, Parent.atk);
                        attack_extend_count += 1;
                    }
                    else if (level == 2 | level == 3)
                    {
                        bec.SetAttackReady_inArea(Parent.my_num, posX, 1, false, -1, -1, Parent.atk);
                        bec.SetAttackReady_inArea(Parent.my_num, 2 + posX, 1, false, -1, -1, Parent.atk);
                        attack_extend_count += 2;
                    }
                    bec.nomalSE.Play("emerge1");
                }
            }
            else if (attack_kind == 3)
            {
                bec.SetAttackReady_inArea(Parent.my_num, 2 + posX, 1, false, -1, -1, Parent.atk);
                attack_extend_count += 1;
                bec.nomalSE.Play("emerge1");
            }
            else if (attack_kind == 5)
            {
                bec.SetAttackReady_inArea(Parent.my_num, posX, 1, false, -1, -1, Parent.atk);
                attack_extend_count += 1;
                bec.nomalSE.Play("emerge1");
            }
        }
        else if(attack_extend_count == 1)
        {
            if (attack_kind == 3)
            {
                bec.SetAttackReady_inArea(Parent.my_num, 1 + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                attack_extend_count += 1;
            }
            else if (attack_kind == 5)
            {
                bec.SetAttackReady_inArea(Parent.my_num, 1 + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                attack_extend_count += 1;
            }
        }
        else if(attack_extend_count == 2)
        {
            if(attack_kind == 1)
            {
                bec.SetAttackReady_inArea(Parent.my_num, 1 + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                attack_extend_count += 1;
            }
            else if (attack_kind == 3)
            {
                bec.SetAttackReady_inArea(Parent.my_num, posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                attack_extend_count += 1;
            }
            else if (attack_kind == 5)
            {
                bec.SetAttackReady_inArea(Parent.my_num, 2 + posX, 1, false, -1, -1, Parent.atk);
                bec.nomalSE.Play("emerge1");
                attack_extend_count += 1;
            }
        }

        if(attack_extend_count != level)
        {
            time = TIME_ATTACK;
        }
    }

    public void AttackedCount()
    {
        bec.nomalSE.Play("attack2");
        attacked_count += 1;

        if(level == 0)
        {
            if (attacked_count == attacked_count_max)
            {
                t_end_attack = TIME_END;
            }
        }
        
        if(step == STEP_ATTACK_EXTEND)
        {
            if (level == 1)
            {
                if (attacked_count == attacked_count_max + 1)
                {
                    t_end_attack = TIME_END;
                }
            }
            else if (level == 2)
            {
                if (attacked_count == attacked_count_max + 2)
                {
                    t_end_attack = TIME_END;
                }
            }
            else
            {
                if (attacked_count == attacked_count_max + 3)
                {
                    t_end_attack = TIME_END;
                }
            }
        }
    }

    private void EndAttack()
    {
        bec.battle_enemy_attacked += 1;
        this.enabled = false;
    }
}
