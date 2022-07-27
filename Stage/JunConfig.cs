using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class JunConfig : MonoBehaviour
{
    const int SPEED = 1;
    const int MOVEROOM_FADE_T = 20;
    const float JUN_SIZE_X = 7f;
    const float JUN_SIZE_Y = 2.5f;
    const float HEIGHT = 20.5f;
    const int ROLLING_TIME = 26;

    private enum State { idle, walk, action, items_select }
    private State state = State.idle;
    private Animator ani;
    public SpriteRenderer sr;

    [SerializeField] Controller Control;
    [SerializeField] MiniMapConfig Minimap;
    public MasStageConfig Master;
    [SerializeField] StageItemConfig StageItem;
    [SerializeField] LayerMask ground;
    [SerializeField] DataStage Data;
    [SerializeField] GameObject grid;
    [SerializeField] GameObject Movefade;
    [SerializeField] Image Movefade_img;

    int posX;
    int posY;
    int rolling = 0;
    float rolling_dir_x;
    float rolling_dir_y;
    int premovedirX;
    int premovedirY;

    public bool canmove;
    public bool first_move;
    public bool Moveroom;
    int moveroom_fadeout_T;
    int moveroom_fadein_T;
    public int myroom;
    private int nextroom;

    public GameObject myroom_obj;
    private GameObject nextroom_obj;

    public bool action = false;
    public int t_action = 0;
    public bool play_action;
    private ActionConfig action_script;

    public bool load = false;
    [SerializeField] LoadingConfig loadC;

    public int t_starmode = 0;
    public bool no_stage;

    public bool tutorial;
    [SerializeField] GameObject item_button;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Transform transform = gameObject.transform;
        Vector3 pos = transform.position;
        Transform ctransform = Camera.main.transform;
        Vector3 cpos = ctransform.position;
        cpos.x = pos.x;
        cpos.y = pos.y + 0.5f;
        ctransform.position = cpos;
    }

    private void OnEnable()
    {
        if(Master.t_battle_end == 0)
        {
            if (load == true)
            {
                canmove = true;
            }
            else
            {
                canmove = false;
            }
        }
        
        state = State.idle;
        moveroom_fadein_T = 0;
        moveroom_fadeout_T = 0;
        Master.stage_charas.Add(gameObject);
        Master.stage_charas_height.Add(HEIGHT);

        if (play_action == true)
        {
            t_action = 0;
            play_action = false;
            if (action == true)
            {
                if(action_script.action_num == 1)
                {
                    action_script.ChangeSprite(1);
                }
                else
                {
                    action = false;
                }
            }
        }
    }

    //Enemy
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EncountEnemy(collision.gameObject);
        }
    }

    //Moveroom
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Moveroom == false & canmove == true)
        {
            if (collision.tag == "Moveroom")
            {
                Moveroom = true;

                MoveroomConfig moveroomC = collision.GetComponent<MoveroomConfig>();
                int toroom;
                if(moveroomC.toRoom[0] != myroom)
                {
                    toroom = moveroomC.toRoom[0];
                }
                else
                {
                    toroom = moveroomC.toRoom[1];
                }
                moveroom_fadeout_T = MOVEROOM_FADE_T;
                moveroom_fadein_T = 0;
                nextroom = toroom;
                myroom = nextroom;
                nextroom_obj = Data.rooms[toroom];
                gameObject.transform.parent = grid.transform;
                Movefade.SetActive(true);
                Movefade_img.color = new Color(0f, 0f, 0f, 0f);
                ResetCharaList();
            }
            else if(collision.tag == "Action")
            {
                if(tutorial == false)
                {
                    action = true;
                    action_script = collision.GetComponent<ActionConfig>();
                    action_script.ChangeSprite(1);
                }
                else
                {
                    if(collision.name != "Door_Range")
                    {
                        action = true;
                        action_script = collision.GetComponent<ActionConfig>();
                        action_script.ChangeSprite(1);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Moveroom == true)
        {
            if (collision.tag == "Moveroom")
            {
                Moveroom = false;
            }
        }

        if (action == true)
        {
            if (collision.tag == "Action")
            {
                action = false;

                if(play_action == false)
                {
                    action_script.ChangeSprite(0);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Control.PushDownAction();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Control.PushUpAction();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Control.PushDownDash();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Control.PushUpDash();
        }
    }
    public void ControllAction()
    {
        if (action == true & t_action == 0 & canmove == true)
        {
            state = State.idle;
            ani.SetInteger("state", (int)state);
            if (action_script.action_num <= 2)
            {
                ani.SetTrigger("action");
            }
            action_script.Action();
            play_action = true;
            rolling = 0;
        }
    }
    public void ControllRolling()
    {
        if (Moveroom == false & canmove == true & play_action == false)
        {
            if (rolling == 0)
            {
                (float Hdirection, float Vdirection) = GetDirection();

                if (Hdirection != 0f | Vdirection != 0f)
                {
                    rolling = ROLLING_TIME;
                    rolling_dir_x = Hdirection;
                    rolling_dir_y = Vdirection;

                    Master.nomalSE.Play("dash2");
                    if (Hdirection > 0)
                    {
                        sr.flipX = false;
                    }
                    else if (Hdirection < 0)
                    {
                        sr.flipX = true;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Transform transform = gameObject.transform;
        Vector3 pos = transform.position;
        posX = (int)(pos.x / 25);
        posY = (int)((pos.y - 20.5f) / 25);

        if(canmove == true)
        {
            if(play_action == false)
            {
                if (rolling == 0)
                {
                    if (Moveroom == false)
                    {
                        (float Hdirection, float Vdirection) = GetDirection();

                        if (Hdirection > 0)
                        {
                            sr.flipX = false;
                        }
                        else if (Hdirection < 0)
                        {
                            sr.flipX = true;
                        }

                        if (Hdirection != 0 | Vdirection != 0)
                        {
                            Walk(Hdirection, Vdirection);

                            if (state == State.idle)
                            {
                                state = State.walk;
                                ani.SetInteger("state", (int)state);
                            }

                            if (first_move == false)
                            {
                                first_move = true;
                            }
                        }
                        else
                        {
                            if (state == State.walk)
                            {
                                state = State.idle;
                                ani.SetInteger("state", (int)state);
                            }
                        }
                    }
                    else
                    {
                        Walk(premovedirX, premovedirY);
                    }
                }
                else
                {
                    Roll();
                }
            }
            else
            {
                if(t_action > 0)
                {
                    t_action -= 1;
                    if(t_action == 0)
                    {
                        play_action = false;
                        if(action == true)
                        {
                            action_script.ChangeSprite(1);
                        }
                    }
                }
            }
        }

        Transform ctransform = Camera.main.transform;
        Vector3 cpos = ctransform.position;
        float moveX = pos.x - cpos.x;
        float moveY = pos.y - cpos.y;
        if (moveX > 2f)
        {
            moveX = 2f;
        }
        else if(moveX < -2f)
        {
            moveX = -2f;
        }
        if (moveY > 2f)
        {
            moveY = 2f;
        }
        else if (moveY < -2f)
        {
            moveY = -2f;
        }
        cpos.x = cpos.x + (int)moveX;
        cpos.y = cpos.y + (int)moveY;
        ctransform.position = cpos;

        //moveroom
        if(moveroom_fadeout_T > 0)
        {
            moveroom_fadeout_T -= 1;
            int t = MOVEROOM_FADE_T - moveroom_fadeout_T;
            float myroom_a = t / (float)MOVEROOM_FADE_T;

            Movefade_img.color = new Color(0f, 0f, 0f, myroom_a);

            if (moveroom_fadeout_T == 0)
            {
                moveroom_fadein_T = MOVEROOM_FADE_T;
                myroom_obj.SetActive(false);
                nextroom_obj.SetActive(true);
                myroom_obj = nextroom_obj;
                Master.stage_charas.Add(gameObject);
                Master.stage_charas_height.Add(HEIGHT);
            }
        }
        if(moveroom_fadein_T > 0)
        {
            if(moveroom_fadein_T == MOVEROOM_FADE_T - 1)
            {
                Master.enemy_reset_pos = new List<int>();
                gameObject.transform.parent = nextroom_obj.transform;
                Transform ltransform = gameObject.transform;
                Vector3 lpos = ltransform.localPosition;
                lpos.z = (lpos.y - 20f) / 10f;
                ltransform.localPosition = lpos;
            }

            moveroom_fadein_T -= 1;
            float myroom_a = moveroom_fadein_T / (float)MOVEROOM_FADE_T;

            Movefade_img.color = new Color(0f, 0f, 0f, myroom_a);

            if(moveroom_fadein_T == 0)
            {
                gameObject.transform.parent = nextroom_obj.transform;
                Movefade.SetActive(false);
            }
        }

        if(Moveroom == false)
        {
            Transform ltransform = gameObject.transform;
            Vector3 lpos = ltransform.localPosition;
            lpos.z = (lpos.y - 20f) / 10f;
            ltransform.localPosition = lpos;
        }

        //start mode
        if(t_starmode > 0)
        {
            t_starmode -= 1;

            int alpha = t_starmode / 4;
            sr.color = new Color(1f, 1f, 1f, (alpha + 1) % 2);
        }
    }

    public void EncountEnemy(GameObject enemy)
    {
        if (Moveroom == false & canmove == true & t_starmode == 0)
        {
            Master.enemy_reset_pos = new List<int>();
            SetPosFlat();
            Movefade.SetActive(false);
            StageItem.QuitMenu();
            Master.SetBattleConfig(enemy, myroom_obj);
            rolling = 0;
            canmove = false;
            Master.StageUI.SetActive(false);
            ani.SetBool("item_select_stay", false);
            ResetCharaList();
            Control.StartBattle();
        }
    }

    public void EncountBoss(GameObject enemy)
    {
        if (Moveroom == false & canmove == true)
        {
            Master.enemy_reset_pos = new List<int>();
            SetPosFlat();
            Movefade.SetActive(false);
            StageItem.QuitMenu();
            Master.SetBattleConfig(enemy, myroom_obj);
            rolling = 0;
            canmove = false;
            Master.StageUI.SetActive(false);
            ani.SetBool("item_select_stay", false);
            ResetCharaList();
            Control.StartBattle();
            t_starmode = 0;
        }
    }

    private void Walk(float Hdirection, float Vdirection)
    {
        int moveX = 0;
        int moveY = 0;
        if (Vdirection > 0)
        {
            moveY += SPEED;
        }
        else if (Vdirection < 0)
        {
            moveY -= SPEED;
        }

        if (Hdirection > 0)
        {
            moveX += SPEED;
        }
        else if (Hdirection < 0)
        {
            moveX -= SPEED;
        }

        DecideMove(moveX, moveY);
    }

    private void Roll()
    {
        int moveX = 0;
        int moveY = 0;
        int R = (rolling - (ROLLING_TIME - 4)) / 2;
        if (R < 0 | rolling == ROLLING_TIME)
        {
            R = 0;
        }

        if (rolling_dir_y > 0)
        {
            moveY = R + 2;
        }
        else if (rolling_dir_y < 0)
        {
            moveY = -(R + 2);
        }

        if (rolling_dir_x > 0)
        {
            moveX = R + 2;
        }
        else if (rolling_dir_x < 0)
        {
            moveX = -(R + 2);
        }
        rolling -= 1;

        DecideMove(moveX, moveY);
    }

    private void DecideMove(int moveX, int moveY)
    {
        Transform transform = gameObject.transform;
        Vector3 pos = transform.position;
        int searchX = 0;
        int searchY = 0;
        int dirX = 0;
        if (moveX != 0)
        {
            dirX = moveX / Mathf.Abs(moveX);
        }
        int dirY = 0;
        if (moveY != 0)
        {
            dirY = moveY / Mathf.Abs(moveY);
        }
        bool finishsearchX = false;
        bool finishsearchY = false;

        if (moveX == searchX)
        {
            finishsearchX = true;
        }
        if (moveY == searchY)
        {
            finishsearchY = true;
        }

        for (int i=0; i<7; i++)
        {
            if (finishsearchX == false)
            {
                if (moveX != searchX)
                {
                    searchX += dirX;
                }
            }
            if (finishsearchY == false)
            {
                if (moveY != searchY)
                {
                    searchY += dirY;
                }
            }

            Vector2 GroundPos = new Vector2(pos.x + searchX, pos.y - HEIGHT + searchY);
            Vector2 GroundArea = new Vector2(JUN_SIZE_X, JUN_SIZE_Y);
            Debug.DrawLine(GroundPos + GroundArea, GroundPos - GroundArea, Color.red);
            bool hit = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);

            if(hit == true)
            {
                if(finishsearchY == false & finishsearchX == false)
                {
                    GroundPos = new Vector2(pos.x + searchX, pos.y - HEIGHT + (searchY - dirY));
                    bool hitX = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);
                    GroundPos = new Vector2(pos.x + (searchX - dirX), pos.y - HEIGHT + searchY);
                    bool hitY = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);

                    if(hitX == true)
                    {
                        searchX = searchX - dirX;
                        moveX = searchX;
                        finishsearchX = true;
                    }
                    if (hitY == true)
                    {
                        searchY = searchY - dirY;
                        moveY = searchY;
                        finishsearchY = true;
                    }

                    if(hitX == false & hitY == false)
                    {
                        searchX = searchX - dirX;
                        moveX = searchX;
                        finishsearchX = true;
                    }
                }
                else
                {
                    if(finishsearchX == true)
                    {
                        searchY = searchY - dirY;
                        moveY = searchY;
                        finishsearchY = true;

                        if (dirY > 0)
                        {
                            Vector2 pos1 = new Vector2(pos.x + searchX + 6, pos.y - HEIGHT + searchY + JUN_SIZE_Y);
                            Vector2 pos2 = new Vector2(pos.x + searchX - 6, pos.y - HEIGHT + searchY + JUN_SIZE_Y);
                            int move = MoveAssist(pos1, pos2, new Vector2(3, 1));

                            if (move != 0)
                            {
                                GroundPos = new Vector2(pos.x + searchX + move, pos.y - HEIGHT + searchY);
                                hit = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);

                                if(hit == false)
                                {
                                    moveX = move;
                                }
                            }
                        }
                        else if (dirY < 0)
                        {
                            Vector2 pos1 = new Vector2(pos.x + searchX + 6, pos.y - HEIGHT + searchY - JUN_SIZE_Y);
                            Vector2 pos2 = new Vector2(pos.x + searchX - 6, pos.y - HEIGHT + searchY - JUN_SIZE_Y);
                            int move = MoveAssist(pos1, pos2, new Vector2(3, 1));

                            if (move != 0)
                            {
                                GroundPos = new Vector2(pos.x + searchX + move, pos.y - HEIGHT + searchY);
                                hit = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);

                                if(hit == false)
                                {
                                    moveX = move;
                                }
                            }
                        }
                    }
                    else if (finishsearchY == true)
                    {
                        searchX = searchX - dirX;
                        moveX = searchX;
                        finishsearchX = true;

                        if (dirX > 0)
                        {
                            Vector2 pos1 = new Vector2(pos.x + searchX + JUN_SIZE_X, pos.y - HEIGHT + searchY + JUN_SIZE_Y + 7);
                            Vector2 pos2 = new Vector2(pos.x + searchX + JUN_SIZE_X, pos.y - HEIGHT + searchY - JUN_SIZE_Y - 7);
                            int move = MoveAssist(pos1, pos2, new Vector2(1, 1));
                            
                            if (move != 0)
                            {
                                GroundPos = new Vector2(pos.x + searchX, pos.y - HEIGHT + searchY + move);
                                hit = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);

                                if(hit == false)
                                {
                                    moveY = move;
                                }
                            }
                        }
                        else if (dirX < 0)
                        {
                            Vector2 pos1 = new Vector2(pos.x + searchX - JUN_SIZE_X, pos.y - HEIGHT + searchY + JUN_SIZE_Y + 7);
                            Vector2 pos2 = new Vector2(pos.x + searchX - JUN_SIZE_X, pos.y - HEIGHT + searchY - JUN_SIZE_Y - 7);
                            int move = MoveAssist(pos1, pos2, new Vector2(1, 1));
                            if (move != 0)
                            {
                                GroundPos = new Vector2(pos.x + searchX, pos.y - HEIGHT + searchY + move);
                                hit = Physics2D.OverlapArea(GroundPos + GroundArea, GroundPos - GroundArea, ground);

                                if (hit == false)
                                {
                                    moveY = move;
                                }
                            }
                        }
                    }
                }
            }

            if (moveX == searchX)
            {
                finishsearchX = true;
            }
            if (moveY == searchY)
            {
                finishsearchY = true;
            }

            if (finishsearchY == true & finishsearchX == true)
            {
                break;
            }
        }

        premovedirX = dirX;
        premovedirY = dirY;
        pos.x += moveX;
        pos.y += moveY;
        transform.position = pos;
    }

    private int MoveAssist(Vector2 pos1, Vector2 pos2, Vector2 size)
    {
        Vector2 GroundPos = pos1;
        bool hit_1 = Physics2D.OverlapArea(GroundPos + size, GroundPos - size, ground);
        Debug.DrawLine(GroundPos + size, GroundPos - size, Color.green);
        GroundPos = pos2;
        bool hit_2 = Physics2D.OverlapArea(GroundPos + size, GroundPos - size, ground);
        Debug.DrawLine(GroundPos + size, GroundPos - size, Color.green);

        int move = 0;
        if (hit_1 == false & hit_2 == true)
        {
            move = 1;
        }
        if (hit_1 == true & hit_2 == false)
        {
            move = -1;
        }

        return move;
    }

    private void ResetCharaList()
    {
        Master.stage_charas = new List<GameObject>();
        Master.stage_charas_height = new List<float>();
    }

    public void UseStair()
    {
        if(no_stage == false & Master.tutorial == false)
        {
            Status.stage_layer += 1;
        }

        Data.start_text.text = "衛門の間 " + (14 - Status.stage_layer) + "F";
        myroom_obj.SetActive(false);
        Master.fade.SetActive(true);
        Master.fade_img.color = new Color(0f, 0f, 0f, 1f);
        loadC.gameObject.SetActive(true);
        loadC.StartLoad();
    }

    public void OpenSelectItems()
    {
        play_action = true;
        state = State.items_select;
        ani.Play("jun_ready_items_select");
        rolling = 0;
        ani.SetInteger("state", (int)state);
    }

    public void CloseSelectItems()
    {
        play_action = false;
        state = State.idle;
        ani.SetTrigger("items_select_end");
        ani.SetInteger("state", (int)state);
    }

    public void UseItem(int index)
    {
        ani.SetTrigger("items_use");

        if(index == 8)
        {
            ani.SetBool("item_select_stay", true);
        }
    }

    public void CancelItem(bool quit)
    {
        if(quit == false)
        {
            ani.SetTrigger("cancel_item");
        }
        ani.SetBool("item_select_stay", false);
    }

    public void DropItem()
    {
        ani.SetTrigger("items_drop");
    }

    private void SetPosFlat()
    {
        Transform transform = gameObject.transform;
        Vector3 pos = transform.localPosition;
        int map_height = Data.active_box_map[myroom].GetLength(0);
        int x = (int)(pos.x / 25f);
        int y = map_height - (int)((pos.y - HEIGHT) / 25f) - 1;
        Master.enemy_reset_pos.Add(y * 100 + x);
    }

    public (float, float) GetDirection()
    {
        float Hdirection = Input.GetAxis("Horizontal");
        float Vdirection = Input.GetAxis("Vertical");

        int cHdirection = Control.Hdirection;
        int cVdirection = Control.Vdirection;

        if (cHdirection != 0 | cVdirection != 0)
        {
            Hdirection = cHdirection;
            Vdirection = cVdirection;
        }

        bool[] wasd_on = new bool[4];
        if (Hdirection > 0)
        {
            wasd_on[3] = true;
        }
        else if(Hdirection < 0)
        {
            wasd_on[1] = true;
        }
        if(Vdirection > 0)
        {
            wasd_on[0] = true;
        }
        else if (Vdirection < 0)
        {
            wasd_on[2] = true;
        }
        Control.ChangeMoveButton(wasd_on);

        return (Hdirection, Vdirection);
    }

    public void EndTutorial()
    {
        tutorial = false;
        item_button.SetActive(true);
    }
}
