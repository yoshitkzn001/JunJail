using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasStageConfig : MonoBehaviour
{
    public bool DebugMode;
    public bool start_from_this;

    private const int TIME_ENCOUNT_MOVE_WAIT = 35;
    private const int TIME_ENCOUNT_MOVE = 25;
    private const int TIME_ENCOUNT_FADE = 15;
    private const int ENCOUNT_STEP_WAIT = 0;
    private const int ENCOUNT_STEP_MOVE = 1;

    public CriAtomSource nomalSE;

    public Image level_value_text;
    public GameObject level_bar;

    [SerializeField] private FuncBar Bar;
    [SerializeField] private BattleEnemyConfig bec;
    [SerializeField] private DataStage Data;
    [SerializeField] private FuncMove Move;
    [SerializeField] private JunConfig junC;

    public int t_nav;
    public NavMeshBuilder2D nav;

    public Dictionary<int, (int, int)> enemies_count = new Dictionary<int, (int, int)>(); //<room, count, max>
    public List<GameObject> stage_charas = new List<GameObject>();
    public List<float> stage_charas_height = new List<float>();
    [SerializeField] private DataNomal DataNomal;
    [SerializeField] private FuncMiniFigure Fig;
    [SerializeField] private GameObject battle_field;
    public GameObject fade;
    public Image fade_img;
    [SerializeField] private GameObject encount_jun;
    public SpriteRenderer encount_jun_sr;
    private Vector2 encount_jun_pos;
    [SerializeField] private GameObject encount_enemy;
    public SpriteRenderer encount_enemy_sr;
    private Vector2 encount_enemy_pos;
    [SerializeField] private GameObject target_jun;
    [SerializeField] private GameObject target_enemy;
    private int step_encount = 0;
    private int t_encount = 0;
    public int t_battle_end;

    private List<Image[]> level_figs = new List<Image[]>();
    private List<GameObject[]> level_fig_obj = new List<GameObject[]>();
    public bool select_orb;
    private int select_orb_num;
    public ActionOrbConfig orb_script;

    //StageUI
    public GameObject StageUI;
    public GameObject stageUI_map;
    public GameObject stageUI_status;
    public float[] move_stageUI_map_posX;
    [SerializeField] GameObject move_fade;

    public bool battle_status = false; //バトル開始時にTrue、終了時にFalse

    public List<int> enemy_reset_pos = new List<int>();

    public bool tutorial;
    public TutorialConfig tutorialConfig;

    const float BGM_TUTORIAL_VOLUME = 0.3f;
    const float BGM_VOLUME = 0.55f;

    // Start is called before the first frame update
    void Awake()
    {
        fade_img = fade.GetComponent<Image>();
        encount_jun_sr = encount_jun.GetComponent<SpriteRenderer>();
        encount_enemy_sr = encount_enemy.GetComponent<SpriteRenderer>();
        AdjustBattleUI();
    }

    private void Start()
    {
        SetMyStatus();
        Data.orb_menu_back.SetActive(false);

        if (Status.level != 9)
        {
            Data.MasterBattle.SetLevel(level_value_text, level_bar);
        }
        else
        {
            level_value_text.sprite = DataNomal.figure_mini[Status.level];
            level_value_text.color = new Color(1f, 1f, 0f);
            Bar.SetBar(1, 1, 25, level_bar);
        }
        CountItem();
    }

    void Update()
    {
        if(t_nav > 0)
        {
            if (t_nav == 3)
            {
                Data.all_area.SetActive(true);
            }

            if (t_nav == 2)
            {
                nav.RebuildNavmesh(false);
            }

            if(t_nav == 1)
            {
                Data.all_area.SetActive(false);
            }

            t_nav -= 1;
        }
    }

    private void FixedUpdate()
    {
        if(step_encount == ENCOUNT_STEP_WAIT)
        {
            if(t_encount > 0)
            {
                t_encount -= 1;

                if(t_encount == 0)
                {
                    step_encount = ENCOUNT_STEP_MOVE;
                    t_encount = TIME_ENCOUNT_MOVE+1;
                    encount_jun_pos = new Vector2(encount_jun.transform.position.x, encount_jun.transform.position.y);
                    encount_enemy_pos = new Vector2(encount_enemy.transform.position.x, encount_enemy.transform.position.y);
                }
            }
        }
        else if(step_encount == ENCOUNT_STEP_MOVE)
        {
            if(t_encount > 0)
            {
                t_encount -= 1;

                if(t_encount >= 3)
                {
                    Move.AccelMoveX(t_encount - 3, TIME_ENCOUNT_MOVE - 3, encount_jun_pos.x, target_jun.transform.position.x, encount_jun);
                    Move.AccelMoveY(t_encount - 3, TIME_ENCOUNT_MOVE - 3, encount_jun_pos.y, target_jun.transform.position.y, encount_jun);
                    Move.AccelMoveX(t_encount - 3, TIME_ENCOUNT_MOVE - 3, encount_enemy_pos.x, target_enemy.transform.position.x, encount_enemy);
                    Move.AccelMoveY(t_encount - 3, TIME_ENCOUNT_MOVE - 3, encount_enemy_pos.y, target_enemy.transform.position.y, encount_enemy);
                }

                if(t_encount < TIME_ENCOUNT_FADE)
                {
                    float alpha = t_encount / (float)TIME_ENCOUNT_FADE;

                    fade_img.color = new Color(0f, 0f, 0f, alpha);
                }

                if (t_encount == 0)
                {
                    fade_img.color = new Color(0f, 0f, 0f, 1f);
                    fade.SetActive(false);
                    encount_jun.SetActive(false);
                    encount_enemy.SetActive(false);
                    step_encount = ENCOUNT_STEP_WAIT;
                    Data.MasterBattle.StartBattle();
                }
            }
        }
        if(t_battle_end > 0)
        {
            t_battle_end -= 1;

            if(DebugMode == false)
            {
                if (tutorial == false)
                {
                    Sound.myBGM.volume = BGM_VOLUME * ((40 - t_battle_end) / 40f);
                }
                else
                {
                    Sound.myBGM.volume = BGM_TUTORIAL_VOLUME * ((40 - t_battle_end) / 40f);
                }
            }

            if (t_battle_end < TIME_ENCOUNT_FADE)
            {
                float alpha = t_battle_end / (float)TIME_ENCOUNT_FADE;

                fade_img.color = new Color(0f, 0f, 0f, alpha);
            }

            if (t_battle_end == 0)
            {
                fade.SetActive(false);

                if(tutorial == false)
                {
                    junC.canmove = true;
                }
                else
                {
                    tutorialConfig.OpenTalk3();
                }
            }
        }
    }



    //AdjustUI
    void AdjustBattleUI()
    {

        //message
        int name_size = 405 - (int)Status.SCREEN_WIDTH;
        int message_size = 559 - name_size;
        RectTransform recttransform = Data.DataBattle.menu_name.GetComponent<RectTransform>();
        Vector2 size = recttransform.sizeDelta;
        size.x = name_size;
        recttransform.sizeDelta = size;
        recttransform = Data.DataBattle.menu_message.GetComponent<RectTransform>();
        size = recttransform.sizeDelta;
        size.x = message_size;
        recttransform.sizeDelta = size;
        recttransform = Data.DataBattle.menu_message_textobj.GetComponent<RectTransform>();
        size = recttransform.sizeDelta;
        size.x = message_size / 2 - name_size / 2 + (int)Status.SCREEN_WIDTH - 10;
        recttransform.sizeDelta = size;

        for (int i=0; i<4; i++)
        {
            recttransform = Data.DataBattle.menu_line[i].GetComponent<RectTransform>();
            size = recttransform.sizeDelta;
            if (Status.SCREEN_WIDTH < 200f)
            {
                size.x = (int)Status.SCREEN_WIDTH * 2 - 88;
            }
            else
            {
                size.x = 200 * 2 - 88;
            }
            recttransform.sizeDelta = size;
        }
        Data.MasterBattle.pos_move_returnbuttons = new float[2] { -(Status.SCREEN_WIDTH - 58.5f), -(Status.SCREEN_WIDTH + 16.5f) };

        //roms
        int rom_length = 1;
        Data.MasterBattle.pagenum_min = rom_length;
        Data.MasterBattle.pagenum_max = 7 - rom_length * 2;

        //HPSP
        Data.MasterBattle.pos_move_HPSP = new int[2] { 8, (int)(Status.SCREEN_WIDTH - 10f) };

        //movefade
        RectTransform faderecttransform = move_fade.GetComponent<RectTransform>();
        Vector2 fadesize = recttransform.sizeDelta;
        fadesize.x = Status.SCREEN_WIDTH * 2;
        fadesize.y = 225;
        faderecttransform.sizeDelta = fadesize;
    }

    public void SetBattleConfig(GameObject enemy, GameObject room)
    {
        nomalSE.Play("encount1");
        if(DebugMode == false)
        {
            Sound.myBGM.Pause();
            Debug.Log(true);
        }

        StageEnemyConfig eneC = enemy.GetComponent<StageEnemyConfig>();
        if(eneC.sub_obj == true)
        {
            if(eneC.boss == false)
            {
                eneC = eneC.parent;
                enemy = eneC.gameObject;
            }
        }
        Move.SetSamePos(encount_jun, junC.gameObject, 0, 0, true);
        if(eneC.boss == false)
        {
            Move.SetSamePos(encount_enemy, enemy, 0, 0, true);
            bec.master.boss_battle = false;
        }
        else
        {
            Move.SetSamePos(encount_enemy, enemy, 0, 11.5f, true);

            if(tutorial == true)
            {
                bec.master.boss_battle = false;
            }
            else
            {
                bec.master.boss_battle = true;
            }
        }
        encount_jun.SetActive(true);
        encount_enemy.SetActive(true);

        encount_enemy.GetComponent<Animator>().SetInteger("id", eneC.my_id);
        Move.SetSamePos(battle_field, Camera.main.gameObject, 0, 4.5f);
        t_encount = TIME_ENCOUNT_MOVE_WAIT;

        bec.battle_active_enemy_id = new List<int>(eneC.battle_id);
        bec.battle_active_enemy_level = new List<int>(eneC.battle_level);
        List<int> nums = new List<int>();
        for(int i=0; i< bec.battle_active_enemy_id.Count; i++)
        {
            nums.Add(i);
        }
        bec.battle_active_enemy_num = nums;

        bec.EnemyStart();

        battle_field.SetActive(true);
        fade.SetActive(true);
        fade_img.color = new Color(0f, 0f, 0f, 1f);
        Data.MasterBattle.stage_enemy = enemy;
        Data.MasterBattle.stage_room = room;
        room.SetActive(false);
        Data.MasterBattle.SetBattle();
        junC.canmove = false;

        junC.first_move = false;
        battle_status = true;
    }
    public void EndBattleConfig()
    {
        if(DebugMode == false)
        {
            Sound.myBGM.volume = 0.0f;
            Sound.myBGM.Play();
        }
        Data.MasterBattle.SetHP(-1, Data.hp_bar_dec, Data.hp_bar, Data.hp_slash, Data.hp_fig);
        Data.MasterBattle.SetSP(-1, Data.sp_bar_dec, Data.sp_bar, Data.sp_slash, Data.sp_fig, Data.spnum_fig);
        StageUI.SetActive(true);
        CountItem();
    }

    private void SetMyStatus()
    {
        Fig.ChangeFigure(Status.maxhp, Data.hp_fig_max, false, DataNomal.figure_mini);
        SetHP(-1);
        SetSP(-1);

        Fig.ChangeFigureCenter(Status.twitch_level, 3, -24, Data.twitch_level_fig, Data.twitch_level_fig_obj, DataNomal.figure_mini);
        level_figs.Add(Data.twitch_level_fig);
        level_fig_obj.Add(Data.twitch_level_fig_obj);
        Fig.ChangeFigureCenter(Status.twitch_level, 3, -24, Data.niconico_level_fig, Data.niconico_level_fig_obj, DataNomal.figure_mini);
        level_figs.Add(Data.niconico_level_fig);
        level_fig_obj.Add(Data.twitch_level_fig_obj);
        Fig.ChangeFigureCenter(Status.twitch_level, 3, -24, Data.youtube_level_fig, Data.youtube_level_fig_obj, DataNomal.figure_mini);
        level_figs.Add(Data.youtube_level_fig);
        level_fig_obj.Add(Data.youtube_level_fig_obj);
        select_orb_num = -1;
    }

    //Status
    public void SetHP(int value)
    {
        Fig.ChangeFigure(Status.maxhp, Data.hp_fig_max, false, DataNomal.figure_mini);
        Data.MasterBattle.SetHP(value, Data.hp_bar_dec, Data.hp_bar, Data.hp_slash, Data.hp_fig);
    }
    public void SetSP(int value)
    {
        Data.MasterBattle.SetSP(-1, Data.sp_bar_dec, Data.sp_bar, Data.sp_slash, Data.sp_fig, Data.spnum_fig);
    }

    //orb
    public void SetOrbSelect()
    {
        for (int i = 0; i < 3; i++)
        {
            Data.DataBattle.button_first[i].sprite = Data.button_orb_pushup[i];
        }
    }
    public void PushDownOrbSelect(int num)
    {
        if (select_orb == true)
        {
            Data.DataBattle.button_first[num].sprite = Data.button_orb_pushdown[num];
        }
    }
    public void PushUpOrbSelect(int num)
    {
        if (select_orb == true)
        {
            Data.DataBattle.button_first[num].sprite = Data.button_orb_pushup[num];
        }
    }
    public void PushOrbSelect(int num)
    {
        if (select_orb == true)
        {
            if(select_orb_num != num)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i == num)
                    {
                        Fig.ChangeFigureCenter(Status.twitch_level + 1, 3, -24, level_figs[i], level_fig_obj[i], DataNomal.figure_mini);
                        Fig.ChangeColorFigure(level_figs[i], 1, 0, 1);
                    }
                    else
                    {
                        Fig.ChangeFigureCenter(Status.twitch_level, 3, -24, level_figs[i], level_fig_obj[i], DataNomal.figure_mini);
                        Fig.ChangeColorFigure(level_figs[i], 1, 1, 1);
                    }
                }
                select_orb_num = num;
            }
            else
            {
                Fig.ChangeColorFigure(level_figs[num], 1, 1, 1);
                Data.DataBattle.button_first[num].sprite = Data.button_orb_pushup[num];
                select_orb = false;
                select_orb_num = -1;
                orb_script.Close();
            }
        }
    }

    //Item
    public void CountItem()
    {
        //item_count
        int value_all = 0;
        for (int i = 0; i < Status.items_value.Length; i++)
        {
            int value = Status.items_value[i];
            if (value > 0)
            {
                value_all += value;
            }
        }
        Status.baggage_count = value_all;
    }
}
