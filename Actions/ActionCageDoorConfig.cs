using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionCageDoorConfig : MonoBehaviour
{
    const float DOOR_RANGE = 8f;
    const int DOOR_ANIM_SPEED = 12;

    [SerializeField] DataStage Data;
    [SerializeField] MasStageConfig Master;
    [SerializeField] GameObject door;
    [SerializeField] SpriteRenderer door_sr;
    [SerializeField] TutorialConfig TutorialConfig;
    public int dir;

    public int area_x;
    public int area_y;

    public bool tutorial;

    private bool open;
    private int t_move;
    private int door_ani_index;

    private float closed_posX;

    private void Start()
    {
        Vector2 mypos = transform.localPosition;
        closed_posX = mypos.x;

        if(tutorial == true)
        {
            TutorialConfig.actionCages.Add(this);
        }
    }

    private void OnDisable()
    {
        if(t_move > 0)
        {
            t_move = 0; 
            door_sr.sprite = Data.cage_door[2];
        }
    }

    private void FixedUpdate()
    {
        if(t_move > 0)
        {
            t_move -= 1;

            if(t_move == DOOR_ANIM_SPEED / 2)
            {
                door_sr.sprite = Data.cage_door[door_ani_index];
                door_ani_index += 1;
            }

            if(t_move == 0)
            {
                door_sr.sprite = Data.cage_door[door_ani_index];
            }
        }
    }

    public void MoveDoor()
    {
        Transform transform = door.transform;
        Vector3 pos = transform.localPosition;
        if (open == false)
        {
            open = true;
            pos.x += 25 * dir;
            //Master.t_nav = 3;
            OpenDoorAni(1);
        }
        else
        {
            open = false;
            pos.x -= 25 * dir;
            MoveCharainDoor();
            //Master.t_nav = 3;
            OpenDoorAni(-1);
        }
        transform.localPosition = pos;
    }

    private void MoveCharainDoor()
    {
        Transform mytransform = gameObject.transform;
        Vector2 mypos = mytransform.localPosition;
        float myposY = mypos.y - 23.5f;

        for (int i=0; i< Master.stage_charas.Count; i++)
        {
            Transform transform = Master.stage_charas[i].transform;
            Vector3 pos = transform.localPosition;
            float targetY = pos.y - Master.stage_charas_height[i];

            if(closed_posX - 12.5f <= pos.x & pos.x <= closed_posX + 12.5f)
            {
                if (myposY < targetY & targetY < myposY + DOOR_RANGE)
                {
                    pos.y = myposY + DOOR_RANGE + Master.stage_charas_height[i] + (targetY - (int)targetY);

                    if (transform.tag != "Player")
                    {
                        Master.stage_charas[i].GetComponent<StageEnemyConfig>().SetChasePositionY(myposY + DOOR_RANGE + (targetY - (int)targetY));
                        Debug.Log(myposY);
                        Debug.Log(targetY);
                    }
                }
                else if (myposY - DOOR_RANGE < targetY & targetY <= myposY)
                {
                    pos.y = myposY - DOOR_RANGE + Master.stage_charas_height[i] - (targetY - (int)targetY);

                    if (transform.tag != "Player")
                    {
                        Debug.Log(myposY);
                        Debug.Log(targetY);
                        Master.stage_charas[i].GetComponent<StageEnemyConfig>().SetChasePositionY(myposY - DOOR_RANGE + (targetY - (int)targetY));
                    }
                }
                transform.localPosition = pos;
            }
        }
    }

    private void OpenDoorAni(int d)
    {
        if(d * dir == 1)
        {
            door_ani_index = 0;
        }
        else
        {
            door_ani_index = 3;
        }
        t_move = DOOR_ANIM_SPEED;
        door_sr.sprite = Data.cage_door[door_ani_index];
        door_ani_index += 1;
    }
}
