using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncMiniFigure : MonoBehaviour
{
    [SerializeField] private FuncMove Move;

    public void ChangeFigure(int figure, Image[] sr, bool right, Sprite[] font, int len = -1)
    {
        int hundred = figure / 100;
        int ten = (figure - hundred * 100) / 10;
        int one = figure - hundred * 100 - ten * 10;
        int[] figs = new int[3] { hundred, ten, one };

        if(len == -1)
        {
            len = FigureLength(figure);
        }

        if (right)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < len)
                {
                    sr[2 - i].sprite = font[figs[2 - i]];
                }
                else
                {
                    sr[2 - i].sprite = font[11];
                }
            }
        }
        else
        {
            int n = 0;
            for (int i = 2; i >= 0; i--)
            {
                if (i >= len)
                {
                    sr[i].sprite = font[11];
                }
                else
                {
                    sr[i].sprite = font[figs[2 - n]];
                    n += 1;
                }
            }
        }
    }

    private int FigureLength(int figure)
    {
        int len = 1;
        if (figure / (int)Mathf.Pow(10, 1) != 0)
        {
            len += 1;
        }
        if (figure / (int)Mathf.Pow(10, 2) != 0)
        {
            len += 1;
        }

        return len;
    }

    public void ChageFigure_x(int figure, Image[] sr, Sprite[] font)
    {
        int len = FigureLength(figure);
        ChangeFigure(figure, sr, true, font, len);

        if(len == 1)
        {
            sr[1].sprite = font[10];
        }
        else
        {
            sr[0].sprite = font[10];
        }
    }

    public void ChangeFigure_SlashtoLeft(int figure, Image[] sr, GameObject slash, float[] slashpos, Sprite[] font)
    {
        int len = FigureLength(figure);
        ChangeFigure(figure, sr, false, font, len);

        Transform transform = slash.transform;
        Vector3 pos = transform.localPosition;
        pos.x = slashpos[len - 1];
        transform.localPosition = pos;
    }

    public void ChangeColorFigure(Image[] sr, float r, float g, float b)
    {
        for(int i=0; i<3; i++)
        {
            sr[i].color = new Color(r, g, b);
        }
    }

    public void ChangeFigureCenter(int figure, float width, float centerX, Image[] sr,GameObject[] obj, Sprite[] font)
    {
        int len = FigureLength(figure);

        bool mainasu = false;
        if (figure < 0)
        {
            mainasu = true;
            len += 1;
            figure = figure * -1;
        }
        ChangeFigure(figure, sr, true, font, len);

        if(mainasu == true)
        {
            if (len == 2)
            {
                sr[1].sprite = font[12];
            }
            else if(len == 3)
            {
                sr[0].sprite = font[12];
            }
        }

        if(len == 1)
        {
            obj[2].SetActive(true);
            Move.MoveLocalPosX(obj[2], centerX);
            obj[1].SetActive(false);
            obj[0].SetActive(false);
        }
        else if(len == 2)
        {
            obj[2].SetActive(true);
            obj[1].SetActive(true);
            float w = width / 2f;
            Move.MoveLocalPosX(obj[2], centerX + w);
            Move.MoveLocalPosX(obj[1], centerX - w);
            obj[0].SetActive(false);
        }
        else
        {
            obj[2].SetActive(true);
            obj[1].SetActive(true);
            obj[0].SetActive(true);
            Move.MoveLocalPosX(obj[2], centerX + width);
            Move.MoveLocalPosX(obj[1], centerX);
            Move.MoveLocalPosX(obj[0], centerX - width);
        }
    }
}
