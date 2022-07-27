using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndConfig : MonoBehaviour
{
    const int STEP_FADE_IN = 1;
    const int TIME_FADE_IN = 140;
    const int STEP_WALK = 2;
    const int TIME_WALK = 20;
    const int STEP_STUMBLE = 3;
    const int TIME_STUMBLE = 30;
    const int STEP_STUMBLE_WAIT = 4;
    const int TIME_STUMBLE_WAIT = 30;
    const int STEP_STUMBLE_DOWN = 5;
    const int TIME_STUMBLE_DOWN = 60;

    const int STEP_TALK1 = 6;

    const int STEP_FADE_OUT = 7;
    const int TIME_FADE_OUT = 140;
    const int STEP_TALK2_WAIT = 8;
    const int TIME_TALK2_WAIT = 120;

    const int STEP_TALK2 = 9;

    const int STEP_TALK3_WAIT = 10;
    const int TIME_TALK3_WAIT = 40;

    const int STEP_TALK3 = 11;

    const int STEP_LAST_MESSAGE_WAIT = 12;
    const int TIME_LAST_MESSAGE_WAIT = 60;
    const int STEP_LAST_MESSAGE = 13;
    const int TIME_LAST_MESSAGE = 100;

    const int STEP_END_WAIT = 14;

    const int STEP_END = 15;
    const int TIME_END = 100;

    [SerializeField] MessageEndingConfig message;
    [SerializeField] FuncMove Move;
    [SerializeField] GameObject jun;
    [SerializeField] Animator junani;
    [SerializeField] CriAtomSource nomalSE;
    [SerializeField] CriAtomSource voiceSE;
    [SerializeField] Image EndFade;

    [SerializeField] TextMeshProUGUI[] texts;

    int step = 0;
    int time = 0;

    bool shake = false;
    int t_shake = 0;

    int[] stumble_pos = new int[] { 8, 3, 1, 1, 0, 0, 0};

    private void Start()
    {
        EndFade.gameObject.SetActive(true);
        EndFade.color = new Color(0f, 0f, 0f, 1f);

        step = STEP_FADE_IN;
        time = TIME_FADE_IN;

        texts[0].gameObject.SetActive(false);
        texts[1].gameObject.SetActive(false);

        Sound.myBGM.clip = Sound.BGM[1];
        Sound.myBGM.volume = 0.0f;
        Sound.myBGM.Play();
    }

    private void Update()
    {
        if(step == STEP_END_WAIT)
        {
            if (Input.GetMouseButtonDown(0))
            {
                step = STEP_END;
                time = TIME_END;
            }
        }
    }

    private void FixedUpdate()
    {
        if(step == STEP_FADE_IN)
        {
            time -= 1;

            Move.MoveLocalPosPlusX(jun, 1f);
            EndFade.color = new Color(0f, 0f, 0f, time / (float)TIME_FADE_IN);

            Sound.myBGM.volume = 0.5f * ((TIME_FADE_IN - time) / (float)(TIME_FADE_IN));

            if (time == 0)
            {
                EndFade.gameObject.SetActive(false);
                step = STEP_WALK;
                time = TIME_WALK;
            }
        }
        else if (step == STEP_WALK)
        {
            time -= 1;
            Move.MoveLocalPosPlusX(jun, 1f);

            if (time == 0)
            {
                step = STEP_STUMBLE;
                time = TIME_STUMBLE;
                junani.SetInteger("step", 1);
                Move.MoveLocalPosPlusX(jun, stumble_pos[0]);

                nomalSE.Play("damage1");
                voiceSE.Play("voice_pain5");
            }
        }
        else if (step == STEP_STUMBLE)
        {
            time -= 1;

            if(time % 5 == 0)
            {
                Move.MoveLocalPosPlusX(jun, stumble_pos[(TIME_STUMBLE - time) / 5]);
            }

            if(time == 0)
            {
                step = STEP_STUMBLE_WAIT;
                time = TIME_STUMBLE_WAIT;
            }
        }
        else if (step == STEP_STUMBLE_WAIT)
        {
            time -= 1;

            if (time == 0)
            {
                Move.MoveLocalPosPlusX(jun, 20);
                junani.SetInteger("step", 2);
                step = STEP_STUMBLE_DOWN;
                time = TIME_STUMBLE_DOWN;

                nomalSE.Play("damage4");
                voiceSE.Play("voice_pain7");
            }
        }
        else if(step == STEP_STUMBLE_DOWN)
        {
            time -= 1;

            if(time == 0)
            {
                List<(int, int, int, string)> message_info = new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "あーもう疲れたわ\n死闘続きでよぉ..."),
                    (-1, 0, 0, "なんかもうやる気もでな"),
                    (-1, 0, 1, "ぃいやああるうううおおおお！！"),
                    (-1, 0, 0, "って　言いたいとこだけどよ\nまじで疲れたから少し..."),
                    (-1, 0, 0, "ひん"),
                };

                bool[] action_timing = new bool[5] { false, false, true, true, false };

                Open(action_timing, message_info);

                step = STEP_TALK1;
            }
        }
        else if(step == STEP_FADE_OUT)
        {
            time -= 1;
            EndFade.color = new Color(0f, 0f, 0f, (TIME_FADE_OUT - time) / (float)TIME_FADE_OUT);
            Sound.myBGM.volume = 0.5f * (time / (float)(TIME_FADE_OUT));

            if (time == 0)
            {
                Sound.myBGM.Stop();
                step = STEP_TALK2_WAIT;
                time = TIME_TALK2_WAIT;
            }
        }
        else if(step == STEP_TALK2_WAIT)
        {
            time -= 1;

            if(time == 0)
            {
                List<(int, int, int, string)> message_info = new List<(int, int, int, string)>()
                {
                    (1, 1, 0, "へぇ！？"),
                    (1, 1, 0, "加藤純一が倒れて寝てますわぁ..."),
                    (1, 1, 0, "どうします？"),
                    (1, 2, 0, "ﾎﾎﾎw　これはみぞうちぶん殴っても起きなそうやなぁ"),
                    (1, 1, 0, "え！？ぶん殴るんすか！？"),
                    (1, 2, 0, "冗談や、冗談"),
                    (1, 2, 0, "ベッドのあるとこまで運んでやりますわぁ\n頭の方持ってくれへん？"),
                };

                bool[] action_timing = new bool[7] { false, false, false, false, false, false, false };

                Open(action_timing, message_info);

                step = STEP_TALK2;
            }
        }
        else if(step == STEP_TALK3_WAIT)
        {
            time -= 1;

            if(time == 0)
            {
                List<(int, int, int, string)> message_info = new List<(int, int, int, string)>()
                {
                    (-1, 1, 0, "おぉん！？　重い..."),
                    (1, 2, 0, "ﾎﾎﾎﾎﾎw　ほな、行こか"),
                };

                bool[] action_timing = new bool[7] { false, false, false, false, false, false, false };

                Open(action_timing, message_info);

                step = STEP_TALK3;
            }
        }
        else if(step == STEP_LAST_MESSAGE_WAIT)
        {
            time -= 1;

            if(time == 0)
            {
                step = STEP_LAST_MESSAGE;
                time = TIME_LAST_MESSAGE;

                texts[0].gameObject.SetActive(true);
                texts[0].color = new Color(1f, 1f, 1f, 0f);
                texts[1].gameObject.SetActive(true);
                texts[1].color = new Color(1f, 1f, 0f, 0f);
            }
        }
        else if (step == STEP_LAST_MESSAGE)
        {
            time -= 1;
            texts[0].color = new Color(1f, 1f, 1f, (TIME_LAST_MESSAGE - time) / (float)TIME_LAST_MESSAGE);
            texts[1].color = new Color(1f, 1f, 0f, (TIME_LAST_MESSAGE - time) / (float)TIME_LAST_MESSAGE);

            if (time == 0)
            {
                step = STEP_END_WAIT;
            }
        }
        else if(step == STEP_END)
        {
            time -= 1;
            texts[0].color = new Color(1f, 1f, 1f, time / (float)STEP_END);
            texts[1].color = new Color(1f, 1f, 0f, time / (float)STEP_END);

            if (time == 0)
            {
                PlayerPrefs.SetInt("Tutorial", 0);
                SceneManager.LoadScene("StartScene");
            }
        }

        if (shake == true)
        {
            t_shake += 1;

            if(t_shake == 4)
            {
                Move.MoveLocalPosY(Camera.main.gameObject, 1);
            }
            else if(t_shake == 8)
            {
                Move.MoveLocalPosY(Camera.main.gameObject, -1);
                t_shake = 0;
            }
        }
    }

    private void Open(bool[] action_timing, List<(int, int, int, string)> talk_info)
    {
        message.message_info = talk_info;
        message.action_timing = action_timing;
        message.Open(false, null);
    }

    public void MessageClose()
    {
        if (step == STEP_TALK1)
        {
            step = STEP_FADE_OUT;
            time = TIME_FADE_OUT;

            EndFade.gameObject.SetActive(true);
            EndFade.color = new Color(0f, 0f, 0f, 0f);
        }
        else if(step == STEP_TALK2)
        {
            step = STEP_TALK3_WAIT;
            time = TIME_TALK3_WAIT;
        }
        else if(step == STEP_TALK3)
        {
            step = STEP_LAST_MESSAGE_WAIT;
            time = TIME_LAST_MESSAGE_WAIT;
        }
    }

    public void Action(int t)
    {
        if(step == STEP_TALK1)
        {
            if(t == 2)
            {
                junani.SetBool("shake", true);
                shake = true;
                Move.MoveLocalPosY(Camera.main.gameObject, 1);
                nomalSE.Play("damage3");
            }
            else if(t == 3)
            {
                shake = false;
                t_shake = 0;
                junani.SetBool("shake", false);
                Move.MoveLocalPosY(Camera.main.gameObject, 0);
            }
        }
    }
}
