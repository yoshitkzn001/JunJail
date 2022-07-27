using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill6Config : MonoBehaviour
{
    const int STEP_WAIT = 0;
    const int STEP_READY1 = 1;
    const int STEP_READY2 = 2;
    const int STEP_READY3 = 3;
    const int STEP_READY4 = 4;
    const int STEP_READY5 = 5;
    const int STEP_READY6 = 6;
    const int STEP_SELECT = 7;
    const int STEP_PLAY1 = 8;
    const int STEP_PLAY2 = 9;
    const int STEP_PLAY3 = 10;
    const int STEP_GETCOIN = 11;
    const int STEP_LOSTCOIN = 12;
    const int STEP_READY7 = 13;
    const int STEP_READY8 = 14;
    const int STEP_LOST_END1 = 15;
    const int STEP_LOST_END2 = 16;
    const int STEP_ATTACK_READY1 = 17;
    const int STEP_ATTACK_READY2 = 18;
    const int STEP_ATTACK = 19;
    const int STEP_ATTACK_END_WAIT = 20;
    const int STEP_END_WALK = 30;

    const int TIME_READY1 = 18;
    const int TIME_READY2 = 75;
    const int TIME_READY3 = 55;
    const int TIME_READY4 = 65;
    const int TIME_READY5 = 50;
    const int TIME_READY6 = 10;
    const int TIME_SELECTED = 60;
    const int TIME_PLAY1 = 40;
    const int TIME_PLAY2 = 135;
    const int TIME_PLAY3 = 40;
    const int TIME_GETCOIN = 64;
    const int TIME_LOSTCOIN = 64;
    const int TIME_READY7 = 15;
    const int TIME_READY8 = 50;
    const int TIME_LOST_END1 = 50;
    const int TIME_LOST_END2 = 80;
    const int TIME_ATTACK_READY1 = 60;
    const int TIME_ATTACK_READY2 = 60;
    const int TIME_ATTACK_LATE = 10;
    const int TIME_ATTACK_FALL = 60;
    const int TIME_ATTACK_END_WAIT = 60;

    const int SELECT_HIGH = 0;
    const int SELECT_LOW = 1;
    const int SELECT_ATTACK = 2;
    const int SELECT_MAX = 3;

    public BattleEnemyConfig bec;
    [SerializeField] private EffectJudgeConfig effect_judge;
    [SerializeField] private AudioClip[] voice_clips;
    [SerializeField] private GameObject stage;
    [SerializeField] private CanvasGroup SelectObj_alpha;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject[] Texts;
    [SerializeField] private GameObject[] Selects;
    [SerializeField] private GameObject Select_cursor;
    [SerializeField] private CursorSelectConfig Select_cursor_C;
    public MasBattleConfig Master;
    [SerializeField] private Controller Control;
    [SerializeField] private FuncMove Move;
    [SerializeField] private DataNomal DataNomal;
    [SerializeField] private Animator ani_jun;
    [SerializeField] private Animator[] emons;
    [SerializeField] private Animator[] emons_effect;
    [SerializeField] private SpriteRenderer[] card_sr;

    [SerializeField] private GameObject coin_parent;
    [SerializeField] private GameObject first_coin;
    [SerializeField] private Skill6CoinConfig first_coin_config;
    private Queue<GameObject> coins = new Queue<GameObject>();
    private Queue<(GameObject, Skill6CoinConfig)> set_coins = new Queue<(GameObject, Skill6CoinConfig)>();
    private Queue<(GameObject, Skill6CoinConfig)> seted_coins = new Queue<(GameObject, Skill6CoinConfig)>();
    [SerializeField]  private GameObject[] attack_coins;
    List<int> coin_set_time;

    int[] win_count_list = new int[3] { 4, 5, 6 };

    bool attack_end;
    int t_shake;
    int t_first_attack;
    int t_last_attack;
    int t_selected;
    int step;
    int time;
    int select;
    int fig_target;
    int fig_select;
    int emon_select;
    int win_count;
    int win_count_max;

    private bool test = true;

    // Start is called before the first frame update
    void Awake()
    {
        coins = new Queue<GameObject>();
        gameObject.SetActive(false);

        seted_coins = new Queue<(GameObject, Skill6CoinConfig)>();
        step = STEP_WAIT;
        time = 0;
        select = 0;
        win_count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_shake > 0)
        {
            t_shake -= 1;

            if(t_shake == 0)
            {
                Move.MoveLocalPosPlusY(stage, 3);
            }
        }

        if(t_selected > 0)
        {
            t_selected -= 1;

            if(t_selected > TIME_SELECTED - 12)
            {
                float a = (12 - (TIME_SELECTED - t_selected)) / 12f;
                Select_cursor_C.SetColor(1f, 1f, 1f, a);
            }

            if(t_selected == TIME_SELECTED - 6)
            {
                Move.MoveLocalScale(Select_cursor, 0.95f, 0.95f);
                Move.MoveLocalScale(Texts[select], 0.95f, 0.95f);
                Move.MoveLocalScale(Selects[select], 0.95f, 0.95f);
            }
            else if (t_selected == TIME_SELECTED - 12)
            {
                Move.MoveLocalScale(Select_cursor, 1.0f, 1.0f);
                Move.MoveLocalScale(Texts[select], 1.0f, 1.0f);
                Move.MoveLocalScale(Selects[select], 1.0f, 1.0f);
                Select_cursor.SetActive(false);
            }

            if(t_selected <= 20)
            {
                Move.AccelMoveY(t_selected, 20, -72.5f, -128.5f, Selects[select]);
                Move.AccelMoveY(t_selected, 20, -72f, -128f, Texts[select]);

                if(t_selected == 0)
                {
                    Selects[select].SetActive(false);
                    Texts[select].SetActive(false);
                    Move.MoveLocalPosY(Selects[select], -72.5f);
                    Move.MoveLocalPosY(Texts[select], -72f);
                }
            }
        }

        if (step == STEP_WAIT)
        {
            if (Master.step == 8) // STEP_SELECT_ATTACK_DO を変えたらここも変える
            {
                win_count_max = win_count_list[Master.skill_level - 1];

                time = TIME_READY1;
                step = STEP_READY1;
            }
        }

        else if(step == STEP_READY1)
        {
            time -= 1;

            if(time == 8)
            {
                Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[0]);
            }

            if(time == 0)
            {
                time = TIME_READY2;
                step = STEP_READY2;
                ani_jun.SetInteger("step", 2);
            }
        }

        else if (step == STEP_READY2)
        {
            time -= 1;

            if (time == 0)
            {
                time = TIME_READY3;
                step = STEP_READY3;
                Master.battleSE.PlayOneShot(Master.battleSE.Nomal_SE[1]);

                foreach (Animator emons_ani in emons)
                {
                    emons_ani.gameObject.SetActive(true);
                }

                foreach (Animator emons_effect_ani in emons_effect)
                {
                    emons_effect_ani.gameObject.SetActive(true);
                    emons_effect_ani.SetTrigger("in");
                }
            }
        }

        else if (step == STEP_READY3)
        {
            time -= 1;

            if (time == 0)
            {
                time = TIME_READY4;
                step = STEP_READY4;

                emons[2].SetInteger("step", 1);
                emons[3].SetInteger("step", 1);
            }
        }

        else if (step == STEP_READY4)
        {
            time -= 1;

            if (time == 0)
            {
                time = TIME_READY5;
                step = STEP_READY5;

                emons[2].SetInteger("step", 2);

                float coinX = -120f;
                float coinY = 22f;
                float coinZ = coinY + 10f;
                Move.MoveLocalPosZ(first_coin, coinX, coinY + 38f, coinZ);
                first_coin.SetActive(true);
                first_coin_config.Set(0);
                Master.battleSE.PlayOneShot(Master.battleSE.Nomal_SE[0]);
            }
        }

        else if (step == STEP_READY5)
        {
            time -= 1;

            if (time == 0)
            {
                time = TIME_READY6;
                step = STEP_READY6;

                emons[2].SetInteger("step", 3);
                fig_target = Random.Range(0, 10);
                if(test == true)
                {
                    fig_target = 3;
                }
                card_sr[0].sprite = DataNomal.figure_BigWhite[fig_target];
                card_sr[0].gameObject.SetActive(true);
                emon_select = 1;
                Select_cursor.SetActive(true);
            }
        }

        else if (step == STEP_READY6)
        {
            time -= 1;

            SelectObj_alpha.alpha = (TIME_READY6 - time) / (float)TIME_READY6;

            if (time == 0)
            {
                step = STEP_SELECT;
                ani_jun.SetInteger("step", 3);
                emons[0].SetBool("success", false);
                emons[1].SetBool("success", false);

                Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[14 + (win_count / 2)]);
            }
        }

        else if (step == STEP_READY7)
        {
            time -= 1;

            if (time == 0)
            {
                if(win_count != win_count_max)
                {
                    emons[emon_select + 2].SetInteger("step", 2);
                    step = STEP_READY8;
                    time = TIME_READY8;
                }
                else
                {
                    ani_jun.SetInteger("step", 7);

                    emons[2].SetInteger("step", 0);
                    emons[3].SetInteger("step", 0);

                    card_sr[0].gameObject.SetActive(false);
                    card_sr[1].gameObject.SetActive(false);

                    step = STEP_ATTACK_READY1;
                    time = TIME_ATTACK_READY1;
                }
            }
        }

        else if (step == STEP_READY8)
        {
            time -= 1;

            if (time == 0)
            {
                SelectObj_alpha.alpha = 0f;
                emons[emon_select + 2].SetInteger("step", 1);
                fig_target = fig_select;
                Select_cursor.SetActive(true);
                box.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    Texts[i].SetActive(true);
                    Selects[i].SetActive(true);
                }

                time = TIME_READY6;
                step = STEP_READY6;
            }
        }

        else if(step == STEP_PLAY1)
        {
            time -= 1;

            if(time == 0)
            {
                step = STEP_PLAY2;
                time = TIME_PLAY2;
                emons[emon_select + 2].SetInteger("step", 2);
                Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[17]);
            }
        }

        else if (step == STEP_PLAY2)
        {
            time -= 1;

            if (time == 0)
            {
                step = STEP_PLAY3;
                time = TIME_PLAY3;
                card_sr[emon_select].sprite = DataNomal.figure_BigWhite[fig_select];
                card_sr[emon_select].gameObject.SetActive(true);
                emons[emon_select + 2].SetInteger("step", 3);
            }
        }

        else if (step == STEP_PLAY3)
        {
            time -= 1;

            if (time == 0)
            {
                if(fig_target > fig_select)
                {
                    if(select == SELECT_LOW)
                    {
                        GetCoin();

                        Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[7 + win_count]);
                    }
                    else if(select == SELECT_HIGH)
                    {
                        LostCoin();
                    }
                }
                else
                {
                    if (select == SELECT_LOW)
                    {
                        LostCoin();
                    }
                    else if (select == SELECT_HIGH)
                    {
                        GetCoin();

                        Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[1 + win_count]);
                    }
                }
            }
        }

        else if(step == STEP_GETCOIN)
        {
            time -= 1;

            if (coin_set_time.Contains(time))
            {
                (GameObject coin, Skill6CoinConfig config) = set_coins.Dequeue();
                coin.SetActive(true);
                Master.battleSE.PlayOneShot(Master.battleSE.Nomal_SE[0]);
                seted_coins.Enqueue((coin, config));
            }

            if(time == 0)
            {
                step = STEP_READY7;
                time = TIME_READY7;
            }
        }

        else if (step == STEP_LOSTCOIN)
        {
            time -= 1;

            if (coin_set_time.Contains(time))
            {
                GameObject coin;
                Skill6CoinConfig config;
                if (seted_coins.Count != 0)
                {
                    (coin, config) = seted_coins.Dequeue();
                    coins.Enqueue(coin);
                }
                else
                {
                    coin = first_coin;
                    config = first_coin_config;
                }

                config.DestroyCoinAni();
            }

            if (time == 0)
            {
                emons[2].SetInteger("step", 0);
                emons[3].SetInteger("step", 0);
                card_sr[0].gameObject.SetActive(false);
                card_sr[1].gameObject.SetActive(false);

                step = STEP_LOST_END1;
                time = TIME_LOST_END1;
            }
        }

        else if(step == STEP_LOST_END1)
        {
            time -= 1;

            if(time == 0)
            {
                emons[0].SetInteger("fault", 0);
                emons[1].SetInteger("fault", 0);
                Master.battleSE.PlayOneShot(Master.battleSE.Nomal_SE[1]);

                foreach (Animator emons_ani in emons)
                {
                    emons_ani.gameObject.SetActive(false);
                }

                foreach (Animator emons_effect_ani in emons_effect)
                {
                    emons_effect_ani.SetTrigger("out");
                }

                step = STEP_LOST_END2;
                time = TIME_LOST_END2;
            }
        }

        else if (step == STEP_LOST_END2)
        {
            time -= 1;

            if (time == 0)
            {
                ani_jun.SetInteger("step", 12);

                step = STEP_END_WALK;
            }
        }

        else if(step == STEP_ATTACK_READY1)
        {
            time -= 1;

            if(time == 0)
            {
                Move.MoveLocalPosY(coin_parent, 0);

                int enemy_count = bec.battle_active_enemy_num.Count;
                foreach ((GameObject coin, Skill6CoinConfig config) in seted_coins)
                {
                    CreateAttackCoin(coin, config, bec.battle_active_enemy_num[Random.Range(0, enemy_count)]);
                }
                first_coin.SetActive(false);
                for(int i=0; i<enemy_count; i++)
                {
                    GameObject coin = attack_coins[i];
                    coin.SetActive(true);
                    CreateAttackCoin(coin, null, bec.battle_active_enemy_num[i]);
                }

                ani_jun.SetInteger("step", 8);
                Master.battleSE.PlayOneShot(Master.battleSE.Nomal_SE[2]);

                foreach (Animator emons_ani in emons)
                {
                    emons_ani.gameObject.SetActive(false);
                }

                foreach (Animator emons_effect_ani in emons_effect)
                {
                    emons_effect_ani.SetTrigger("out");
                }

                Master.nomalSE.Play("attack_sccess");
                Attack();
                t_first_attack = TIME_ATTACK_FALL;
                step = STEP_ATTACK;
            }
            else
            {
                Move.AccelMoveY(time, TIME_ATTACK_READY1, 0, 104, coin_parent);
            }
        }

        else if(step == STEP_ATTACK)
        {
            time -= 1;

            if(time == 0)
            {
                Attack();
            }

            if(t_first_attack > 0)
            {
                t_first_attack -= 1;

                if(t_first_attack == 0)
                {
                    ani_jun.SetInteger("step", 9);
                    if (win_count <= 5)
                    {
                        Master.battleSE.PlayOneShot(Master.battleSE.Skill2_SE[win_count / 2]);
                    }
                    else
                    {
                        Master.battleSE.PlayOneShot(Master.battleSE.Skill2_SE[2]);
                    }
                }
            }

            if (t_last_attack > 0)
            {
                t_last_attack -= 1;

                if (t_last_attack == 0)
                {
                    Master.nomalSE.Play("attack3");
                    Shake();
                    int enemy_count = bec.battle_active_enemy_num.Count;
                    for (int i = 0; i < enemy_count; i++)
                    {
                        int num = bec.battle_active_enemy_num[i];
                        attack_coins[i].SetActive(false);
                        bec.Damaged(1, num);
                    }

                    step = STEP_ATTACK_END_WAIT;
                    time = TIME_ATTACK_END_WAIT;
                }
                else
                {
                    int enemy_count = bec.battle_active_enemy_num.Count;
                    for(int i=0; i<enemy_count; i++)
                    {
                        int num = bec.battle_active_enemy_num[i];
                        Move.AccelMoveY(t_last_attack, TIME_ATTACK_FALL, 117f, bec.battle_enemy[num].transform.localPosition.y, attack_coins[i]);
                    }
                }
            }
        }

        else if(step == STEP_ATTACK_END_WAIT)
        {
            time -= 1;

            if(time == 0)
            {
                ani_jun.SetInteger("step", 12);

                step = STEP_END_WALK;
            }
        }

        else if(step == STEP_END_WALK)
        {
            Transform transform = ani_jun.gameObject.transform;
            Vector3 pos = transform.localPosition;
            if (pos.x < -62.5f)
            {
                pos.x += 1f;
                transform.localPosition = pos;
            }
            else
            {
                End();
            }
        }
    }

    private void Update()
    {
        if(step == STEP_SELECT)
        {
            if (Control.GetButtonDownRight())
            {
                select += 1;
                if(select >= SELECT_MAX)
                {
                    select = 0;
                }
                Move.SetSamePos(Select_cursor, Selects[select], 0, 0);
                Select_cursor_C.reset(0);
                Master.nomalSE.Play("select1");
            }

            else if (Control.GetButtonDownLeft())
            {
                select -= 1;
                if (select < 0)
                {
                    select = SELECT_MAX - 1;
                }
                Move.SetSamePos(Select_cursor, Selects[select], 0, 0);
                Select_cursor_C.reset(0);
                Master.nomalSE.Play("select1");
            }

            else if (Control.GetButtonDownDecide())
            {
                Master.nomalSE.Play("select2");

                Select_cursor_C.SetColor(1f, 1f, 1f, 1f);
                Move.MoveLocalScale(Select_cursor, 1.1f, 1.1f);
                Move.MoveLocalScale(Texts[select], 1.1f, 1.1f);
                Move.MoveLocalScale(Selects[select], 1.1f, 1.1f);
                box.SetActive(false);
                for (int i = 0; i < 3; i++)
                {
                    if (i != select)
                    {
                        Texts[i].SetActive(false);
                        Selects[i].SetActive(false);
                    }
                }
                t_selected = TIME_SELECTED;

                if (select != SELECT_ATTACK)
                {
                    ani_jun.SetInteger("step", 4);

                    List<int> fig_select_list = new List<int>();
                    for(int i=0; i<10; i++)
                    {
                        if(i != fig_target)
                        {
                            fig_select_list.Add(i);
                        }
                    }
                    fig_select = fig_select_list[Random.Range(0, 9)];
                    if (test == true)
                    {
                        if(win_count == 0)
                        {
                            fig_select = 7;
                        }
                        else if (win_count == 1)
                        {
                            fig_select = 4;
                        }
                        else if (win_count == 2)
                        {
                            fig_select = 5;
                        }
                        else if (win_count == 3)
                        {
                            fig_select = 8;
                        }
                    }

                    step = STEP_PLAY1;
                    time = TIME_PLAY1;
                }
                else
                {
                    ani_jun.SetInteger("step", 7);

                    emons[2].SetInteger("step", 0);
                    emons[3].SetInteger("step", 0);
                    card_sr[0].gameObject.SetActive(false);
                    card_sr[1].gameObject.SetActive(false);

                    step = STEP_ATTACK_READY1;
                    time = TIME_ATTACK_READY1;
                }
            }
        }
    }

    private void GetCoin()
    {
        ani_jun.SetInteger("step", 5);
        emons[0].SetBool("success", true);
        emons[1].SetBool("success", true);
        Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[19]);

        int effect_judge_num = 1;
        if(win_count > 2)
        {
            effect_judge_num = 0;
        }
        effect_judge.SetJudge(-104f, 145f, effect_judge_num, 85, 10);

        int plus_coin_count = 1;
        for(int i=0; i<win_count; i++)
        {
            plus_coin_count = plus_coin_count * 2;
        }
        win_count += 1;

        for(int i=0; i<plus_coin_count; i++)
        {
            CreateCoin(0);
        }

        coin_set_time = new List<int>();
        int set_time = TIME_GETCOIN / plus_coin_count;
        for(int i=0; i<plus_coin_count; i++)
        {
            coin_set_time.Add(set_time * (i + 1));
        }

        emon_select = (emon_select + 1) % 2;
        step = STEP_GETCOIN;
        time = TIME_GETCOIN + 1;
    }

    private void LostCoin()
    {
        ani_jun.SetInteger("step", 6);
        emons[0].SetInteger("fault", 2);
        emons[1].SetInteger("fault", 1);
        Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[18]);

        int lost_coin_count = 1 + seted_coins.Count;
        coin_set_time = new List<int>();
        int des_time = TIME_LOSTCOIN / lost_coin_count;
        for (int i = 0; i < lost_coin_count; i++)
        {
            coin_set_time.Add(des_time * (i + 1));
        }

        step = STEP_LOSTCOIN;
        time = TIME_LOSTCOIN + 1;

        Master.battleSE.PlayOneShot(Master.battleSE.Skill6_SE[1]);

        effect_judge.SetJudge(-104f, 145f, 2, 85, 10);
    }

    private void CreateCoin(int mode)
    {
        GameObject coin;

        if (coins.Count == 0)
        {
            coin = Instantiate(first_coin);
            coin.transform.parent = coin_parent.transform;
        }
        else
        {
            coin = coins.Dequeue();
        }

        float posY = Random.Range(-12, 48);
        float posX = Random.Range(-119, -182);
        float posZ = posY + 10f;
        posY += Random.Range(10, 45);
        Move.MoveLocalPosZ(coin, posX, posY, posZ);

        Skill6CoinConfig config = coin.GetComponent<Skill6CoinConfig>();
        config.Set(mode);

        set_coins.Enqueue((coin, config));
        coin.SetActive(false);
    }

    private void CreateAttackCoin(GameObject coin, Skill6CoinConfig config, int enemy_num)
    {
        GameObject target_enemy = bec.battle_enemy[enemy_num];

        Transform ene_transform = target_enemy.transform;
        Vector3 ene_pos = ene_transform.localPosition;
        Transform coin_transform = coin.transform;
        Vector3 coin_pos = coin_transform.localPosition;
        coin_pos.x = (int)ene_pos.x;
        coin_pos.y = 117f;
        coin_pos.z = ene_pos.z - 1;
        coin_transform.localPosition = coin_pos;

        if(config != null)
        {
            config.StopCoinAni(117f, ene_pos.y, enemy_num);
        }
    }

    private void Attack()
    {
        if(seted_coins.Count != 0)
        {
            time = TIME_ATTACK_LATE;
            (GameObject coin, Skill6CoinConfig config) = seted_coins.Dequeue();
            config.SetAttack(TIME_ATTACK_FALL, Move, this);
            coins.Enqueue(coin);
        }
        else
        {
            if(attack_end == false)
            {
                time = TIME_ATTACK_LATE;
                t_last_attack = TIME_ATTACK_FALL;
                attack_end = true;
            }
        }
    }

    public void Shake()
    {
        t_shake = 5;
        Move.MoveLocalPosPlusY(stage, -3);
    }

    private void End()
    {
        ani_jun.SetInteger("step", 0);
        ani_jun.SetInteger("skill", 0);
        ani_jun.SetTrigger("skill_end");

        foreach (Animator emons_effect_ani in emons_effect)
        {
            emons_effect_ani.gameObject.SetActive(false);
        }

        seted_coins = new Queue<(GameObject, Skill6CoinConfig)>();
        step = STEP_WAIT;
        time = 0;
        select = 0;
        win_count = 0;
        attack_end = false;
        Move.SetSamePos(Select_cursor, Selects[0], 0, 0);

        SelectObj_alpha.alpha = 0f;
        box.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            Texts[i].SetActive(true);
            Selects[i].SetActive(true);
        }

        Master.EndAttack();
    }
}
