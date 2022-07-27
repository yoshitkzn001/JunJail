using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill6CoinConfig : MonoBehaviour
{
    [SerializeField] Animator ani;
    private FuncMove Move;
    private Skill6Config main;

    const int MOVE_LATE = 4;
    private int[] MovePoses = new int[] {0, 4, 3, 2, 1, 1, 1 };

    int target_num;
    int t_move;
    int t_attack;
    int t_attack_max;
    float y1;
    float y2;

    // Start is called before the first frame update
    public void Set(int mode)
    {
        if(mode == 0)
        {
            t_move = (MovePoses.Length - 1) * MOVE_LATE + 1;
        }
        else
        {
            t_move = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_move > 0)
        {
            t_move -= 1;

            if(t_move % MOVE_LATE == 0)
            {
                int t = (MovePoses.Length - 1) - (t_move / MOVE_LATE);

                Transform transform = gameObject.transform;
                Vector3 pos = transform.localPosition;
                pos.y += MovePoses[t];
                transform.localPosition = pos;
            }
        }

        if(t_attack > 0)
        {
            t_attack -= 1;

            if(t_attack == 0)
            {
                main.Master.nomalSE.Play("attack1");
                main.bec.Attacked(target_num);
                main.Shake();
                gameObject.SetActive(false);
            }
            else
            {
                Move.AccelMoveY(t_attack, t_attack_max, y1, y2, gameObject);
            }
        }
    }

    public void DestroyCoinAni()
    {
        ani.SetTrigger("destroy");
    }

    public void DesetCoin()
    {
        gameObject.SetActive(false);
    }

    public void StopCoinAni(float _y1, float _y2, int _target_num)
    {
        ani.SetTrigger("stop");
        y1 = _y1;
        y2 = _y2;
        target_num = _target_num;
    }

    public void SetAttack(int _t_attack, FuncMove _Move, Skill6Config _main)
    {
        t_attack = _t_attack;
        t_attack_max = _t_attack;
        Move = _Move;
        main = _main;
    }
}
