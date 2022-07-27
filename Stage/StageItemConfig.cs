using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageItemConfig : MonoBehaviour
{
    const float ROMUPGRADE_POS = -37.5f;
    const int UPGRADE_WAIT_TIME = 45;
    const int CHANGE_PAGE = 6;
    const int USE_ITEM_ANIM = 12;
    const int EFFECT_TIME = 50;
    const int EFFECT_SHAKE_TIME = 6;
    const int EFFECT_DEC_TIME = 10;
    const int CHANGE_TIME = 29;
    const int OPEN_TIME = 20;
    const int CLOSE_TIME = 13;
    const int BUTTON_CHANGE_TIME = 28;
    const int BUTTON_OPEN_ANIM = 12;
    const int BUTTON_CLOSE_ANIM = 16;
    const int SELECT_ITEM = 0;
    const int SELECT_ROM = 1;
    const int SELECT_STATUS_ROM = 2;
    const int SELECT_MATERIAL = 3;
    const int SELECT_MAX = 2;

    const int DEPTH_SELECT_ACT = 0;
    const int DEPTH_SELECT = 1;
    const int DEPTH_ITEM_ACT = 2;

    [SerializeField] CriAtomSource nomalSE;
    [SerializeField] Animator jun_ani;
    [SerializeField] GameObject tops;
    [SerializeField] Controller Control;
    [SerializeField] GameObject jun_item;
    [SerializeField] GameObject buttons_parent;
    [SerializeField] JunConfig junC;
    [SerializeField] MasStageConfig Master;
    [SerializeField] DataNomal DataNomal;
    [SerializeField] FuncMove Move;
    [SerializeField] FuncMiniFigure Fig;
    [SerializeField] DataBattle Data_Battle;
    [SerializeField] DataStage Data_Stage;
    [SerializeField] MasBattleConfig MasterBattle;
    [SerializeField] GameObject selects_mask;
    [SerializeField] GameObject selects_back;
    [SerializeField] GameObject select_parent;
    [SerializeField] GameObject select_buttons_parent;
    [SerializeField] GameObject select_act_button;
    [SerializeField] Image select_act_button_sr;
    [SerializeField] Sprite[] select_act_button_sprites;
    [SerializeField] GameObject[] select_buttons;
    [SerializeField] GameObject select_cursor;
    [SerializeField] Sprite[] select_buttons_sprite;
    [SerializeField] GameObject count_baggage;
    [SerializeField] Image[] fig_baggage;
    [SerializeField] GameObject fig_baggage_slash;
    [SerializeField] Image[] fig_baggage_max;
    [SerializeField] GameObject jun_effect;
    [SerializeField] GameObject romupgrade_box;
    [SerializeField] CanvasGroup romupgrade_alpha;
    [SerializeField] GameObject act_box;
    private SpriteRenderer jun_effect_sr;
    [SerializeField] GameObject jun_effect_text;
    private SpriteRenderer jun_effect_text_sr;

    private float[] slash_pos = new float[3] { 0.5f, 8.5f, 16.5f};
    private int select_depth = 0;
    public int select_num = 0;
    public int stay_item_num = -1;
    private bool stay_item_open = false;
    private bool stay_item_close = false;
    private bool stay_item_used = false;

    public bool can_push_menubutton = false;
    public bool open = false;
    public int t_open;
    public int t_close;
    public int t_open_message;
    public int t_close_message;
    private int t_changepage;

    private int t_open_buttons;
    private int t_close_buttons;
    private int t_change_buttons;

    private int t_use_item;
    private int t_use_item_anim;
    private int t_upgrade_wait;

    //item_status
    public List<int> items_all;
    public List<int> items_inbattle;
    public int item_page = 1;
    private int item_page_max;
    public int rom_page = 1;
    private int rom_page_max;
    public int status_rom_page = 1;
    private int status_rom_page_max;
    public int act_num;
    private int act_item_index;
    private bool act_use;
    private int drop_item_voice_num;


    private List<int> special_item = new List<int> { 6 };
    private float[] button_anim_size = new float[5] { 0.0f, 0.3f, 0.8f, 1.1f, 1.0f };

    // Start is called before the first frame update
    void Start()
    {
        selects_mask.SetActive(false);
        selects_back.SetActive(false);
        jun_effect_sr = jun_effect.GetComponent<SpriteRenderer>();
        jun_effect_text_sr = jun_effect_text.GetComponent<SpriteRenderer>();

        status_rom_page_max = 3 / MasterBattle.pagenum_max + 1;
    }

    private void Update()
    {
        if (Control.GetButtonDownTab())
        {
            Control.PushDownItem();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            SetItemSelect();
        }

        //item select
        if(can_push_menubutton == true)
        {
            if(select_depth == DEPTH_SELECT_ACT)
            {
                if (Control.GetButtonDownLeft())
                {
                    int num = select_num - 1;
                    if(num < 0)
                    {
                        num = SELECT_MAX;
                    }
                    ChangeSelectNum(num);
                } //left

                else if (Control.GetButtonDownRight())
                {
                    int num = select_num + 1;
                    if (num > SELECT_MAX)
                    {
                        num = 0;
                    }
                    ChangeSelectNum(num);
                } //right

                else if (Control.GetButtonDownDecide()) //decide
                {
                    select_depth = DEPTH_SELECT;
                    ChangeSelectButtons(select_num, 1);
                    selects_mask.SetActive(false);
                    select_cursor.SetActive(false);

                    if(select_num == SELECT_ITEM)
                    {
                        MasterBattle.PushItem(MasterBattle.pagenum_min, false);
                    }
                    else if(select_num == SELECT_ROM | select_num == SELECT_STATUS_ROM)
                    {
                        MasterBattle.PushRom(MasterBattle.pagenum_min);
                    }
                }
            } //Item, Rom, StatusRomの選択

            else if(select_depth == DEPTH_SELECT)
            {
                if (Control.GetButtonDownLeft())
                {
                    if (select_num == SELECT_ITEM)
                    {
                        if (items_all.Count > 0)
                        {
                            int selected = Control.GetSelect_inPage(MasterBattle.item_selected, item_page, items_all.Count, -1, MasterBattle.pagenum_min, MasterBattle.pagenum_max);
                            if (MasterBattle.item_selected != selected)
                            {
                                MasterBattle.PushItem(selected, false);
                            }
                        }
                    }
                    else if (select_num == SELECT_ROM)
                    {
                        int selected = Control.GetSelect_inPage(MasterBattle.rom_selected, rom_page, Status.myskill.Count, -1, MasterBattle.pagenum_min, MasterBattle.pagenum_max);
                        if (MasterBattle.rom_selected != selected)
                        {
                            MasterBattle.PushRom(selected);
                        }
                    }
                    else if (select_num == SELECT_STATUS_ROM)
                    {
                        int selected = Control.GetSelect_inPage(MasterBattle.rom_selected, rom_page, Status.status_roms_level.Count, -1, MasterBattle.pagenum_min, MasterBattle.pagenum_max);
                        if (MasterBattle.rom_selected != selected)
                        {
                            MasterBattle.PushRom(selected);
                        }
                    }
                } //left

                else if (Control.GetButtonDownRight())
                {
                    if (select_num == SELECT_ITEM)
                    {
                        if (items_all.Count > 0)
                        {
                            int selected = Control.GetSelect_inPage(MasterBattle.item_selected, item_page, items_all.Count, 1, MasterBattle.pagenum_min, MasterBattle.pagenum_max);
                            if (MasterBattle.item_selected != selected)
                            {
                                MasterBattle.PushItem(selected, false);
                            }
                        }
                    }
                    else if (select_num == SELECT_ROM)
                    {
                        int selected = Control.GetSelect_inPage(MasterBattle.rom_selected, rom_page, Status.myskill.Count, 1, MasterBattle.pagenum_min, MasterBattle.pagenum_max);
                        if (MasterBattle.rom_selected != selected)
                        {
                            MasterBattle.PushRom(selected);
                        }
                    }
                    else if (select_num == SELECT_STATUS_ROM)
                    {
                        int selected = Control.GetSelect_inPage(MasterBattle.rom_selected, rom_page, Status.status_roms_level.Count, 1, MasterBattle.pagenum_min, MasterBattle.pagenum_max);
                        if (MasterBattle.rom_selected != selected)
                        {
                            MasterBattle.PushRom(selected);
                        }
                    }
                } //right

                else if (Control.GetButtonDownUp())
                {
                    if (select_num == SELECT_ITEM)
                    {
                        PushArrowButton(-1);
                    }
                } //up

                else if (Control.GetButtonDownDown())
                {
                    if (select_num == SELECT_ITEM)
                    {
                        PushArrowButton(1);
                    }
                } //Down

                else if (Control.GetButtonDownDecide())
                {
                    if (select_num == SELECT_ITEM)
                    {
                        if (items_all.Count > 0)
                        {
                            select_depth = DEPTH_ITEM_ACT;
                            select_act_button.SetActive(true);
                            Move.SetSamePos(select_act_button, MasterBattle.Data.menu_items[MasterBattle.item_selected], 0, 40f);
                            MasterBattle.Data.item_box.SetActive(false);
                            act_num = 0;
                            MoveSelectAct();

                            int index_count = (item_page - 1) * MasterBattle.pagenum_max + (MasterBattle.item_selected - MasterBattle.pagenum_min);
                            act_item_index = items_all[index_count];
                            if (!items_inbattle.Contains(act_item_index))
                            {
                                select_act_button_sr.sprite = select_act_button_sprites[0];
                                act_use = true;
                            }
                            else
                            {
                                select_act_button_sr.sprite = select_act_button_sprites[1];
                                act_use = false;
                            }
                        }
                    }
                } //decide

            } //どの項目にカーソルを合わせるか選択

            else if(select_depth == DEPTH_ITEM_ACT)
            {
                if (Control.GetButtonDownUp())
                {
                    act_num -= 1;
                    if(act_num < 0)
                    {
                        act_num = 1;
                    }
                    MoveSelectAct();
                } //up

                else if (Control.GetButtonDownDown())
                {
                    act_num += 1;
                    if (act_num > 1)
                    {
                        act_num = 0;
                    }
                    MoveSelectAct();
                } //down

                else if (Control.GetButtonDownDecide())
                {
                    DecidesSelectAct();
                } //decide

            } //アイテムをすてるか使うかの選択
        }
    }

    private void MoveSelectAct()
    {
        if(act_num == 0)
        {
            Move.MoveLocalPosY(act_box, 13.5f);
        }
        else if (act_num == 1)
        {
            Move.MoveLocalPosY(act_box, -3.5f);
        }
    }

    private void DecidesSelectAct()
    {
        if(act_num == 0)
        {
            if(act_use == true)
            {
                UseItem();
            }
        }
        else
        {
            DropItem();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //open select
        if(t_open > 0)
        {
            t_open -= 1;

            Move.DeccelMoveY(t_open, OPEN_TIME, -154, -81, selects_back);
            Move.DeccelMoveY(t_open, OPEN_TIME, -154, -81, selects_mask);

            OpenMenu(t_open, OPEN_TIME);
              
            if(t_open == 0)
            {
                can_push_menubutton = true;
            }
        }

        //close select
        if(t_close > 0)
        {
            t_close -= 1;

            Move.AccelMoveY(t_close, CLOSE_TIME, -81, -154, selects_back);
            Move.AccelMoveY(t_close, CLOSE_TIME, -81, -154, selects_mask);

            CloseMenu(t_close, CLOSE_TIME, select_num);

            if (t_close == 0)
            {
                Control.gameObject.SetActive(true);
                open = false;
                EndMenu(false);
            }
        }

        if(t_open_message > 0)
        {
            OpenMessage();
        }
        if(t_close_message > 0)
        {
            CloseMessage();
        }
        if (t_changepage > 0)
        {
            ChangePageAnim();
        }

        if (t_open_buttons > 0)
        {
            t_open_buttons -= 1;

            if(t_open_buttons % 4 == 0)
            {
                int num = (BUTTON_OPEN_ANIM - t_open_buttons) / 4 + 1;

                ButtonAnim(select_buttons[0], num);
                ButtonAnim(select_buttons[1], num);
                ButtonAnim(select_buttons[2], num);
                ButtonAnim(select_buttons[3], num);
                ButtonAnim(select_cursor, num);
            }
        }
        if(t_close_buttons > 0)
        {
            t_close_buttons -= 1;

            if (t_close_buttons % 4 == 0)
            {
                int num = t_close_buttons / 4;

                ButtonAnim(select_buttons[0], num);
                ButtonAnim(select_buttons[1], num);
                ButtonAnim(select_buttons[2], num);
                ButtonAnim(select_buttons[3], num);
                ButtonAnim(select_cursor, num);
            }
        }
        if(t_change_buttons > 0)
        {
            t_change_buttons -= 1;

            if(t_change_buttons % 4 == 0)
            {
                if (t_change_buttons > BUTTON_OPEN_ANIM)
                {
                    int num = (t_change_buttons - BUTTON_OPEN_ANIM) / 4 - 1;

                    if (stay_item_num == 8)
                    {
                        if(stay_item_open == false)
                        {
                            ButtonAnim(select_buttons[0], num);
                            ButtonAnim(select_buttons[1], num);
                            ButtonAnim(select_buttons[2], num);
                            ButtonAnim(select_buttons[3], num);
                            ButtonAnim(select_cursor, num);
                        }
                        else if(stay_item_close == true)
                        {
                            //ButtonAnim(select_buttons[1], num);
                            //ButtonAnim(select_buttons[2], num);
                        }
                    }
                }
                else
                {
                    if(t_change_buttons == BUTTON_OPEN_ANIM)
                    {
                        if (stay_item_num == 8)
                        {
                            ChangeSelectButtons(select_num, 1);

                            if (stay_item_open == false)
                            {
                                Move.MoveLocalPosX(buttons_parent, -(int)(Status.SCREEN_WIDTH - 200) - 43);
                            }
                            else if (stay_item_close == true)
                            {
                                Move.MoveLocalPosX(buttons_parent, -(int)(Status.SCREEN_WIDTH - 200));
                                if(stay_item_used == false)
                                {
                                    Status.items_value[stay_item_num] += 1;
                                }
                                else
                                {
                                    stay_item_used = false;
                                }
                                stay_item_num = -1;
                                jun_ani.SetBool("item_select_stay", false);
                                jun_ani.ResetTrigger("cancel_item");
                                SetItemList();
                                SetItems();
                            }
                        }
                    }

                    int num = (BUTTON_OPEN_ANIM - t_change_buttons) / 4 + 1;
                    if (stay_item_num == -1)
                    {
                        if (stay_item_close == true)
                        {
                            ButtonAnim(select_buttons[0], num);
                            ButtonAnim(select_buttons[1], num);
                            ButtonAnim(select_buttons[2], num);
                            ButtonAnim(select_buttons[3], num);

                            if (t_change_buttons == 0)
                            {
                                stay_item_open = false;
                                stay_item_close = false;
                            }
                        }
                    }
                    else if(stay_item_num == 8)
                    {
                        if (stay_item_open == false)
                        {
                            //ButtonAnim(select_buttons[1], num);
                            //ButtonAnim(select_buttons[2], num);

                            if (t_change_buttons == 0)
                            {
                                stay_item_open = true;
                            }
                        }
                    }
                }
            }
        }

        if(t_use_item > 0)
        {
            t_use_item -= 1;

            if (t_use_item >= EFFECT_TIME - EFFECT_SHAKE_TIME)
            {
                int t = EFFECT_SHAKE_TIME - (EFFECT_TIME - t_use_item);
                Move.DeccelShake(t, EFFECT_SHAKE_TIME, 4, 0, 2, 3, jun_effect_text);
            }
            else if (t_use_item < EFFECT_DEC_TIME)
            {
                float alpha = t_use_item / (float)EFFECT_DEC_TIME;
                jun_effect_text_sr.color = new Color(1f, 1f, 1f, alpha);
                jun_effect_sr.color = new Color(1f, 1f, 1f, alpha);

                if (t_use_item == 0)
                {
                    jun_effect_text.SetActive(false);
                    jun_effect.SetActive(false);
                }
            }
        }
        if(t_use_item_anim > 0)
        {
            t_use_item_anim -= 1;

            if(t_use_item_anim == 6)
            {
                if(stay_item_num == 8)
                {
                    jun_item.GetComponent<SpriteRenderer>().sprite = Data_Battle.item_sprite[26];
                }
            }
            else if(t_use_item_anim == 0)
            {
                if (stay_item_num == 8)
                {
                    jun_item.GetComponent<SpriteRenderer>().sprite = Data_Battle.item_sprite[24];
                }
            }
        }
        if(t_upgrade_wait > 0)
        {
            t_upgrade_wait -= 1;

            if(t_upgrade_wait == 0) 
            {
                UpgradeReturnToMain();
            }
        }
    }

    public void SelectSetNum(int num)
    {
        if (num == SELECT_ITEM)
        {
            Data_Battle.move_secondbuttons_items.SetActive(true);
            Data_Battle.move_secondbuttons_items_value.SetActive(true);
            count_baggage.SetActive(true);
            SetItemList();
            SetItems();
        }
        else if(num == SELECT_ROM)
        {
            Data_Battle.move_secondbuttons_roms.SetActive(true);
            Data_Battle.move_secondbuttons_roms_sp.SetActive(true);
            Move.MoveLocalPosY(Data_Battle.move_secondbuttons_roms, MasterBattle.pos_move_secondbuttons[0]);
            Move.MoveLocalPosY(Data_Battle.move_secondbuttons_roms_sp, MasterBattle.pos_move_secondbuttons[0]);
            SetRoms();
        }
        else if(num == SELECT_STATUS_ROM)
        {
            Data_Battle.move_secondbuttons_roms.SetActive(true);
            Data_Battle.move_secondbuttons_status_roms_level.SetActive(true);
            Move.MoveLocalPosY(Data_Battle.move_secondbuttons_roms, MasterBattle.pos_move_secondbuttons[0]);
            Move.MoveLocalPosY(Data_Battle.move_secondbuttons_status_roms_level, MasterBattle.pos_move_secondbuttons[0]);
            SetStatusRoms();
        }
    }
    public void SelectEndNum(int num)
    {
        if (num == SELECT_ITEM)
        {
            Data_Battle.move_secondbuttons_items.SetActive(false);
            Data_Battle.move_secondbuttons_items_value.SetActive(false);
            count_baggage.SetActive(false);
        }
        else if(num == SELECT_ROM)
        {
            Data_Battle.move_secondbuttons_roms.SetActive(false);
            Data_Battle.move_secondbuttons_roms_sp.SetActive(false);
        }
        else if(num == SELECT_STATUS_ROM)
        {
            Data_Battle.move_secondbuttons_roms.SetActive(false);
            Data_Battle.move_secondbuttons_status_roms_level.SetActive(false);
        }
    }
    public void OpenMenu(int time, int time_max)
    {
        Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_box);

        if (select_num == SELECT_ITEM)
        {
            Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_items);
            Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_items_value);
        }
        else if(select_num == SELECT_ROM)
        {
            Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_roms);
            Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_roms_sp);
        }
        else if(select_num == SELECT_STATUS_ROM)
        {
            Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_roms);
            Move.DeccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[1], MasterBattle.pos_move_secondbuttons[0], Data_Battle.move_secondbuttons_status_roms_level);
        }

        if(stay_item_num == 8)
        {
            romupgrade_alpha.alpha = (time_max - time) / (float)time_max;
            Move.DeccelMoveY(time, time_max, ROMUPGRADE_POS + 10f, ROMUPGRADE_POS , romupgrade_box);
        }
    }
    public void CloseMenu(int time, int time_max, int num)
    {
        Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_box);

        if (num == SELECT_ITEM)
        {
            Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_items);
            Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_items_value);
        }
        else if(num == SELECT_ROM)
        {
            Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_roms);
            Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_roms_sp);
        }
        else if (num == SELECT_STATUS_ROM)
        {
            Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_roms);
            Move.AccelMoveY(time, time_max, MasterBattle.pos_move_secondbuttons[0], MasterBattle.pos_move_secondbuttons[1], Data_Battle.move_secondbuttons_status_roms_level);
        }

        if (stay_item_num == 8 & stay_item_open == true)
        {
            romupgrade_alpha.alpha = time / (float)time_max;
            Move.AccelMoveY(time, time_max, ROMUPGRADE_POS, ROMUPGRADE_POS + 10f, romupgrade_box);

            if(time == 0)
            {
                romupgrade_alpha.alpha = 0;
                romupgrade_box.SetActive(false);
            }
        }
    }
    private void OpenMessage()
    {
        t_open_message -= 1;
        Move.DeccelMoveY(t_open_message, 15, MasterBattle.pos_move_message[0], MasterBattle.pos_move_message[1], Data_Battle.menu_item_message);

        Move.DeccelMoveY(t_open_message, 15, MasterBattle.pos_move_message[1], -MasterBattle.pos_move_message[0], tops);
        Move.DeccelMoveY(t_open_message, 15, MasterBattle.pos_move_message[1], -MasterBattle.pos_move_message[0], Master.stageUI_map);
    }
    private void CloseMessage()
    {
        t_close_message -= 1;
        Move.DeccelMoveY(t_close_message, 15, MasterBattle.pos_move_message[1], MasterBattle.pos_move_message[0], Data_Battle.menu_item_message);

        Move.DeccelMoveY(t_close_message, 15, -MasterBattle.pos_move_message[0], MasterBattle.pos_move_message[1], tops);
        Move.DeccelMoveY(t_close_message, 15, -MasterBattle.pos_move_message[0], MasterBattle.pos_move_message[1], Master.stageUI_map);
    }

    public void SetItemSelect()
    {
        if (open == false & (t_open == 0 & t_close == 0))
        {
            if (Master.battle_status == false & junC.Moveroom == false & junC.play_action == false & junC.canmove == true)
            {
                select_cursor.SetActive(true);
                jun_ani.ResetTrigger("items_select_end");
                select_num = SELECT_ITEM;
                t_open_buttons = BUTTON_OPEN_ANIM + 1;
                t_open = OPEN_TIME + 1;
                select_parent.SetActive(true);
                selects_back.SetActive(true);
                selects_mask.SetActive(true);
                count_baggage.SetActive(true);
                Data_Battle.move_secondbuttons_box.SetActive(true);
                select_buttons_parent.SetActive(true);
                MasterBattle.item_selected = -1;
                MasterBattle.rom_selected = -1;
                act_num = 0;
                open = true;
                Move.MoveLocalPosX(buttons_parent, -(int)(Status.SCREEN_WIDTH - 200));
                junC.OpenSelectItems();
                nomalSE.Play("select9");

                SelectSetNum(select_num);
                Control.PushItem(2);
                Control.gameObject.SetActive(false);
            }
        }
        else if (open == true & (t_open == 0 & t_close == 0) & can_push_menubutton == true)
        {
            nomalSE.Play("select10");

            if (stay_item_num == -1)
            {
                if (Master.battle_status == false & junC.Moveroom == false & junC.canmove == true)
                {
                    ReturnDepth();
                }
            }
            else
            {
                UpgradeReturnToMain();
                Control.PushItem(2);
            }
        }
    }
    private void ReturnDepth()
    {
        if (select_depth == DEPTH_SELECT_ACT)
        {
            t_close_buttons = BUTTON_CLOSE_ANIM + 1;
            t_close = CLOSE_TIME + 1;
            can_push_menubutton = false;

            Control.PushItem(0);
        }
        else if (select_depth == DEPTH_SELECT)
        {
            select_depth = DEPTH_SELECT_ACT;
            ChangeSelectButtons(select_num, 0);
            selects_mask.SetActive(true);
            select_cursor.SetActive(true);
            Control.PushItem(2);

            if (select_num == SELECT_ITEM)
            {
                MasterBattle.DeselectItem(false, false);
            }
            else if(select_num == SELECT_ROM | select_num == SELECT_STATUS_ROM)
            {
                MasterBattle.DeselectRom(false, false);
            }
        }
        else if (select_depth == DEPTH_ITEM_ACT)
        {
            select_depth = DEPTH_SELECT;
            Control.PushItem(2);
            select_act_button.SetActive(false);
            MasterBattle.Data.item_box.SetActive(true);
        }
    }


    private void SetItemList()
    {
        items_inbattle = new List<int>();
        items_all = new List<int>();
        int value_all = 0;
        
        for (int i = 0; i < Status.items_value.Length; i++)
        {
            int value = Status.items_value[i];
            if (value > 0)
            {
                if (Status.items_use[i] == 1)
                {
                    items_inbattle.Add(i);
                }

                value_all += value;
                items_all.Add(i);
            }
        }

        Status.baggage_count = value_all;
        item_page_max = (items_all.Count - 1) / MasterBattle.pagenum_max + 1;
        Fig.ChangeFigure_SlashtoLeft(value_all, fig_baggage, fig_baggage_slash, slash_pos, DataNomal.figure_mini_back);
        Fig.ChangeFigure(Status.baggage_count_max, fig_baggage_max, false, DataNomal.figure_mini_back);
    }
    private void SetItems()
    {
        MasterBattle.ChangeItempageFigure(item_page, item_page_max);
        MasterBattle.SetItem(items_all, item_page);

        int rom_index_max = items_all.Count;
        if (rom_index_max != 0)
        {
            if ((item_page - 1) * MasterBattle.pagenum_max >= rom_index_max)
            {
                item_page -= 1;
            }
        }
    }
    private void SetRoms()
    {
        rom_page_max = (Status.myskill.Count - 1) / MasterBattle.pagenum_max + 1;
        MasterBattle.ChangeItempageFigure(rom_page, rom_page_max);
        MasterBattle.SetRom(rom_page, false);
    }
    private void SetStatusRoms()
    {
        MasterBattle.ChangeItempageFigure(status_rom_page, status_rom_page_max);
        int romnum = 0 + MasterBattle.pagenum_min;
        for (int i = (status_rom_page - 1) * MasterBattle.pagenum_max; i < status_rom_page * MasterBattle.pagenum_max; i++)
        {
            GameObject rom = Data_Battle.menu_roms[romnum];

            if (i < 4)
            {
                rom.SetActive(true);
                Data_Battle.rom_status_level[romnum].SetActive(true);
                Data_Battle.rom_label_image[romnum].gameObject.SetActive(false);
                Data_Battle.rom_image[romnum].sprite = Data_Battle.rom_status_speite[i];

                if(stay_item_num == 8)
                {
                    if(Status.status_roms_level[i] == 5)
                    {
                        Data_Battle.rom_image[romnum].sprite = Data_Battle.rom_status_speite[i + 4];
                    }
                }

                Image level_img = Data_Battle.rom_status_level_image[romnum];
                int level = Status.status_roms_level[i];
                level_img.sprite = DataNomal.figure_mini_back[level];

                if (level == 5)
                {
                    level_img.color = new Color(1f, 1f, 0f);
                }
                else
                {
                    level_img.color = new Color(1f, 1f, 1f);
                }
            }
            else
            {
                if(romnum < 5)
                {
                    Data_Battle.rom_status_level[romnum].SetActive(false);
                }
                rom.SetActive(false);
            }

            romnum += 1;
        }
    }

    public void PushArrowButtonDown(int num)
    {
        if(can_push_menubutton == true)
        {
            Data_Battle.button_second_arrow[num].sprite = Data_Battle.button_second_arrow_pushdown[1];
        }
    }
    public void PushArrowButtonUp(int num)
    {
        Data_Battle.button_second_arrow[num].sprite = Data_Battle.button_second_arrow_pushup[1];
    }
    public void PushArrowButton(int num)
    {
        if (can_push_menubutton == true)
        {
            if(num == 1)
            {
                if (select_num == SELECT_ROM)
                {
                    if(rom_page_max != 1)
                    {
                        int page = rom_page - 1;
                        if (page < 1)
                        {
                            page = rom_page_max;
                        }
                        ChangePage(page, false);
                    }
                }
                else if (select_num == SELECT_ITEM)
                {
                    if (item_page_max != 1)
                    {
                        int page = item_page - 1;
                        if(page < 1)
                        {
                            page = item_page_max;
                        }
                        ChangePage(page, false);
                    }
                }
                else if (select_num == SELECT_STATUS_ROM)
                {
                    if (status_rom_page > 1)
                    {
                        ChangePage(-1, false);
                    }
                }
            }
            else
            {
                if (select_num == SELECT_ROM)
                {
                    if (rom_page < rom_page_max)
                    {
                        ChangePage(1, false);
                    }
                }
                else if (select_num == SELECT_ITEM)
                {
                    if (item_page_max != 1)
                    {
                        int page = item_page + 1;
                        if (page > item_page_max)
                        {
                            page = 1;
                        }
                        ChangePage(page, false);
                    }
                }
                else if (select_num == SELECT_STATUS_ROM)
                {
                    if (status_rom_page < status_rom_page_max)
                    {
                        ChangePage(1, false);
                    }
                }
            }
        }
    }
    private void ChangePage(int page, bool use_item)
    {
        nomalSE.Play("select9");

        if (use_item == false)
        {
            t_changepage = CHANGE_PAGE + 1;
        }

        if (select_num == SELECT_ITEM)
        {
            item_page = page;
            SetItems();

            MasterBattle.item_selected = -1;
            MasterBattle.PushItem(MasterBattle.pagenum_min, true);
        }
        else if (select_num == SELECT_ROM)
        {
            if(stay_item_num != 8)
            {
                rom_page = page;
                SetRoms();
            }
            else
            {
                item_page = page;
                SetItems();
            }
        }
        else if(select_num == SELECT_STATUS_ROM)
        {
            status_rom_page = page;
            SetStatusRoms();
        }
    }
    private void ChangePageAnim()
    {
        t_changepage -= 1;
        if (select_num == SELECT_ROM)
        {
            Move.DeccelShake(t_changepage, CHANGE_PAGE, 10, 0, -2, 3, Data_Battle.move_secondbuttons_roms);
        }
        else if (select_num == SELECT_ITEM)
        {
            Move.DeccelShake(t_changepage, CHANGE_PAGE, 10, 0, -2, 3, Data_Battle.move_secondbuttons_items);
        }
        else if(select_num == SELECT_STATUS_ROM)
        {
            Move.DeccelShake(t_changepage, CHANGE_PAGE, 10, 0, -2, 3, Data_Battle.move_secondbuttons_roms);
        }
    }
    public void ReloadPage(bool item_zero)
    {
        SetItemList();

        if (item_zero == true)
        {
            ReturnDepth();
        }

        if (item_page_max < item_page)
        {
            ChangePage(item_page - 1, true);
        }
        else
        {
            SetItems();

            if(item_zero == true)
            {
                if(items_all.Count == 0)
                {
                    MasterBattle.DeselectItem(false, false);
                }
                else
                {
                    int selected = MasterBattle.item_selected;
                    MasterBattle.item_selected = -1;

                    int index_count = (item_page - 1) * MasterBattle.pagenum_max + (selected - MasterBattle.pagenum_min);
                    if(index_count > items_all.Count - 1)
                    {
                        selected -= 1;
                    }

                    MasterBattle.PushItem(selected, true);
                }
            }
        }
    }

    private void EndMenu(bool quit)
    {
        junC.CloseSelectItems();
        select_parent.SetActive(false);
        selects_back.SetActive(false);
        selects_mask.SetActive(false);
        select_buttons_parent.SetActive(false);
        Data_Battle.menu_item_message.SetActive(false);

        Move.MoveLocalPosY(Master.stageUI_map, MasterBattle.pos_move_message[1]);
        Move.MoveLocalPosY(tops, MasterBattle.pos_move_message[1]);
        Move.MoveLocalPosY(Data_Battle.menu_item_message, MasterBattle.pos_move_message[0]);
        Move.MoveLocalPosY(selects_back, -154);
        Move.MoveLocalPosY(selects_mask, -154);
        Move.MoveLocalPosY(Data_Battle.move_secondbuttons_box, MasterBattle.pos_move_secondbuttons[1]);
        Move.MoveLocalPosY(Data_Battle.move_secondbuttons_items, MasterBattle.pos_move_secondbuttons[1]);
        Move.MoveLocalPosY(Data_Battle.move_secondbuttons_items_value, MasterBattle.pos_move_secondbuttons[1]);
        Move.MoveLocalPosY(Data_Battle.move_secondbuttons_roms, MasterBattle.pos_move_secondbuttons[1]);
        Move.MoveLocalPosY(Data_Battle.move_secondbuttons_roms_sp, MasterBattle.pos_move_secondbuttons[1]);
        Move.MoveLocalPosY(Data_Battle.move_secondbuttons_status_roms_level, MasterBattle.pos_move_secondbuttons[1]);
        Move.SetSamePos(select_cursor, select_buttons[0], 0, -2);

        item_page = 1;
        rom_page = 1;
        status_rom_page = 1;

        if (quit == false)
        {
            SelectEndNum(select_num);
        }
        ChangeSelectButtons(select_num, 0);

        can_push_menubutton = false;
        open = false;

        for (int i = MasterBattle.pagenum_min; i < 7 - MasterBattle.pagenum_min; i++)
        {
            Data_Battle.item_image[i].color = new Color(1f, 1f, 1f, 1f);
        }

        if(select_num != SELECT_ITEM)
        {
            count_baggage.SetActive(true);
            ButtonAnim(count_baggage, 4);
        }

        if(t_use_item > 0)
        {
            t_use_item = 0;
            jun_effect_text_sr.color = new Color(1f, 1f, 1f, 1f);
            jun_effect_sr.color = new Color(1f, 1f, 1f, 1f);
            jun_effect_text.SetActive(false);
            jun_effect.SetActive(false);
            Move.MoveLocalPos(jun_effect_text, 0, 25);
        }

        select_num = 0;
        select_depth = DEPTH_SELECT_ACT;

        if (stay_item_num != -1)
        {
            if(stay_item_used == false)
            {
                Status.items_value[stay_item_num] += 1;
            }
            stay_item_num = -1;
        }
        stay_item_open = false;
        stay_item_close = false;
        stay_item_used = false;

        Move.MoveLocalPosY(romupgrade_box, ROMUPGRADE_POS + 10f);
        romupgrade_box.SetActive(false);
        romupgrade_alpha.alpha = 0;

        jun_ani.SetBool("item_select_stay", false);
        jun_ani.ResetTrigger("cancel_item");
        jun_ani.ResetTrigger("items_use");
        jun_ani.ResetTrigger("items_drop");
    }

    public void QuitMenu()
    {
        if(open == true)
        {
            if(MasterBattle.item_selected != -1)
            {
                Data_Battle.item_box.SetActive(false);
                MasterBattle.ChangeValue(MasterBattle.item_selected, 1, item_page, items_all);
                MasterBattle.item_selected = -1;
            }
            if(MasterBattle.rom_selected != -1)
            {
                SetStatusRomValue(-1);
                Data_Battle.rom_box.SetActive(false);
                MasterBattle.rom_selected = -1;
                MasterBattle.SetRom(rom_page, false);
            }

            Data_Battle.move_secondbuttons_items.SetActive(false);
            Data_Battle.move_secondbuttons_items_value.SetActive(false);
            Data_Battle.move_secondbuttons_roms.SetActive(false);
            Data_Battle.move_secondbuttons_roms_sp.SetActive(false);
            Data_Battle.move_secondbuttons_status_roms_level.SetActive(false);

            ButtonAnim(select_buttons[0], 0);
            ButtonAnim(select_buttons[1], 0);
            ButtonAnim(select_buttons[2], 0);
            ButtonAnim(select_buttons[3], 0);
            ButtonAnim(select_cursor, 0);

            t_open = 0;
            t_close = 0;
            t_open_message = 0;
            t_close_message = 0;
            t_changepage = 0;
            t_open_buttons = 0;
            t_close_buttons = 0;
            t_change_buttons = 0;
            t_use_item_anim = 0;
            t_upgrade_wait = 0;

            jun_item.SetActive(false);
            junC.CancelItem(true);

            Control.PushItem(0);

            EndMenu(true);
        }
    }

    public void ChangeSelectNum(int num)
    {
        if(can_push_menubutton == true & select_num != num)
        {
            nomalSE.Play("select9");

            if (select_num == SELECT_ITEM)
            {
                MasterBattle.DeselectItem(false, false);
            }
            else if (select_num == SELECT_ROM)
            {
                MasterBattle.DeselectRom(false, false);
                MasterBattle.SetRom(rom_page, false);
            }
            else if(select_num == SELECT_STATUS_ROM)
            {
                MasterBattle.DeselectRom(false, false);
                SetStatusRomValue(-1);
            }

            SelectEndNum(select_num);

            act_num = 0;
            select_num = num;

            SelectSetNum(select_num);

            Move.SetSamePos(select_cursor, select_buttons[num], 0, -2);

            t_change_buttons = BUTTON_CHANGE_TIME + 1;
        }
    }
    private void ChangeSelectButtons(int num, int num_plus)
    {
        int sprite_num = num * 2 + num_plus;
        select_buttons[num].GetComponent<Image>().sprite = select_buttons_sprite[sprite_num];
    }
    private void ButtonAnim(GameObject obj, int num)
    {
        Transform transform = obj.transform;
        Vector3 scale = transform.localScale;
        scale.y = button_anim_size[num];
        transform.localScale = scale;
    }

    public void UseItem()
    {
        int index = act_item_index;

        if(!special_item.Contains(index))
        {
            junC.UseItem(index);

            if (index == 0 | index == 1)
            {
                nomalSE.Play("select5");
                t_use_item = EFFECT_TIME + 1;
                if (index == 0)
                {
                    Status.hp += 30;
                }
                else
                {
                    Status.hp += 100;
                }
                
                if(Status.hp > Status.maxhp)
                {
                    Status.hp = Status.maxhp;
                }
                MasterBattle.SetHP(-1, Data_Stage.hp_bar_dec, Data_Stage.hp_bar, Data_Stage.hp_slash, Data_Stage.hp_fig);
                MasterBattle.ItemEffect(1, jun_effect, jun_effect.GetComponent<Animator>());

                jun_effect_text_sr.color = new Color(1f, 1f, 1f, 1f);
                jun_effect_sr.color = new Color(1f, 1f, 1f, 1f);
                jun_effect_text.SetActive(true);
                Move.MoveLocalPos(jun_effect_text, 0, 25);
                jun_effect_text_sr.sprite = Data_Stage.effect_text[index];
            }
            else if(index == 7)
            {
                nomalSE.Play("select5");
                t_use_item = EFFECT_TIME + 1;
                Status.sp += 20;

                MasterBattle.SetSP(-1, Data_Stage.sp_bar_dec, Data_Stage.sp_bar, Data_Stage.sp_slash, Data_Stage.sp_fig, Data_Stage.spnum_fig);
                MasterBattle.ItemEffect(4, jun_effect, jun_effect.GetComponent<Animator>());

                jun_effect_text_sr.color = new Color(1f, 1f, 1f, 1f);
                jun_effect_sr.color = new Color(1f, 1f, 1f, 1f);
                jun_effect_text.SetActive(true);
                Move.MoveLocalPos(jun_effect_text, 0, 25);
                jun_effect_text_sr.sprite = Data_Stage.effect_text[2];
            }
            else if(index == 8)
            {
                stay_item_num = 8;

                select_num = SELECT_ROM;

                t_change_buttons = BUTTON_CHANGE_TIME + 1;
                can_push_menubutton = false;

                romupgrade_alpha.alpha = 0f;
                romupgrade_box.SetActive(true);
                Move.MoveLocalPosY(romupgrade_box, ROMUPGRADE_POS + 10f);

                jun_item.SetActive(true);
                if (junC.sr.flipX == false)
                {
                    Move.MoveLocalPosX(jun_item, 11f);
                }
                else
                {
                    Move.MoveLocalPosX(jun_item, -11f);
                }

                t_use_item_anim = USE_ITEM_ANIM;
                jun_item.GetComponent<SpriteRenderer>().sprite = Data_Battle.item_sprite[25];
            }

            int item_value = Status.items_value[index] - 1;
            Status.items_value[index] = item_value;

            if (item_value > 0)
            {
                ReloadPage(false);
            }
            else
            {
                ReloadPage(true);
            }
        }
    }

    public void DropItem()
    {
        junC.DropItem();

        MasterBattle.voiceSE.Play("voice_drop" + (drop_item_voice_num + 1));
        drop_item_voice_num += 1;
        if (drop_item_voice_num == 2)
        {
            drop_item_voice_num = 0;
        }

        int item_value = Status.items_value[act_item_index] - 1;
        Status.items_value[act_item_index] = item_value;

        if (item_value > 0)
        {
            ReloadPage(false);
        }
        else
        {
            ReloadPage(true);
        }
    }

    public void SetStatusRomValue(int num)
    {
        int index = (num - MasterBattle.pagenum_min) + (status_rom_page - 1) * MasterBattle.pagenum_max;

        int romnum = 0 + MasterBattle.pagenum_min;
        for (int i = (status_rom_page - 1) * MasterBattle.pagenum_max; i < status_rom_page * MasterBattle.pagenum_max; i++)
        {
            if (i < 4)
            {
                Image level_img = Data_Battle.rom_status_level_image[romnum];

                int level;
                if (i == index)
                {
                    level = Status.status_roms_level[i] + 1;
                    if(level == 6)
                    {
                        level_img.color = new Color(1f, 1f, 0f);
                        level = 5;
                    }
                    else
                    {
                        level_img.color = new Color(1f, 0f, 1f);
                    }
                }
                else
                {
                    level = Status.status_roms_level[i];
                    if(level == 5)
                    {
                        level_img.color = new Color(1f, 1f, 0f);
                    }
                    else
                    {
                        level_img.color = new Color(1f, 1f, 1f);
                    }
                }
                level_img.sprite = DataNomal.figure_mini_back[level];
            }
            else
            {
                break;
            }

            romnum += 1;
        }
    }
    public void UpgradeStatusRom(int num)
    {
        int index = (num - MasterBattle.pagenum_min) + (status_rom_page - 1) * MasterBattle.pagenum_max;

        nomalSE.Play("select4");
        int plus = Status.status_roms_up[index];
        if (index == 0)
        {
            Status.maxhp += plus;
            Status.hp += plus;
            MasterBattle.SetHPinBattle();
            t_use_item = EFFECT_TIME + 1;
        }
        else if(index == 1)
        {
            Status.sp += plus;
            MasterBattle.SetSPinBattle();
            t_use_item = EFFECT_TIME + 1;
        }
        else if(index == 2)
        {
            Status.atk += 3;
        }
        else if(index == 3)
        {
            Status.guard_maxhp += 15;
            Status.guard_hp = Status.guard_maxhp;
            Status.guard_sp += 3;
        }

        Status.status_roms_level[index] += 1;
        SetStatusRomValue(-1);
        SetStatusRoms();
    }
    public void UpgradeRom(int num)
    {
        jun_item.SetActive(false);
        junC.UseItem(0);

        nomalSE.Play("select4");
        can_push_menubutton = false;
        int skill = Status.myskill[(rom_page - 1) * MasterBattle.pagenum_max + (num - MasterBattle.pagenum_min)];
        Status.myskill_level[skill] += 1;

        t_upgrade_wait = UPGRADE_WAIT_TIME;
        stay_item_used = true;
    }
    private void UpgradeReturnToMain()
    {
        if (MasterBattle.rom_selected != -1)
        {
            MasterBattle.DeselectRom(false, false);
            if (select_num == SELECT_ROM)
            {
                MasterBattle.SetRom(rom_page, false);
            }
            else if (select_num == SELECT_STATUS_ROM)
            {
                SetStatusRomValue(-1);
            }
        }

        select_num = SELECT_ITEM;
        junC.CancelItem(false);

        t_change_buttons = BUTTON_CHANGE_TIME + 1;
        can_push_menubutton = false;
        stay_item_close = true;
        jun_item.SetActive(false);
    }
}
