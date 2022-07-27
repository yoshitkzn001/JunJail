using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmonSnipeSkill : MonoBehaviour
{
    private const int SCOPE_SET_TIME = 5;
    private const int SCOPE_DEL_TIME = 15;
    private const int SCOPE_TIME = 70;
    private const int SCOPE_LATE_TIME = 40;
    private const int STEP_SIT = 0;
    private const int STEP_ATTACK = 1;
    private const int STEP_END = 2;

    BattleEnemySingleConfig Parent;
    BattleEnemyConfig bec;
    public bool load;
    private int step;

    private int t_sit;
    private int t_end;
    private int t_attack_level4;

    private float shot_pos_x;
    private float shot_pos_y;
    public int shot_num;
    List<GameObject> scopes = new List<GameObject>();
    List<SpriteRenderer> scopes_sr = new List<SpriteRenderer>();
    List<int> t_scopes = new List<int>();
    List<int> scope_pos = new List<int>();
    List<bool> scopes_done = new List<bool>();
    List<Vector2> scopes_target = new List<Vector2>();

    public bool tutorial;

    public void OnStart()
    {
        if(load == false)
        {
            Parent = gameObject.GetComponent<BattleEnemySingleConfig>();
            bec = Parent.bec;
            load = true;
        }

        scope_pos = new List<int>();
        scopes = new List<GameObject>();
        scopes_sr = new List<SpriteRenderer>();
        t_scopes = new List<int>();
        scopes_done = new List<bool>();
        scopes_target = new List<Vector2>();
        step = STEP_SIT;
        Parent = gameObject.GetComponent<BattleEnemySingleConfig>();
        t_sit = 60;

        t_attack_level4 = 0;
        bec.battle_enemy_ani[Parent.my_num].SetBool("attack_ready", true);
    }


    void FixedUpdate()
    {
        if(step == STEP_SIT)
        {
            if(t_sit > 0)
            {
                t_sit -= 1;

                if(t_sit == 0)
                {
                    step = STEP_ATTACK;
                    SetScope();
                }
            }
        }
        else if (step == STEP_ATTACK)
        {
            int count = scopes.Count;
            for (int i = 0; i < count; i++)
            {
                if(scopes_done[i] == false)
                {
                    t_scopes[i] -= 1;
                    int t_scope_now = t_scopes[i];
                    GameObject scope = scopes[i];
                    if (t_scope_now >= 0)
                    {
                        if(t_scope_now >= SCOPE_TIME - SCOPE_SET_TIME)
                        {
                            int t_scope_set = SCOPE_TIME - t_scope_now;
                            float alpha = t_scope_set / (float)SCOPE_SET_TIME;
                            scopes_sr[i].color = new Color(0f, 0f, 0f, alpha);
                        }

                        bec.Move.AccelMoveX(t_scope_now, SCOPE_TIME, shot_pos_x, scopes_target[i].x, scope);
                        bec.Move.AccelMoveY(t_scope_now, SCOPE_TIME, shot_pos_y, scopes_target[i].y, scope);

                        if (t_scope_now == SCOPE_TIME - SCOPE_LATE_TIME)
                        {
                            if(tutorial == false)
                            {
                                if (count < shot_num)
                                {
                                    SetScope();
                                }
                            }
                            else
                            {
                                SetScope();
                            }
                        }

                        if(t_scope_now == 0)
                        {
                            bec.SetAttackReady_inArea(Parent.my_num, scope_pos[i], 0, false, -1, -1, Parent.atk);
                            bec.nomalSE.Play("emerge1");
                        }
                    }
                    else
                    {
                        int t_scope_del = SCOPE_DEL_TIME + (t_scope_now+1);
                        if (t_scope_del == 0)
                        {
                            scopes_sr[i].color = new Color(1f, 1f, 1f, 1f);
                            scope.SetActive(false);
                            bec.enemy_effects.Enqueue(scope);
                            scopes_done[i] = true;

                            if(tutorial == false)
                            {
                                if (i == shot_num - 1)
                                {
                                    t_end = 51;
                                    step = STEP_END;
                                }
                                else
                                {
                                    if (Parent.level == 3)
                                    {
                                        t_attack_level4 = 20;
                                    }
                                }
                            }
                            else
                            {
                                if(bec.master.tutorial_step == 3)
                                {
                                    t_end = 51;
                                    step = STEP_END;
                                }
                            }
                        }
                        else
                        {
                            float alpha = t_scope_del / (float)SCOPE_DEL_TIME;
                            scopes_sr[i].color = new Color(1f, 1f, 1f, alpha);
                        }
                    }
                }
            }

            if(t_attack_level4 > 0)
            {
                t_attack_level4 -= 1;

                if(t_attack_level4 == 0)
                {
                    SetScope();
                }
            }
        }
        else if(step == STEP_END)
        {
            if(t_end > 0)
            {
                t_end -= 1;

                if(t_end == 0)
                {
                    EndAttack();
                }
            }
        }
    }

    private void SetScope()
    {
        GameObject scope = bec.EnemyEffect();
        SpriteRenderer sr = scope.GetComponent<SpriteRenderer>();
        sr.sprite = bec.DataEnemy.EmonSnipe_scope_sprite;
        sr.color = new Color(0f, 0f, 0f, 0f);
        scope.SetActive(true);

        Transform mytransform = gameObject.transform;
        Vector3 mypos = mytransform.localPosition;
        Transform stransform = scope.transform;
        Vector3 spos = transform.localPosition;
        shot_pos_x = mypos.x - 25f;
        shot_pos_y = mypos.y - 9.5f;
        spos.x = shot_pos_x;
        spos.y = shot_pos_y;
        spos.z = -1;
        stransform.localPosition = spos;

        scopes.Add(scope);
        scopes_sr.Add(sr);
        if(Parent.level != 3)
        {
            if(tutorial == true)
            {
                t_scopes.Add(150);
            }
            else
            {
                t_scopes.Add(SCOPE_TIME);
            }
        }
        else
        {
            t_scopes.Add(1);
        }
        scopes_done.Add(false);

        int pos;
        if (Parent.level != 3)
        {
            if (bec.battle_active_enemy_num.Count == 1)
            {
                pos = bec.master.walk_box_pos;
            }
            else
            {
                pos = Random.Range(0, 6);
            }
        }
        else
        {
            pos = bec.master.walk_box_pos;
        }

        scope_pos.Add(pos);
        float posX = -63.5f - (26f * (pos / 3));
        float posY = 27 - (17f * (pos % 3));
        scopes_target.Add(new Vector2(posX, posY));
    }

    private void EndAttack()
    {
        bec.battle_enemy_ani[Parent.my_num].SetBool("attack_ready", false);
        bec.battle_enemy_attacked += 1;
        this.enabled = false;
    }
}
