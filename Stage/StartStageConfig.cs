using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartStageConfig : MonoBehaviour
{
    public static int load = 0;

    [SerializeField] FuncMove Move;
    [SerializeField] MasStageConfig Master;

    [SerializeField] GameObject stageUI_items_text;
    [SerializeField] GameObject stageUI_items_buttons;
    [SerializeField] GameObject stageUI_left;
    [SerializeField] GameObject stageUI_move_buttons;
    [SerializeField] GameObject stageUI_action_buttons;

    private void Awake()
    {
        if (Master.start_from_this == true)
        {
            if (load == 0)
            {
                load = 1;
                SceneManager.LoadScene("LoadDataScene");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float sideX = (int)(Status.SCREEN_WIDTH - 200);
        Move.MoveLocalPosX(Master.stageUI_map, sideX);
        Move.MoveLocalPosX(Master.stageUI_status, sideX);
        Move.MoveLocalPosX(stageUI_items_text, sideX);
        Move.MoveLocalPosX(stageUI_items_buttons, -sideX);
        Move.MoveLocalPosX(stageUI_left, -sideX);

        if (sideX > 0)
        {
            Move.MoveLocalPosX(stageUI_move_buttons, (int)(-sideX / 3));
            Move.MoveLocalPosX(stageUI_action_buttons, (int)(sideX / 3));
        }
        else
        {
            Move.MoveLocalPosX(stageUI_move_buttons, -sideX);
            Move.MoveLocalPosX(stageUI_action_buttons, sideX);
        }

        Master.move_stageUI_map_posX = new float[2] { sideX, sideX + 53 };

        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
