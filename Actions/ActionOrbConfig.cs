using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOrbConfig : MonoBehaviour
{
    [SerializeField] DataStage Data;
    [SerializeField] MiniMapConfig Minimap;
    [SerializeField] JunConfig junC;

    public int myroom;

    private const int CLOSE_TIME = 20;
    private const int OPEN_TIME = 30;
    private int t_open;
    private int t_close;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(t_open > 0)
        {
            t_open -= 1;

            Data.Move.DeccelMoveY(t_open, OPEN_TIME, -154, -71, Data.orb_menu_back);
            Data.Move.DeccelMoveY(t_open, OPEN_TIME, Data.MasterBattle.pos_move_firstbuttons[1], Data.MasterBattle.pos_move_firstbuttons[0], Data.DataBattle.move_firstbuttons);

            if(t_open == 0)
            {
                Data.MasterStage.select_orb = true;
            }
        }

        if(t_close > 0)
        {
            t_close -= 1;

            if(t_close <= CLOSE_TIME)
            {
                Data.Move.AccelMoveY(t_close, CLOSE_TIME, -71, -154, Data.orb_menu_back);
                Data.Move.AccelMoveY(t_close, CLOSE_TIME, Data.MasterBattle.pos_move_firstbuttons[0], Data.MasterBattle.pos_move_firstbuttons[1], Data.DataBattle.move_firstbuttons);
            }

            if(t_close == 0)
            {
                Data.orb_menu_back.SetActive(false);
                Data.DataBattle.move_firstbuttons.SetActive(false);
                Minimap.ChangeActionValue(myroom, -1);
                junC.t_action = 0;
                junC.play_action = false;
                junC.action = false;
            }
        }
    }

    public void Open()
    {
        gameObject.tag = "Untagged";
        ActionConfig action_config = GetComponent<ActionConfig>();
        action_config.sr.sprite = action_config.sprite[0];
        t_open = OPEN_TIME+1;
    }

    public void Close()
    {
        t_close = CLOSE_TIME + 10;
    }
}
