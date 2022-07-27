using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBoxConfig : MonoBehaviour
{
    const int OPEN_ANIM = 12;
    const int CLOSE_ANIM = 12;
    const int ITEM_MOVE_ANIM = 70;
    const int ITEM_SET_ANIM = 100;

    [SerializeField] MasStageConfig Master;
    [SerializeField] MiniMapConfig Minimap;
    [SerializeField] JunConfig junC;
    [SerializeField] DataBattle BattleData;
    [SerializeField] GameObject item;
    [SerializeField] SpriteRenderer item_sr;
    [SerializeField] FuncMove Move;
    [SerializeField] DataStage Data;
    [SerializeField] GameObject range;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] ItemGetAnim ItemGet;
    [SerializeField] ItemNotGetAnim NotItemGet;

    int event_done;
    int t_open;
    int t_close;
    int t_item_move;
    int t_item_set;

    public int rom_num = -1;
    public int item_num = -1;
    public int myroom;

    public bool tutorial;

    private void OnDisable()
    {
        if(t_open > 0 | t_item_set > 0 | t_item_move > 0 | t_close > 0)
        {
            item.SetActive(false);
            t_close = 0;
            t_open = 0;
            t_item_set = 0;
            t_item_move = 0;

            if (item_num >= 0 | rom_num >= 0)
            {
                if (event_done == 0)
                {
                    if(Status.baggage_count < Status.baggage_count_max)
                    {
                        sr.sprite = Data.box[2];
                        GetItem();
                        event_done = 2;
                    }
                    else
                    {
                        QuitBox();
                    }
                }
                else if (event_done == 1)
                {
                    sr.sprite = Data.box[2];
                    ItemGet.DesetThis();
                    event_done = 2;
                }
                else if(event_done == 3)
                {
                    QuitBox();
                }
            }
            else
            {
                QuitBox();
            }
        }
    }
    private void QuitBox()
    {
        if (junC.play_action == true)
        {
            sr.sprite = Data.box_action;
            junC.play_action = false;
        }
        else
        {
            sr.sprite = Data.box[0];
        }
        item.SetActive(false);
        junC.t_action = 0;
        junC.play_action = false;
        event_done = 0;
        range.tag = "Action";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_open > 0)
        {
            t_open -= 1;

            if(t_open % 6 == 0)
            {
                sr.sprite = Data.box[(OPEN_ANIM - t_open) / 6];
            }
        }
        if(t_item_move > 0)
        {
            t_item_move -= 1;
            if(t_item_move % 2 == 0)
            {
                Move.DeccelMoveY(t_item_move, ITEM_MOVE_ANIM, 2, 30, item);
                item_sr.color = new Color(1f, 1f, 1f, (ITEM_MOVE_ANIM - t_item_move) / (float)ITEM_MOVE_ANIM);
                
                if(t_item_move == 1)
                {
                    ItemGet.DesetThis();
                }

                if(t_item_move == 0)
                {
                    if(item_num >= 0)
                    {
                        if (Status.baggage_count < Status.baggage_count_max)
                        {
                            item_sr.sprite = BattleData.item_sprite[item_num * 3 + 2];
                            t_item_set = ITEM_SET_ANIM;
                            junC.t_action = 0;
                            junC.play_action = false;
                            junC.action = false;
                            GetItem();
                            ItemGet.SetThis(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 11f), Status.items_name[item_num]);
                            NotItemGet.DesetThis();
                        }
                        else
                        {
                            t_close = CLOSE_ANIM;
                            item.SetActive(false);
                            NotItemGet.SetThis(new Vector2(gameObject.transform.position.x + 2f, gameObject.transform.position.y + 11f));
                            ItemGet.DesetThis();
                            Master.nomalSE.Play("action2");
                            event_done = 3;
                        }
                    }
                    if(rom_num >= 0)
                    {
                        item_sr.sprite = BattleData.rom_get_sprite[1];
                        t_item_set = ITEM_SET_ANIM;
                        junC.t_action = 0;
                        junC.play_action = false;
                        junC.action = false;
                        GetItem();
                        ItemGet.SetThis(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 11f), Status.skills_name[rom_num]);
                        NotItemGet.DesetThis();

                        if(tutorial == true)
                        {
                            junC.EndTutorial();
                        }
                    }
                }
            }
        }
        if(t_item_set > 0)
        {
            t_item_set -= 1;
            if(t_item_set == ITEM_SET_ANIM - 7)
            {
                if(item_num >= 0)
                {
                    item_sr.sprite = BattleData.item_sprite[item_num * 3];
                }
                if(rom_num >= 0)
                {
                    item_sr.sprite = BattleData.rom_get_sprite[2];
                }
            }

            if (t_item_set < 20)
            {
                if(t_item_set % 2 == 0)
                {
                    Move.MoveLocalPosPlusY(item, -1);
                    item_sr.color = new Color(1f, 1f, 1f, t_item_set / 20f);
                }
            }

            if (t_item_set == 0)
            {
                event_done = 2;
                item.SetActive(false);
            }
        }

        if (t_close > 0)
        {
            t_close -= 1;

            if(t_close == 6)
            {
                sr.sprite = Data.box2[0];
            }

            if(t_close == 0)
            {
                event_done = 0;
                junC.t_action = 0;
                junC.play_action = false;
                sr.sprite = Data.box_action;
                range.tag = "Action";
            }
        }
    }

    public void OpneBox()
    {
        Master.nomalSE.Play("action1");
        range.tag = "Untagged";
        t_open = OPEN_ANIM;
        sr.sprite = Data.box[0];
        t_item_move = ITEM_MOVE_ANIM;
        Move.MoveLocalPosY(item, 2);
        item_sr.color = new Color(1f, 1f, 1f, 0f);
        item.SetActive(true);
        if(item_num >= 0)
        {
            item_sr.sprite = BattleData.item_sprite[item_num * 3 + 1];
        }
        if(rom_num >= 0)
        {
            item_sr.sprite = BattleData.rom_get_sprite[0];
        }
    }

    private void GetItem()
    {
        Master.nomalSE.Play("select2");
        if (item_num >= 0)
        {
            Status.items_value[item_num] += 1;
            Status.baggage_count += 1;
        }
        if(rom_num >= 0)
        {
            if (!Status.myskill.Contains(rom_num))
            {
                Status.myskill.Add(rom_num);
            }
        }
        event_done = 1;
        Minimap.ChangeActionValue(myroom, -1);
    }
}
