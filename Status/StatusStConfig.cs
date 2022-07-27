using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusStConfig : MonoBehaviour
{
    [SerializeField] DataNomal DataNomal;
    [SerializeField] FuncMiniFigure Fig;
    [SerializeField] Image[] value_img;
    [SerializeField] GameObject[] value_obj;
    [SerializeField] GameObject value_obj_parent;
    [SerializeField] Image ball_img;
    [SerializeField] FuncMove Move;

    [SerializeField] CanvasGroup all_alpha;

    private int t_up = 0;
    private int t_open;
    private int max_t_open;
    private int t_close;
    private int max_t_close;

    private void Start()
    {
        SetST();
    }

    void OnEnable()
    {
        SetST();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_up > 0)
        {
            t_up -= 1;

            if(t_up == 5)
            {
                Move.MoveLocalPosPlusY(value_obj_parent, -3f);
            }
            if (t_up == 0)
            {
                Move.MoveLocalPosPlusY(value_obj_parent, 1f);
            }

            float c = (10 - t_up) / 10f;
            Fig.ChangeColorFigure(value_img, c, 1f, c);
        }

        if(t_open > 0)
        {
            t_open -= 1;

            Move.DeccelMoveY(t_open, max_t_open, 10f, 0f, gameObject);
            all_alpha.alpha = (max_t_open - t_open) / (float)max_t_open;
        }

        if(t_close > 0)
        {
            t_close -= 1;

            Move.AccelMoveY(t_close, max_t_close, 0f, 10f, gameObject);
            all_alpha.alpha = t_close / (float)max_t_close;

            if(t_close == 0)
            {
                all_alpha.gameObject.SetActive(false);
            }
        }
    }

    public void SetST(int reserveST = -1)
    {
        Move.MoveLocalPos(value_obj_parent, 24, 6);

        int st;
        if (reserveST == -1 | reserveST == -2)
        {
            st = Status.st;
            float c;
            if (st >= 0)
            {
                c = (st * 2.5f) / 360f;
            }
            else
            {
                c = 0;
            }
            ball_img.color = Color.HSVToRGB(c, 1f, 1f);

            if(reserveST == -1)
            {
                Fig.ChangeColorFigure(value_img, 1f, 1f, 1f);
            }
        }
        else
        {
            st = Status.st - reserveST;
            Fig.ChangeColorFigure(value_img, 1f, 0f, 1f);
        }

        Fig.ChangeFigureCenter(st, 4, -24, value_img, value_obj, DataNomal.figure_mini);
    }
    public void ChangeST(int plus_st)
    {
        Status.st = Status.st + plus_st;
        if(Status.st > Status.st_max)
        {
            Status.st = Status.st_max;
        }

        if (plus_st < 0)
        {
            SetST();
        }
        else
        {
            t_up = 10;
            SetST(-2);
            Fig.ChangeColorFigure(value_img, 0f, 1f, 0f);
            Move.MoveLocalPosPlusY(value_obj_parent, 2f);
        }
    }

    public void OpenST(int time)
    {
        all_alpha.gameObject.SetActive(true);
        t_open = time;
        max_t_open = time;
        all_alpha.alpha = 0f;
        Move.MoveLocalPosY(gameObject, 10f);
    }
    public void CloseST(int time)
    {
        t_close = time;
        max_t_close = time;
        all_alpha.alpha = 1f;
        Move.MoveLocalPosY(gameObject, 0f);
    }
}
