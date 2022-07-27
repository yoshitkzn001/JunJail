using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEmergeAttacked : MonoBehaviour
{
    private const int SPEED_EMERGE = 5;

    public BattleEnemyConfig bec;
    public int enemy_num;
    public int enemy_pos;

    [SerializeField] private SpriteRenderer sr;
    private int t_emerge;
    private int t_attacked;


    public int atk = 2;
    private int attack_index;
    private int attack_speed;
    public int attack_kind;
    public bool attack_only;

    // Start is called before the first frame update
    void OnEnable()
    {
        attack_speed = bec.attack_speed_list[attack_kind];

        if(attack_kind == 0)
        {
            attack_index = bec.attack_effect_sprites.Length;
        }
        else if (attack_kind == 1)
        {
            attack_index = bec.attack_effect2_sprites.Length;
        }
        else if(attack_kind == 2)
        {
            attack_index = bec.attack_effect_sprites.Length;
        }

        if (attack_only == false)
        {
            bec.Move.MoveLocalPosZ(gameObject, gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0);
            if (attack_kind != 2)
            {
                sr.sprite = bec.emerge_sprites[0];
            }
            else
            {
                sr.sprite = bec.emerge3_sprites[0];
            }
            t_emerge = 0;
            t_attacked = -1;
        }
        else
        {
            AttackAnim(0);
            t_emerge = -1;
            t_attacked = 1;
            Attack();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_emerge >= 0)
        {
            t_emerge += 1;

            if (t_emerge % SPEED_EMERGE == 1)
            {
                int index = t_emerge / SPEED_EMERGE;
                if (index == 8)
                {
                    t_emerge = -1;
                    t_attacked = 0;
                }
                else
                {
                    if(attack_kind != 2)
                    {
                        sr.sprite = bec.emerge_sprites[index];
                    }
                    else
                    {
                        sr.sprite = bec.emerge3_sprites[index];
                    }
                }
            }
        }

        if(t_attacked >= 0)
        {
            t_attacked += 1;

            if (t_attacked % attack_speed == 1)
            {
                int index = t_attacked / attack_speed;
                if (index == attack_index)
                {
                    gameObject.SetActive(false);
                    gameObject.transform.parent = bec.enemy_attack_boxes_parent.transform;
                    bec.enemy_attack_boxes.Enqueue(gameObject);
                }
                else
                {
                    AttackAnim(index);

                    if (index == 0)
                    {
                        Attack();
                    }
                }
            }
        }
    }

    private void Attack()
    {
        if(attack_only == false)
        {
            bec.SetAttack(enemy_num);
        }

        if(enemy_pos != -1)
        {
            if (enemy_pos == bec.master.walk_box_pos)
            {
                bec.master.JunDamaged(atk, gameObject);
            }
            else
            {
                if (attack_kind == 2)
                {
                    if (!bec.master.a_pos.Contains(enemy_pos))
                    {
                        bec.master.a_pos.Add(enemy_pos);
                        bec.master.a_floor[enemy_pos].SetActive(true);
                    }
                }

                if (bec.master.tutorial == true)
                {
                    if (bec.master.tutorial_step == 0)
                    {
                        bec.master.TutorialStep();
                    }
                }
            }
        }
    }

    private void AttackAnim(int index)
    {
        if (index == 0)
        {
            gameObject.transform.parent = bec.stage.transform;

            Transform transform = gameObject.transform;
            Vector3 pos = transform.localPosition;
            float posY = pos.y + bec.attack_posYplus_list[attack_kind];

            if (attack_kind == 1)
            {
                pos.z = (posY - 18) + 15f;
            }

            pos.y = posY;
            transform.localPosition = pos;
        }

        if (attack_kind == 0)
        {
            sr.sprite = bec.attack_effect_sprites[index];
        }
        else if (attack_kind == 1)
        {
            sr.sprite = bec.attack_effect2_sprites[index];
        }
        else if (attack_kind == 2)
        {
            sr.sprite = bec.attack_effect_sprites[index];
        }
    }
}
