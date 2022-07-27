using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill2Config : MonoBehaviour
{
    private const int NOTE_LENGTH = 26;
    private const int MISS_TIME = 70;
    private const int ATTACK_SHAKE_0 = 60;
    private const int ATTACK_SHAKE_1 = 90;
    private const int ATTACK_SHAKE_2 = 120;
    private const int ATTACK_CREATE_TIME = 10;
    private const int ATTACK_SPEED = 6;
    private const int ATTACK_RATE = 3;
    private const int ATTACK_WAIT = 50;
    private const int NOTE_ANIM_SPEED = 4;
    private const int NOTE_ANIM = 12;
    private const int JUDGE_ANIM_WAIT = 25;
    private const int JUDGE_ANIM = 10;
    private const int BUTTON_ANIM = 12;
    private const int NOTE_PERFECT = 3;
    private const int NOTE_GOOD = 8;
    private const int NOTE_BAD = 12;
    private const int NOTE_TIMING = 14;
    private const int OPEN_CLOSE_SPEED = 12;
    private const int STEP_WAIT = 0;
    private const int STEP_OPEN = 1;
    private const int STEP_DO = 2;
    private const int STEP_ATTACK_WAIT = 3;
    private const int STEP_ATTACK = 4;
    private const int STEP_MISS = 5;

    [SerializeField] EffectJudgeConfig effect_judge_config;
    [SerializeField] private BattleEnemyConfig bec;
    [SerializeField] private GameObject stage;
    [SerializeField] private GameObject[] attack_effect;
    [SerializeField] private Animator ani_jun;
    [SerializeField] private FuncMove Move;
    [SerializeField] private FuncBar Bar;
    [SerializeField] private MasBattleConfig Master;
    [SerializeField] private GameObject[] button;
    [SerializeField] private Image[] button_img;
    [SerializeField] private GameObject first_box;
    [SerializeField] private GameObject main_box;
    [SerializeField] private GameObject main_obj;
    [SerializeField] private Sprite[] note_sprite;
    [SerializeField] private Image effect_box_img;
    [SerializeField] private Image effect_note_img;
    [SerializeField] private Sprite[] effect_note_sprite;
    [SerializeField] private Sprite alpha;
    [SerializeField] private GameObject bar_power;
    [SerializeField] private GameObject[] lines;
    [SerializeField] private GameObject[] notes;
    private Queue<GameObject> notes_queue = new Queue<GameObject>();
    private Queue<GameObject> lines_queue = new Queue<GameObject>();

    private int skill_num = 1;
    private int step;
    private int score;
    private float score_result;
    private int score_num;
    private int max_score;
    private int combo;
    private int max_combo;
    private int pre_score;
    private int[] score_list = new int[3] { 0, 1, 2 }; //bad, good, perfect
    private int[] shake_list = new int[3] { ATTACK_SHAKE_0, ATTACK_SHAKE_1, ATTACK_SHAKE_2 };

    private int[] atk_list = new int[3] { 10, 20, 40 };
    private float atk_max;

    //T
    private int t_open;
    private int t_close;
    private int t_note;
    private int t_button_right;
    private int t_button_left;
    private int t_effect_note;
    private int t_attack_wait;
    private int t_attack;
    private int t_shake;
    private int t_miss;

    //note
    private int[,,] note_arrays = new int[3, 3, NOTE_LENGTH]
    {
        {
            { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0  },
            { 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0  },
        },
        {
            { 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 2, 0, 0, 0, 0, 0, 0  },
            { 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0  },
            { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0  },
        },
        {
            { 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0  },
            { 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 2  },
            { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 2, 0, 0  },
        }
    };
    private List<int> note_array = new List<int>();
    private int note_index;
    private bool note_created;
    private List<GameObject> line_active = new List<GameObject>();
    private List<bool> line_active_done = new List<bool>();
    private List<int> line_active_time = new List<int>();
    private List<GameObject> note_active = new List<GameObject>();
    private List<int> note_active_color = new List<int>();
    private List<int> note_active_time = new List<int>();
    private List<bool> note_active_done = new List<bool>();
    private List<bool> note_tapped = new List<bool>();

    private List<GameObject> attack_active_effect = new List<GameObject>();
    private List<bool> attack_active_effect_done = new List<bool>();

    private void Awake()
    {
        SetStart();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        LoadSkill();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t_close > 0)
        {
            Close();
        }
        if (step == STEP_WAIT)
        {
            if(Master.step == 8) // STEP_SELECT_ATTACK_DO を変えたらここも変える
            {
                atk_max = atk_list[Master.skill_level - 1] + (Master.skill_atk - 8f) * 1.2f;
                first_box.SetActive(false);
                main_box.SetActive(true);
                t_open = OPEN_CLOSE_SPEED;
                step = STEP_OPEN;
            }
        }
        else if(step == STEP_OPEN)
        {
            if(t_open > 0)
            {
                Open();
            }
        }

        if (step == STEP_DO)
        {
            for (int i= 0; i< note_active.Count; i++)
            {
                if(note_active_done[i] == false)
                {
                    GameObject note = note_active[i];
                    Move.MoveLocalPosPlusX(note, -1.5f);

                    int time = t_note - note_active_time[i];
                    if(time == 114)
                    {
                        ResetNote(note, i, true);
                    }
                }
            }

            if (note_created == false)
            {
                SetNote();
            }
        }
        else if(step == STEP_ATTACK_WAIT)
        {
            if (t_attack_wait > 0)
            {
                t_attack_wait -= 1;

                if (t_attack_wait == 0)
                {
                    ani_jun.SetInteger("step", 2);
                    GameObject effect = attack_effect[0];
                    effect.SetActive(true);
                    attack_active_effect.Add(effect);
                    attack_active_effect_done.Add(false);
                    step = STEP_ATTACK;
                    t_shake = shake_list[score_num] + 1;
                    t_close = OPEN_CLOSE_SPEED;

                    Master.voiceSE.Play("voice_skill2_" + (score_num + 1));
                }
            }
        }
        else if(step == STEP_ATTACK)
        {
            if (t_attack < ATTACK_CREATE_TIME * ((score_num + 1) * ATTACK_RATE - 1))
            {
                t_attack += 1;
                if (t_attack % ATTACK_CREATE_TIME == 0)
                {
                    GameObject effect = attack_effect[t_attack / ATTACK_CREATE_TIME];
                    effect.SetActive(true);
                    attack_active_effect.Add(effect);
                    attack_active_effect_done.Add(false);
                }
            }

            int end = 0;
            for (int i = 0; i < attack_active_effect.Count; i++)
            {
                if (attack_active_effect_done[i] == false)
                {
                    GameObject effect = attack_active_effect[i];
                    Transform transform = effect.transform;
                    Vector3 pos = transform.localPosition;
                    pos.x += ATTACK_SPEED;
                    transform.localPosition = pos;

                    if(i % ATTACK_RATE == 0)
                    {
                        if((int)pos.x == 128)
                        {
                            if(i / ATTACK_RATE == score_num)
                            {
                                int damage = (int)(atk_max * score_result);
                                bec.Damaged(damage);
                            }
                            else
                            {
                                bec.Attacked();
                            }

                            Master.nomalSE.Play("attack3");
                        }
                    }

                    if (pos.x - 28.5 > Status.SCREEN_WIDTH)
                    {
                        attack_active_effect_done[i] = true;
                        effect.SetActive(false);
                        Move.MoveLocalPosX(effect, -45.4f);
                        end += 1;
                    }
                }
                else
                {
                    end += 1;
                }
            }
            if (end == attack_active_effect_done.Count)
            {
                End();
            }
        }
        else if(step == STEP_MISS)
        {
            if (t_miss > 0)
            {
                t_miss -= 1;

                if(t_miss == 20)
                {
                    t_close = OPEN_CLOSE_SPEED;
                }

                if(t_miss == 0)
                {
                    End();
                }
            }
        }

        if (step >= STEP_DO)
        {
            MoveLine();
            t_note += 1;
        }

        if (t_button_left > 0)
        {
            t_button_left -= 1;
            ButtonEffectLeft();
        }

        if (t_button_right > 0)
        {
            t_button_right -= 1;
            ButtonEffectRight();
        }

        if(t_effect_note > 0)
        {
            t_effect_note -= 1;
            if (t_effect_note % NOTE_ANIM_SPEED == 0)
            {
                if(t_effect_note == 0)
                {
                    effect_note_img.sprite = alpha;
                }
                else
                {
                    int i = 3 - t_effect_note / NOTE_ANIM_SPEED;
                    effect_note_img.sprite = effect_note_sprite[pre_score * 3 + i];
                }
            }
        }

        if (t_shake > 0)
        {
            t_shake -= 1;
            Move.DeccelShake(t_shake, shake_list[score_num], 2, 0, 1, 5, stage);
        }
    }

    void Update()
    {
        if (step == STEP_DO)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //tap
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float posX = pos.x - Camera.main.transform.position.x;
                if (posX > 0)
                {
                    t_button_right = BUTTON_ANIM;
                    ButtonEffectRight();
                    Master.nomalSE.Play("note_right");
                    TapNote(0);
                }
                else
                {
                    t_button_left = BUTTON_ANIM;
                    ButtonEffectLeft();
                    Master.nomalSE.Play("note_left");
                    TapNote(1);
                }
            }
            else
            {
                //keyboard
                if (Input.GetKeyDown(KeyCode.D))
                {
                    t_button_right = BUTTON_ANIM;
                    ButtonEffectRight();
                    Master.nomalSE.Play("note_right");
                    TapNote(0);
                }
                else
                if (Input.GetKeyDown(KeyCode.A))
                {
                    t_button_left = BUTTON_ANIM;
                    ButtonEffectLeft();
                    Master.nomalSE.Play("note_left");
                    TapNote(1);
                }
            }
        }
    }

    //note
    private void SetNote()
    {
        if(t_note % NOTE_TIMING == 0)
        {
            int i = t_note / NOTE_TIMING;
            int note_kind = note_array[i];
            if (note_kind > 0)
            {
                GameObject note = CreateNote();
                note.SetActive(true);

                int color;
                Image note_img = note.GetComponent<Image>();
                if (note_kind == 2)
                {
                    color = 2;
                    note_created = true;
                    note_img.sprite = note_sprite[2];
                }
                else
                {
                    color = Random.Range(0, 2);
                    note_img.sprite = note_sprite[color];
                }

                note_active.Add(note);
                note_active_color.Add(color);
                note_active_time.Add(t_note);
                note_active_done.Add(false);
                note_tapped.Add(false);
            }

            if(i % 3 == 0)
            {
                GameObject line = CreateLine();
                line.SetActive(true);
                line_active.Add(line);
                line_active_time.Add(t_note);
                line_active_done.Add(false);
            }
        }
    }
    private void ResetNote(GameObject note, int num, bool notap)
    {
        note_active_done[num] = true;
        note.SetActive(false);
        notes_queue.Enqueue(note);
        Move.MoveLocalPosX(note, 85.5f);

        if(notap == true)
        {
            score += score_list[0];
            if (score < 0)
            {
                score = 0;
            }

            if(note_active_color[num] == 2)
            {
                Miss();
            }
        }
    }
    private void TapNote(int need_color)
    {
        for (int i = 0; i < note_active.Count; i++)
        {
            if (note_active_done[i] == false)
            {
                int time = t_note - note_active_time[i];
                GameObject note = note_active[i];

                if(time >= 98 - NOTE_BAD & 98 + NOTE_BAD >= time)
                {
                    int color = note_active_color[i];
                    ResetNote(note, i, false);

                    if (color == need_color | color == 2)
                    {
                        if (time >= 98 - NOTE_PERFECT & 98 + NOTE_PERFECT >= time)
                        {
                            JudgeNote(2, color, i);
                        }
                        else if (time >= 98 - NOTE_GOOD & 98 + NOTE_GOOD >= time)
                        {
                            JudgeNote(1, color, i);
                        }
                        else
                        {
                            JudgeNote(0, color, i);
                        }
                    }
                    else
                    {
                        JudgeNote(0, color, i);
                    }
                    break;
                }
            }
        }
    }

    //box
    private void Open()
    {
        t_open -= 1;
        if (t_open % 4 == 0)
        {
            Move.DeccelMoveY(t_open, OPEN_CLOSE_SPEED, -10, 0, main_obj);

            if (t_open == 0)
            {
                step = STEP_DO;
            }
        }
    }
    private void Close()
    {
        t_close -= 1;
        if (t_close % 2 == 0)
        {
            Move.AccelMoveY(t_close, OPEN_CLOSE_SPEED, 0, -10, main_obj);
        }

        if(t_close == 0)
        {
            first_box.SetActive(true);
            main_box.SetActive(false);
        }
    }

    //note
    private void JudgeNote(int score_num, int color, int num)
    {
        pre_score = score_num;
        int myscore = score_list[score_num];
        if(score_num > 0)
        {
            note_tapped[num] = true;
        }

        if(color == 2)
        {
            for(int i=0; i<note_active.Count; i++)
            {
                if(note_tapped[i] == true)
                {
                    combo += 1;
                }
                else
                {
                    combo = 0;

                    if(note_active_done[i] == false)
                    {
                        ResetNote(note_active[i], i, true);
                    }
                }
            }
        }
        score += myscore;
        if (score < 0)
        {
            score = 0;
        }

        if (color == 2)
        {
            if(score_num > 0)
            {
                Attack();
            }
            else
            {
                Miss();
            }
        }
        else
        {
            ani_jun.SetFloat("power", score / (float)max_score);
        }

        ChangePower();
        JudgeNoteEffect(score_num);
    }
    private void ChangePower()
    {
        Bar.SetBarInt(score + combo, (max_score + max_combo), 57, bar_power);
    }
    private void MoveLine()
    {
        for (int i = 0; i < line_active.Count; i++)
        {
            if (line_active_done[i] == false)
            {
                Move.MoveLocalPosPlusX(line_active[i], -1.5f);
                int time = t_note - line_active_time[i];
                if (time == 114)
                {
                    GameObject line = line_active[i];
                    line.SetActive(false);
                    lines_queue.Enqueue(line);
                    Move.MoveLocalPosX(line, 87f);
                    line_active_done[i] = true;
                }
            }
        }
    }
    private GameObject CreateNote()
    {
        GameObject note;
        note = notes_queue.Dequeue();

        return note;
    }
    private GameObject CreateLine()
    {
        GameObject line;
        line = lines_queue.Dequeue();

        return line;
    }

    //Effect
    private void ButtonEffectLeft()
    {
        float color_button = (75 + 100 * (t_button_left / (float)(BUTTON_ANIM))) / 255f;
        button_img[0].color = new Color(0, color_button, color_button);
        float color_effect = (200 * (t_button_left / (float)(BUTTON_ANIM))) / 255f;
        effect_box_img.color = new Color(0, color_effect, color_effect);
    }
    private void ButtonEffectRight()
    {
        float color_button = (75 + 100 * (t_button_right / (float)(BUTTON_ANIM))) / 255f;
        button_img[1].color = new Color(color_button, 0, 0);
        float color_effect = (250 * (t_button_right / (float)(BUTTON_ANIM))) / 255f;
        effect_box_img.color = new Color(color_effect, 0, 0);
    }
    private void JudgeNoteEffect(int num)
    {
        effect_note_img.sprite = effect_note_sprite[num * 3];
        t_effect_note = NOTE_ANIM;

        effect_judge_config.SetJudge(-60f, 35f, -num + 2, JUDGE_ANIM_WAIT, JUDGE_ANIM);
    }

    //Config
    public void SetStart()
    {
        foreach (GameObject line in lines)
        {
            lines_queue.Enqueue(line);
        }
        foreach (GameObject note in notes)
        {
            notes_queue.Enqueue(note);
        }

        int button_x = 123 + ((int)Status.SCREEN_WIDTH - 150) / 2;
        Move.MoveLocalPosX(button[1], button_x);
        Move.MoveLocalPosX(button[0], -button_x);
    }
    private void LoadSkill()
    {
        note_index = Random.Range(0, note_arrays.GetLength(1));
        for(int i=0; i< NOTE_LENGTH; i++)
        {
            note_array.Add(note_arrays[Master.skill_level - 1, note_index, i]);
        }
        for (int i=0; i< note_array.Count; i++)
        {
            if(note_array[i] > 0)
            {
                max_score += score_list[2];
                max_combo += 1;
            }
        }
    }
    private void Attack()
    {
        ani_jun.SetInteger("step", 1);
        t_attack_wait = ATTACK_WAIT;
        step = STEP_ATTACK_WAIT;
        score_result = (score + combo) / (float)(max_score + max_combo);
        Master.nomalSE.Play("attack_sccess");

        if (score_result < 0.4)
        {
            score_num = 0;
        }
        else if(score_result < 0.8)
        {
            score_num = 1;
        }
        else
        {
            score_num = 2;
        }
    }
    private void Miss()
    {
        Bar.SetBarInt(1, 1, 0, bar_power);
        ani_jun.SetInteger("step", -1);
        step = STEP_MISS;
        t_miss = MISS_TIME;
        Master.nomalSE.Play("attack_fail");
    }

    private void End()
    {
        ani_jun.SetTrigger("skill_end");
        ani_jun.SetFloat("power", 0);
        ani_jun.SetInteger("skill", 0);
        ani_jun.SetInteger("step", 0);
        step = STEP_WAIT;
        note_created = false;
        score = 0;
        combo = 0;
        ChangePower();
        note_array = new List<int>();
        line_active = new List<GameObject>();
        line_active_done = new List<bool>();
        line_active_time = new List<int>();
        note_active = new List<GameObject>();
        note_active_color = new List<int>();
        note_active_time = new List<int>();
        note_active_done = new List<bool>();
        note_tapped = new List<bool>();
        attack_active_effect = new List<GameObject>();
        attack_active_effect_done = new List<bool>();
        t_attack = 0;
        t_note = 0;
        max_combo = 0;
        max_score = 0;

        Master.EndAttack();
    }
}
