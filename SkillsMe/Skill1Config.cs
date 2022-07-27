using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill1Config : MonoBehaviour
{
    const int STEP_WAIT = 0;
    const int STEP_OPEN = 1;
    const int STEP_GAME = 2;
    const int STEP_MISS = 3;
    const int STEP_CLOSE = 4;
    const int STEP_ATTACK_WAIT = 5;
    const int STEP_ATTACK_DO = 6;
    const int STEP_ATTACKED_WAIT = 7;
    const int STEP_RETURN_POS = 8;

    const int NOTE_ANIM_SPEED = 4;
    const int NOTE_ANIM = 12;
    const int JUDGE_ANIM = 10;
    const int JUDGE_ANIM_WAIT = 25;
    const int BUTTON_ANIM = 12;
    const int TIME_BOX_MOVE = 32;
    const int BAR_MAX = 57;
    const int TIME_ATTACK = 228;
    const int TIME_MISS = 70;
    const int TIME_ATTACK_WAIT = 132; //俺の右ストレートで　のタイミングに合わせる
    const int TIME_ATTACK_DO = 10;
    const int TIME_ATTACKED_WAIT = 50;

    int[] up_pos = new int[4] { 10, 12, -1, -1 };
    int[] notes_count_max = new int[3] { 7, 11, 15 };

    [SerializeField] EffectJudgeConfig effect_judge_config;
    [SerializeField] BattleEnemyConfig bec;
    [SerializeField] GameObject jun;
    [SerializeField] FuncBar Bar;
    [SerializeField] GameObject bar_power;
    [SerializeField] GameObject bar_time;
    [SerializeField] Animator ani_jun;
    [SerializeField] Sprite alpha;
    [SerializeField] MasBattleConfig Master;
    [SerializeField] FuncMove Move;
    [SerializeField] GameObject[] button;
    [SerializeField] Image[] button_img;
    [SerializeField] GameObject main_box;
    [SerializeField] GameObject[] notes;
    [SerializeField] Sprite[] notes_sprite;
    [SerializeField] Image[] effect_box_img;
    [SerializeField] GameObject effect_note;
    [SerializeField] Image effect_note_img;
    [SerializeField] Sprite[] effect_note_sprite;
    [SerializeField] GameObject effect_missmessage;
    [SerializeField] Image effect_missmessage_img;
    [SerializeField] Sprite[] effect_missmessage_sprite;

    private Queue<GameObject> notes_queue = new Queue<GameObject>();
    private List<int> notes_judge = new List<int>();
    private List<GameObject> notes_active = new List<GameObject>();

    private int[] atk_list = new int[3] { 13, 28, 45 };
    private float atk_max;

    private int score_max;
    private int combo_max;
    private int score;
    private int combo;
    private int pre_judge_kind;
    private int note_count_max;
    private int note_count;
    private int done_note_count;
    private int step;
    private int time;
    private int t_attack_time;
    private int t_button_right;
    private int t_button_left;
    private int t_effect_judge;
    private int t_effect_note;
    private int t_effect_missmessage;

    public bool tutorial;
    public GameObject tap;
    public bool stop_note;

    // Start is called before the first frame update
    void Awake()
    {
        SetStart();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        int count = notes_active.Count;
        for (int i=0; i < count; i++)
        {
            notes_active[0].SetActive(false);
            notes_queue.Enqueue(notes_active[0]);
            notes_active.RemoveAt(0);
            notes_judge.RemoveAt(0);
        }

        Bar.SetBarInt(1, 1, BAR_MAX, bar_time);
        Bar.SetBarInt(0, 1, BAR_MAX, bar_power);
    }

    // Update is called once per frame
    void FixedUpdate()
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
        if (t_effect_note > 0)
        {
            t_effect_note -= 1;
            EffectNote(t_effect_note);
        }
        if (t_attack_time > 0)
        {
            if(stop_note == false)
            {
                t_attack_time -= 1;

                if (t_attack_time % 4 == 0)
                {
                    Bar.SetBarInt(t_attack_time / 4, BAR_MAX, BAR_MAX, bar_time);

                    if (t_attack_time == 0)
                    {
                        Miss(1);
                    }
                }
            }
        }
        if (t_effect_missmessage > 0)
        {
            t_effect_missmessage -= 1;
            EffectJudge(t_effect_missmessage, -74f, effect_missmessage, effect_missmessage_img, JUDGE_ANIM_WAIT + 50);
        }

        if (step == STEP_WAIT)
        {
            if (Master.step == 8) // STEP_SELECT_ATTACK_DO を変えたらここも変える
            {
                atk_max = atk_list[Master.skill_level - 1] + (Master.skill_atk - 8f) * 1.3f;
                time = TIME_BOX_MOVE + 1;
                step = STEP_OPEN;
                note_count = 0;
                note_count_max = notes_count_max[Master.skill_level - 1];
                combo_max = note_count_max;
                score_max = combo_max * 2;
                CreateNote(Random.Range(0, 2), 1);
            }
        }
        else if(step == STEP_OPEN)
        {
            MoveBox(1);
        }

        if (step == STEP_GAME)
        {
            if(time > 0)
            {
                time -= 1;

                if(time % 2 == 0)
                {
                    foreach(GameObject note in notes_active)
                    {
                        Move.MoveLocalPosPlusY(note, up_pos[((up_pos.Length - 1) * 2 - time) / 2]);
                    }
                }

                if(time == 0)
                {
                    if (tutorial == true)
                    {
                        SetTap();
                    }
                }
            }
        }
        else if(step == STEP_MISS)
        {
            time -= 1;

            if(time == 0)
            {
                step = STEP_CLOSE;
                time = TIME_BOX_MOVE + 1;
            }
        }
        else if(step == STEP_ATTACK_WAIT)
        {
            if(time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    step = STEP_ATTACK_DO;
                    time = TIME_ATTACK_DO - 1;
                    Move.MoveLocalPosPlusX(jun, 20f);
                    ani_jun.SetInteger("step", 2);
                    int damage = (int)(atk_max * ((score + combo) / (float)(score_max + combo_max)));
                    if(Master.skill_level == 1)
                    {
                        Master.nomalSE.Play("attack1");
                    }
                    else if(Master.skill_level == 2)
                    {
                        Master.nomalSE.Play("attack2");
                    }
                    else
                    {
                        Master.nomalSE.Play("attack3");
                    }
                    bec.Damaged(damage);
                }
            }
        }
        else if(step == STEP_ATTACK_DO)
        {
            if(time > 0)
            {
                time -= 1;

                if(time % 2 == 0)
                {
                    Move.DeccelMoveX(time, TIME_ATTACK_DO, -43, -32, jun);

                    if(time == 0)
                    {
                        step = STEP_ATTACKED_WAIT;
                        time = TIME_ATTACKED_WAIT;
                    }
                }
            }
        }
        else if(step == STEP_ATTACKED_WAIT)
        {
            if(time > 0)
            {
                time -= 1;

                if(time == 0)
                {
                    step = STEP_RETURN_POS;
                    time = TIME_BOX_MOVE + 1;
                    ani_jun.SetInteger("step", 3);
                }
            }
        }
        else if(step == STEP_RETURN_POS)
        {
            if(time > 0)
            {
                MoveBox(-1);
            }

            Transform transform = jun.transform;
            Vector3 pos = transform.localPosition;

            if(pos.x > -62.5f)
            {
                pos.x -= 1;
            }
            else
            {
                End();
            }

            transform.localPosition = pos;
        }

        if(step == STEP_CLOSE)
        {
            MoveBox(-1);
        }
    }

    private void Update()
    {
        if(step == STEP_GAME)
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
                    Master.nomalSE.Play("note_right");
                    TapNote(1);
                }
                else
                {
                    t_button_left = BUTTON_ANIM;
                    ButtonEffectLeft(t_button_left);
                    Master.nomalSE.Play("note_left");
                    TapNote(0);
                }
            }
            else
            {
                //keyboard
                if (Input.GetKeyDown(KeyCode.D))
                {
                    t_button_right = BUTTON_ANIM;
                    ButtonEffectRight(t_button_right);
                    TapNote(1);
                    Master.nomalSE.Play("note_right");
                }
                else
                if (Input.GetKeyDown(KeyCode.A))
                {
                    t_button_left = BUTTON_ANIM;
                    ButtonEffectLeft(t_button_left);
                    Master.nomalSE.Play("note_left");
                    TapNote(0);
                }
            }
        }
    }

    private void CreateNote(int note_kind, int posY = 3)
    {
        if(note_count != note_count_max)
        {
            note_count += 1;
            notes_judge.Add(note_kind);

            GameObject note = notes_queue.Dequeue();
            if(note_count != note_count_max)
            {
                note.GetComponent<Image>().sprite = notes_sprite[note_kind];
            }
            else
            {
                note.GetComponent<Image>().sprite = notes_sprite[2];
            }
            Move.MoveLocalPos(note, (note_kind * 2 - 1) * 17f, 17.5f - 20f * posY);
            note.SetActive(true);
            notes_active.Add(note);
        }
    }

    private void SetNextNotes(bool first = false)
    {
        if(first == false)
        {
            notes_active[0].SetActive(false);
            notes_queue.Enqueue(notes_active[0]);
            notes_active.RemoveAt(0);
            notes_judge.RemoveAt(0);
        }

        CreateNote(Random.Range(0, 2));

        if(done_note_count != note_count_max)
        {
            time = up_pos.Length * 2;
        }

        for(int i=0; i< notes_active.Count; i++)
        {
            Move.MoveLocalPosY(notes_active[i], 17.5f - 20f * (i + 1));
        }
    }
    private void TapNote(int num)
    {
        if (done_note_count != note_count_max & t_attack_time > 0)
        {
            bool next = true;

            if (notes_judge[0] == num)
            {
                JudgeNoteEffect(num, 0);
                score += 2;
                combo += 1;

                if(tutorial == true)
                {
                    stop_note = false;
                    tap.SetActive(false);
                }
            }
            else
            {
                if(tutorial == false)
                {
                    JudgeNoteEffect(num, 2);
                    combo = 0;

                    if (done_note_count != note_count_max - 1)
                    {
                        t_attack_time -= 60;
                        if (t_attack_time <= 0)
                        {
                            t_attack_time = 0;
                            Miss(1);
                        }
                        Bar.SetBarInt(t_attack_time / 4, BAR_MAX, BAR_MAX, bar_time);
                    }
                }
                else
                {
                    next = false;
                }
            }

            if(next == true)
            {
                if (t_attack_time > 0)
                {
                    done_note_count += 1;
                    SetNextNotes();

                    if (done_note_count == note_count_max)
                    {
                        if (combo > 0)
                        {
                            Bar.SetBarInt(score + combo, (score_max + combo_max), BAR_MAX, bar_power);
                            Master.nomalSE.Play("attack_sccess");
                            Attack();
                        }
                        else
                        {
                            Miss(0);
                        }
                        t_attack_time = 0;
                    }
                    else
                    {
                        Bar.SetBarInt(score, (score_max + combo_max), BAR_MAX, bar_power);
                    }
                }
            }
        }
    }

    private void JudgeNoteEffect(int num, int kind)
    {
        Move.MoveLocalPosX(effect_note, (num * 2 - 1) * 17f);
        effect_note_img.sprite = effect_note_sprite[(kind * 3) / 2];
        t_effect_note = NOTE_ANIM;
        pre_judge_kind = kind;

        effect_judge_config.SetJudge((num * 2 - 1) * 17f, 44.5f, kind, JUDGE_ANIM_WAIT, JUDGE_ANIM);
    }

    private void MissMessage(int num)
    {
        Move.MoveLocalPosY(effect_missmessage, -72f);
        effect_missmessage_img.color = new Color(1f, 1f, 1f, 1f);
        effect_missmessage_img.sprite = effect_missmessage_sprite[num];
        t_effect_missmessage = JUDGE_ANIM_WAIT + 51;
    }

    private void SetStart()
    {
        int button_x = 123 + ((int)Status.SCREEN_WIDTH - 150) / 2;
        Move.MoveLocalPosX(button[1], button_x);
        Move.MoveLocalPosX(button[0], -button_x);

        foreach (GameObject note in notes)
        {
            notes_queue.Enqueue(note);
        }
    }
    private void Miss(int num)
    {
        Bar.SetBarInt(0, 1, BAR_MAX, bar_power);
        ani_jun.SetInteger("step", -1);
        step = STEP_MISS;
        time = TIME_MISS;
        MissMessage(num);
        Master.nomalSE.Play("attack_fail");
    }
    private void Attack()
    {
        ani_jun.SetInteger("step", 1);
        step = STEP_ATTACK_WAIT;
        time = TIME_ATTACK_WAIT;
        string play_name = "voice_skill1_" + (Master.skill_level);
        Master.voiceSE.Play(play_name);
    }
    private void MoveBox(int dir)
    {
        time -= 1;

        if (time % 2 == 0)
        {
            if (time > TIME_BOX_MOVE - up_pos.Length * 2)
            {
                Move.MoveLocalPosPlusY(main_box, dir * up_pos[(TIME_BOX_MOVE - time) / 2]);
            }

            int half_time = TIME_BOX_MOVE / 2;
            if ((half_time - (up_pos.Length - 1) * 2 <= time) & (time <= half_time))
            {
                if (time == half_time & dir == 1)
                {
                    CreateNote(Random.Range(0, 2), 2);
                }
                Move.MoveLocalPosPlusY(main_box, dir * up_pos[(half_time - time) / 2]);
            }

            if (time == 0)
            {
                if(dir == 1)
                {
                    step = STEP_GAME;
                    t_attack_time = TIME_ATTACK;
                    SetNextNotes(true);

                    if(tutorial == true)
                    {
                        SetTap();
                    }
                }
                else if(dir == -1)
                {
                    if(step == STEP_CLOSE)
                    {
                        End();
                    }
                }
            }
        }
    }

    private void End()
    {
        ani_jun.SetTrigger("skill_end");
        ani_jun.SetFloat("power", 0);
        ani_jun.SetInteger("skill", 0);
        ani_jun.SetInteger("step", 0);
        step = STEP_WAIT;
        score = 0;
        combo = 0;
        combo_max = 0;
        score_max = 0;
        note_count_max = 0;
        note_count = 0;
        done_note_count = 0;
        time = 0;
        t_attack_time = 0;
        t_button_right = 0;
        t_button_left = 0;
        t_effect_judge = 0;
        t_effect_note = 0;
        ButtonEffectLeft(0);
        ButtonEffectRight(0);
        EffectNote(0);

        Master.EndAttack();
    }

    //Effect
    private void ButtonEffectLeft(int t)
    {
        float color_button = (75 + 100 * (t / (float)(BUTTON_ANIM))) / 255f;
        button_img[0].color = new Color(0, color_button, color_button);
        float color_effect = (200 * (t / (float)(BUTTON_ANIM))) / 255f;
        effect_box_img[0].color = new Color(0, color_effect, color_effect);
    }
    private void ButtonEffectRight(int t)
    {
        float color_button = (75 + 100 * (t / (float)(BUTTON_ANIM))) / 255f;
        button_img[1].color = new Color(color_button, 0, 0);
        float color_effect = (250 * (t / (float)(BUTTON_ANIM))) / 255f;
        effect_box_img[1].color = new Color(color_effect, 0, 0);
    }
    private void EffectJudge(int t, float posY, GameObject effect, Image img, int t_wait)
    {
        if (t == t_wait - 2)
        {
            Move.MoveLocalPosY(effect, posY);
        }

        if (t < JUDGE_ANIM)
        {
            Move.AccelMoveY(t, JUDGE_ANIM, posY, posY + 6, effect);
            img.color = new Color(1f, 1f, 1f, t / (float)JUDGE_ANIM);
        }
    }
    private void EffectNote(int t)
    {
        if (t % NOTE_ANIM_SPEED == 0)
        {
            if (t == 0)
            {
                effect_note_img.sprite = alpha;
            }
            else
            {
                int i = 3 - t / NOTE_ANIM_SPEED;
                effect_note_img.sprite = effect_note_sprite[(pre_judge_kind * 3) / 2 + i];
            }
        }
    }

    private void SetTap()
    {
        stop_note = true;

        tap.SetActive(true);
        Move.SetSamePos(tap, button_img[notes_judge[0]].gameObject, 0, 70);
    }
}
