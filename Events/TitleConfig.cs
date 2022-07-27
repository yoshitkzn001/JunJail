using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleConfig : MonoBehaviour
{
    const int STEP_FIRE_WAIT = 0;
    const int TIME_STEP_FIRE_WAIT = 50;
    const int STEP_FADE_WAIT = 1;
    const int TIME_STEP_FADE_WAIT = 100;
    const int STEP_FADE = 2;
    const int TIME_STEP_FADE = 180;
    const int STEP_SET_TITLE = 3;
    const int TIME_STEP_SET_TITLE = 100;
    const int STEP_SET_BUTTONS = 4;
    const int TIME_SET_BUTTONS = 25;
    const int STEP_TAP_TO_START = 5;
    const int STEP_START = 6;
    const int TIME_START = 40;
    const int STEP_FADE_OUT = 7;
    const int TIME_FADE_OUT = 100;

    [SerializeField] AudioSource se;

    [SerializeField] AudioClip[] SE_LIST; //0:fire

    [SerializeField] FuncMove Move;
    [SerializeField] GameObject fire;
    [SerializeField] Image panel;
    [SerializeField] GameObject title_obj;
    [SerializeField] Image title_img;
    [SerializeField] Image taptostart;

    [SerializeField] Image fadeout_panel;

    [SerializeField] CriAtomSource nomalSE;

    [SerializeField] Controller control;

    private int step;
    private int time;

    private void Start()
    {
        Status.load = false;
        step = STEP_FIRE_WAIT;
        time = TIME_STEP_FIRE_WAIT;
    }

    void FixedUpdate()
    {
        if(step == STEP_FIRE_WAIT)
        {
            if(time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    fire.SetActive(true);
                    se.PlayOneShot(SE_LIST[0]);
                    time = TIME_STEP_FADE_WAIT;
                    step = STEP_FADE_WAIT;
                }
            }
        }
        else if(step == STEP_FADE_WAIT)
        {
            if (time > 0)
            {
                time -= 1;

                if (time == 0)
                {
                    time = TIME_STEP_FADE;
                    step = STEP_FADE;
                    Sound.myBGM.clip = Sound.BGM[0];
                    Sound.myBGM.volume = 0.30f;
                    Sound.myBGM.Play();
                }
            }
        }
        else if (step == STEP_FADE)
        {
            if (time > 0)
            {
                time -= 1;

                float a = time / (float)TIME_STEP_FADE;
                panel.color = new Color(0f, 0f, 0f, a);

                if (time == 0)
                {
                    panel.gameObject.SetActive(false);
                    step = STEP_SET_TITLE;
                    time = TIME_STEP_SET_TITLE;
                }
            }
        }
        else if(step == STEP_SET_TITLE)
        {
            if(time > 0)
            {
                time -= 1;

                Move.DeccelMoveY(time, TIME_STEP_SET_TITLE, 65.5f, 51.5f, title_obj);
                float a = (TIME_STEP_SET_TITLE - time) / (float)TIME_STEP_SET_TITLE;
                title_img.color = new Color(1f, 1f, 1f, a);

                if(time == 0)
                {
                    step = STEP_SET_BUTTONS;
                    time = TIME_SET_BUTTONS;
                }
            }
        }
        else if(step == STEP_SET_BUTTONS)
        {
            if(time > 0)
            {
                time -= 1;

                float a = (TIME_SET_BUTTONS - time) / (float)TIME_SET_BUTTONS;

                if(time == 0)
                {
                    step = STEP_TAP_TO_START;
                    taptostart.gameObject.SetActive(true);
                    taptostart.color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }
        else if(step == STEP_START)
        {
            time -= 1;

            if(time % 5 == 0)
            {
                int index = time / 5;

                if(index % 2 == 0)
                {
                    taptostart.gameObject.SetActive(false);
                }
                else
                {
                    taptostart.gameObject.SetActive(true);
                }
            }

            if(time == 0)
            {
                step = STEP_FADE_OUT;
                time = TIME_FADE_OUT;
                fadeout_panel.gameObject.SetActive(true);
            }
        }
        else if(step == STEP_FADE_OUT)
        {
            time -= 1;

            fadeout_panel.color = new Color(0f, 0f, 0f, (TIME_FADE_OUT - time) / (float)TIME_FADE_OUT);
            Sound.myBGM.volume = 0.4f * (time / (float)TIME_FADE_OUT);

            if (time == 0)
            {
                Sound.myBGM.Stop();

                if (PlayerPrefs.GetInt("Tutorial") == 1)
                {
                    SceneManager.LoadScene("StageStart");
                }
                else
                {
                    SceneManager.LoadScene("Op");
                }
            }
        }

        if (step == STEP_TAP_TO_START)
        {
            time += 1;

            if(time <= 15)
            {
                taptostart.color = new Color(1f, 1f, 1f, time / 15f);
            }

            if(time >= 105)
            {
                taptostart.color = new Color(1f, 1f, 1f, (120 - time) / 15f);

                if(time == 120)
                {
                    time = 0;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) | control.GetButtonDownDecide())
        {
            if (step == STEP_FADE | step == STEP_SET_TITLE | step == STEP_SET_BUTTONS)
            {
                panel.gameObject.SetActive(false);
                title_img.color = new Color(1f, 1f, 1f, 1f);
                Move.MoveLocalPosY(title_obj, 51.5f);
                taptostart.color = new Color(1f, 1f, 1f, 0f);
                taptostart.gameObject.SetActive(true);
                step = STEP_TAP_TO_START;
                time = 0;
            }
            else if(step == STEP_TAP_TO_START)
            {
                nomalSE.Play("start2");
                step = STEP_START;
                time = TIME_START + 1;

                taptostart.color = new Color(1f, 1f, 1f, 1f);
                taptostart.gameObject.SetActive(true);
            }
        }
    }
}
