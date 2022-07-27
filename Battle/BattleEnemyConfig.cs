using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleEnemyConfig : MonoBehaviour
{
    private const int ENEMY_ATTACK_SPEED = 45;
    private const int TARGET_ALL = -1;
    private const int MOVE_HP = 15;
    private const int ATTACK = 4;
    private const int DEFFEND = 4;
    private const int HP = 10;

    public CriAtomSource nomalSE;
    public GameObject stage;
    public DataBattleNomalEnemy DataEnemy;
    public DataNomal DataNomal;
    public BattleEnemySingleConfig[] Enemy;
    public MasBattleConfig master;
    public FuncBar Bar;
    public FuncMove Move;
    public FuncMiniFigure Fig;
    public GameObject[] battle_enemy;
    [SerializeField] private GameObject[] battle_enemy_hpobj;
    public CanvasGroup[] battle_enemy_hp_alpha;
    public Animator[] battle_enemy_ani;
    public List<int> battle_active_enemy_id = new List<int>();
    public List<int> battle_active_enemy_num = new List<int>();
    public List<int> battle_active_enemy_level = new List<int>();

    [SerializeField] private GameObject enemy_effects_parent;
    public Queue<GameObject> enemy_effects = new Queue<GameObject>();

    [SerializeField] private GameObject[] target_buttons;
    [SerializeField] private GameObject enemy_attack_box;
    [SerializeField] private GameObject enemy_attack2_box;
    public GameObject enemy_attack_boxes_parent;
    [SerializeField] private GameObject enemy_attack2_boxes_parent;
    public Queue<GameObject> enemy_attack_boxes = new Queue<GameObject>();
    public Queue<GameObject> enemy_attack2_boxes = new Queue<GameObject>();
    public Sprite[] emerge_sprites;
    public Sprite[] emerge2_sprites;
    public Sprite[] emerge3_sprites;
    public Sprite[] attack_effect_sprites;
    public Sprite[] attack_effect2_sprites;
    public int[] attack_speed_list;
    public int[] attack_posYplus_list;

    [SerializeField] private GameObject[] enemy_name_obj;
    [SerializeField] private TextMeshProUGUI[] enemy_name;

    public List<int> a_pos_all = new List<int>() { 0, 1, 2, 3, 4, 5 };
    public Queue<GameObject> effects_damaged = new Queue<GameObject>();
    [SerializeField] private GameObject effect_damaged_parent;
    [SerializeField] private GameObject effect_damaged;

    public bool enemy_attack;
    public bool enemy_attack_end;
    private int t_enemy_attack;
    public int t_enemy_attack_end;
    public int battle_enemy_attacked;
    public bool attack_end_wait;

    private void OnEnable()
    {
        int enemy_count = battle_active_enemy_num.Count;
        for (int i = 0; i < 3; i++)
        {
            if (i < enemy_count)
            {
                if(i == 0)
                {
                    battle_enemy[0].SetActive(false);
                }
                else
                {
                    int num = battle_active_enemy_num[i];
                    battle_enemy_ani[num].SetInteger("enemy", battle_active_enemy_id[i]);
                    battle_enemy_ani[num].speed = 0;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (enemy_attack)
        {
            t_enemy_attack += 1;

            if(t_enemy_attack % ENEMY_ATTACK_SPEED == 1)
            {
                int n = t_enemy_attack / ENEMY_ATTACK_SPEED;
                int num = battle_active_enemy_num[n];

                StartAttack(num);

                if (n == battle_active_enemy_num.Count-1)
                {
                    t_enemy_attack = 0;
                    enemy_attack = false;
                    enemy_attack_end = true;
                }
            }
        }
        else if(enemy_attack_end == true)
        {
            if(t_enemy_attack_end > 0)
            {
                t_enemy_attack_end -= 1;

                if(t_enemy_attack_end == 0)
                {
                    attack_end_wait = true;
                }
            }
            else
            {
                if (battle_enemy_attacked == battle_active_enemy_num.Count)
                {
                    t_enemy_attack_end = 15;
                }
            }

            if (attack_end_wait == true)
            {
                master.EndDefence();
            }
        }
    }

    public void EnemyStart()
    {
        int enemy_count = battle_active_enemy_num.Count;
        for (int i=0; i<3; i++)
        {
            if(i < enemy_count)
            {
                int num = battle_active_enemy_num[i];
                int id = battle_active_enemy_id[num];
                int lv = battle_active_enemy_level[i];
                (int hp, int atk) = SetEnemyStatus(id, lv);
                Enemy[num].SetStart(DataEnemy.enemy_height_list[id], hp, atk, lv);
                if(lv == 4)
                {
                    enemy_name[num].text = "Lv0 " + DataEnemy.enemy_name_list[id];
                }
                else
                {
                    enemy_name[num].text = "Lv" + (lv + 1) + " " + DataEnemy.enemy_name_list[id];
                }
                battle_enemy[i].SetActive(true);
                battle_enemy_hpobj[i].SetActive(true);
                enemy_name_obj[i].SetActive(true);
            }
            else
            {
                battle_enemy[i].SetActive(false);
                battle_enemy_hpobj[i].SetActive(false);
                enemy_name_obj[i].SetActive(false);
            }
        }

        foreach (GameObject button in target_buttons)
        {
            button.SetActive(true);
        }
        if (enemy_count == 1)
        {
            Move.MoveLocalPos(battle_enemy[0], 92.5f, 22f - 14.5f);
            Move.MoveLocalPos(battle_enemy_hpobj[0], 92f, 8f);

            Move.MoveLocalPos(enemy_name_obj[0], 75f, 23f);

            foreach (GameObject button in target_buttons)
            {
                button.SetActive(false);
            }
        }
        else if(enemy_count == 2)
        {
            Move.MoveLocalPos(battle_enemy[0], 76.5f, 33f - 14.5f);
            Move.MoveLocalPos(battle_enemy_hpobj[0], 76f, 19f);
            Move.MoveLocalPos(battle_enemy[1], 108.5f, 11f - 14.5f);
            Move.MoveLocalPos(battle_enemy_hpobj[1], 108f, -3f);

            Move.MoveLocalPos(enemy_name_obj[0], 59f, 34f);
            Move.MoveLocalPos(enemy_name_obj[1], 91f, 12f);

            Move.MoveLocalPos(target_buttons[0], 76.5f, 43.5f);
            Move.MoveLocalPos(target_buttons[1], 108.5f, 21.5f);
            target_buttons[2].SetActive(false);
        }
        else if (enemy_count == 3)
        {
            Move.MoveLocalPos(battle_enemy[0], 60.5f, 44f - 14.5f);
            Move.MoveLocalPos(battle_enemy_hpobj[0], 60f, 30f);
            Move.MoveLocalPos(battle_enemy[1], 92.5f, 22f - 14.5f);
            Move.MoveLocalPos(battle_enemy_hpobj[1], 92f, 8f);
            Move.MoveLocalPos(battle_enemy[2], 124.5f, 0f - 14.5f);
            Move.MoveLocalPos(battle_enemy_hpobj[2], 124f, -14f);

            Move.MoveLocalPos(enemy_name_obj[0], 43f, 45f);
            Move.MoveLocalPos(enemy_name_obj[1], 75f, 23f);
            Move.MoveLocalPos(enemy_name_obj[2], 107f, 1f);

            Move.MoveLocalPos(target_buttons[0], 60.5f, 54.5f);
            Move.MoveLocalPos(target_buttons[1], 92.5f, 32.5f);
            Move.MoveLocalPos(target_buttons[2], 124.5f, 10.5f);
        }

        for (int i = 0; i < enemy_count; i++)
        {
            int num = battle_active_enemy_num[i];
            int id = battle_active_enemy_id[num];
            Move.MoveLocalPosPlusY(battle_enemy[num], DataEnemy.enemy_height_list[id]);
        }
    }

    public void CloseAllHP()
    {
        foreach(int num in battle_active_enemy_num)
        {
            Enemy[num].t_close_HP = MOVE_HP;
        }
    }
    public void OpenAllHP()
    {
        foreach (int num in battle_active_enemy_num)
        {
            Enemy[num].t_open_HP = MOVE_HP;
        }
    }
    public void Attacked(int target = -2)
    {
        int num;
        if(target == -2)
        {
            num = master.target_enemy;
        }
        else
        {
            num = target;
        }

        if (num == TARGET_ALL)
        {
            foreach(int n in battle_active_enemy_num)
            {
                if(Enemy[n].hp > 0)
                {
                    battle_enemy_ani[n].SetTrigger("damaged");
                    DamagedEffect(battle_enemy[n]);
                }
            }
        }
        else
        {
            if (Enemy[num].hp > 0)
            {
                battle_enemy_ani[num].SetTrigger("damaged");
                DamagedEffect(battle_enemy[num]);
            }
        }
    }
    public void Damaged(int damage_value, int target = -2)
    {
        Attacked(target);

        int num;
        if(target == -2)
        {
            num = master.target_enemy;
        }
        else
        {
            num = target;
        }

        if (num == TARGET_ALL)
        {
            foreach (int n in battle_active_enemy_num)
            {
                if (Enemy[n].hp > 0)
                {
                    Enemy[n].Damaged(damage_value);
                }
            }
        }
        else
        {
            if (Enemy[num].hp > 0)
            {
                Enemy[num].Damaged(damage_value);
            }
        }
    }

    public void DamagedEffect(GameObject targetobj)
    {
        GameObject obj;
        if(effects_damaged.Count != 0)
        {
            obj = effects_damaged.Dequeue();
        }
        else
        {
            obj = Instantiate(effect_damaged);
            obj.transform.parent = effect_damaged_parent.transform;
            obj.GetComponent<EffectDamagedConfig>().bec = this;
        }
        Move.SetSamePos(obj, targetobj, 0, 0);

        obj.SetActive(true);
    }

    private void StartAttack(int num)
    {
        GameObject enemy = battle_enemy[num];
        int id = battle_active_enemy_id[num];

        if (id == 0)
        {
            NashiSkill script = enemy.GetComponent<NashiSkill>();
            script.OnStart();
            script.enabled = true;
        }
        else if (id == 1)
        {
            EmonSnipeSkill script = enemy.GetComponent<EmonSnipeSkill>();
            script.OnStart();
            script.enabled = true;
            int shot_num;
            int enemy_count = battle_active_enemy_num.Count;
            if(enemy_count == 2)
            {
                shot_num = 3;
            }
            else
            {
                shot_num = 5 - battle_active_enemy_num.Count;
            }
            shot_num += battle_active_enemy_level[num] + 1;
            script.shot_num = shot_num;
        }
        else if(id == 2)
        {
            MokokidSkill script = enemy.GetComponent<MokokidSkill>();
            script.OnStart();
            script.enabled = true;
        }
        else if(id == 3)
        {
            EmonSkill script = enemy.GetComponent<EmonSkill>();
            script.OnStart();
            script.enabled = true;
        }
        else if (id == 4)
        {
            HatoSkill script = enemy.GetComponent<HatoSkill>();
            script.OnStart();
            script.enabled = true;
        }
    }
    public GameObject EnemyEffect()
    {
        GameObject effect;
        if(enemy_effects.Count == 0)
        {
            effect = new GameObject("Enemy_Effect");
            effect.AddComponent<SpriteRenderer>();
            effect.transform.parent = enemy_effects_parent.transform;
        }
        else
        {
            effect = enemy_effects.Dequeue();
        }

        return effect;
    }
    public void SetAttackReady_inArea(int num, int pos, int attack_kind, bool attack_only, int out_posX = -1, int out_posY = -1, int atk = 2)
    {
        GameObject box;
        bool loaded = true;
        if(enemy_attack_boxes.Count == 0)
        {
            box = Instantiate(enemy_attack_box);
            loaded = false;
        }
        else
        {
            box = enemy_attack_boxes.Dequeue();
        }

        BattleEmergeAttacked box_script = box.GetComponent<BattleEmergeAttacked>();
        box_script.atk = atk;
        box_script.enemy_num = num;
        box_script.enemy_pos = pos;
        box_script.attack_only = attack_only;
        box_script.attack_kind = attack_kind;

        if (loaded == false)
        {
            box_script.bec = this;
        }

        box.transform.parent = enemy_attack_boxes_parent.transform;
        float posX;
        float posY;
        if(pos != -1)
        {
            posX = 13.5f - (26f * (pos / 3));
            posY = 18 - (17f * (pos % 3));
        }
        else
        {
            posX = 39.5f + (26f * out_posX);
            posY = 18 - (17f * out_posY);
        }
        Move.MoveLocalPos(box, posX, posY);
        box.SetActive(true);
    }
    public void SetAttackReady_outArea(int num, int posX, int posY, int plusZ = 0)
    {
        GameObject box;
        bool loaded = true;
        if (enemy_attack2_boxes.Count == 0)
        {
            box = Instantiate(enemy_attack2_box);
            loaded = false;
        }
        else
        {
            box = enemy_attack2_boxes.Dequeue();
        }

        BattleEmergeConfig box_script = box.GetComponent<BattleEmergeConfig>();
        if (loaded == false)
        {
            box_script.bec = this;
        }
        box_script.enemy_num = num;

        box.transform.parent = enemy_attack2_boxes_parent.transform;
        float positionX = posX + 0.5f;
        float positionY = 32f - (17f * posY);
        float positionZ = (positionY - 9f) + 15.5f + plusZ;
        Move.MoveLocalPosZ(box, positionX, positionY, positionZ);
        box.SetActive(true);
    }
    public void SetAttack(int num)
    {
        int id = battle_active_enemy_id[num];

        if(id == 1)
        {
            nomalSE.Play("attack1");
            battle_enemy_ani[num].SetTrigger("attack");
        }
        
        Enemy[num].Attack(id);
    }

    private (int, int) SetEnemyStatus(int id, int lv)
    {
        int hp = 10;
        int atk = 10;

        if(id == 0)  //nashi
        {
            if(lv == 0)
            {
                hp = 15;
                atk = 8;
            }
            else if(lv == 1)
            {
                hp = 24;
                atk = 16;
            }
            else if(lv == 2)
            {
                hp = 46;
                atk = 28;
            }
            else
            {
                hp = 75;
                atk = 28;
            }

        }
        else if (id == 1)  //sniper
        {
            if (lv == 0)
            {
                hp = 10;
                atk = 4;
            }
            else if (lv == 1)
            {
                hp = 24;
                atk = 7;
            }
            else if (lv == 2)
            {
                hp = 50;
                atk = 11;
            }
            else if(lv == 3)
            {
                hp = 100;
                atk = 18;
            }
            else
            {
                hp = 15;
                atk = 1;
            }
        }
        else if (id == 2)  //mokokid
        {
            if (lv == 0)
            {
                hp = 15;
                atk = 6;
            }
            else if (lv == 1)
            {
                hp = 30;
                atk = 11;
            }
            else if (lv == 2)
            {
                hp = 50;
                atk = 18;
            }
            else
            {
                hp = 100;
                atk = 22;
            }
        }
        else if (id == 3)  //emon
        {
            if (lv == 0)
            {
                hp = 15;
                atk = 6;
            }
            else if (lv == 1)
            {
                hp = 40;
                atk = 18;
            }
            else if (lv == 2)
            {
                hp = 65;
                atk = 26;
            }
            else
            {
                hp = 130;
                atk = 26;
            }
        }
        else if (id == 4)  //hato
        {
            if (lv == 0)
            {
                hp = 13;
                atk = 8;
            }
            else if (lv == 1)
            {
                hp = 24;
                atk = 16;
            }
            else if (lv == 2)
            {
                hp = 46;
                atk = 28;
            }
            else
            {
                hp = 80;
                atk = 28;
            }
        }

        return (hp, atk);
    }
}
