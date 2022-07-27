using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmonSkill : MonoBehaviour
{
    private const int STEP_ATTACK_READY = 0;
    private const int STEP_ATTACK_END = 1;
    private const int STEP_END = 2;

    private const int TIME_EFFECT_ENTER = 36;
    private const int TIME_EFFECT_EXIT = 10;
    private const int TIME_ATTACK_READY = 100;
    private const int TIME_ATTACK_END = 50;
    private const int TIME_END = 30;

    BattleEnemySingleConfig Parent;
    BattleEnemyConfig bec;
    public bool load;
    private int step;

    private int time;
    private int t_effect_exit;

    private List<GameObject> effects = new List<GameObject>();
    private List<SpriteRenderer> effects_sr = new List<SpriteRenderer>();
    private List<float> effect_pos_y = new List<float>();

    public List<int> attack_pos = new List<int>();
    public int attack_count = 2;
    public bool attack;

    // Start is called before the first frame update
    public void OnStart()
    {
        if (load == false)
        {
            Parent = gameObject.GetComponent<BattleEnemySingleConfig>();
            bec = Parent.bec;
            load = true;
        }

        int enemy_count = bec.battle_active_enemy_num.Count;
        if(enemy_count == 1)
        {
            attack_count = 4;
        }
        else if(enemy_count >= 2)
        {
            int id_count = 0;
            foreach(int num in bec.battle_active_enemy_num)
            {
                if(bec.battle_active_enemy_id[num] == 3)
                {
                    id_count += 1;
                }
            }
            if(id_count == 1)
            {
                attack_count = 3;
            }
            else
            {
                attack_count = 2;
            }
        }

        effects = new List<GameObject>();
        effects_sr = new List<SpriteRenderer>();
        effect_pos_y = new List<float>();
        attack_pos = new List<int>();
        attack = false;
        step = STEP_ATTACK_READY;
        time = TIME_ATTACK_READY;
        t_effect_exit = 0;

        bec.battle_enemy_ani[Parent.my_num].SetBool("attack_ready", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_effect_exit > 0)
        {
            t_effect_exit -= 1;
            for (int i = 0; i < attack_count; i++)
            {
                float alpha = t_effect_exit / (float)TIME_EFFECT_EXIT;
                effects_sr[i].color = new Color(1f, 1f, 1f, alpha);
                bec.Move.DeccelMoveY(t_effect_exit, TIME_EFFECT_EXIT, effect_pos_y[i], effect_pos_y[i] + 5f, effects[i]);
            }

            if(t_effect_exit == 0)
            {
                for (int i = 0; i < attack_count; i++)
                {
                    GameObject a = effects[i];
                    effects_sr[i].color = new Color(1f, 1f, 1f, 1f);
                    a.SetActive(false);
                    bec.enemy_effects.Enqueue(a);
                }
            }
        }

        if (step == STEP_ATTACK_READY)
        {
            if (time > 0)
            {
                time -= 1;

                if(time == TIME_EFFECT_ENTER)
                {
                    Set_A_Effect();
                }

                if (time == 0)
                {
                    bec.battle_enemy_ani[Parent.my_num].SetInteger("attack_step", 1);
                    SetAttackPos();
                }
                else if (time < TIME_EFFECT_ENTER)
                {
                    float alpha = 0.6f * ((TIME_EFFECT_ENTER - time) / (float)TIME_EFFECT_ENTER);
                    for (int i = 0; i < attack_count; i++)
                    {
                        effects_sr[i].color = new Color(1f, 1f, 1f, alpha);
                        bec.Move.DeccelMoveY(time, TIME_EFFECT_ENTER, effect_pos_y[i] + 10f, effect_pos_y[i], effects[i]);
                    }
                }
            }
        }
        else if(step == STEP_ATTACK_END)
        {
            if (time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    bec.battle_enemy_ani[Parent.my_num].SetBool("attack_ready", false);
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
                    EndAttack();
                }
            }
        }
    }

    private void SetAttackPos()
    {
        for (int i = 0; i < attack_count; i++)
        {
            effects_sr[i].color = new Color(1f, 1f, 1f, 1f);
            bec.Move.MoveLocalPosY(effects[i], effect_pos_y[i]);
        }
        t_effect_exit = TIME_EFFECT_EXIT;

        foreach (int pos in attack_pos)
        {
            bec.SetAttackReady_inArea(Parent.my_num, pos, 2, false, -1, -1, Parent.atk);
        }
        bec.nomalSE.Play("emerge1");
    }

    private void Set_A_Effect()
    {
        if (attack_pos.Count == 0)
        {
            for (int i = 0; i < attack_count; i++)
            {
                if(bec.a_pos_all.Count != 0)
                {
                    int index = Random.Range(0, bec.a_pos_all.Count);
                    attack_pos.Add(bec.a_pos_all[index]);
                    bec.a_pos_all.RemoveAt(index);
                }
                else
                {
                    attack_pos.Add(Random.Range(0, 6));
                }
            }
        }

        foreach (int pos in attack_pos)
        {
            GameObject a = bec.EnemyEffect();
            SpriteRenderer sr = a.GetComponent<SpriteRenderer>();
            sr.sprite = bec.DataEnemy.Emon_a_sprite;
            sr.color = new Color(1f, 1f, 1f, 0f);
            a.SetActive(true);

            float posX = -63.5f - (26f * (pos / 3));
            float posY = 27 - (17f * (pos % 3));
            bec.Move.MoveLocalPos(a, posX, posY + 10f);
            effect_pos_y.Add(posY);
            effects.Add(a);
            effects_sr.Add(sr);
        }
    }

    public void Attack()
    {
        bec.battle_enemy_ani[Parent.my_num].SetTrigger("attack");
        attack = true;
        step = STEP_ATTACK_END;
        time = TIME_ATTACK_END;
        bec.nomalSE.Play("attack2");
    }

    private void EndAttack()
    {
        bec.battle_enemy_ani[Parent.my_num].SetInteger("attack_step", 0);
        bec.battle_enemy_attacked += 1;
        this.enabled = false;
    }
}
