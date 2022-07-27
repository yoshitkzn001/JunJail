using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class StageEnemyConfig : MonoBehaviour
{
    public bool chase_bool;
    public bool sub_obj;
    public bool boss;
    public StageEnemyConfig parent;

    [SerializeField] MasStageConfig Master;
    [SerializeField] DataStage Data;
    ChaseScript chase;
    int[,] eight_dir = new int[8, 2]
    {
        {1, 0 },
        {1, 1 },
        {0, 1 },
        {-1, 1 },
        {-1, 0 },
        {-1, -1 },
        {0, -1 },
        {1, -1 },
    };

    public float height;
    public Animator ani;
    public GameObject action;
    public int myroom;

    public int my_id;
    public List<int> battle_id = new List<int>();
    public List<int> battle_level = new List<int>();

    public bool dead;

    // Start is called before the first frame update
    void Awake()
    {
        if(sub_obj == false)
        {
            if (chase_bool == true)
            {
                chase = GetComponent<ChaseScript>();
            }
            Transform transform = gameObject.transform;
            Vector3 pos = transform.localPosition;
            pos.y += height;
            transform.localPosition = pos;
        }
    }
    private void Start()
    {
        if (sub_obj == true)
        {
            if(boss == false)
            {
                battle_id = new List<int>(parent.battle_id);
                battle_level = new List<int>(parent.battle_level);
            }
        }
    }
    private void OnDisable()
    {
        if (sub_obj == false)
        {
            int[,] box = Data.active_box_map[myroom];
            Transform transform = gameObject.transform;
            Vector3 pos = transform.localPosition;
            int x = (int)(pos.x / 25f);
            int y = box.GetLength(0) - (int)((pos.y - height) / 25f) - 1;

            if (box[y, x] == 5 | Master.enemy_reset_pos.Contains(y * 100 + x))
            {
                bool get_pos = false;
                for (int i = 0; i < 5; i++)
                {
                    (int newY, int newX) = ChangeCharaPos(x, y, box, i + 1);

                    if (newY != -1 & newX != -1)
                    {
                        x = newX;
                        y = newY;
                        get_pos = true;
                        break;
                    }
                }

                if (get_pos == false)
                {
                    for (int i = 0; i < eight_dir.GetLength(0); i++)
                    {
                        int searchX = x + eight_dir[i, 0];
                        int searchY = y + eight_dir[i, 1];

                        if (0 < searchX & searchX < box.GetLength(1))
                        {
                            if (0 < searchY & searchY < box.GetLength(0))
                            {
                                int searchnum = box[searchY, searchX];
                                if (searchnum == 1 | searchnum == 13)
                                {
                                    x = searchX;
                                    y = searchY;
                                }
                            }
                        }
                    }
                }
            }

            Master.enemy_reset_pos.Add(y * 100 + x);
            float posX = x * 25f + 12.5f;
            float posY = (box.GetLength(0) - y - 1) * 25f + 12.5f;
            pos.x = posX;
            pos.y = posY + height;
            transform.localPosition = pos;
            if (chase_bool == true)
            {
                Transform ctransform = chase.target;
                Vector3 cpos = ctransform.localPosition;
                cpos.x = posX;
                cpos.y = posY;
                ctransform.localPosition = cpos;
            }

            action.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (sub_obj == false)
        {
            Master.stage_charas.Add(gameObject);
            Master.stage_charas_height.Add(height);
        }
    }

    private void FixedUpdate()
    {
        if (sub_obj == false)
        {
            SetPositionZ();
        }
    }

    private void SetPositionZ()
    {
        Transform ltransform = gameObject.transform;
        Vector3 lpos = ltransform.localPosition;
        lpos.z = (lpos.y - height) / 10f;
        ltransform.localPosition = lpos;
    }

    public void SetChasePositionY(float y)
    {
        if(chase_bool == true)
        {
            Debug.Log(true);
            Transform ctransform = chase.target.transform;
            Vector3 cpos = ctransform.localPosition;
            cpos.y = y;
            ctransform.localPosition = cpos;
            chase.pre_y = (int)y + height + 0.5f;
        }
        SetPositionZ();
    }

    private (int newY, int newX) ChangeCharaPos(int x, int y, int[,] box, int count)
    {
        for (int i = 0; i < eight_dir.GetLength(0); i++)
        {
            int searchX = x + eight_dir[i, 0] * count;
            int searchY = y + eight_dir[i, 1] * count;

            if (0 < searchX & searchX < box.GetLength(1))
            {
                if (0 < searchY & searchY < box.GetLength(0))
                {
                    int searchnum = box[searchY, searchX];
                    if (searchnum == 1 | searchnum == 13)
                    {
                        if (!Master.enemy_reset_pos.Contains(searchY * 100 + searchX))
                        {
                            return (searchY, searchX);
                        }
                    }
                }
            }
        }

        return (-1, -1);
    }

    public void Dead()
    {
        dead = true;
        gameObject.SetActive(false);

        if(sub_obj == true)
        {
            if(boss == false)
            {
                parent.dead = true;
                parent.gameObject.SetActive(false);
            }
        }
    }
}
