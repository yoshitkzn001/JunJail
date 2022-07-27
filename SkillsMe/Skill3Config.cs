using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill3Config : MonoBehaviour
{
    const int BAR_MAX = 57;
    const int GAME_END_TIME = 45;
    const int STEP_WAIT = 0;
    const int STEP_OPEN = 1;
    const int STEP_GAME = 2;
    const int STEP_GAME_END = 3;
    const int STEP_GAME_END_WAIT = 4;
    const int BUTTON_ANIM = 12;

    [SerializeField] BattleEnemyConfig bec;
    [SerializeField] MasBattleConfig Master;
    [SerializeField] FuncMove Move;
    [SerializeField] FuncBar Bar;
    [SerializeField] GameObject[] button;
    [SerializeField] Image[] button_img;
    [SerializeField] GameObject bar_time;
    [SerializeField] Animator ani_jun;
    [SerializeField] Image renda_sr;
    [SerializeField] Sprite[] renda_sprite;
    [SerializeField] GameObject okureiman;
    [SerializeField] GameObject stone;
    [SerializeField] SpriteRenderer stone_sr;

    private int[] attack_time_level = new int[3]{114, 171, 228};
    private List<float> attack_damaged = new List<float>() { 0, 0, 0 };
    private int attack_target_range;

    private int step;
    private int time;
    private int t_button_right;
    private int t_button_left;
    private int t_attack_time;

    private float damage_value = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        SetStart();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Bar.SetBarInt(1, 1, BAR_MAX, bar_time);
    }

    private void FixedUpdate()
    {
        if (t_button_left > 0)
        {
            t_button_left -= 1;
            ButtonEffectLeft(t_button_left);
        }
        if (t_button_right > 0)
        {
            t_button_right -= 1;
            ButtonEffectRight(t_button_right);
        }
        if (t_attack_time > 0)
        {
            t_attack_time -= 1;

            if (t_attack_time % 4 == 0)
            {
                Bar.SetBarInt(t_attack_time / (Master.skill_level + 1), BAR_MAX, BAR_MAX, bar_time);

                if (t_attack_time == 0)
                {
                    step = STEP_GAME_END;
                    time = GAME_END_TIME + 1;
                }
            }
        }

        if (step == STEP_WAIT)
        {
            if (Master.step == 8) // STEP_SELECT_ATTACK_DO を変えたらここも変える
            {
                damage_value = 0.5f * ( 1f + ((Master.skill_atk - 8f) / 16f));
                attack_target_range = bec.battle_active_enemy_num.Count;
                attack_damaged = new List<float>() { 0, 0, 0 };
                step = STEP_OPEN;
                time = 40;
                okureiman.GetComponent<Animator>().SetTrigger("start");
                stone.SetActive(true);
                Master.voiceSE.Play("voice_skill3_start" + Random.Range(1, 3));
            }
        }
        else if(step == STEP_OPEN)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 10)
                {
                    renda_sr.gameObject.SetActive(true);
                    renda_sr.sprite = renda_sprite[0];
                }

                if(time == 5)
                {
                    renda_sr.sprite = renda_sprite[1];
                }

                if(time == 0)
                {
                    renda_sr.sprite = renda_sprite[2];
                    t_attack_time = 0;
                    t_attack_time += attack_time_level[Master.skill_level - 1];
                    step = STEP_GAME;
                }
            }
        }
        else if(step == STEP_GAME_END)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == GAME_END_TIME)
                {
                    renda_sr.sprite = renda_sprite[1];
                }
                if(time == GAME_END_TIME - 5)
                {
                    renda_sr.sprite = renda_sprite[0];
                }
                if (time == GAME_END_TIME - 10)
                {
                    renda_sr.gameObject.SetActive(false);
                }

                if (time == 0)
                {
                    AttackEnd();
                    step = STEP_GAME_END_WAIT;
                    time = 60;
                }
            }
        }
        else if(step == STEP_GAME_END_WAIT)
        {
            if(time > 0)
            {
                time -= 1;

                if(time < 30)
                {
                    float alpha = time / 30f;
                    stone_sr.color = new Color(1f, 1f, 1f, alpha);
                }

                if(time == 0)
                {
                    End();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (step == STEP_GAME)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //tap
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float posX = pos.x - Camera.main.transform.position.x;
                if (posX > 0)
                {
                    t_button_right = BUTTON_ANIM;
                    ButtonEffectRight(t_button_right);
                    Attack();
                }
                else
                {
                    t_button_left = BUTTON_ANIM;
                    ButtonEffectLeft(t_button_left);
                    Attack();
                }
            }
            else
            {
                //keyboard
                if (Input.GetKeyDown(KeyCode.D))
                {
                    t_button_right = BUTTON_ANIM;
                    ButtonEffectRight(t_button_right);
                    Attack();
                }
                else
                if (Input.GetKeyDown(KeyCode.A))
                {
                    t_button_left = BUTTON_ANIM;
                    ButtonEffectLeft(t_button_left);
                    Attack();
                }
            }
        }
    }

    void Attack()
    {
        ani_jun.SetTrigger("attack");
        Master.nomalSE.Play("attack1");

        int target = bec.battle_active_enemy_num[Random.Range(0, attack_target_range)];
        bec.Attacked(target);

        attack_damaged[target] += damage_value;
    }

    void AttackEnd()
    {
        ani_jun.ResetTrigger("attack");
        ani_jun.SetInteger("step", 1);

        foreach(int target in bec.battle_active_enemy_num)
        {
            attack_damaged[target] += damage_value * 2;
        }

        Master.nomalSE.Play("attack3");
        foreach (int target in bec.battle_active_enemy_num)
        {
            float damage = attack_damaged[target];
            bec.Damaged((int)damage, target);
        }

        Master.voiceSE.Play("voice_skill3_attack" + Random.Range(1, 3));
    }

    void End()
    {
        attack_damaged = new List<float>() { 0, 0, 0 };
        stone_sr.color = new Color(1f, 1f, 1f, 1f);
        stone.SetActive(false);
        okureiman.SetActive(false);

        ani_jun.SetTrigger("skill_end");
        ani_jun.SetInteger("skill", 0);
        ani_jun.SetInteger("step", 0);
        step = STEP_WAIT;
        time = 0;
        t_attack_time = 0;
        t_button_right = 0;
        t_button_left = 0;
        ButtonEffectLeft(0);
        ButtonEffectRight(0);
        Master.EndAttack();
    }

    void SetStart()
    {
        int button_x = 123 + ((int)Status.SCREEN_WIDTH - 150) / 2;
        Debug.Log(button_x);
        Move.MoveLocalPosX(button[1], button_x);
        Move.MoveLocalPosX(button[0], -button_x);
    }

    private void ButtonEffectLeft(int t)
    {
        float color_button = (75 + 100 * (t / (float)(BUTTON_ANIM))) / 255f;
        button_img[0].color = new Color(color_button, color_button, color_button);
    }

    private void ButtonEffectRight(int t)
    {
        float color_button = (75 + 100 * (t / (float)(BUTTON_ANIM))) / 255f;
        button_img[1].color = new Color(color_button, color_button, color_button);
    }
}
