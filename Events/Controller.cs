using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public bool out_of_stage;

    [SerializeField] MasBattleConfig MasterBattle;
    [SerializeField] MasStageConfig Master;
    [SerializeField] StageItemConfig ItemC;
    [SerializeField] GameObject WASD;
    [SerializeField] JunConfig junC;
    [SerializeField] Text mytext;

    [SerializeField] Sprite[] WASD_sprite;
    [SerializeField] Image[] WASD_img;

    public CanvasGroup controller_alpha;
    [SerializeField] GameObject dash_button;
    [SerializeField] GameObject action_button;
    [SerializeField] GameObject guard_button;
    public Sprite[] dash_sprite;
    public Image dash_img;
    public Sprite[] action_sprite;
    public Image action_img;
    public Sprite[] guard_sprite;
    public Image guard_img;
    [SerializeField] Sprite[] item_sprite;
    [SerializeField] Image item_img;

    private Vector2 WASD_pos;
    private Vector2 touch_pos;
    private int touchID;
    private bool move_touch;

    public bool dash = false;

    public int Hdirection;
    public int Vdirection;

    // Start is called before the first frame update
    void OnDisable()
    {
        if(out_of_stage == false)
        {
            for (int i = 0; i < 4; i++)
            {
                WASD_img[i].sprite = WASD_sprite[i];
            }
            dash_img.sprite = dash_sprite[0];
            action_img.sprite = action_sprite[0];
            guard_img.sprite = guard_sprite[0];
            dash = false;
            move_touch = false;

            Hdirection = 0;
            Vdirection = 0;
        }
    }

    private void Update()
    {
        if (out_of_stage == false)
        {
            if (dash == true)
            {
                junC.ControllRolling();
            }
        }
    }

    private void FixedUpdate()
    {
        if (out_of_stage == false)
        {
            if (move_touch == true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(touchID);
                    touch_pos = new Vector2(touch.position.x, touch.position.y);
                }

                Vector3 pos = Camera.main.ScreenToWorldPoint(touch_pos);
                Vector2 sa_pos = new Vector2(pos.x - WASD.transform.position.x, pos.y - WASD.transform.position.y);
                mytext.text = "" + sa_pos.x + ", " + sa_pos.y;

                float rad = Mathf.Atan2(sa_pos.y, sa_pos.x);
                float degree = rad * Mathf.Rad2Deg;

                if (-24.5f <= degree & degree <= 24.5f) //右
                {
                    Hdirection = 1;
                    Vdirection = 0;
                }
                else if (24.5f <= degree & degree <= 65.5f) //右上
                {
                    Hdirection = 1;
                    Vdirection = 1;
                }
                else if (65.5f <= degree & degree <= 114.5f) //上
                {
                    Hdirection = 0;
                    Vdirection = 1;
                }
                else if (114.5f <= degree & degree <= 155.5f) //左上
                {
                    Hdirection = -1;
                    Vdirection = 1;
                }
                else if (155.5f <= degree | degree <= -155.5f) //左
                {
                    Hdirection = -1;
                    Vdirection = 0;
                }
                else if (-155.5f <= degree & degree <= -110.5f) //左下
                {
                    Hdirection = -1;
                    Vdirection = -1;
                }
                else if (-110.5f <= degree & degree <= -69.5f) //下
                {
                    Hdirection = 0;
                    Vdirection = -1;
                }
                else if (-69.5f <= degree & degree <= -24.5f) //右下
                {
                    Hdirection = 1;
                    Vdirection = -1;
                }
            }
        }
    }

    //dash
    public void PushDownDash()
    {
        if (junC.Moveroom == false & junC.canmove == true & junC.play_action == false)
        {
            dash_img.sprite = dash_sprite[1];
            dash = true;
        }
    }
    public void PushUpDash()
    {
        dash = false;
        dash_img.sprite = dash_sprite[0];
    }

    //action
    public void PushDownAction()
    {
        action_img.sprite = action_sprite[1];
        junC.ControllAction();
    }
    public void PushUpAction()
    {
        action_img.sprite = action_sprite[0];
    }

    //move
    public void PushDownMove()
    {
        if(move_touch == false)
        {
            touchID = Input.GetTouch(0).fingerId;
            move_touch = true;
        }
    }
    public void PushUpMove()
    {
        move_touch = false;
        Hdirection = 0;
        Vdirection = 0;

        for (int i = 0; i < 4; i++)
        {
            WASD_img[i].sprite = WASD_sprite[i];
        }
    }
    public void ChangeMoveButton(bool[] wasd_on)
    {
        for (int i = 0; i < 4; i++)
        {
            int on = 0;
            if (wasd_on[i] == true)
            {
                on = 4;
            }

            WASD_img[i].sprite = WASD_sprite[i + on];
        }
    }

    //item
    public void PushDownItem()
    {
        if(ItemC.open == false & (ItemC.t_open == 0 & ItemC.t_close == 0))
        {
            if (Master.battle_status == false & junC.Moveroom == false & junC.play_action == false & junC.canmove == true)
            {
                item_img.sprite = item_sprite[1];
            }
        }
        else if (ItemC.open == true & (ItemC.t_open == 0 & ItemC.t_close == 0) & ItemC.can_push_menubutton == true)
        {
            item_img.sprite = item_sprite[3];
        }
    }
    public void PushItem(int num)
    {
        item_img.sprite = item_sprite[num];
    }
    public void StartBattle()
    {
        controller_alpha.alpha = 0f;
        gameObject.SetActive(false);
        dash_button.SetActive(false);
        action_button.SetActive(false);
        guard_button.SetActive(true);
    }
    public void EndBattle()
    {
        controller_alpha.alpha = 0.6f;
        gameObject.SetActive(true);
        dash_button.SetActive(true);
        action_button.SetActive(true);
        guard_button.SetActive(false);
    }

    //key control
    public bool GetButtonDownTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetButtonDownDecide() //decide
    {
        if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.Return))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetButtonDownLeft() //left
    {
        if(Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetButtonDownRight() //right
    {
        if (Input.GetKeyDown(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetButtonDownUp() //up
    {
        if (Input.GetKeyDown(KeyCode.W) | Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetButtonDownDown() //down
    {
        if (Input.GetKeyDown(KeyCode.S) | Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //tool
    public int GetSelect_inPage(int selected, int page, int count, int plus, int inpage_min_select, int pagenum_max)
    {
        int target_select = selected + plus;
        int inpage_max_select = count - (page - 1) * pagenum_max;
        if (inpage_max_select > pagenum_max)
        {
            inpage_max_select = pagenum_max;
        }

        if (target_select < inpage_min_select)
        {
            target_select = inpage_max_select;
        }
        else if (target_select > inpage_max_select)
        {
            target_select = inpage_min_select;
        }

        return target_select;
    }
}
