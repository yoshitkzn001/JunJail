using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class ChaseScript : MonoBehaviour
{
    [SerializeField] MasStageConfig Master;
    [SerializeField] LayerMask ground;
    [SerializeField] DataStage data;
    [SerializeField] JunConfig junC;
    [SerializeField] int rangeX;
    [SerializeField] int rangeY;
    AgentScript agent;
    StageEnemyConfig enemy;
    [SerializeField] int found_time;
    [SerializeField] int speed;
    [SerializeField] int tired_time;

    public Transform target;
    float height;
    int t_found;
    int dir = 1;
    float chase_x;
    int t_tired;

    public float pre_x;
    public float pre_y;
    private bool load_agent;


    // Start is called before the first frame update
    void Awake()
    {
        enemy = GetComponent<StageEnemyConfig>();
        height = enemy.height;
        GameObject chase = Instantiate(data.chase);
        chase.SetActive(true);
        agent = chase.GetComponent<AgentScript>();
        Transform mytransform = gameObject.transform;
        Vector3 mypos = mytransform.localPosition;
        Transform transform = chase.transform;
        transform.parent = mytransform.parent;
        Vector3 pos = transform.localPosition;
        pos.x = mypos.x;
        pos.y = mypos.y;
        transform.localPosition = pos;
        target = transform;
        pre_y = mypos.y;
        pre_x = mypos.x;

        agent.enemy = enemy;
        agent.agent.speed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_found == -1)
        {
            Vector2 targetpos = target.position;
            Transform transform = gameObject.transform;
            Vector3 pos = transform.position;
            float posX = (int)targetpos.x + 0.5f;
            float posY = (int)targetpos.y + height + 0.5f;

            if (pre_x != posX)
            {
                pos.x = posX;
                pre_x = posX;
            }

            if (pre_y != posY)
            {
                int dirY = (int)(posY - pre_y) / (int)(Mathf.Abs(posY - pre_y));
                Vector2 GroundPos = new Vector2(pos.x, pre_y - height - 0.5f);
                Vector2 GroundArea1 = new Vector2(1, 4 * dirY);
                Vector2 GroundArea2 = new Vector2(-1, 2 * dirY);
                Debug.DrawLine(GroundPos + GroundArea1, GroundPos + GroundArea2, Color.red);
                bool hit = Physics2D.OverlapArea(GroundPos + GroundArea1, GroundPos - GroundArea2, ground);

                if (hit == true)
                {
                    Transform targettransform = target.transform;
                    targetpos.y = pre_y - height - 0.5f;
                    targettransform.position = targetpos;
                }
                else
                {
                    pos.y = posY;
                    pre_y = posY;
                }
            }

            transform.position = pos;
        }

        if(t_tired > 0)
        {
            t_tired -= 1;

            if(t_tired == 0)
            {
                agent.agent.enabled = false;
                agent.agent_active = false;
                agent.in_area = false;
                load_agent = false;
                t_tired = -70;
                enemy.ani.SetBool("tired", true);
            }
        }
        else if(t_tired < 0)
        {
            t_tired += 1;

            if(t_tired == 0)
            {
                enemy.ani.SetBool("tired", false);
                t_found = 0;
            }
        }

        if (t_found > 0)
        {
            t_found -= 1;

            if(t_found == found_time - 30)
            {
                enemy.action.SetActive(false);
            }

            if(t_found == 0)
            {
                enemy.ani.SetTrigger("run");
                agent.in_area = true;
                t_found = -1;

                if (tired_time > 0)
                {
                    t_tired = tired_time;
                }

                Vector2 targetpos = target.position;
                pre_x = (int)targetpos.x + 0.5f;
                pre_y = (int)targetpos.y + height + 0.5f;
            }
        }

        if(t_found == -1) //chase
        {
            SetDir();
        }
    }

    private void OnDisable()
    {
        t_found = 0;
        enemy.action.SetActive(false);
        load_agent = false;

        if (tired_time > 0)
        {
            enemy.ani.SetBool("tired", false);
            t_tired = 0;
        }
    }

    private void Update()
    {
        if(load_agent == false)
        {
            if(data.MasterStage.t_nav == 0)
            {
                agent.SetAgent();
                load_agent = true;
            }
        }

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
                    t_found = found_time;
                    SetDirFirst();
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

        int change_dir = dir;
        if(dir == -1)
        {
            if(pos.x > jpos.x)
            {
                change_dir = 1;
            }
        }
        else
        {
            if (pos.x < jpos.x)
            {
                change_dir = -1;
            }
        }

        if(dir != change_dir)
        {
            Vector3 scale = transform.localScale;
            scale.x = change_dir;
            transform.localScale = scale;
            dir = change_dir;
        }

        chase_x = pos.x;
    }
    private void SetDir()
    {
        Transform transform = gameObject.transform;
        Vector2 pos = transform.position;

        int change_dir = dir;
        if (dir == -1)
        {
            if (pos.x < chase_x)
            {
                change_dir = 1;
            }
        }
        else
        {
            if (pos.x > chase_x)
            {
                change_dir = -1;
            }
        }

        if (dir != change_dir)
        {
            Vector3 scale = transform.localScale;
            scale.x = change_dir;
            transform.localScale = scale;
            dir = change_dir;
        }
        chase_x = pos.x;
    }
}
