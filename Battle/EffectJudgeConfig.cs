using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectJudgeConfig : MonoBehaviour
{
    [SerializeField] FuncMove Move;
    [SerializeField] Image effect_judge_img;
    [SerializeField] Sprite[] effect_judge_sprite;

    float first_posY;
    int t_effect_judge;
    int TIME_STAY;
    int TIME_FADE;

    private void FixedUpdate()
    {
        if(t_effect_judge > 0)
        {
            t_effect_judge -= 1;

            if (t_effect_judge == TIME_STAY - 3)
            {
                Move.MoveLocalPosY(gameObject, first_posY - 2f);
            }

            if (t_effect_judge < TIME_FADE)
            {
                if(t_effect_judge % 2 == 0)
                {
                    Move.AccelMoveY(t_effect_judge, TIME_FADE, first_posY - 2f, first_posY + 4f, gameObject);
                }
                effect_judge_img.color = new Color(1f, 1f, 1f, t_effect_judge / (float)TIME_FADE);
            }

            if(t_effect_judge == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetJudge(float posX, float posY, int kind, int _TIME_STAY, int _TIME_FADE)
    {
        first_posY = posY;
        Move.MoveLocalPos(gameObject, posX, posY);
        effect_judge_img.sprite = effect_judge_sprite[kind];
        effect_judge_img.color = new Color(1f, 1f, 1f, 1f);
        t_effect_judge = _TIME_STAY;
        TIME_STAY = _TIME_STAY;
        TIME_FADE = _TIME_FADE;
        gameObject.SetActive(true);
    }
}
