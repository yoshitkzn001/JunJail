using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasBattleConfig : MonoBehaviour
{
    //const
    private const float LEVEL_POS = 63.5f;
    private const float LEVELUP_POS = -8.5f;
    private const int TIME_WINMES_DIS_WAIT = 130;
    private const int TIME_WINMES_DIS = 50;
    private const int TIME_SROM_APP = 30;
    private const int TIME_SROM_DIS_WAIT = 90;
    private const int TIME_SROM_DIS = 50;
    private const int TIME_WINMES_APP = 20;
    private const int GET_EXP_TIME = 30;
    private const int RUN_WAIT_TIME = 40;
    private const int TARGET_NONE = -3;
    private const int TARGET_ME = -2;
    private const int TARGET_ALL = -1;
    private const int CHANGE_PAGE = 6;
    private const int STATUS_TIME = 55;
    private const int MYBUFF_TIME = 100;
    private const int MYBUFF_TIME_FADE = 10;
    private const int STATUS_SHAKE_TIME = 6;
    private const int STATUS_DEC_TIME = 10;
    private const int ITEM_WAIT_TIME = 30;
    private const int MENU_MOVET = 15;
    private const int MENU_MOVET2 = 25;
    private const int MENU_MOVET3 = 50;
    private const int STEP_BATTLE_WAIT = -3;
    private const int STEP_OPEN = -1;
    private const int STEP_SELECT_FIRST = 0;
    private const int STEP_SELECT_MOVE = 1;
    private const int STEP_SELECT_ITEM = 3;
    private const int STEP_SELECT_RUN = 4;
    private const int STEP_SELECT_ATTACK = 2;
    private const int STEP_SELECT_CHANGEPAGE = 5;
    private const int STEP_SELECT_ATTACK_WAIT = 6;
    private const int STEP_SELECT_ATTACK_READY = 7;
    private const int STEP_SELECT_ATTACK_DO = 8;
    private const int STEP_SELECT_ATTACK_END = 9;
    private const int STEP_SELECT_ITEM_WAIT = 10;
    private const int STEP_SELECT_ITEM_USE = 11;
    private const int STEP_SELECT_ITEM_END = 12;
    private const int STEP_SELECT_RUN_WAIT = 13;
    private const int STEP_SELECT_RUN_DO = 14;
    private const int STEP_SELECT_RUN_FAULT = 15;
    private const int STEP_DEFENCE_READY = 20;
    private const int STEP_DEFENCE_DO = 21;
    private const int STEP_DEFENCE_END = 22;
    private const int STEP_SET_BUFFS = 23;
    private const int STEP_WIN = 24;
    private const int STEP_WIN_WAIT = 25;
    private const int STEP_WIN_END = 26;
    private const int STEP_LEVEL_UP = 30;
    private const int STEP_LEVEL_UP_END = 31;

    private const float BGM_VOLUME = 0.85f;
    public CriAtomSource nomalSE;
    public CriAtomSource voiceSE;
    public AudioSource battleBGM;
    public AudioSE battleSE;

    [SerializeField] private GameOverConfig gameover;
    [SerializeField] private CanvasGroup hp_line_alpha;
    [SerializeField] private Controller Control;
    [SerializeField] private JunConfig junC;
    [SerializeField] private StageItemConfig StageItem;
    [SerializeField] private MasStageConfig MasterStage;
    [SerializeField] private BattleEnemyConfig Enemy;
    [SerializeField] private TaptostartConfig Taptostart;
    [SerializeField] private FuncMiniFigure Fig;
    [SerializeField] private FuncBar Bar;
    [SerializeField] private FuncMove Move;
    [SerializeField] private DataNomal DataNomal;
    public DataBattle Data;
    [SerializeField] private RomDecidedConfig Romdecided;
    [SerializeField] private ItemDecidedConfig Itemdecided;
    public GameObject skill3_okureiman;
    public StatusStConfig St_obj_config;
    public GameObject jun;
    public GameObject jun_effect;
    public GameObject guard_obj;
    public GameObject guard_effect_obj;
    private SpriteRenderer guard_effect_sr;
    private SpriteRenderer guard_sr;
    private Animator guard_effect_ani;
    private Animator guard_ani;
    public Animator jun_effect_ani;
    public Animator jun_ani;
    private SpriteRenderer jun_effect_sr;

    private int drop_item_voice_num;

    //status
    private float[] slash_pos = new float[3] { 10, 14, 18 };
    private int walk_dir;
    public int walk_box_pos;
    public int pagenum_min;
    public int pagenum_max;

    //buff
    private const int BUFF_KIND = 5;
    private const int DEBUFF_KIND = 6;
    private int buffs_step;
    private int buffs_count;
    private List<int> buffs = new List<int>();  //atk, atk2, def, def2, heal, break, hirou
    public int[] buff_time = new int[BUFF_KIND];  //0,1, atk  2,3, def  4, heal
    public int[] buff_value = new int[BUFF_KIND];
    public int[] debuff_time = new int[DEBUFF_KIND];  //0, break
    public int[] debuff_value = new int[DEBUFF_KIND];

    //act
    private const int ACT_FIRST_BUTTON = 2;
    private int act_num;
    private int act_num_max;

    //roms
    private int rom_page = 1;
    private int rom_page_max;
    public int rom_selected = -1;
    public int skill_selected;
    public int skill_level;
    public int skill_atk;

    //items
    private List<int> items_inbattle;
    private int item_page = 1;
    private int item_page_max;
    public int item_selected = -1;
    private List<int> items_do;
    private int item_step;

    //run
    public int run_selected = -1;
    private int run_do = 0;
    private int run_speed = 0;

    //step
    bool first_open;
    public int step_sub = 0;
    public int step = 0;

    //T
    public int[] pos_move_HPSP;
    public float[] pos_move_returnbuttons;
    int t_close_firstbuttons;
    int t_open_firstbuttons;
    public int[] pos_move_firstbuttons = new int[2] { 0, -57};
    int t_close_secondbuttons;
    int t_open_secondbuttons;
    public int[] pos_move_secondbuttons = new int[2] { 19, -38 };
    int t_changepage;
    int t_open_message;
    int t_close_message;
    public float[] pos_move_message = new float[2] { 31, 0 };
    public int t_close_skill;
    int t_oepn_defense;
    int t_close_defense;
    int t_item_wait;
    int t_hp_display;
    int t_hp_display_max;
    int t_sp_display;
    int t_sp_display_max;
    int t_buff_display;
    int t_mybuff_display;
    int t_close_item;
    int t_damaged;
    int t_battle_start;
    int t_win_message;
    int t_run;
    int t_get_exp;
    int t_level_up;

    const int FADE_BGM_TIME = 70;
    int t_fade_bgm;

    //guard
    private const int JUSTGUARD_TIME = 15;
    private const int JUSTGUARD_RANGE = 7;
    private const int GUARD_TIME = 25;
    private const int KNOCK_BACK_LATE = 2;
    bool button_guard = false;
    bool guard = false;
    public int t_just_guard;
    int t_guard;
    int guard_break;
    int guard_sp;
    bool guard_buff;

    //target
    public int target_enemy;
    private int target_single_enemy;
    private int pre_target_enemy;

    //spball
    [SerializeField] GameObject spball_parent;
    [SerializeField] GameObject spball_resources;
    public Queue<GameObject> spballs = new Queue<GameObject>();
    List<GameObject> spballs_active = new List<GameObject>();

    //stage config
    public GameObject stage_enemy;
    public GameObject stage_room;

    //floor
    public GameObject[] a_floor;
    public SpriteRenderer[] a_floor_sr;
    public List<int> a_pos = new List<int>();

    //level
    bool get_exp = false;
    int level_up = 0;

    public bool tutorial;
    public int tutorial_step;
    private int tutorial_time;
    public GameObject[] tutorial_text_obj;
    public CanvasGroup[] tutorial_text_alpha;
    public Text[] tutorial_text;
    public GameObject tutorial_press;
    public GameObject guard_button;

    public bool boss_battle;

    private void OnEnable()
    {
        jun.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        drop_item_voice_num = 0;
        step = -4;
        jun_effect_ani = jun_effect.GetComponent<Animator>();
        jun_effect_sr = jun_effect.GetComponent<SpriteRenderer>();
        guard_ani = guard_obj.GetComponent<Animator>();
        guard_effect_ani = guard_effect_obj.GetComponent<Animator>();
        guard_effect_sr = guard_effect_obj.GetComponent<SpriteRenderer>();
        guard_sr = guard_obj.GetComponent<SpriteRenderer>();
        Move.MoveLocalPosX(Data.menu_SP, pos_move_HPSP[1]);
        Move.MoveLocalPosX(Data.menu_HP, -pos_move_HPSP[1]);
        Data.enemy_names.SetActive(false);
        Data.move_firstbuttons.SetActive(false);
        Data.menu_item_message.SetActive(false);
        Data.move_secondbuttons_roms.SetActive(false);
        Data.move_secondbuttons_roms_sp.SetActive(false);
        Data.move_secondbuttons_items.SetActive(false);
        Data.move_secondbuttons_items_value.SetActive(false);
        Data.move_secondbuttons_status_roms_level.SetActive(false);
        Data.move_secondbuttons_run.SetActive(false);
        Data.move_second.SetActive(false);
        for (int i = 0; i < pagenum_min; i++)
        {
            Data.menu_roms[i].SetActive(false);
            Data.menu_sps[i].SetActive(false);
            Data.menu_items[i].SetActive(false);
            Data.menu_values[i].SetActive(false);
            Data.rom_status_level[i].SetActive(false);
        }
        for (int i = 7 - pagenum_min; i < 7; i++)
        {
            Data.menu_roms[i].SetActive(false);
            Data.menu_sps[i].SetActive(false);
            Data.menu_items[i].SetActive(false);
            Data.menu_values[i].SetActive(false);
        }
        if(pagenum_min == 0)
        {
            Data.rom_status_level[4].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(t_fade_bgm > 0)
        {
            t_fade_bgm -= 1;

            battleBGM.volume = BGM_VOLUME * (t_fade_bgm / (float)FADE_BGM_TIME);
        }

        if(t_get_exp > 0)
        {
            t_get_exp -= 1;

            float c = (GET_EXP_TIME - t_get_exp) / (float)GET_EXP_TIME;
            Data.level_text.color = new Color(1f, 1f, c);
            Data.level_value_text.color = new Color(1f, 1f, c);
        }

        //move buttons
        if (t_open_firstbuttons > 0)
        {
            OpenFirstButtons();
        }
        if (t_close_firstbuttons > 0)
        {
            CloseFirstButtons();
        }
        if (t_open_secondbuttons > 0)
        {
            OpenSecondButtons();
        }
        if (t_close_secondbuttons > 0)
        {
            CloseSecondButtons();
        }
        //changepage
        if (t_changepage > 0)
        {
            ChangePageAnimation();
        }
        //message
        if (t_open_message > 0)
        {
            OpenMessage();
        }
        if (t_close_message > 0)
        {
            CloseMessage();
        }

        if (step == STEP_BATTLE_WAIT)
        {
            if (t_battle_start > 0)
            {
                t_battle_start -= 1;

                if(t_battle_start == 0)
                {
                    jun_ani.SetTrigger("battle_start");
                    ResetMyTurn();
                    step = STEP_OPEN;
                    t_open_firstbuttons = MENU_MOVET3;
                    Data.move_firstbuttons.SetActive(true);
                    St_obj_config.OpenST(MENU_MOVET3 - 20);
                    if(MasterStage.DebugMode == false)
                    {
                        if (boss_battle == false)
                        {
                            battleBGM.clip = Sound.BGM[3];
                        }
                        else
                        {
                            battleBGM.clip = Sound.BGM[4];
                        }

                        battleBGM.Play();
                        battleBGM.volume = BGM_VOLUME;
                    }
                }
            }
        }

        else if (step == STEP_OPEN)
        {
            if(t_open_firstbuttons > 0)
            {
                OpenFirstButtons();
            }
        }

        else if(step == STEP_SELECT_ATTACK_END)
        {
            if(t_close_skill > 0)
            {
                CloseSkill();
            }
            else if(t_close_skill == 0)
            {
                JudgeWin();
            }
        }
        
        else if(step == STEP_DEFENCE_READY)
        {
            if(t_oepn_defense > 0 | t_oepn_defense == -1)
            {
                OpenDefence();
            }
        }

        else if(step == STEP_DEFENCE_DO)
        {
            if(t_just_guard == 0 & t_guard == 0 & guard == false)
            {
                JunMovement();
            }
        }

        else if(step == STEP_DEFENCE_END)
        {
            bool done = JunReturnMovement();
            if(t_close_defense > 0)
            {
                CloseDefence();
            }
            else if(done == true)
            {
                bool end = false;
                if(Status.hp == 0 & tutorial == false)
                {
                    end = true;
                }

                if(end == false)
                {
                    step = STEP_SET_BUFFS;
                    item_step = -1;
                    items_do = new List<int>();
                    DoBuff_befereTurnStart();
                    StepItem();
                }
                else
                {
                    gameover.GameOver(battleBGM, nomalSE, voiceSE);
                    gameObject.SetActive(false);
                }
            }
        }
        
        else if(step == STEP_SELECT_ITEM_WAIT)
        {
            t_item_wait -= 1;
            if(t_item_wait == 0)
            {
                step = STEP_SELECT_ITEM_USE;
                StepItem();
            }
        }

        else if(step == STEP_SELECT_ITEM_END)
        {
            if(t_close_item > 0)
            {
                if(t_close_item > 0)
                {
                    t_close_item -= 1;
                    Move.AccelMoveY(t_close_item, MENU_MOVET, -73, -130, Itemdecided.parent);
                    if(t_close_item == 0)
                    {
                        Itemdecided.parent.SetActive(false);
                    }
                }

                if(t_close_item == 0)
                {
                    JudgeWin();
                }
            }
        }

        else if(step == STEP_WIN)
        {
            if(t_win_message > 0)
            {
                t_win_message -= 1;

                Data.level_parent_alpha.alpha = (60 - t_win_message) / 60f;
                Move.DeccelMoveY(t_win_message, 60, LEVEL_POS + 10f, LEVEL_POS, Data.level_parent);

                if(t_win_message == 0)
                {
                    Data.win_message2.SetActive(true);
                }
            }
            else
            {
                if(Data.win_message2_ani.progress >= 1.0f)
                {
                    GetExp();

                    if (level_up == 0)
                    {
                        Data.win_message_cursor.SetActive(true);
                        step = STEP_WIN_WAIT;
                    }
                    else
                    {
                        step = STEP_LEVEL_UP;
                        step_sub = STEP_LEVEL_UP;
                        t_level_up = TIME_WINMES_DIS_WAIT;
                    }
                }
            }
        }

        else if(step == STEP_WIN_END)
        {
            if(t_win_message > 0)
            {
                t_win_message -= 1;

                if (t_win_message < 30)
                {
                    float alpha = (30 - t_win_message) / 30f;

                    Data.fade_img.color = new Color(0f, 0f, 0f, alpha);
                }

                if(t_win_message == 0)
                {
                    EndBattle();
                }
            }
        }

        else if(step == STEP_SELECT_RUN_WAIT)
        {
            if(t_run > 0)
            {
                t_run -= 1;

                if(t_run == RUN_WAIT_TIME - 5)
                {
                    Data.run_button_selected_image.sprite = Data.run_buttons_sprite[run_selected + 4];
                }
                else if (t_run == RUN_WAIT_TIME - 10)
                {
                    Data.run_button_selected_image.sprite = Data.run_buttons_sprite[run_selected * 2];
                }

                if(t_run <= MENU_MOVET)
                {
                    Move.AccelMoveY(t_run, MENU_MOVET, -82.5f, -139.5f, Data.run_button_selected);
                }

                if(t_run == 0)
                {
                    Data.run_button_selected.SetActive(false);
                    step = STEP_SELECT_RUN_DO;
                    t_run = 90;
                    Data.run_messages[0].SetActive(true);
                }
            }
        }

        else if(step == STEP_SELECT_RUN_DO)
        {
            if (t_run > 0)
            {
                t_run -= 1;

                if (t_run == 0)
                {
                    if(run_selected == 1)
                    {
                        Data.run_messages[1].SetActive(true);
                        run_do = 1;
                        run_speed = 2;
                        nomalSE.Play("dash1");
                        jun_ani.SetInteger("run", 3);
                    }
                    else
                    {
                        int tf = Random.Range(1, 11);
                        if (tf <= 4)
                        {
                            Data.run_messages[1].SetActive(true);
                            run_do = 1;
                            run_speed = 1;
                            nomalSE.Play("dash1");
                            jun_ani.SetInteger("run", 3);
                        }
                        else
                        {
                            Data.run_messages[2].SetActive(true);
                            step = STEP_SELECT_RUN_FAULT;
                            t_run = 80;
                            nomalSE.Play("attack3");
                            jun_ani.SetInteger("run", 2);
                        }
                    }
                    run_selected = -1;
                }
            }
        }

        else if(step == STEP_SELECT_RUN_FAULT)
        {
            if(t_run > 0)
            {
                t_run -= 1;

                if(t_run < 20)
                {
                    float alpha = t_run / 20f;
                    Data.run_messages_alpha.alpha = alpha;

                    if(t_run == 0)
                    {
                        jun_ani.SetInteger("run", 4);
                        Data.run_messages[0].SetActive(false);
                        Data.run_messages[2].SetActive(false);
                        Data.run_messages_alpha.alpha = 1f;
                        step = STEP_DEFENCE_READY;
                        t_oepn_defense = MENU_MOVET2;
                        Control.gameObject.SetActive(true);
                        St_obj_config.OpenST(MENU_MOVET2);
                        Data.battle_walk_box[walk_box_pos].SetActive(true);
                        Data.battle_walk_box_sr[walk_box_pos].color = new Color(1f, 1f, 1f, 0f);
                        step_sub = STEP_SELECT_ATTACK;
                    }
                }
            }
        }

        else if(step == STEP_LEVEL_UP)
        {
            if(t_level_up > 0)
            {
                t_level_up -= 1;

                if(t_level_up >= TIME_WINMES_DIS)
                {
                    //winmeesage消失待機
                }
                else if(t_level_up >= TIME_SROM_APP)
                {
                    Data.win_message_alpha.alpha = (t_level_up - TIME_SROM_APP) / (float)(TIME_WINMES_DIS - TIME_SROM_APP);

                    if(t_level_up == TIME_SROM_APP)
                    {
                        Data.move_second.SetActive(true);
                        StageItem.stay_item_num = 8;
                        StageItem.select_num = 2;
                        StageItem.SelectEndNum(0);
                        StageItem.SelectSetNum(2);
                        St_obj_config.OpenST(TIME_SROM_APP);
                        Move.MoveLocalPosY(Data.levelup_text, LEVELUP_POS + 20f);
                        Data.levelup_text.SetActive(true);
                        Data.levelup_value_text.text = "" + level_up;
                        Data.levelup_text_alpha.alpha = 0f;
                        rom_selected = -1;
                        Move.MoveLocalPosY(Data.move_secondbuttons_roms, pos_move_secondbuttons[1]);
                        Move.MoveLocalPosY(Data.move_secondbuttons_status_roms_level, pos_move_secondbuttons[1]);
                        rom_page = 1;
                        Move.SetSamePos(Data.rom_box, Data.menu_roms[pagenum_min], 0, -3.5f);
                        SelectRom(pagenum_min);
                    }
                }
                else if(t_level_up >= 0)
                {
                    Data.levelup_text_alpha.alpha = (TIME_SROM_APP - t_level_up) / (float)TIME_SROM_APP;
                    StageItem.OpenMenu(t_level_up, TIME_SROM_APP);
                    Move.DeccelMoveX(t_level_up, TIME_SROM_APP, pos_move_HPSP[1], pos_move_HPSP[0], Data.menu_SP);
                    Move.DeccelMoveX(t_level_up, TIME_SROM_APP, -pos_move_HPSP[1], -pos_move_HPSP[0], Data.menu_HP);
                    Move.DeccelMoveY(t_level_up, TIME_SROM_APP, LEVELUP_POS + 20f, LEVELUP_POS, Data.levelup_text);

                    if(t_level_up == 0)
                    {
                        StageItem.can_push_menubutton = true;
                    }
                }
            }
        }
        else if(step == STEP_LEVEL_UP_END)
        {
            if(t_level_up > 0)
            {
                t_level_up -= 1;

                if (t_level_up >= TIME_SROM_DIS)
                {
                    //srom消失待機
                    if(t_level_up == TIME_SROM_DIS)
                    {
                        St_obj_config.CloseST(TIME_SROM_DIS - TIME_WINMES_APP);
                        Data.levelup_text_alpha.alpha = 1f;
                    }
                }
                else if (t_level_up >= TIME_WINMES_APP)
                {
                    int time = t_level_up - TIME_WINMES_APP;
                    int maxtime = TIME_SROM_DIS - TIME_WINMES_APP;
                    StageItem.CloseMenu(time, maxtime, 2);
                    Move.AccelMoveX(time, maxtime, pos_move_HPSP[0], pos_move_HPSP[1], Data.menu_SP);
                    Move.AccelMoveX(time, maxtime, -pos_move_HPSP[0], -pos_move_HPSP[1], Data.menu_HP);
                    Move.AccelMoveY(time, maxtime, LEVELUP_POS, LEVELUP_POS + 20f, Data.levelup_text);
                    Data.levelup_text_alpha.alpha = time / (float)maxtime;

                    if (t_level_up == TIME_WINMES_APP)
                    {
                        Data.levelup_text.SetActive(false);
                        Data.move_second.SetActive(false);
                        StageItem.stay_item_num = -1;
                        StageItem.select_num = 0;
                        StageItem.SelectEndNum(2);
                        rom_selected = -1;
                    }
                }
                else if (t_level_up >= 0)
                {
                    Data.win_message_alpha.alpha = (TIME_WINMES_APP - t_level_up) / (float)TIME_WINMES_APP;

                    if(t_level_up == 0)
                    {
                        Data.win_message_cursor.SetActive(true);
                        step = STEP_WIN_WAIT;
                    }
                }
            }
        }

        //status animation
        if(t_hp_display > 0)
        {
            AnimHP();
        }
        if (t_sp_display > 0)
        {
            AnimSP();
        }
        if (t_buff_display > 0)
        {
            AnimBuff();
        }
        if(t_mybuff_display > 0)
        {
            AnimMyBuff();
        }

        //damaged
        if(t_damaged > 0)
        {
            t_damaged -= 1;

            if(t_damaged == 0)
            {
                if(step == STEP_SET_BUFFS)
                {
                    StepItem();
                }
            }
        }
        if(t_guard > 0)
        {
            t_guard -= 1;

            if(t_guard > GUARD_TIME - KNOCK_BACK_LATE * 3 - 2)
            {
                Transform transform = jun.transform;
                Vector3 pos = transform.localPosition;
                float posX = pos.x;
                int moveX = 0;
                if (t_guard == GUARD_TIME - KNOCK_BACK_LATE)
                {
                    moveX = -1;
                }
                if (t_guard == GUARD_TIME - KNOCK_BACK_LATE * 2)
                {
                    moveX = -1;
                }
                if (t_guard == GUARD_TIME - KNOCK_BACK_LATE * 3)
                {
                    moveX = -1;
                }

                if(posX + moveX >= -98)
                {
                    (float moved_X, float moved_Y) = CheckBoxPos(posX, pos.y, moveX, 0);

                    pos.x = moved_X;
                    transform.localPosition = pos;
                }
            }

            if(t_guard == 0)
            {
                jun_ani.SetBool("guard_damaged", false);
            }
        }
        if(t_just_guard > 0)
        {
            t_just_guard -= 1;

            if(t_just_guard == 0)
            {
                guard_obj.SetActive(false);
            }
        }

        //run
        if(run_do > 0)
        {
            Transform transform = jun.transform;
            Vector3 pos = transform.localPosition;
            if (pos.x > -320)
            {
                pos.x -= run_speed;
            }
            transform.localPosition = pos;
            if(run_do == 1)
            {
                if(run_speed == 2)
                {
                    if(pos.x < -143)
                    {
                        run_do = 2;
                        Data.win_message_cursor.SetActive(true);
                    }
                }
                else
                {
                    if (pos.x < -103)
                    {
                        run_do = 2;
                        Data.win_message_cursor.SetActive(true);
                    }
                }
            }
        }

        //tutorial
        if(tutorial == true)
        {
            if(tutorial_time > 0)
            {
                tutorial_time -= 1;

                if (tutorial_time > 20)
                {
                    tutorial_text_alpha[tutorial_step].alpha = (tutorial_time - 20) / 20f;
                    Move.MoveLocalPosPlusY(tutorial_text_obj[tutorial_step], -1f);
                }
                else if (tutorial_time == 20)
                {
                    tutorial_text_obj[tutorial_step].SetActive(false);
                    tutorial_step += 1;

                    if (tutorial_step == 3)
                    {
                        tutorial_time = 0;
                    }
                    else
                    {
                        tutorial_text_obj[tutorial_step].SetActive(true);

                        if(tutorial_step == 1)
                        {
                            guard_button.SetActive(true);
                            tutorial_press.SetActive(true);
                            Move.SetSamePos(tutorial_press, guard_button, 0, 50);
                        }
                        else if(tutorial_step == 2)
                        {
                            tutorial_press.SetActive(false);
                        }
                    }
                }
                else
                {
                    tutorial_text_alpha[tutorial_step].alpha = (20 - tutorial_time) / 20f;
                    Move.MoveLocalPosPlusY(tutorial_text_obj[tutorial_step], 1f);
                }
            }
        }
    }

    void Update()
    {
        if(step == STEP_SELECT_FIRST)
        {
            if (Control.GetButtonDownLeft() & tutorial == false)
            {
                act_num = act_num - 1;
                if(act_num < 0)
                {
                    act_num = ACT_FIRST_BUTTON;
                }
                Move.SetSamePos(Data.button_first_cursor, Data.menu_button_firstselect[act_num], 0, 0);
                nomalSE.Play("select1");
            }
            else if (Control.GetButtonDownRight() & tutorial == false)
            {
                act_num = act_num + 1;
                if (act_num > ACT_FIRST_BUTTON)
                {
                    act_num = 0;
                }
                Move.SetSamePos(Data.button_first_cursor, Data.menu_button_firstselect[act_num], 0, 0);
                nomalSE.Play("select1");
            }
            else if (Control.GetButtonDownDecide())
            {
                PushFirstButton(act_num);
            }
        }
        else if (step == STEP_SELECT_ATTACK | step == STEP_SELECT_ITEM | step == STEP_SELECT_RUN | step == STEP_SELECT_CHANGEPAGE | step == STEP_LEVEL_UP)
        {
            if (Control.GetButtonDownTab())
            {
                if (step != STEP_LEVEL_UP)
                {
                    PushArrowButton(0);
                }
            }

            else if (Control.GetButtonDownLeft())
            {
                if (step == STEP_SELECT_ITEM)
                {
                    if (items_inbattle.Count > 0)
                    {
                        int selected = Control.GetSelect_inPage(item_selected, item_page, items_inbattle.Count, -1, pagenum_min, pagenum_max);
                        if (item_selected != selected)
                        {
                            PushItem(selected, false);
                        }
                    }
                }
                else if (step == STEP_SELECT_ATTACK)
                {
                    int selected = Control.GetSelect_inPage(rom_selected, rom_page, Status.myskill.Count, -1, pagenum_min, pagenum_max);
                    if (rom_selected != selected)
                    {
                        PushRom(selected);
                    }
                }
                else if (step == STEP_SELECT_RUN)
                {
                    int selected = Control.GetSelect_inPage(run_selected, 1, 2, -1, 0, 1);
                    if (run_selected != selected)
                    {
                        PushRun(selected);
                    }
                }
                else if (step == STEP_LEVEL_UP)
                {
                    int selected = Control.GetSelect_inPage(rom_selected, rom_page, Status.status_roms_level.Count, -1, pagenum_min, pagenum_max);
                    if (rom_selected != selected)
                    {
                        PushRom(selected);
                    }
                }
            } //left

            else if (Control.GetButtonDownRight())
            {
                if (step == STEP_SELECT_ITEM)
                {
                    if (items_inbattle.Count > 0)
                    {
                        int selected = Control.GetSelect_inPage(item_selected, item_page, items_inbattle.Count, 1, pagenum_min, pagenum_max);
                        if (item_selected != selected)
                        {
                            PushItem(selected, false);
                        }
                    }
                }
                else if (step == STEP_SELECT_ATTACK)
                {
                    int selected = Control.GetSelect_inPage(rom_selected, rom_page, Status.myskill.Count, 1, pagenum_min, pagenum_max);
                    if (rom_selected != selected)
                    {
                        PushRom(selected);
                    }
                }
                else if (step == STEP_SELECT_RUN)
                {
                    int selected = Control.GetSelect_inPage(run_selected, 1, 2, 1, 0, 1);
                    if (run_selected != selected)
                    {
                        PushRun(selected);
                    }
                }
                else if (step == STEP_LEVEL_UP)
                {
                    int selected = Control.GetSelect_inPage(rom_selected, rom_page, Status.status_roms_level.Count, 1, pagenum_min, pagenum_max);
                    if (rom_selected != selected)
                    {
                        PushRom(selected);
                    }
                }
            } //right

            else if (Control.GetButtonDownUp())
            {
                if(step == STEP_SELECT_ITEM)
                {
                    PushArrowButton(-1);
                }
            } //up

            else if (Control.GetButtonDownDown())
            {
                if (step == STEP_SELECT_ITEM)
                {
                    PushArrowButton(1);
                }
            } //down

            else if (Control.GetButtonDownDecide())
            {
                if (step == STEP_SELECT_ITEM)
                {
                    PushItem(item_selected, false);
                }
                else if (step == STEP_SELECT_ATTACK)
                {
                    PushRom(rom_selected);
                }
                else if (step == STEP_SELECT_RUN)
                {
                    PushRun(run_selected);
                }
                else if (step == STEP_LEVEL_UP)
                {
                    PushRom(rom_selected);
                }
            }
        }

        else if (step == STEP_SELECT_ATTACK_READY) //attack_ready
        {
            if (Input.GetMouseButtonDown(0) | Control.GetButtonDownDecide())
            {
                Taptostart.Tapped();
                nomalSE.Play("select3");
                step = STEP_SELECT_ATTACK_DO;
            }
        }
        else if(step == STEP_DEFENCE_DO)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetGuard();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                DesetGuard();
            }

            if (guard == false) //ガード配置
            {
                if (button_guard == true & t_damaged == 0 & t_just_guard == 0 & debuff_value[0] == 0)
                {
                    JunWalkAnimation(0);
                    jun_ani.SetBool("guard", true);
                    guard_obj.SetActive(true);
                    guard = true;
                }
            }
            else  //ガード削除
            {
                if (button_guard == false & t_guard == 0)
                {
                    jun_ani.SetBool("guard", false);
                    guard_ani.SetTrigger("del");
                    t_just_guard = JUSTGUARD_TIME;
                    guard = false;
                }
            }
        }
        else if(step == STEP_WIN)
        {
            if (Input.GetMouseButtonDown(0) | Control.GetButtonDownDecide())
            {
                Data.win_message1_ani.Skip();
                Data.win_message2_ani.Skip();
                Data.win_message2.SetActive(true);
                Data.level_parent.SetActive(true);
                Move.MoveLocalPosY(Data.level_parent, LEVEL_POS);
                Data.level_parent_alpha.alpha = 1.0f;
                GetExp();

                if (level_up == 0)
                {
                    step = STEP_WIN_WAIT;
                    Data.win_message_cursor.SetActive(true);
                }
                else
                {
                    step = STEP_LEVEL_UP;
                    t_level_up = TIME_WINMES_DIS_WAIT;
                    step_sub = STEP_LEVEL_UP;
                }
            }
        }
        else if (step == STEP_WIN_WAIT)
        {
            if (Input.GetMouseButtonDown(0) | Control.GetButtonDownDecide())
            {
                Data.win_message_cursor.SetActive(false);
                step = STEP_WIN_END;
                t_win_message = 60;
                Data.fade.SetActive(true);
                Data.fade_img.color = new Color(0f, 0f, 0f, 0f);
            }
        }

        if(run_do == 2)
        {
            if (Input.GetMouseButtonDown(0) | Control.GetButtonDownDecide())
            {
                Data.win_message_cursor.SetActive(false);
                step = STEP_WIN_END;
                t_win_message = 60;
                Data.fade.SetActive(true);
                Data.fade_img.color = new Color(0f, 0f, 0f, 0f);
            }
        }
    }


    //move
    private void OpenFirstButtons()
    {
        t_open_firstbuttons -= 1;

        if(first_open == false)
        {
            Move.DeccelMoveX(t_open_firstbuttons, MENU_MOVET3, pos_move_HPSP[1], pos_move_HPSP[0], Data.menu_SP);
            Move.DeccelMoveX(t_open_firstbuttons, MENU_MOVET3, -pos_move_HPSP[1], -pos_move_HPSP[0], Data.menu_HP);
            Move.DeccelMoveY(t_open_firstbuttons, MENU_MOVET3, pos_move_firstbuttons[1], pos_move_firstbuttons[0], Data.move_firstbuttons);
            float alpha = t_open_firstbuttons / (float)MENU_MOVET3;
            Data.enemy_names_alpha.alpha = alpha;
        }
        else
        {
            if(step == STEP_OPEN)
            {
                Move.DeccelMoveY(t_open_firstbuttons, MENU_MOVET2, pos_move_firstbuttons[1], pos_move_firstbuttons[0], Data.move_firstbuttons);
            }
            else
            {
                Move.DeccelMoveY(t_open_firstbuttons, MENU_MOVET, pos_move_firstbuttons[1], pos_move_firstbuttons[0], Data.move_firstbuttons);
            }
        }

        if(t_open_firstbuttons == 0)
        {
            Data.buttons_target.SetActive(true);
            step_sub = 0;
            step = STEP_SELECT_FIRST;
            act_num_max = ACT_FIRST_BUTTON;
            voiceSE.Stop();

            if (first_open == false)
            {
                Data.enemy_names_alpha.alpha = 1f;
                Data.enemy_names.SetActive(false);
                first_open = true;
            }
        }
    }
    private void CloseFirstButtons()
    {
        t_close_firstbuttons -= 1;
        Move.AccelMoveY(t_close_firstbuttons, MENU_MOVET, pos_move_firstbuttons[0], pos_move_firstbuttons[1], Data.move_firstbuttons);

        if (t_close_firstbuttons == 8)
        {
            t_open_secondbuttons = MENU_MOVET;
            Data.move_second.SetActive(true);

            if(step_sub == STEP_SELECT_ATTACK)
            {
                Data.move_secondbuttons_roms.SetActive(true);
                Data.move_secondbuttons_roms_sp.SetActive(true);
                ChangeRompageFigure();
                PushRom(pagenum_min);
            }
            else if (step_sub == STEP_SELECT_ITEM)
            {
                Data.move_secondbuttons_items.SetActive(true);
                Data.move_secondbuttons_items_value.SetActive(true);
                ChangeItempageFigure(item_page, item_page_max);
                PushItem(pagenum_min, false);
            }
            else if(step_sub == STEP_SELECT_RUN)
            {
                Data.move_secondbuttons_run.SetActive(true);
                PushRun(0);
            }
        }

        if (t_close_firstbuttons == 0)
        {
            Data.move_firstbuttons.SetActive(false);
        }
    }
    private void OpenSecondButtons()
    {
        t_open_secondbuttons -= 1;
        Move.DeccelMoveX(t_open_secondbuttons, MENU_MOVET, pos_move_returnbuttons[1], pos_move_returnbuttons[0], Data.menu_button_return);

        if (step_sub == STEP_SELECT_ATTACK)
        {
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_box);
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_roms);
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_roms_sp);
        }
        else if (step_sub == STEP_SELECT_ITEM)
        {
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_box);
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_items);
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_items_value);
        }
        else if(step_sub == STEP_SELECT_RUN)
        {
            Move.DeccelMoveY(t_open_secondbuttons, MENU_MOVET, pos_move_secondbuttons[1], pos_move_secondbuttons[0], Data.move_secondbuttons_run);
        }

        if (t_open_secondbuttons == 0)
        {
            step = step_sub;
        }
    }
    private void CloseSecondButtons()
    {
        t_close_secondbuttons -= 1;

        if(step_sub == STEP_SELECT_ATTACK)
        {
            Move.AccelMoveX(t_close_secondbuttons, MENU_MOVET, pos_move_returnbuttons[0], pos_move_returnbuttons[1], Data.menu_button_return);
            Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_box);
            Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_roms);
            Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_roms_sp);
        }
        else if (step_sub == STEP_SELECT_ITEM)
        {
            Move.AccelMoveX(t_close_secondbuttons, MENU_MOVET, pos_move_returnbuttons[0], pos_move_returnbuttons[1], Data.menu_button_return);
            Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_box);
            Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_items);
            Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_items_value);
        }
        else if(step_sub == STEP_SELECT_RUN)
        {
            if(step == STEP_SELECT_RUN_WAIT)
            {
                Move.AccelMoveX(t_close_secondbuttons, MENU_MOVET2, pos_move_returnbuttons[0], pos_move_returnbuttons[1], Data.menu_button_return);
                Move.AccelMoveX(t_close_secondbuttons, MENU_MOVET2, pos_move_HPSP[0], pos_move_HPSP[1], Data.menu_SP);
                Move.AccelMoveX(t_close_secondbuttons, MENU_MOVET2, -pos_move_HPSP[0], -pos_move_HPSP[1], Data.menu_HP);
                Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET2, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_run);
            }
            else
            {
                Move.AccelMoveX(t_close_secondbuttons, MENU_MOVET, pos_move_returnbuttons[0], pos_move_returnbuttons[1], Data.menu_button_return);
                Move.AccelMoveY(t_close_secondbuttons, MENU_MOVET, pos_move_secondbuttons[0], pos_move_secondbuttons[1], Data.move_secondbuttons_run);
            }
        }

        if (t_close_secondbuttons == 8)
        {
            if(step == STEP_SELECT_MOVE)
            {
                t_open_firstbuttons = MENU_MOVET;
                Data.move_firstbuttons.SetActive(true);
            }
        }

        if (t_close_secondbuttons == 0)
        {
            Data.move_second.SetActive(false);

            if(step == STEP_SELECT_MOVE)
            {
                step = STEP_SELECT_FIRST;
                act_num_max = ACT_FIRST_BUTTON;
                rom_selected = -1;
                item_selected = -1;
                Data.rom_box.SetActive(false);
            }
            else if(step == STEP_SELECT_ATTACK_WAIT)
            {
                Data.menu_roms[rom_selected].SetActive(true);
            }
            else if (step == STEP_SELECT_ITEM_WAIT)
            {
                Data.menu_items[item_selected].SetActive(true);
            }

            if (step_sub == STEP_SELECT_ATTACK)
            {
                Data.move_secondbuttons_roms.SetActive(false);
                Data.move_secondbuttons_roms_sp.SetActive(false);
            }
            else if (step_sub == STEP_SELECT_ITEM)
            {
                Data.move_secondbuttons_items.SetActive(false);
                Data.move_secondbuttons_items_value.SetActive(false);
            }
        }
    }
    public void OpenMessage()
    {
        t_open_message -= 1;
        Move.DeccelMoveY(t_open_message, MENU_MOVET, pos_move_message[0], pos_move_message[1], Data.menu_item_message);
    }
    private void CloseMessage()
    {
        t_close_message -= 1;
        Move.DeccelMoveY(t_close_message, MENU_MOVET, pos_move_message[1], pos_move_message[0], Data.menu_item_message);

        if(t_close_message == 0)
        {
            Data.menu_item_message.SetActive(false);
        }
    }
    private void CloseSkill()
    {
        t_close_skill -= 1;
        Move.AccelMoveY(t_close_skill, MENU_MOVET, 80, 0, Data.skills[skill_selected]);

        if(t_close_skill == 0)
        {
            Data.skills[skill_selected].SetActive(false);
        }
    }
    private void OpenDefence()
    {
        if(t_oepn_defense > 0)
        {
            t_oepn_defense -= 1;

            if (step_sub == STEP_SELECT_ATTACK)
            {
                Move.DeccelMoveX(t_oepn_defense, MENU_MOVET2, pos_move_HPSP[1], pos_move_HPSP[0], Data.menu_SP);
                Move.DeccelMoveX(t_oepn_defense, MENU_MOVET2, -pos_move_HPSP[1], -pos_move_HPSP[0], Data.menu_HP);
            }

            Data.battle_walk_box_sr[walk_box_pos].color = new Color(1f, 1f, 1f, (MENU_MOVET2 - t_oepn_defense) / (float)MENU_MOVET2);
            hp_line_alpha.alpha = t_oepn_defense / (float)MENU_MOVET2;
            Control.controller_alpha.alpha = ((MENU_MOVET2 - t_oepn_defense) / (float)MENU_MOVET2) * 0.6f;

            if (t_oepn_defense == 0)
            {
                t_oepn_defense = -1;
            }
        }

        if (t_oepn_defense == -1)
        {
            string ani_name = jun_ani.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            ani_name = ani_name.Substring(0, 15);
            if(ani_name == "battle_jun_idle")
            {
                jun_ani.SetInteger("run", 4);
                jun_ani.ResetTrigger("attack_end");
                jun_ani.ResetTrigger("walk_end");
                Enemy.enemy_attack = true;
                step = STEP_DEFENCE_DO;
            }
        }
    }
    private void CloseDefence()
    {
        t_close_defense -= 1;
        float alpha = t_close_defense / (float)MENU_MOVET;
        Data.battle_walk_box_sr[walk_box_pos].color = new Color(1f, 1f, 1f, alpha);
        Control.controller_alpha.alpha = alpha * 0.6f;
        hp_line_alpha.alpha = (MENU_MOVET - t_close_defense) / (float)MENU_MOVET;


        if (a_pos.Count != 0)
        {
            foreach (int box_pos in a_pos)
            {
                a_floor_sr[box_pos].color = new Color(1f, 1f, 1f, t_close_defense / (float)MENU_MOVET);
            }
        }

        if(t_close_defense == 0)
        {
            Data.battle_walk_box[walk_box_pos].SetActive(false);
            Data.battle_walk_box_sr[walk_box_pos].color = new Color(1f, 1f, 1f, 1f);
            Control.gameObject.SetActive(false);

            if (a_pos.Count != 0)
            {
                foreach (int box_pos in a_pos)
                {
                    a_floor_sr[box_pos].color = new Color(1f, 1f, 1f, 1f);
                    a_floor[box_pos].SetActive(false);
                }
                a_pos = new List<int>();
            }
            Enemy.a_pos_all = new List<int>() { 0, 1, 2, 3, 4, 5 };
        }
    }

    //buttons
    public void PushEnemy(int num)
    {
        if (step == STEP_SELECT_ATTACK)
        {
            if (pre_target_enemy >= 0)
            {
                if (Enemy.battle_active_enemy_num.Contains(num))
                {
                    if(target_enemy != num)
                    {
                        target_enemy = num;
                        nomalSE.Play("select9");
                        GetTarget(rom_selected);
                    }
                }
            }
        }
    }
    public void PushDownFirstButton(int num)
    {
        if(step == STEP_SELECT_FIRST)
        {
            Data.button_first[num].sprite = Data.button_first_pushdown[num];
        }
    }
    public void PushUpFirstButton(int num)
    {
        if(step >= STEP_SELECT_FIRST)
        {
            Data.button_first[num].sprite = Data.button_first_pushup[num];
        }
    }
    public void PushFirstButton(int num)
    {
        nomalSE.Play("select9");
        step_sub = num + 2;
        step = STEP_SELECT_MOVE;
        t_close_firstbuttons = MENU_MOVET;
    }
    public void PushDownArrowButton(int num)
    {
        if (step == STEP_SELECT_ATTACK | step == STEP_SELECT_ITEM | step == STEP_SELECT_RUN | step == STEP_SELECT_CHANGEPAGE)
        {
            int i = num;
            if(i > 1)
            {
                i = num - 1;
            }

            if(i == 0)
            {
                if(step != STEP_SELECT_CHANGEPAGE)
                {
                    Data.button_second_arrow[num].sprite = Data.button_second_arrow_pushdown[i];
                }
            }
            else
            {
                Data.button_second_arrow[num].sprite = Data.button_second_arrow_pushdown[i];
            }
        }
    }
    public void PushUpArrowButton(int num)
    {
        int i = num;
        if (i > 1)
        {
            i = num - 1;
        }
        Data.button_second_arrow[num].sprite = Data.button_second_arrow_pushup[i];
    }
    public void PushArrowButton(int num)
    {
        if (num == 0)
        {
            if (step != STEP_SELECT_CHANGEPAGE)
            {
                nomalSE.Play("select10");
                step_sub = step;
                t_close_secondbuttons = MENU_MOVET;
                step = STEP_SELECT_MOVE;

                if (step_sub == STEP_SELECT_ATTACK)
                {
                    DeselectRom(false, true);
                }
                else if (step_sub == STEP_SELECT_ITEM)
                {
                    DeselectItem(false, true);
                }
                else if (step_sub == STEP_SELECT_RUN)
                {
                    DeselectRun(false);
                }
            }
        }
        else
        {
            if (num == 1)
            {
                if (step == STEP_SELECT_ATTACK)
                {
                    if (rom_page > 1)
                    {
                        ChangePage(-1);
                    }
                }
                else if (step == STEP_SELECT_ITEM)
                {
                    if(item_page_max != 1)
                    {
                        ChangePage(1);
                    }
                }
            }
            else
            {
                if (step == STEP_SELECT_ATTACK)
                {
                    if (rom_page < rom_page_max)
                    {
                        ChangePage(1);
                    }
                }
                else if (step == STEP_SELECT_ITEM)
                {
                    if (item_page_max != 1)
                    {
                        ChangePage(-1);
                    }
                }
            }
        }
    }
    public void PushUpRom(int num)
    {
        if(StageItem.select_num == 2)
        {
            int index = (num - pagenum_min) + (StageItem.status_rom_page - 1) * pagenum_max;
            Data.rom_image[num].sprite = Data.rom_status_speite[index];

            if (StageItem.stay_item_num == 8)
            {
                if (Status.status_roms_level[index] == 5)
                {
                    Data.rom_image[num].sprite = Data.rom_status_speite[index + 4];
                }
            }
        }
        else
        {
            (int skill, int level) = SearchSkill(num - pagenum_min);
            int level_plus = 0;
            int level_maxed = 0;

            if (StageItem.stay_item_num == 8 & StageItem.select_num == 1)
            {
                if(level < 3)
                {
                    if (rom_selected == num)
                    {
                        level_plus = 1;
                    }
                }
                else
                {
                    level_maxed = 1;
                }
            }

            if(step == STEP_SELECT_ATTACK)
            {
                if (skill == 4)
                {
                    Debug.Log(true);
                    int use_sp = Status.skills_sp[11 + level];
                    if (Status.sp < use_sp)
                    {
                        level_maxed = 1;
                    }
                }
            }

            Data.rom_image[num].sprite = Data.rom_sprite[(level + level_plus - 1) * 2 + level_maxed];
            Data.rom_label_image[num].sprite = Data.rom_label_sprite[skill * 2 + level_maxed];
        }
    }
    public void PushDownRom(int num)
    {
        if (step == STEP_SELECT_ATTACK | StageItem.can_push_menubutton == true)
        {
            if (num != rom_selected)
            {
                if (step == STEP_SELECT_ATTACK | StageItem.select_num == 1)
                {
                    (int skill, int level) = SearchSkill(num - pagenum_min);

                    Data.rom_image[num].sprite = Data.rom_sprite[(level - 1) * 2 + 1];
                    Data.rom_label_image[num].sprite = Data.rom_label_sprite[skill * 2 + 1];
                }
                else
                {
                    int index = (num - pagenum_min) + (StageItem.status_rom_page - 1) * pagenum_max;
                    Data.rom_image[num].sprite = Data.rom_status_speite[index + 4];
                }
            }
        }
    }
    public void PushRom(int num)
    {
        if (step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK | StageItem.can_push_menubutton == true)
        {
            bool can_push = true;
            if (StageItem.stay_item_num == 8)
            {
                if(StageItem.select_num == 1)
                {
                    (int skill, int level) = SearchSkill(num - pagenum_min);
                    if(level == 3)
                    {
                        can_push = false;
                    }
                }
            }

            if(step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK)
            {
                (int skill, int level) = SearchSkill(num - pagenum_min);
                if (skill == 4)
                {
                    int use_sp = Status.skills_sp[11 + level];
                    if (Status.sp < use_sp)
                    {
                        can_push = false;
                    }
                }
            }

            if (can_push == true)
            {
                if (num != rom_selected)
                {
                    Move.SetSamePos(Data.rom_box, Data.menu_roms[num], 0, -3.5f);
                    SelectRom(num);
                }
                else
                {
                    if (step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK)
                    {
                        nomalSE.Play("select2");
                        Data.menu_roms[num].SetActive(false);
                        DeselectRom(true, true);
                    }
                    else if(StageItem.stay_item_num == 8)
                    {
                        if (StageItem.select_num == 2)
                        {
                            int index = (num - pagenum_min) + (StageItem.status_rom_page - 1) * pagenum_max;
                            if (Status.status_roms_level[index] != 5)
                            {
                                DeselectRom(true, false);
                            }
                        }
                        else
                        {
                            DeselectRom(true, false);
                        }
                    }
                }
            }
        }
    }
    public void PushDownItem(int num)
    {
        if (step == STEP_SELECT_ITEM | StageItem.can_push_menubutton == true)
        {
            int index_count = (item_page - 1) * pagenum_max + (num - pagenum_min);
            if(index_count < items_inbattle.Count)
            {
                if (num != item_selected)
                {
                    Data.item_button_image[num].sprite = Data.item_button_sprite[1];
                }
            }
        }
    }
    public void PushUpItem(int num)
    {
        Data.item_button_image[num].sprite = Data.item_button_sprite[0];
    }
    public void PushItem(int num, bool change_page)
    {
        if (step == STEP_SELECT_ITEM | step_sub == STEP_SELECT_ITEM)
        {
            int index_count = (item_page - 1) * pagenum_max + (num - pagenum_min);
            if (index_count < items_inbattle.Count)
            {
                if (num != item_selected)
                {
                    Move.SetSamePos(Data.item_box, Data.menu_items[num], 0, 0);
                    SelectItem(num, true, change_page);
                }
                else
                {
                    nomalSE.Play("select2");
                    DeselectItem(true, true);
                }
            }
        }

        else if(StageItem.can_push_menubutton == true)
        {
            int index_count = (StageItem.item_page - 1) * pagenum_max + (num - pagenum_min);
            if(index_count < StageItem.items_all.Count)
            {
                if (num != item_selected)
                {
                    Move.SetSamePos(Data.item_box, Data.menu_items[num], 0, 0);
                    SelectItem(num, false, change_page);
                }
                else
                {
                    DeselectItem(true, false);
                }
            }
        }
    }
    public void PushDownRun(int num)
    {
        if(step == STEP_SELECT_RUN)
        {
            if(num != run_selected)
            {
                Data.run_buttons_image[num].sprite = Data.run_buttons_sprite[num * 2 + 1];
            }
        }
    }
    public void PushUpRun(int num)
    {
        int plus_num = 0;
        if(num == 1)
        {
            if(Status.sp < 30)
            {
                plus_num = 1;
            }
        }
        Data.run_buttons_image[num].sprite = Data.run_buttons_sprite[num * 2 + plus_num];
    }
    public void PushRun(int num)
    {
        if(step == STEP_SELECT_RUN | step_sub == STEP_SELECT_RUN)
        {
            bool can_push = true;
            if(num == 1)
            {
                if (Status.sp < 30)
                {
                    can_push = false;
                }
            }

            if (can_push == true)
            {
                if (run_selected != num)
                {
                    SelectRun(num);
                }
                else
                {
                    nomalSE.Play("select2");
                    DeselectRun(true);
                }
            }
        }
    }

    //rom
    public void SetRom(int page, bool battle = true)
    {
        int rom_index_max = Status.myskill.Count;
        int romnum = 0 + pagenum_min;
        for(int i = (page - 1) * pagenum_max; i < page * pagenum_max; i++)
        {
            GameObject rom = Data.menu_roms[romnum];

            if (i < rom_index_max)
            {
                int skill = Status.myskill[i];
                int level = Status.myskill_level[skill];
                Image[] rom_sp_images = new Image[3] { Data.rom_sp[romnum * 3], Data.rom_sp[romnum * 3 + 1], Data.rom_sp[romnum * 3 + 2] };

                rom.SetActive(true);
                Data.menu_sps[romnum].SetActive(true);
                Data.rom_label_image[romnum].gameObject.SetActive(true);
                Data.rom_label_image[romnum].sprite = Data.rom_label_sprite[skill * 2];

                int level_plus = 0;
                if(rom_selected == romnum)
                {
                    if (StageItem.stay_item_num == 8 & StageItem.select_num == 1)
                    {
                        level_plus = 1;
                        Fig.ChangeColorFigure(rom_sp_images, 1f, 0f, 1f);
                    }
                }
                else
                {
                    Fig.ChangeColorFigure(rom_sp_images, 1f, 1f, 1f);
                }

                Data.rom_image[romnum].sprite = Data.rom_sprite[(level + level_plus - 1) * 2];
                Fig.ChangeFigure(Status.skills_sp[skill * 3 + (level + level_plus - 1)], rom_sp_images, true, DataNomal.figure_mini);

                if (StageItem.stay_item_num == 8 & StageItem.select_num == 1)
                {
                    if(level == 3)
                    {
                        Data.rom_label_image[romnum].sprite = Data.rom_label_sprite[skill * 2 + 1];
                        Data.rom_image[romnum].sprite = Data.rom_sprite[(level - 1) * 2 + 1];
                    }
                }

                if(battle == true)
                {
                    if(skill == 4)
                    {
                        int use_sp = Status.skills_sp[11 + level];
                        if (Status.sp < use_sp)
                        {
                            Data.rom_label_image[romnum].sprite = Data.rom_label_sprite[skill * 2 + 1];
                            Data.rom_image[romnum].sprite = Data.rom_sprite[(level - 1) * 2 + 1];
                        }
                    }
                }
            }
            else
            {
                rom.SetActive(false);
                Data.menu_sps[romnum].SetActive(false);
            }

            romnum += 1;
        }
    }
    public void DeselectRom(bool decide_rom, bool battle)
    {
        ChangeText(true);

        if (rom_selected != -1)
        {
            if(battle == true)
            {
                t_open_message = 0;
                t_close_message = MENU_MOVET;
            }
            else
            {
                if(step == STEP_LEVEL_UP)
                {
                    t_open_message = 0;
                    t_close_message = MENU_MOVET;
                }
                else
                {
                    StageItem.t_open_message = 0;
                    StageItem.t_close_message = MENU_MOVET;
                }
            }
            
            Data.rom_box.SetActive(false);

            if(decide_rom == true)
            {
                if(step == STEP_SELECT_ATTACK)
                {
                    Data.rom_decided.SetActive(true);
                    Move.SetSamePos(Data.rom_decided, Data.menu_roms[rom_selected], 0, 0);
                    t_close_secondbuttons = MENU_MOVET;

                    int dec_sp = Status.sp - SearchSP();
                    if (dec_sp < 0)
                    {
                        int dec_hp = Status.hp + dec_sp;
                        if (dec_hp < 0)
                        {
                            dec_hp = 0;
                        }
                        Status.hp = dec_hp;
                        dec_sp = 0;
                    }
                    Status.sp = dec_sp;

                    step = STEP_SELECT_ATTACK_WAIT;
                    (int skill, int level) = SearchSkill(rom_selected - pagenum_min);
                    skill_selected = skill;
                    skill_level = level;
                    Romdecided.SetRomDecided(skill, level, -70, -60, -92);
                    Enemy.CloseAllHP();
                    target_enemy = pre_target_enemy;
                    Data.buttons_target.SetActive(false);
                    Data.buff_display.SetActive(false);
                    t_mybuff_display = 0;

                    St_obj_config.ChangeST(-1);

                    if (skill == 2)
                    {
                        skill3_okureiman.SetActive(true);
                    }
                }
                else if (StageItem.stay_item_num == 8)
                {
                    if (StageItem.select_num == 2)
                    {
                        StageItem.UpgradeStatusRom(rom_selected);
                        rom_selected = -1;
                        jun_ani.SetTrigger("levelup");

                        level_up -= 1;
                        Data.levelup_value_text.text = "" + level_up;

                        if (level_up == 0)
                        {
                            StageItem.can_push_menubutton = false;
                            step = STEP_LEVEL_UP_END;
                            t_level_up = TIME_SROM_DIS_WAIT;
                        }
                    }
                    else if (StageItem.select_num == 1)
                    {
                        StageItem.UpgradeRom(rom_selected);
                        rom_selected = -1;
                        SetRom(StageItem.rom_page, false);
                    }
                }
            }
            else
            {
                rom_selected = -1;
            }

            if(battle == true)
            {
                ReleaseTarget(true);
                St_obj_config.SetST();
            }
        }

        SetSP(-1, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
    }
    private void SelectRom(int num)
    {
        if (rom_selected == -1)
        {
            Data.rom_box.SetActive(true);
            if (step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK)
            {
                t_open_message = MENU_MOVET;
                t_close_message = 0;
            }
            else
            {
                if(step == STEP_LEVEL_UP)
                {
                    t_open_message = MENU_MOVET;
                    t_close_message = 0;
                }
                else
                {
                    StageItem.t_open_message = MENU_MOVET;
                    StageItem.t_close_message = 0;
                }
            }
            Data.menu_item_message.SetActive(true);
        }
        else
        {
            nomalSE.Play("select1");
        }

        rom_selected = num;

        if (StageItem.stay_item_num == 8)
        {
            if(StageItem.select_num == 2)
            {
                StageItem.SetStatusRomValue(num);
            }
            else if(StageItem.select_num == 1)
            {
                SetRom(StageItem.rom_page, false);
            }
        }

        if(step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK)
        {
            int reserveSP = SearchSP();
            SetSP(reserveSP, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
            GetTarget(num);
            St_obj_config.SetST(1);
        }

        ChangeText(false);
    }
    private void ChangeText(bool none)
    {
        string name = "";
        string message = "";
        if(none == false)
        {
            if(step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK)
            {
                int i = rom_selected - pagenum_min;
                int romindex = (rom_page - 1) * pagenum_max + i;
                int skill = Status.myskill[romindex];
                name = Status.skills_name[skill];
                message = Status.skills_message[skill];
            }
            else if (step == STEP_SELECT_ITEM | step_sub == STEP_SELECT_ITEM)
            {
                int index = items_inbattle[(item_page - 1) * pagenum_max + (item_selected - pagenum_min)];
                name = Status.items_name[index];
                message = Status.items_message[index];
            }
            else if(step == STEP_SELECT_RUN | step_sub == STEP_SELECT_RUN)
            {
                name = Status.run_name[run_selected];
                message = Status.run_message[run_selected];
            }
            else if (StageItem.can_push_menubutton == true | step == STEP_LEVEL_UP)
            {
                if(StageItem.select_num == 0)
                {
                    int index = StageItem.items_all[(StageItem.item_page - 1) * pagenum_max + (item_selected - pagenum_min)];
                    name = Status.items_name[index];
                    message = Status.items_message[index];
                }
                else if(StageItem.select_num == 1)
                {
                    int i = rom_selected - pagenum_min;
                    int romindex = (StageItem.rom_page - 1) * pagenum_max + i;
                    int skill = Status.myskill[romindex];
                    name = Status.skills_name[skill];
                    message = Status.skills_message[skill];
                }
                else if(StageItem.select_num == 2)
                {
                    int index = (rom_selected - pagenum_min) + (StageItem.status_rom_page - 1) * pagenum_max;
                    name = Status.status_roms_name[index];

                    if (StageItem.stay_item_num == 8)
                    {
                        message = Status.status_roms_message_up[index];
                    }
                    else
                    {
                        message = Status.status_roms_message[index];
                    }
                }
            }
        }

        Data.item_text_name.text = name;
        Data.item_text_message.text = message;
    }

    //item
    public void DeselectItem(bool decide_item, bool battle)
    {
        ChangeText(true);

        if (item_selected != -1)
        {
            if(battle == true)
            {
                t_open_message = 0;
                t_close_message = MENU_MOVET;
            }
            else
            {
                StageItem.t_open_message = 0;
                StageItem.t_close_message = MENU_MOVET;
            }

            Data.item_box.SetActive(false);

            if (decide_item == true)
            {
                if (battle == true)
                {
                    nomalSE.Play("select2");
                    t_close_secondbuttons = MENU_MOVET;
                    t_item_wait = ITEM_WAIT_TIME;
                    jun_ani.SetBool("item", true);
                    step = STEP_SELECT_ITEM_WAIT;
                    target_enemy = pre_target_enemy;
                    Data.buttons_target.SetActive(false);
                    int item_index = items_inbattle[(item_page - 1) * pagenum_max + (item_selected - pagenum_min)];
                    Itemdecided.SetItemDecided(item_selected, item_index);
                    Status.items_value[item_index] -= 1;
                    UseItem(item_index);
                    Enemy.CloseAllHP();
                    Data.buff_display.SetActive(false);
                    t_mybuff_display = 0;
                }
            }

            if (battle == true)
            {
                ChangeValue(item_selected, 1, item_page, items_inbattle);
            }
            else
            {
                ChangeValue(item_selected, 1, StageItem.item_page, StageItem.items_all);
            }

            if (decide_item == false)
            {
                item_selected = -1;
            }
            else if(battle == false)
            {
                item_selected = -1;
            }

            if(battle == true)
            {
                ReleaseTarget(true);
            }
        }
    }
    private void SelectItem(int num, bool battle, bool change_page)
    {
        if(change_page == false)
        {
            if (item_selected == -1)
            {
                Data.item_box.SetActive(true);
                if (battle == true)
                {
                    t_open_message = MENU_MOVET;
                    t_close_message = 0;
                }
                else
                {
                    StageItem.t_open_message = MENU_MOVET;
                    StageItem.t_close_message = 0;
                }

                Data.menu_item_message.SetActive(true);
            }
            else
            {
                nomalSE.Play("select1");

                if (battle == true)
                {
                    ChangeValue(item_selected, 1, item_page, items_inbattle);
                }
                else
                {
                    ChangeValue(item_selected, 1, StageItem.item_page, StageItem.items_all);
                }
            }
        }
        
        item_selected = num;

        if(battle == true)
        {
            GetTarget(num);
        }
        ChangeText(false);
    }
    public void SetItem(List<int> itemlist, int page)
    {
        int rom_index_max = itemlist.Count;

        if(rom_index_max != 0)
        {
            if ((page - 1) * pagenum_max >= rom_index_max)
            {
                page -= 1;
            }
        }

        int romnum = 0 + pagenum_min;
        for (int i = (page - 1) * pagenum_max; i < page * pagenum_max; i++)
        {
            GameObject item = Data.item_obj[romnum];

            if (i < rom_index_max)
            {
                int item_index = itemlist[i];
                Image[] item_velue_images = new Image[3] { Data.item_value[romnum * 3], Data.item_value[romnum * 3 + 1], Data.item_value[romnum * 3 + 2] };

                item.SetActive(true);
                Data.menu_values[romnum].SetActive(true);
                Data.item_image[romnum].sprite = Data.item_sprite[item_index * 3];
                Fig.ChageFigure_x(Status.items_value[item_index], item_velue_images, DataNomal.figure_mini);
            }
            else
            {
                item.SetActive(false);
                Data.menu_values[romnum].SetActive(false);
            }

            romnum += 1;
        }
    }
    private void UseItem(int item_index)
    {
        if (item_index == 0)
        {
            items_do.Add(0);   //index
            items_do.Add(30);  //value
            items_do.Add(0);   //time
        }
        else if (item_index == 1)
        {
            items_do.Add(0);
            items_do.Add(100);
            items_do.Add(0);
        }
        else if(item_index == 2)
        {
            items_do.Add(1);
            items_do.Add(1);
            items_do.Add(4);
            items_do.Add(2);
            items_do.Add(1);
            items_do.Add(4);
        }
        else if (item_index == 3)
        {
            items_do.Add(1);
            items_do.Add(1);
            items_do.Add(4);
        }
        else if (item_index == 4)
        {
            items_do.Add(2);
            items_do.Add(1);
            items_do.Add(4);
        }
        else if (item_index == 5)
        {
            items_do.Add(3);
            items_do.Add(30);
            items_do.Add(3);
        }
        else if(item_index == 7)
        {
            items_do.Add(4);
            items_do.Add(20);
            items_do.Add(0);
        }
    }
    private void StepItem()
    {
        item_step += 1;
        if (item_step * 3 < items_do.Count)
        {
            DoItem(items_do[item_step * 3], items_do[item_step * 3 + 1], items_do[item_step * 3 + 2]);
        }
        else
        {
            if(step == STEP_SELECT_ITEM_USE)
            {
                jun_ani.SetBool("item", false);
                step = STEP_SELECT_ITEM_END;
                t_close_item = MENU_MOVET;
            }
            else if(step == STEP_SET_BUFFS)
            {
                bool end = false;
                if (Status.hp == 0 & tutorial == false)
                {
                    end = true;
                }

                if(end == false)
                {
                    step = STEP_OPEN;
                    Data.move_firstbuttons.SetActive(true);
                    t_open_firstbuttons = MENU_MOVET2;
                    ResetMyTurn();
                }
                else
                {
                    gameover.GameOver(battleBGM, nomalSE, voiceSE);
                    gameObject.SetActive(false);
                }
            }
        }
    }
    private void DoItem(int index, int value, int time)
    {
        if(index == 0) //hp回復
        {
            ItemEffect(1, jun_effect, jun_effect_ani);
            ChangeHPdisplay(value, STATUS_TIME);
            nomalSE.Play("select5");
        }
        else if(index == 1) //atkバフ
        {
            int plus_index = SetBuff(buff_value[0], buff_value[1], buff_time[0], buff_time[1], time);

            if(plus_index != -1)
            {
                buff_value[plus_index] = value;
                buff_time[plus_index] = time;
            }

            ItemEffect(2, jun_effect, jun_effect_ani);
            SetBuffDisplay(buff_value[0], buff_value[1], 0);
            nomalSE.Play("select5");
        }
        else if (index == 2) //defバフ
        {
            int plus_index = SetBuff(buff_value[2], buff_value[3], buff_time[2], buff_time[3], time);

            if (plus_index != -1)
            {
                buff_value[2 + plus_index] = value;
                buff_time[2 + plus_index] = time;
            }

            guard_buff = true;
            guard_sp += 3;
            Status.guard_hp = Status.guard_maxhp;
            ItemEffect(3, jun_effect, jun_effect_ani);
            SetBuffDisplay(buff_value[2], buff_value[3], 2);
            nomalSE.Play("select5");
        }
        else if (index == 3) //自動回復
        {
            buff_time[4] = time;
            buff_value[4] = value;

            ItemEffect(1, jun_effect, jun_effect_ani);
            SetBuffDisplay(value, -1, 4);
            nomalSE.Play("select5");
        }
        else if(index == 4) //sp回復
        {
            ItemEffect(4, jun_effect, jun_effect_ani);
            ChangeSPdisplay(value, STATUS_TIME);
            nomalSE.Play("select5");
        }
        else if(index == 5) //スタミナダメージ
        {
            if(time == 0)
            {
                debuff_time[5] = 2;
                debuff_value[5] = 1;
            }
            Enemy.DamagedEffect(jun);
            JunDamaged(value, null, true);
        }
    }
    public void ChangeValue(int num, int white, int page, List<int> items)
    {
        Image[] item_velue_images = new Image[3] { Data.item_value[num * 3], Data.item_value[num * 3 + 1], Data.item_value[num * 3 + 2] };
        Fig.ChangeColorFigure(item_velue_images, 1f, white, 1f);
        int item_index = (page - 1) * pagenum_max + (num - pagenum_min);

        if(item_index < items.Count)
        {
            int index = items[item_index];
            int value = Status.items_value[index] - (1 - white);
            Fig.ChageFigure_x(value, item_velue_images, DataNomal.figure_mini);
        }
    }
    public void ItemEffect(int index, GameObject effect, Animator ani, float x = 0.5f, float y = 0)
    {
        Move.MoveLocalPos(effect, x, y);
        effect.SetActive(false);
        effect.SetActive(true);
        ani.SetInteger("effect_index", index);
    }

    //run
    private void SelectRun(int num)
    {
        if(run_selected == -1)
        {
            Data.run_buttons_box.SetActive(true);
            t_open_message = MENU_MOVET;
            t_close_message = 0;
            Data.menu_item_message.SetActive(true);
        }
        else
        {
            nomalSE.Play("select1");
        }

        if(num == 1)
        {
            SetSP(30, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
        }
        else
        {
            SetSP(-1, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
        }
        St_obj_config.SetST(2);
        run_selected = num;
        Move.MoveLocalPosX(Data.run_buttons_box, 70 * (num * 2 - 1));
        ChangeText(false);
    }
    private void DeselectRun(bool decide_run)
    {
        ChangeText(true);

        if(run_selected != -1)
        {
            t_open_message = 0;
            t_close_message = MENU_MOVET;
            Data.run_buttons_box.SetActive(false);

            if (decide_run == true)
            {
                jun_ani.SetInteger("run", 1);
                Data.run_buttons[run_selected].SetActive(false);
                step = STEP_SELECT_RUN_WAIT;
                t_close_secondbuttons = MENU_MOVET2;
                Data.run_button_selected_image.sprite = Data.run_buttons_sprite[6];
                Data.run_button_selected.SetActive(true);
                Move.MoveLocalPos(Data.run_button_selected, 70 * (run_selected * 2 - 1), -82.5f);
                t_run = RUN_WAIT_TIME;
                Enemy.CloseAllHP();
                Data.buttons_target.SetActive(false);
                Data.buff_display.SetActive(false);
                t_mybuff_display = 0;
                St_obj_config.CloseST(MENU_MOVET2);

                St_obj_config.ChangeST(-2);

                if (run_selected == 1)
                {
                    Status.sp -= 30;
                }
            }
            else
            {
                run_selected = -1;
            }

            SetSP(-1, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
            St_obj_config.SetST();
        }
    }
    private void SetRunButtonFirst()
    {
        if(Status.sp >= 30)
        {
            Data.run_buttons_image[1].sprite = Data.run_buttons_sprite[2];
        }
        else
        {
            Data.run_buttons_image[1].sprite = Data.run_buttons_sprite[3];
        }
        Data.run_buttons[0].SetActive(true);
        Data.run_buttons[1].SetActive(true);
    }

    //page
    private void ChangePage(int dir)
    {
        nomalSE.Play("select9");
        t_changepage = CHANGE_PAGE+1;

        if (step == STEP_SELECT_ATTACK)
        {
            rom_page += dir;
            ChangeRompageFigure();
            SetRom(rom_page);
            //DeselectRom(false, true);
        }
        else if(step == STEP_SELECT_ITEM)
        {
            int page = item_page + dir;

            if(page < 1)
            {
                item_page = item_page_max;
            }
            else if(page > item_page_max)
            {
                item_page = 1;
            }
            else
            {
                item_page = page;
            }

            ChangeItempageFigure(item_page, item_page_max);
            SetItem(items_inbattle, item_page);
            //DeselectItem(false, true);

            item_selected = -1;
            PushItem(pagenum_min, true);
        }

        step_sub = step;
        step = STEP_SELECT_CHANGEPAGE;
    }
    private void ChangeRompageFigure()
    {
        Fig.ChangeFigure(rom_page, Data.fig_page, true, DataNomal.figure_mini);
        Fig.ChangeFigure(rom_page_max, Data.fig_maxpage, false, DataNomal.figure_mini);
    }
    public void ChangeItempageFigure(int page, int pagemax)
    {
        Fig.ChangeFigure(page, Data.fig_page, true, DataNomal.figure_mini);
        Fig.ChangeFigure(pagemax, Data.fig_maxpage, false, DataNomal.figure_mini);
    }
    private void ChangePageAnimation()
    {
        t_changepage -= 1;
        if(step_sub == STEP_SELECT_ATTACK)
        {
            Move.DeccelShake(t_changepage, CHANGE_PAGE, 10, 0, -2, 3, Data.move_secondbuttons_roms);
        }
        else if(step_sub == STEP_SELECT_ITEM)
        {
            Move.DeccelShake(t_changepage, CHANGE_PAGE, 10, 0, -2, 3, Data.move_secondbuttons_items);
        }

        if (t_changepage == 0)
        {
            step = step_sub;
        }
    }

    //status
    public void SetHPinBattle()
    {
        SetHP(-1, Data.bar_hp_dec, Data.bar_hp, Data.hp_slash, Data.fig_hp);
        Fig.ChangeFigure(Status.maxhp, Data.fig_maxhp, false, DataNomal.figure_mini);
    }
    public void SetSPinBattle()
    {
        SetSP(-1, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
    }
    public void SetHP(int reserveHP, GameObject bar_hp_dec, GameObject bar_hp, GameObject hp_slash, Image[] fig_hp)
    {
        int hp = Status.hp;

        if (reserveHP != -1)
        {
            Bar.SetBar(hp, Status.maxhp, 55, bar_hp_dec);

            hp = Status.hp - reserveHP;
            if(hp < 0)
            {
                hp = 0;
            }
            Fig.ChangeColorFigure(fig_hp, 1f, 0f, 1f);
        }

        Bar.SetBar(hp, Status.maxhp, 55, bar_hp);
        Fig.ChangeFigure_SlashtoLeft(hp, fig_hp, hp_slash, slash_pos, DataNomal.figure_mini);

        if (reserveHP == -1)
        {
            Fig.ChangeColorFigure(fig_hp, 1f, 1f, 1f);
            Bar.SetBar(reserveHP, 100, 55, bar_hp_dec);
        }
    }
    public void SetSP(int reserveSP, GameObject bar_sp_dec, GameObject bar_sp, GameObject sp_slash, Image[] fig_sp, Image[] fig_spnum)
    {
        int sp = Status.sp;
        bool sethp = false;

        if (reserveSP != -1)
        {
            sp = sp - reserveSP;
            if(sp < 0)
            {
                sethp = true;
                SetHP(-sp, Data.bar_hp_dec, Data.bar_hp, Data.hp_slash, Data.fig_hp);
                sp = 0;
            }

            int display_spnum_dec = (sp - 1) / 100;
            if(Status.sp != 0)
            {
                Fig.ChangeColorFigure(fig_sp, 1f, 0f, 1f);
            }
            if((Status.sp - 1) / 100 != display_spnum_dec)
            {
                Bar.SetBar(1, 1, 55, bar_sp_dec);
                Fig.ChangeColorFigure(fig_spnum, 1f, 0f, 1f);
            }
            else
            {
                Fig.ChangeColorFigure(fig_spnum, 1f, 1f, 1f);
                Bar.SetBar(Status.sp - display_spnum_dec * 100, 100, 55, bar_sp_dec);
            }
        }

        int display_spnum = (sp - 1) / 100;
        int display_sp = sp - display_spnum * 100;
        Fig.ChangeFigure_SlashtoLeft(display_sp, fig_sp, sp_slash, slash_pos, DataNomal.figure_mini);
        Fig.ChageFigure_x(display_spnum, fig_spnum, DataNomal.figure_mini);
        Bar.SetBar(display_sp, 100, 55, bar_sp);

        if (reserveSP == -1)
        {
            Fig.ChangeColorFigure(fig_sp, 1f, 1f, 1f);
            Fig.ChangeColorFigure(fig_spnum, 1f, 1f, 1f);
            Bar.SetBar(display_sp, 100, 55, bar_sp_dec);
        }
        if(sethp == false)
        {
            SetHP(-1, Data.bar_hp_dec, Data.bar_hp, Data.hp_slash, Data.fig_hp);
        }
    }
    private int SearchSP()
    {
        (int skill, int level) = SearchSkill(rom_selected - pagenum_min);
        int dec_sp = Status.skills_sp[skill * 3 + (level - 1)];

        return dec_sp;
    }
    private (int skill, int level) SearchSkill(int rom_num)
    {
        int skill = Status.myskill[(rom_page - 1) * pagenum_max + rom_num];
        int level = Status.myskill_level[skill];

        return (skill, level);
    }
    public void ChangeHPdisplay(int plusHP, int display_time, float plusX = 0f, float plusY = 0f)
    {
        t_hp_display = display_time + 1;
        t_hp_display_max = display_time;
        Status.hp += plusHP;
        if(Status.hp < 0)
        {
            Status.hp = 0;
        }
        else if(Status.hp > Status.maxhp)
        {
            Status.hp = Status.maxhp;
        }

        Data.fig_dispay_parent.SetActive(false);
        if(plusHP > 0)
        {
            Fig.ChangeFigureCenter(plusHP, 8, 0, Data.fig_hp_display, Data.fig_hp_display_obj, DataNomal.figure_LifeUp);
            Move.SetSamePos(Data.fig_dispay_parent, jun, 0 + plusX, -31.5f + plusY);
        }
        else
        {
            Fig.ChangeFigureCenter(-plusHP, 8, 0, Data.fig_hp_display, Data.fig_hp_display_obj, DataNomal.figure_damaged);
        }
        SetHP(-1, Data.bar_hp_dec, Data.bar_hp, Data.hp_slash, Data.fig_hp);
    }
    private void ChangeSPdisplay(int plusSP, int display_time)
    {
        t_sp_display = display_time + 1;
        t_sp_display_max = display_time;
        Status.sp += plusSP;

        Data.fig_dispay_parent.SetActive(false);
        if (plusSP > 0)
        {
            Fig.ChangeFigureCenter(plusSP, 8, 0, Data.fig_hp_display, Data.fig_hp_display_obj, DataNomal.figure_SpUp);
            Move.SetSamePos(Data.fig_dispay_parent, jun, 0, -31.5f);
        }
        SetSP(-1, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
    }
    private void AnimHP()
    {
        t_hp_display -= 1;
        if (t_hp_display >= t_hp_display_max - 6)
        {
            if (t_hp_display == t_hp_display_max - 1)
            {
                Data.fig_dispay_parent.SetActive(true);
            }
            int t = STATUS_SHAKE_TIME - (t_hp_display_max - t_hp_display);
            Move.DeccelShake(t, STATUS_SHAKE_TIME, 4, 0, 2, 3, Data.fig_dispay_parent);
        }

        if(t_hp_display < STATUS_DEC_TIME)
        {
            float alpha = t_hp_display / (float)STATUS_DEC_TIME;
            Data.fig_dispay_parent_alpha.alpha = alpha;
            jun_effect_sr.color = new Color(1f, 1f, 1f, alpha);

            if(t_hp_display == 0)
            {
                Data.fig_dispay_parent_alpha.alpha = 1f;
                jun_effect_sr.color = new Color(1f, 1f, 1f, 1f);
                Data.fig_dispay_parent.SetActive(false);
                jun_effect.SetActive(false);

                if(step == STEP_SELECT_ITEM_USE | step == STEP_SET_BUFFS)
                {
                    StepItem();
                }
            }
        }
    }
    private void AnimSP()
    {
        t_sp_display -= 1;
        if (t_sp_display >= t_sp_display_max - 6)
        {
            if (t_sp_display == t_sp_display_max - 1)
            {
                Data.fig_dispay_parent.SetActive(true);
            }
            int t = STATUS_SHAKE_TIME - (t_sp_display_max - t_sp_display);
            Move.DeccelShake(t, STATUS_SHAKE_TIME, 4, 0, 2, 3, Data.fig_dispay_parent);
        }

        if (t_sp_display < STATUS_DEC_TIME)
        {
            float alpha = t_sp_display / (float)STATUS_DEC_TIME;
            Data.fig_dispay_parent_alpha.alpha = alpha;
            jun_effect_sr.color = new Color(1f, 1f, 1f, alpha);

            if (t_sp_display == 0)
            {
                Data.fig_dispay_parent_alpha.alpha = 1f;
                jun_effect_sr.color = new Color(1f, 1f, 1f, 1f);
                Data.fig_dispay_parent.SetActive(false);
                jun_effect.SetActive(false);

                if (step == STEP_SELECT_ITEM_USE | step == STEP_SET_BUFFS)
                {
                    StepItem();
                }
            }
        }
    }
    private int SetBuff(int value0, int value1, int time0, int time1, int time)
    {
        int index = -1;

        if (value0 < value1)
        {
            index = 0;
        }
        else if (value0 == 1 & value1 == 1)
        {
            if (time0 <= time1)
            {
                if (time > time0)
                {
                    index = 0;
                }
            }
            else
            {
                if (time > time1)
                {
                    index = 1;
                }
            }
        }
        else
        {
            index = 1;
        }

        return index;
    } 
    private void SetBuffDisplay(int value0, int value1, int index)
    {
        t_buff_display = STATUS_TIME + 1;
        Move.MoveLocalPos(Data.buff_display, -62, 2.5f);

        if(value1 != -1)
        {
            if(value0 != 0 & value1 != 0)
            {
                index += 1;
            }
        }

        Data.buff_display_img.sprite = Data.buff_display_sprite[index];
    }
    private void SetBuffDisplayDeffence(int index, float plusX, float plusY)
    {
        t_buff_display = STATUS_TIME + 1;
        Data.buff_display_img.sprite = Data.buff_display_sprite[index];

        Transform transform = jun.transform;
        Vector3 pos = transform.localPosition;
        if(pos.y > 18)
        {
            Move.SetSamePos(Data.buff_display, jun, 0f + plusX, 28.5f + plusY);
        }
        else if(pos.y > 10)
        {
            Move.MoveLocalPos(Data.buff_display, pos.x + plusX, 48.5f + plusY);
        }
        else
        {
            Move.MoveLocalPos(Data.buff_display, pos.x + plusX, -21.5f + plusY);
        }
    }
    private void DoBuff_befereTurnStart()  //ターン始めの効果
    {
        if (spballs_active.Count > 0)
        {
            int get_sp = 0;
            for(int i=0; i < spballs_active.Count; i++)
            {
                spballs_active[i].GetComponent<Animator>().SetTrigger("get");
                get_sp += guard_sp;
            }

            spballs_active = new List<GameObject>();

            items_do.Add(4);
            items_do.Add(get_sp);
            items_do.Add(0);
        }

        if (buff_time[4] != 0)
        {
            items_do.Add(0);
            items_do.Add(buff_value[4]);
            items_do.Add(0);
        }
        if (debuff_time[0] == 1)
        {
            Status.guard_hp = Status.guard_maxhp;
        }

        if (Status.st < 0)
        {
            items_do.Add(5);
            items_do.Add(-Status.st);
            items_do.Add(debuff_time[5]);
        }
    }
    private void SetMyBuff()
    {
        Move.MoveLocalPos(Data.buff_display, -62, 2.5f);
        buffs = new List<int>();
        buffs_step = 0;
        button_guard = false;
        guard = false;
        guard_obj.SetActive(false);
        guard_effect_obj.SetActive(false);

        //buff
        for (int i=0; i< BUFF_KIND; i++)
        {
            int t = buff_time[i];

            if(t != 0)
            {
                t -= 1;
                buff_time[i] = t;

                if(t == 0)
                {
                    buff_value[i] = 0;
                }
            }
        }
        if(buff_time[0] != 0 | buff_time[1] != 0)
        {
            if(buff_value[0] == 1 & buff_value[1] == 1)
            {
                buffs.Add(1);
                skill_atk = Status.atk + 12;
            }
            else
            {
                buffs.Add(0);
                skill_atk = Status.atk + 6;
            }
        }
        if (buff_time[2] != 0 | buff_time[3] != 0)
        {
            if (buff_value[2] == 1 & buff_value[3] == 1)
            {
                buffs.Add(3);
                guard_sp = Status.guard_sp + 6;
                Status.guard_hp = Status.guard_maxhp;
                guard_buff = true;
            }
            else
            {
                buffs.Add(2);
                guard_sp = Status.guard_sp + 3;
                Status.guard_hp = Status.guard_maxhp;
                guard_buff = true;
            }
        }
        if(buff_time[4] != 0)
        {
            buffs.Add(4);
        }

        //debuff
        for (int i = 0; i < DEBUFF_KIND; i++)
        {
            int t = debuff_time[i];

            if (t != 0)
            {
                t -= 1;
                debuff_time[i] = t;

                if (t == 0)
                {
                    debuff_value[i] = 0;
                }
            }
        }
        if (debuff_time[0] != 0)
        {
            buffs.Add(5);
        }
        else
        {
            float plus_guardhp = Mathf.Round(Status.guard_maxhp / 5f);
            Status.guard_hp += (int)plus_guardhp;
            if (Status.guard_hp > Status.guard_maxhp)
            {
                Status.guard_hp = Status.guard_maxhp;
            }
            float c = Status.guard_hp / (float)Status.guard_maxhp;
            guard_sr.color = new Color(1f, c, c);
            guard_effect_sr.color = new Color(1f, c, c);
        }
        if(debuff_time[5] != 0)
        {
            if(Status.st < 0)
            {
                debuff_time[5] += 1;
                buffs.Add(9);
            }
            else
            {
                debuff_time[5] = 0;
                debuff_value[5] = 0;
            }
        }

        buffs_count = buffs.Count;
        if (buffs_count != 0)
        {
            Data.buff_display.SetActive(true);
            Data.buff_display_img.sprite = Data.buff_display_sprite[buffs[0]];
            Data.buff_display_img.color = new Color(1f, 1f, 1f, 0f);
            t_mybuff_display = MYBUFF_TIME;
        }
    }
    private void AnimBuff()
    {
        t_buff_display -= 1;
        if (t_buff_display >= STATUS_TIME - 6)
        {
            if (t_buff_display == STATUS_TIME - 1)
            {
                Data.buff_display.SetActive(true);
                Data.buff_display_img.color = new Color(1f, 1f, 1f, 1f);
            }
            int t = STATUS_SHAKE_TIME - (STATUS_TIME - t_buff_display);
            Move.DeccelShake(t, STATUS_SHAKE_TIME, 4, 0, 2, 3, Data.buff_display);
        }

        if (t_buff_display < STATUS_DEC_TIME)
        {
            float alpha = t_buff_display / (float)STATUS_DEC_TIME;
            Data.buff_display_img.color = new Color(1f, 1f, 1f, alpha);
            jun_effect_sr.color = new Color(1f, 1f, 1f, alpha);

            if (t_buff_display == 0)
            {
                jun_effect_sr.color = new Color(1f, 1f, 1f, 1f);
                Data.buff_display.SetActive(false);
                jun_effect.SetActive(false);

                if (step == STEP_SELECT_ITEM_USE | step == STEP_SET_BUFFS)
                {
                    StepItem();
                }
            }
        }
    }
    private void AnimMyBuff()
    {
        t_mybuff_display -= 1;

        if (t_mybuff_display >= MYBUFF_TIME - MYBUFF_TIME_FADE)
        {
            int t = MYBUFF_TIME - t_mybuff_display;
            float alpha = t / (float)(MYBUFF_TIME_FADE);
            Data.buff_display_img.color = new Color(1f, 1f, 1f, alpha);

            if(t_mybuff_display == MYBUFF_TIME - MYBUFF_TIME_FADE)
            {
                if(buffs_count == 1)
                {
                    t_mybuff_display = 0;
                }
            }
        }
        else if(t_mybuff_display < MYBUFF_TIME_FADE)
        {
            float alpha = t_mybuff_display / (float)(MYBUFF_TIME_FADE);
            Data.buff_display_img.color = new Color(1f, 1f, 1f, alpha);

            if(t_mybuff_display == 0)
            {
                buffs_step += 1;
                if(buffs_step >= buffs_count)
                {
                    buffs_step = 0;
                }

                t_mybuff_display = MYBUFF_TIME;
                Data.buff_display_img.sprite = Data.buff_display_sprite[buffs[buffs_step]];
            }
        }
    }
    private void GetExp()
    {
        if(get_exp == false)
        {
            get_exp = true;
            nomalSE.Play("select2");

            if (Status.level != 9)
            {
                foreach (int e in Enemy.battle_active_enemy_level)
                {
                    Status.exp += Status.get_ext_list[e];
                }

                int next_level = -1;
                int next_exp = -1;
                for (int i = 1; i < Status.exp_list.Length; i++)
                {
                    if (Status.exp_list[i - 1] <= Status.exp & Status.exp < Status.exp_list[i])
                    {
                        next_level = i;
                        next_exp = Status.exp - Status.exp_list[i - 1];
                        break;
                    }
                }

                if (next_level != -1)
                {
                    int parent = Status.exp_list[next_level] - Status.exp_list[next_level - 1];
                    Debug.Log(Status.exp);
                    Debug.Log(next_exp);
                    Debug.Log(parent);
                    Bar.SetBar(next_exp, parent, 25, Data.level_bar);

                    if (Status.level != next_level)
                    {
                        level_up = next_level - Status.level;
                        Status.level = next_level;
                        Data.level_value_text.sprite = DataNomal.figure_mini[next_level];
                        jun_ani.SetTrigger("levelup");
                        nomalSE.Play("select5");
                        voiceSE.Stop();
                        voiceSE.Play("voice_levelup" + Random.Range(1, 5));
                    }
                }
            }

            if(Status.level == 9)
            {
                Bar.SetBar(1, 1, 25, Data.level_bar);
                Data.level_value_text.color = new Color(1f, 1f, 0f);
                MasterStage.level_value_text.color = new Color(1f, 1f, 0f);
            }
            else
            {
                Data.level_text.color = new Color(1f, 1f, 0f);
                Data.level_value_text.color = new Color(1f, 1f, 0f);
                t_get_exp = GET_EXP_TIME;
            }
        }
    }
    public void SetLevel(Image level_img, GameObject level_bar)
    {
        int exp_child = -1;
        int exp_parent = -1;
        for (int i = 1; i < Status.exp_list.Length; i++)
        {
            if (Status.exp_list[i - 1] <= Status.exp & Status.exp < Status.exp_list[i])
            {
                exp_child = Status.exp - Status.exp_list[i - 1];
                exp_parent = Status.exp_list[i] - Status.exp_list[i-1];
                break;
            }
        }

        level_img.sprite = DataNomal.figure_mini[Status.level];
        Bar.SetBar(exp_child, exp_parent, 25, level_bar);
    }

    //other
    public void StartBattle()
    {
        if (Status.level != 9)
        {
            SetLevel(Data.level_value_text, Data.level_bar);
        }
        else
        {
            Data.level_value_text.sprite = DataNomal.figure_mini[Status.level];
            Data.level_value_text.color = new Color(1f, 1f, 0f);
            Bar.SetBar(1, 1, 25, Data.level_bar);
        }
        int enemy_count = Enemy.battle_active_enemy_num.Count;
        for (int i = 0; i < 3; i++)
        {
            if (i < enemy_count)
            {
                if(i == 0)
                {
                    Enemy.battle_enemy[0].SetActive(true);
                    Enemy.battle_enemy_ani[0].SetInteger("enemy", Enemy.battle_active_enemy_id[i]);
                }
                int num = Enemy.battle_active_enemy_num[i];
                Enemy.battle_enemy_ani[num].speed = 1;
            }
        }
        jun.SetActive(true);
        Data.enemy_names.SetActive(true);

        level_up = 0;
        get_exp = false;
        skill_atk = Status.atk;
        guard_sp = Status.guard_hp;
        guard_buff = false;

        int jun_voice = Random.Range(0, Data.battle_start_time.Length);
        voiceSE.Play("voice_start" + (jun_voice + 1));
        t_battle_start = Data.battle_start_time[jun_voice];

        item_page = 1;
        rom_page = 1;
    }
    public void SetBattle()
    {
        step = STEP_BATTLE_WAIT;
        first_open = false;

        buff_time = new int[BUFF_KIND];
        buff_value = new int[BUFF_KIND];
        debuff_time = new int[DEBUFF_KIND];
        debuff_value = new int[DEBUFF_KIND];
        Status.guard_hp = Status.guard_maxhp;

        Fig.ChangeFigure(Status.maxhp, Data.fig_maxhp, false, DataNomal.figure_mini);
        SetHP(-1, Data.bar_hp_dec, Data.bar_hp, Data.hp_slash, Data.fig_hp);
        SetSP(-1, Data.bar_sp_dec, Data.bar_sp, Data.sp_slash, Data.fig_sp, Data.fig_spnum);
        rom_page_max = (Status.myskill.Count - 1) / pagenum_max + 1;
        SetRom(rom_page);


        if(Status.st < 0)
        {
            debuff_time[5] = 2;
            debuff_value[5] = 1;
        }

        target_enemy = 0;
        target_single_enemy = 0;
        pre_target_enemy = 0;

    SetRunButtonFirst();
    }
    public void ResetMyTurn()
    {
        act_num = 0;
        Move.SetSamePos(Data.button_first_cursor, Data.menu_button_firstselect[0], 0, 0);

        guard = false;
        button_guard = false;
        Enemy.OpenAllHP();
        pre_target_enemy = TARGET_NONE;
        walk_box_pos = 1;
        rom_selected = -1;
        item_selected = -1;
        items_do = new List<int>();

        if (Enemy.battle_active_enemy_num.Contains(target_single_enemy))
        {
            target_enemy = target_single_enemy;
        }
        else
        {
            target_enemy = Enemy.battle_active_enemy_num[0];
        }

        items_inbattle = new List<int>();
        for(int i=0; i< Status.items_value.Length; i++)
        {
            if (Status.items_use[i] != 2)
            {
                if (Status.items_value[i] > 0)
                {
                    items_inbattle.Add(i);
                }
            }
        }

        item_page = 1;
        item_page_max = (items_inbattle.Count - 1) / pagenum_max + 1;
        item_step = -1;
        SetItem(items_inbattle, item_page);
        SetRom(rom_page);

        skill_atk = Status.atk;
        guard_sp = Status.guard_sp;
        guard_buff = false;
        SetMyBuff();

        SetRunButtonFirst();
    }
    public void ReadyAttack()
    {
        step = STEP_SELECT_ATTACK_READY;
    }
    public void EndAttack()
    {
        step = STEP_SELECT_ATTACK_END;
        t_close_skill = MENU_MOVET;
    }
    public void EndDefence()
    {
        if(t_damaged == 0 & t_guard == 0 & t_just_guard == 0)
        {
            jun_ani.SetBool("guard", false);
            jun_ani.SetTrigger("attack_end");
            guard_obj.SetActive(false);
            Enemy.battle_enemy_attacked = 0;
            Enemy.t_enemy_attack_end = 0;
            Enemy.attack_end_wait = false;
            Enemy.enemy_attack_end = false;
            step = STEP_DEFENCE_END;
            t_close_defense = MENU_MOVET;
            guard = false;
        }
    }
    private void JudgeWin()
    {
        int enemy_count = Enemy.battle_active_enemy_num.Count;
        if (enemy_count != 0)
        {
            int active_enemy_count = 0;
            foreach (int n in Enemy.battle_active_enemy_num)
            {
                if (Enemy.Enemy[n].t_damaged == 0)
                {
                    active_enemy_count += 1;
                }
            }
            if (active_enemy_count == enemy_count)
            {
                step = STEP_DEFENCE_READY;
                t_oepn_defense = MENU_MOVET2;
                Control.gameObject.SetActive(true);
                if (step_sub == STEP_SELECT_ATTACK)
                {
                    St_obj_config.OpenST(MENU_MOVET2);
                }
                Data.battle_walk_box[walk_box_pos].SetActive(true);
                Data.battle_walk_box_sr[walk_box_pos].color = new Color(1f, 1f, 1f, 0f);

                if(tutorial == true)
                {
                    guard_button.SetActive(false);
                    tutorial_time = 20;
                    tutorial_text_obj[0].SetActive(true);
                }
            }
        }
        else
        {
            //# JUNICHI_WIN
            step = STEP_WIN;
            jun_ani.SetBool("battle_win", true);
            Data.win_message1.SetActive(true);
            Data.level_parent.SetActive(true);
            Move.MoveLocalPosY(Data.level_parent, LEVEL_POS + 10f);
            t_win_message = 60;
            guard = false;

            voiceSE.Stop();
            if (skill_selected == 2)
            {
                voiceSE.Play("voice_skill3_end");
            }
            else
            {
                voiceSE.Play("voice_win" + Random.Range(1, 4));
            }

            int get_exp = 0;
            foreach (int e in Enemy.battle_active_enemy_level)
            {
                get_exp += Status.get_ext_list[e];
            }
            Data.win_message2_text.text = "" + get_exp + " EXP を かくとく";

            t_fade_bgm = 70;
        }
    }
    private void EndBattle()
    {
        battleBGM.Stop();

        if (Status.level != 9)
        {
            SetLevel(MasterStage.level_value_text, MasterStage.level_bar);
        }
        else
        {
            MasterStage.level_value_text.sprite = DataNomal.figure_mini[Status.level];
            MasterStage.level_value_text.color = new Color(1f, 1f, 0f);
            Bar.SetBar(1, 1, 25, MasterStage.level_bar);
        }
        Data.level_text.color = new Color(1f, 1f, 1f);
        Data.level_value_text.color = new Color(1f, 1f, 1f);
        Data.level_parent.SetActive(true);
        Data.level_parent_alpha.alpha = 0f;

        Data.win_message1.SetActive(false);
        Data.win_message2.SetActive(false);
        Data.run_messages[0].SetActive(false);
        Data.run_messages[1].SetActive(false);
        jun_ani.SetInteger("run", 0);
        if (Status.hp <= 0)
        {
            Status.hp = 1;
        }
        MasterStage.SetSP(-1);
        MasterStage.SetHP(-1);

        Data.battle_field.SetActive(false);
        MasterStage.EndBattleConfig();
        if(run_do == 0)
        {
            StageEnemyConfig stage_enemy_config = stage_enemy.GetComponent<StageEnemyConfig>();
            stage_enemy_config.Dead();
        }
        else
        {
            junC.t_starmode = 181;
        }
        
        run_do = 0;
        step = -4;
        step_sub = 0;
        MasterStage.battle_status = false;
        MasterStage.t_battle_end = 40;
        MasterStage.enemy_reset_pos = new List<int>();
        Move.MoveLocalPosZ(jun, -63, 27, 21);
        Control.EndBattle();
        stage_room.SetActive(true);
    }

    //target
    private void GetTarget(int num)
    {
        int target;
        if(step == STEP_SELECT_ATTACK | step_sub == STEP_SELECT_ATTACK)
        {
            (int skill, int level) = SearchSkill(num - pagenum_min);
            target = Status.skill_target[skill];
        }
        else
        {
            int index = items_inbattle[(item_page - 1) * pagenum_max + (item_selected - pagenum_min)];
            target = Status.items_target[index];
        }

        if(target < 0)
        {
            Target(target);
        }
        else
        {
            Target(target_enemy);
        }
    }
    private void Target(int index)
    {
        if (index != pre_target_enemy)
        {
            //アウトライン削除
            ReleaseTarget(false);

            //アウトライン追加
            if (index == TARGET_ALL)
            {
                foreach (int num in Enemy.battle_active_enemy_num)
                {
                    Data.battle_enemy_sr[num].material = Data.mat_outline_enemy;
                }
            }
            else if(index == TARGET_ME)
            {
                Data.battle_jun_sr.material = Data.mat_outline_jun;
            }
            else
            {
                Data.battle_enemy_sr[index].material = Data.mat_outline_enemy;
                target_single_enemy = index;
            }

            //前のターゲットを変更
            pre_target_enemy = index;
        }
    }
    private void ReleaseTarget(bool end)
    {
        if (pre_target_enemy != TARGET_NONE)
        {
            if (pre_target_enemy == TARGET_ALL)
            {
                foreach (int num in Enemy.battle_active_enemy_num)
                {
                    Data.battle_enemy_sr[num].material = Data.mat_nomal;
                }
            }
            else if (pre_target_enemy == TARGET_ME)
            {
                Data.battle_jun_sr.material = Data.mat_nomal;
            }
            else
            {
                Data.battle_enemy_sr[pre_target_enemy].material = Data.mat_nomal;
            }
        }

        if(end == true)
        {
            pre_target_enemy = TARGET_NONE;
        }
    }

    //character control
    private void JunWalkAnimation(int dir)
    {
        if (walk_dir != dir)
        {
            jun_ani.SetInteger("walk_dir", dir);
            walk_dir = dir;
        }
    }
    private void JunMovement()
    {
        Transform transform = jun.transform;
        Vector3 pos = transform.localPosition;

        (float Hdirection, float Vdirection) = junC.GetDirection();

        int moveX = 0;
        int moveY = 0;

        //Move
        if(Hdirection > 0)
        {
            if(pos.x < -57)
            {
                moveX += 1;
            }
        }
        else if(Hdirection < 0)
        {
            if (pos.x > -98)
            {
                moveX -= 1;
            }
        }
        if (Vdirection > 0)
        {
            if(pos.y < 56)
            {
                moveY += 1;
            }
        }
        else if (Vdirection < 0)
        {
            if(pos.y > 6)
            {
                moveY -= 1;
            }
        }

        //animation
        if(Hdirection == 0 & Vdirection == 0)
        {
            JunWalkAnimation(0);
        }
        else if(Hdirection > 0)
        {
            JunWalkAnimation(1);
        }
        else if(Hdirection < 0)
        {
            JunWalkAnimation(-1);
        }
        else if(Vdirection != 0)
        {
            JunWalkAnimation(1);
        }

        //box pos
        (float moved_X, float moved_Y) = CheckBoxPos(pos.x, pos.y, moveX, moveY);

        pos.x = moved_X;
        pos.y = moved_Y;
        pos.z = moved_Y - 8;

        transform.localPosition = pos;
    }
    private (float x, float y) CheckBoxPos(float x, float y, int moveX, int moveY)
    {
        int moveX_box_pos = CheckBoxPosBox(x + moveX, y);
        int moveY_box_pos = CheckBoxPosBox(x, y + moveY);
        float moved_X = x;
        float moved_Y = y;

        if (walk_box_pos != moveX_box_pos)
        {
            if (!a_pos.Contains(moveX_box_pos))
            {
                moved_X += moveX;
            }
        }
        else
        {
            moved_X += moveX;
        }
        if (walk_box_pos != moveY_box_pos)
        {
            if (!a_pos.Contains(moveY_box_pos))
            {
                moved_Y += moveY;
            }
        }
        else
        {
            moved_Y += moveY;
        }

        int new_walk_box_pos = CheckBoxPosBox(moved_X, moved_Y);
        if (walk_box_pos != new_walk_box_pos)
        {
            Data.battle_walk_box[walk_box_pos].SetActive(false);
            Data.battle_walk_box[new_walk_box_pos].SetActive(true);
            walk_box_pos = new_walk_box_pos;
        }

        return (moved_X, moved_Y);
    }

    private int CheckBoxPosBox(float moved_X, float moved_Y)
    {
        int box_posX;
        int box_posY;

        if (moved_Y > 39)
        {
            box_posY = 0;
        }
        else if (moved_Y > 22)
        {
            box_posY = 1;
        }
        else
        {
            box_posY = 2;
        }
        if (moved_X > -77)
        {
            box_posX = 0;
        }
        else
        {
            box_posX = 1;
        }

        return box_posY + box_posX * 3;
    }

    private bool JunReturnMovement()
    {
        bool done = false;
        Transform transform = jun.transform;
        Vector3 pos = transform.localPosition;
        int moveX = 0;
        int moveY = 0;

        if(pos.x < -63)
        {
            moveX = 1;
        }
        else if(pos.x > -63)
        {
            moveX = -1;
        }
        if(pos.y > 29)
        {
            moveY = -1;
        }
        else if(pos.y < 29)
        {
            moveY = 1;
        }

        if(moveX == 0 & moveY == 0)
        {
            done = true;
            jun_ani.SetTrigger("walk_end");
            JunWalkAnimation(0);
        }
        else if(moveX == 0)
        {
            JunWalkAnimation(1);
        }
        else
        {
            JunWalkAnimation(moveX);
        }
        pos.x += moveX;
        pos.y += moveY;
        pos.z = pos.y - 8;
        transform.localPosition = pos;

        return done;
    }
    public void JunDamaged(int damage, GameObject obj, bool st_damage = false)
    {
        if(guard == false)
        {
            if(t_just_guard > JUSTGUARD_TIME - JUSTGUARD_RANGE)
            {
                obj.SetActive(false);
                guard_effect_obj.SetActive(false);
                guard_effect_obj.SetActive(true);
                guard_effect_ani.SetInteger("kind", 2);
                guard_effect_sr.color = new Color(1f, 1f, 1f);
                SetBuffDisplayDeffence(7, 0, 0);
                SetSpBall();
                nomalSE.Play("damage2");

                if(tutorial == true)
                {
                    if(tutorial_step == 1 | tutorial_step == 2 | tutorial_step == 0)
                    {
                        TutorialStep();
                    }
                }
            }
            else
            {
                if(st_damage == false)
                {
                    Transform transform = jun.transform;
                    Vector3 pos = transform.localPosition;
                    int moveX = -5;
                    float posX = pos.x;
                    if (posX + moveX >= -98)
                    {
                        (float moved_X, float moved_Y) = CheckBoxPos(posX, pos.y, moveX, 0);

                        pos.x = moved_X;
                        transform.localPosition = pos;
                    }
                    jun_ani.SetTrigger("damaged");
                    t_damaged = 15;
                }
                else
                {
                    jun_ani.SetTrigger("st_damaged");
                    t_damaged = 35;
                }

                ChangeHPdisplay(-damage, STATUS_TIME - 25);
                Move.MoveLocalPos(Data.fig_dispay_parent, jun.transform.localPosition.x + 15.5f, jun.transform.localPosition.y + 7f);
                //Move.MoveLocalPos(Data.fig_dispay_parent, -76.5f + (int)(Status.hp / (float)Status.maxhp) * 54f, -33f); //hpの上

                voiceSE.Stop();
                nomalSE.Play("damage1");
                voiceSE.Play("voice_pain" + Random.Range(1, 10));
            }
        }
        else
        {
            ChangeHPdisplay(-damage / 2, STATUS_TIME - 25);
            Move.MoveLocalPos(Data.fig_dispay_parent, jun.transform.localPosition.x + 15.5f, jun.transform.localPosition.y + 7f);
            //Move.MoveLocalPos(Data.fig_dispay_parent, -76.5f + (int)(Status.hp / (float)Status.maxhp) * 54f, -33f);
            obj.SetActive(false);

            if(guard_buff == false)
            {
                Status.guard_hp -= damage / 2;
                if (Status.guard_hp < 0)
                {
                    guard_break = 2;
                    Status.guard_hp = 0;
                }
            }

            float c;
            t_guard = GUARD_TIME + 1;
            guard_effect_obj.SetActive(false);
            guard_effect_obj.SetActive(true);
            if (Status.guard_hp == 0)
            {
                t_just_guard = 0;
                debuff_time[0] = 2;
                debuff_value[0] = 1;
                jun_ani.SetBool("guard", false);
                jun_ani.SetBool("guard_damaged", false);
                jun_ani.SetTrigger("damaged");
                guard_ani.SetTrigger("break");
                t_damaged = 15;
                guard_effect_ani.SetInteger("kind", 3);
                guard = false;
                c = 1;
                SetBuffDisplayDeffence(8, -5, 0);
                nomalSE.Play("damage4");
            }
            else
            {
                jun_ani.SetBool("guard_damaged", true);
                jun_ani.SetTrigger("guard_damaged_tri");
                guard_effect_ani.SetInteger("kind", 1);
                c = Status.guard_hp / (float)Status.guard_maxhp;
                SetBuffDisplayDeffence(6, -5, 0);
                nomalSE.Play("damage1");
            }
            guard_sr.color = new Color(1f, c, c);
            guard_effect_sr.color = new Color(1f, c, c);

            if (tutorial == true)
            {
                if (tutorial_step == 1)
                {
                    TutorialStep();
                }
            }
        }
    }
    
    //guard
    public void SetGuard()
    {
        button_guard = true;
        Control.guard_img.sprite = Control.guard_sprite[1];
    }
    public void DesetGuard()
    {
        button_guard = false;
        Control.guard_img.sprite = Control.guard_sprite[0];
    }
    private void SetSpBall()
    {
        int posX = (int)(jun.transform.localPosition.x - 10) + Random.Range(-8, 9);
        int posY = (int)(jun.transform.localPosition.y - 20) + Random.Range(-7, 2);

        GameObject spball;
        if(spballs.Count != 0)
        {
            spball = spballs.Dequeue();
        }
        else
        {
            spball = Instantiate(spball_resources);
            spball.transform.parent = spball_parent.transform;
            spball.GetComponent<SpBallConfig>().mbc = this;
        }

        float Z_plus = spballs_active.Count * 0.01f;
        Move.MoveLocalPosZ(spball, posX, posY, posY + 10 - Z_plus);
        spball.SetActive(true);
        spballs_active.Add(spball);
    }

    public void TutorialStep()
    {
        nomalSE.Play("select2");
        tutorial_text[tutorial_step].color = new Color(1f, 1f, 0f);
        tutorial_time = 40;
    }
}
