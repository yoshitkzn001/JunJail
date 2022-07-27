using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEmergeConfig : MonoBehaviour
{
    private const int SPEED_EMERGE = 5;
    private const int SPEED_ATTACKED = 5;

    public BattleEnemyConfig bec;

    [SerializeField] private SpriteRenderer sr;
    private int t_emerge;
    public int enemy_num;

    // Start is called before the first frame update
    void OnEnable()
    {
        t_emerge = 0;
        sr.sprite = bec.emerge2_sprites[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t_emerge >= 0)
        {
            t_emerge += 1;

            if (t_emerge % SPEED_EMERGE == 1)
            {
                int index = t_emerge / SPEED_EMERGE;
                if (index == 8)
                {
                    gameObject.SetActive(false);
                    bec.enemy_attack2_boxes.Enqueue(gameObject);
                    bec.SetAttack(enemy_num);
                }
                else
                {
                    if(index > 2)
                    {
                        if(index == 7)
                        {
                            index = 3;
                        }
                        else
                        {
                            index = 2;
                        }
                    }

                    sr.sprite = bec.emerge2_sprites[index];
                }
            }
        }
    }
}
