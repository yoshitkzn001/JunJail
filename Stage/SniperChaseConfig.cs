using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperChaseConfig : MonoBehaviour
{
    const int TIME_FOUND = 30;

    [SerializeField] MasStageConfig Master;
    [SerializeField] FuncMove Move;
    [SerializeField] GameObject scope;
    [SerializeField] JunConfig junC;
    [SerializeField] int rangeX;
    [SerializeField] int rangeY;

    BoxCollider2D scope_col;
    SpriteRenderer scope_sr;
    StageEnemyConfig enemy;

    float height;
    int t_found;
    int t_move;
    int t_move_fast;
    int dir = 1;

    // Start is called before the first frame update
    void Awake()
    {
        scope_sr = scope.GetComponent<SpriteRenderer>();
        enemy = GetComponent<StageEnemyConfig>();
        height = enemy.height;
        scope_col= scope.GetComponent<BoxCollider2D>();
        scope_col.enabled = false;
    }

    private void OnDisable()
    {
        t_found = 0;
        t_move = 0;
        scope.SetActive(false);
        Move.MoveLocalPos(scope, 0f, 0f);
        scope_sr.color = new Color(1f, 1f, 1f, 0f);
        scope_col.enabled = false;
    }

    private void FixedUpdate()
    {
        if(t_found > 0)
        {
            t_found -= 1;

            scope_sr.color = new Color(1f, 1f, 1f, (TIME_FOUND - t_found) / (float)TIME_FOUND);

            if(t_found == 0)
            {
                scope_col.enabled = true;
                enemy.action.SetActive(false);
                t_found = -1;
            }
        }

        if(t_found == -1)
        {
            t_move += 1;

            if(t_move == 1)
            {
                ScopeChase();
            }

            if(t_move == t_move_fast)
            {
                t_move = 0;
            }

            SetMoveFast();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (junC.Moveroom == false & junC.first_move == true & t_found == 0)
        {
            Transform jtransform = junC.gameObject.transform;
            Vector2 jpos = jtransform.position;

            Transform mytransform = gameObject.transform;
            Vector2 mypos = mytransform.position;
            if (mypos.x - rangeX < jpos.x & mypos.x + rangeX > jpos.x)
            {
                if ((mypos.y - height) - rangeY < (jpos.y - 20.5f) & (mypos.y - height) + rangeY > (jpos.y - 20.5f))
                {
                    enemy.ani.SetTrigger("found");
                    enemy.action.SetActive(true);
                    t_found = TIME_FOUND;
                    t_move = 0;
                    scope.SetActive(true);
                    SetDirFirst();
                    Move.MoveLocalPos(scope, -18, -5.5f);
                    SetMoveFast();
                    Master.nomalSE.Play("emerge1");
                }
            }
        }
    }

    private void SetDirFirst()
    {
        Transform jtransform = junC.gameObject.transform;
        Vector2 jpos = jtransform.position;
        Transform transform = gameObject.transform;
        Vector2 pos = transform.position;

        if (pos.x > jpos.x)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }

        Vector3 scale = transform.localScale;
        scale.x = dir;
        transform.localScale = scale;
    }

    private void ScopeChase()
    {
        Transform jtransform = junC.gameObject.transform;
        Vector2 jpos = jtransform.position;
        Transform transform = scope.transform;
        Vector2 pos = transform.position;

        int moveX = 0;
        int moveY = 0;

        if (pos.x > jpos.x + 1)
        {
            moveX = -1;
        }
        else if(pos.x < jpos.x - 1)
        {
            moveX = 1;
        }

        if (pos.y > jpos.y + 1)
        {
            moveY = -1;
        }
        else if (pos.y < jpos.y - 1)
        {
            moveY = 1;
        }

        pos.x += moveX;
        pos.y += moveY;
        transform.position = pos;
    }

    private void SetMoveFast()
    {
        Transform jtransform = junC.gameObject.transform;
        Vector2 jpos = jtransform.position;

        Transform mytransform = scope.transform;
        Vector2 mypos = mytransform.position;

        bool in_x = false;
        bool in_y = false;
        if (mypos.x - 30 < jpos.x & mypos.x + 30 > jpos.x)
        {
            in_x = true;
        }
        if ((mypos.y - height) - 30 < (jpos.y - 20.5f) & (mypos.y - height) + 30 > (jpos.y - 20.5f))
        {
            in_y = true;
        }

        int next_t_move_fasted;
        if (in_x == true & in_y == true)
        {
            next_t_move_fasted = 2;
        }
        else
        {
            next_t_move_fasted = 1;
        }

        if(next_t_move_fasted != t_move_fast)
        {
            t_move_fast = next_t_move_fasted;
            t_move = 0;
        }
    }
}
