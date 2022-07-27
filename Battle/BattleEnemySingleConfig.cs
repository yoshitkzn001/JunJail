using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemySingleConfig : MonoBehaviour
{
    private const int MOVE_HP = 15;
    private const int DAMAGED_TIME = 75;
    private const int DAMAGED_SHAKE_TIME = 6;
    private const int DAMAGED_DEC_TIME = 15;

    public int hp;
    private float max_hp;
    private int dec_hp;
    public int level;
    public int atk;
    public float height;

    public BattleEnemyConfig bec;
    public GameObject shadow;
    [SerializeField] private GameObject hp_bar;
    [SerializeField] private GameObject fig_damaged;
    [SerializeField] private GameObject[] fig_damaged_obj;
    [SerializeField] private Image[] fig_damaged_img;
    public int my_num;
    private RectTransform hp_bar_transform;

    public int t_close_HP;
    public int t_open_HP;
    public int t_damaged;

    private bool load;

    // Start is called before the first frame update
    void Awake()
    {
        hp_bar_transform = hp_bar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(load == false)
        {
            SetPositionZ();
            load = true;
        }

        if (t_close_HP > 0)
        {
            CloseHP();
        }
        if (t_open_HP > 0)
        {
            OpenHP();
        }

        if(t_damaged > 0)
        {
            t_damaged -= 1;

            //揺れ
            if(t_damaged >= DAMAGED_TIME - DAMAGED_SHAKE_TIME)
            {
                if(t_damaged == DAMAGED_TIME)
                {
                    fig_damaged.SetActive(true);
                }
                int t = DAMAGED_SHAKE_TIME - (DAMAGED_TIME - t_damaged);
                bec.Move.DeccelShake(t, DAMAGED_SHAKE_TIME, 4, 0, 2, 3, fig_damaged);
            }

            //バー減らし
            if(t_damaged >= DAMAGED_TIME - DAMAGED_DEC_TIME)
            {
                int t = DAMAGED_DEC_TIME - (DAMAGED_TIME - t_damaged);
                if(t % 2 == 0)
                {
                    bec.Bar.DecBar(t, DAMAGED_DEC_TIME, hp, dec_hp, max_hp, 26, hp_bar_transform);
                }
            }

            if(t_damaged == MOVE_HP)
            {
                t_close_HP = MOVE_HP;
                if(hp > 0)
                {
                    bec.battle_enemy_ani[my_num].SetTrigger("alive");
                }
                else
                {
                    bec.battle_enemy_ani[my_num].SetTrigger("dead");
                    bec.battle_active_enemy_num.Remove(my_num);
                }
            }
        }
    }

    public void SetStart(float my_height, int my_hp, int my_atk, int my_lv)
    {
        level = my_lv;
        hp = my_hp + Random.Range(0, 2);
        max_hp = hp;
        atk = my_atk;
        bec.Bar.SetBar(hp, (int)max_hp, 26, hp_bar);
        height = my_height;
        bec.Move.MoveLocalPosY(shadow, -my_height);
        load = false;
    }

    public void SetPositionZ(float pulsZ = 0)
    {
        Transform transform = shadow.transform;
        Vector3 pos = transform.position;
        Transform mytransform = gameObject.transform;
        Vector3 mypos = mytransform.localPosition;
        mypos.z = (pos.y - bec.stage.transform.position.y) + 15.5f + pulsZ;
        mytransform.localPosition = mypos;
    }

    private void OpenHP()
    {
        t_open_HP -= 1;
        bec.battle_enemy_hp_alpha[my_num].alpha = (MOVE_HP - t_open_HP) / (float)MOVE_HP;
    }
    private void CloseHP()
    {
        t_close_HP -= 1;
        bec.battle_enemy_hp_alpha[my_num].alpha = t_close_HP / (float)MOVE_HP;

        if(t_close_HP == 0)
        {
            fig_damaged.SetActive(false);
        }
    }
    public void Damaged(int damage_value)
    {
        hp -= damage_value;

        bec.battle_enemy_hp_alpha[my_num].alpha = 1f;
        fig_damaged.SetActive(false);

        bec.Fig.ChangeFigureCenter(damage_value, 8, 0, fig_damaged_img, fig_damaged_obj, bec.DataNomal.figure_damaged);

        if (hp < 0)
        {
            damage_value += hp;
            hp = 0;
        }

        dec_hp = damage_value;
        t_damaged = DAMAGED_TIME+1;
    }
    public void Attack(int id)
    {
        if(id == 0)
        {
            NashiSkill script = gameObject.GetComponent<NashiSkill>();
            if(script.attack == false)
            {
                script.Attack();
            }
        }
        else if (id == 2)
        {
            MokokidSkill script = gameObject.GetComponent<MokokidSkill>();
            if (script.attack == false)
            {
                script.Attack();
            }
            script.AttackedCount();
        }
        else if(id == 3)
        {
            EmonSkill script = gameObject.GetComponent<EmonSkill>();
            if (script.attack == false)
            {
                script.Attack();
            }
        }
        else if (id == 4)
        {
            HatoSkill script = gameObject.GetComponent<HatoSkill>();
            if (script.attack == false)
            {
                script.Attack();
            }
        }
    }
}
