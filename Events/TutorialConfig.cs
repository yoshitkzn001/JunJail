using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialConfig : MonoBehaviour
{
    [SerializeField] FuncMove Move;
    [SerializeField] MessageConfig message;

    public List<ActionCageDoorConfig> actionCages = new List<ActionCageDoorConfig>();
    public Animator jun_ani;
    public JunConfig junC;
    public GameObject Control_move;
    public CanvasGroup Control_alpha;

    const int STEP_START_WAIT = 0;
    const int TIME_START_WAIT = 70;
    const int STEP_WALK_WAIT = 1;
    const int TIME_WALK_WAIT = 25;
    const int STEP_WALK = 2;
    const int TIME_WALK = 25;
    const int STEP_TALK = 3;
    const int STEP_ROLL = 4;
    const int TIME_ROLL = 140;
    const int STEP_TALK2 = 5;
    const int STEP_OPEN_CONTROLER = 6;
    const int TIME_OPEN_CONTROLER = 30;
    const int STEP_TALK3 = 6;
    const int STEP_MOVE = 30;

    int time = 0;
    int stage_step = 0;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        stage_step = STEP_START_WAIT;
        time = TIME_START_WAIT;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(stage_step == STEP_START_WAIT)
        {
            time -= 1;
            if(time == 0)
            {
                foreach(ActionCageDoorConfig doorC in actionCages)
                {
                    if(doorC.gameObject.transform.localPosition.y > 350f)
                    {
                        doorC.MoveDoor();
                        break;
                    }
                }
                junC.Master.nomalSE.Play("action3");
                jun_ani.SetTrigger("action");
                stage_step = STEP_WALK_WAIT;
                time = TIME_WALK_WAIT;
            }
        }
        else if(stage_step == STEP_WALK_WAIT)
        {
            time -= 1;
            if(time == 0)
            {
                stage_step = STEP_WALK;
                time = TIME_WALK;
                jun_ani.SetInteger("state", 1);
            }
        }
        else if (stage_step == STEP_WALK)
        {
            time -= 1;

            Move.MoveLocalPosPlusY(junC.gameObject, -1);

            if (time == 0)
            {
                jun_ani.SetInteger("state", 0);
                stage_step = STEP_TALK;

                List<(int, int, int, string)> message_info = new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "おい！ gmゴキブリ看守！"),
                    (-1, 0, 0, "いつまで俺を独房に入れとくつもりだ！\n少し脱獄しようとしただけだろうがよぉ！"),
                    (-1, 0, 0, "てめぇこっちがよぉ！\n下手にでたらいい気になりやがって！"),
                    (-1, 0, 0, "ぜってぇ　頭棒っきれでひっぱたいて　\n心臓ひと突きにしてやっからなてめぇこのやろォ！！！"),
                    (-1, 0, 0, "あれ？"),
                };

                Open(null, message_info);
            }
        }
        else if(stage_step == STEP_ROLL)
        {
            time -= 1;

            if(time == TIME_ROLL / 2)
            {
                junC.sr.flipX = false;
                jun_ani.Play("jun_stop");
            }

            if(time == 0)
            {
                List<(int, int, int, string)> message_info = new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "看守いねぇじゃねぇか"),
                    (-1, 0, 0, "おい　これチャンスなんじゃねェ？"),
                };

                Open(null, message_info);

                stage_step = STEP_TALK2;
            }
        }
        else if(stage_step == STEP_OPEN_CONTROLER)
        {
            time -= 1;

            Control_alpha.alpha = ((TIME_OPEN_CONTROLER - time) / (float)TIME_OPEN_CONTROLER) * 0.6f;

            if(time == 0)
            {
                stage_step = STEP_MOVE;
                junC.canmove = true;
            }
        }
    }

    private void Open(ActionConfig ac, List<(int, int, int, string)> talk_info)
    {
        message.message_info = talk_info;

        message.ac = ac;
        message.Open(false, null);
    }

    public void MessageClose()
    {
        if(stage_step == STEP_TALK)
        {
            stage_step = STEP_ROLL;
            time = TIME_ROLL;
            junC.sr.flipX = true;
            jun_ani.Play("jun_stop");
        }
        else if(stage_step == STEP_TALK2)
        {
            stage_step = STEP_OPEN_CONTROLER;
            time = TIME_OPEN_CONTROLER;
            Control_move.SetActive(true);
        }
        else if(stage_step == STEP_TALK3)
        {
            Control_alpha.alpha = 0.6f;
            junC.canmove = true;
            stage_step = STEP_MOVE;
        }
    }

    public void OpenTalk3()
    {
        List<(int, int, int, string)> message_info = new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "なんだったんだこいつ..."),
                    (-1, 0, 0, "......ま　いっか！"),
                    (-1, 0, 0, "なんか今日は　脱獄できる予感が\nプンプンするんじゃないかぁー！？"),
                    (-1, 0, 0, "さっさと脱獄して\n配信すべーかして！"),
                };

        Open(null, message_info);

        stage_step = STEP_TALK3;
    }
}
