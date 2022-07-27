using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatoSkill : MonoBehaviour
{
    const int STEP_WALK = 0;
    const int TIME_WALK = 35;
    const int STEP_MOVE = 1;
    const int TIME_MOVE = 41;
    const int STEP_RETURN = 2;

    const int TIME_SHAKE = 20;

    BattleEnemySingleConfig Parent;
    BattleEnemyConfig bec;
    Animator ani;
    public bool load;

    GameObject attack_effect;
    SpriteRenderer attack_effect_sr;

    private int attack_count_max;
    private int attack_count;
    private Vector2 first_pos;
    int step;
    int time;
    int target_pos;
    int t_shake;

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

        int lv = Parent.level;
        if (lv == 0)
        {
            attack_count_max = 1;
        }
        else if (lv == 1)
        {
            attack_count_max = 2;
        }
        else if (lv == 2)
        {
            attack_count_max = 3;
        }
        else
        {
            attack_count_max = 5;
        }

        first_pos = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
        attack_count = 0;
        attack = false;
        target_pos = Random.Range(0, 3);
        t_shake = 0;
        time = TIME_WALK;
        step = STEP_WALK;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_shake > 0)
        {
            if (t_shake > 0)
            {
                t_shake -= 1;

                if (t_shake == TIME_SHAKE - 6)
                {
                    bec.Move.MoveLocalPosPlusY(bec.stage, 4f);
                }
                else if (t_shake == TIME_SHAKE - 12)
                {
                    bec.Move.MoveLocalPosPlusY(bec.stage, -1f);
                }

            }
        }

        if(step == STEP_WALK)
        {
            if(time > 0)
            {
                time -= 1;

                bec.Move.DeccelMoveY(time, TIME_WALK, first_pos.y, 5 + (2 - target_pos) * 17, gameObject);
                Parent.SetPositionZ(-5);

                if(time == 25)
                {
                    ani.SetBool("attack_ready", true);
                }

                if (time == 0)
                {
                    bec.SetAttackReady_outArea(Parent.my_num, (int)gameObject.transform.localPosition.x - 5, target_pos, -5);
                    bec.nomalSE.Play("emerge2");
                    step = STEP_MOVE;
                    time = TIME_MOVE;
                }
            }
        }
        else if(step == STEP_MOVE)
        {
            if(time > 0)
            {
                time -= 1;

                bec.Move.AccelMoveX(time, TIME_MOVE, first_pos.x, -75.5f, gameObject);

                if(time == 0)
                {
                    step = STEP_RETURN;
                }
            }
        }
        else if (step == STEP_RETURN)
        {
            if(time == 0)
            {
                Transform transform = gameObject.transform;
                Vector3 pos = transform.localPosition;
                if(pos.x > -283f)
                {
                    pos.x -= 9f;
                    transform.localPosition = pos;
                }
                else
                {
                    if (attack_count != attack_count_max)
                    {
                        target_pos = Random.Range(0, 3);
                        pos.y = 5 + (2 - target_pos) * 17;
                    }
                    else
                    {
                        pos.y = first_pos.y;
                    }

                    pos.x = 282.5f;
                    time = 30;
                }
                transform.localPosition = pos;
            }
            
            if(time > 0)
            {
                time -= 1;
                bec.Move.DeccelMoveX(time, 30, 282.5f, first_pos.x, gameObject);
                Parent.SetPositionZ(-5);

                attack_effect_sr.color = new Color(1f, 1f, 1f, time / 30f);

                if (time == 0)
                {
                    attack_effect_sr.color = new Color(1f, 1f, 1f, 1f);
                    attack_effect.SetActive(false);
                    bec.enemy_effects.Enqueue(attack_effect);

                    if (attack_count == attack_count_max)
                    {
                        AttackEnd();
                    }
                    else
                    {
                        ani.SetInteger("attack_step", 2);
                        bec.SetAttackReady_outArea(Parent.my_num, (int)gameObject.transform.localPosition.x - 5, target_pos, -5);
                        bec.nomalSE.Play("emerge2");
                        step = STEP_MOVE;
                        time = TIME_MOVE;
                        attack = false;
                    }
                }
            }
        }
    }

    private void AttackEnd()
    {
        ani.SetBool("attack_ready", false);
        ani.SetInteger("attack_step", 0);
        bec.battle_enemy_attacked += 1;
        this.enabled = false;
    }

    public void Attack()
    {
        if(attack == false)
        {
            GameObject latter = bec.EnemyEffect();
            SpriteRenderer sr = latter.GetComponent<SpriteRenderer>();
            sr.sprite = bec.DataEnemy.Hato_letter_sprite;
            sr.color = new Color(1f, 1f, 1f, 1f);
            latter.SetActive(true);

            Transform stransform = latter.transform;
            Vector3 spos = transform.localPosition;
            spos.x = -76.5f;
            spos.y = -4 + (2 - target_pos) * 17;
            spos.z = 1 + (2 - target_pos) * 17;
            stransform.localPosition = spos;

            attack_effect = latter;
            attack_effect_sr = sr;

            attack = true;
            attack_count += 1;

            ani.SetInteger("attack_step", 1);
            bec.SetAttackReady_inArea(Parent.my_num, target_pos, 0, true, -1, -1, Parent.atk);
            bec.SetAttackReady_inArea(Parent.my_num, target_pos + 3, 0, true, -1, -1, Parent.atk);
            bec.nomalSE.Play("attack3");

            t_shake = TIME_SHAKE;
            bec.Move.MoveLocalPosPlusY(bec.stage, -3f);
        }
    }
}
