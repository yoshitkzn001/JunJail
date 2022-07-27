using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class DataStage : MonoBehaviour
{
    public GameObject chase;
    public MasBattleConfig MasterBattle;
    public MasStageConfig MasterStage;
    public DataBattle DataBattle;
    public FuncMove Move;

    public Dictionary<int, GameObject> rooms = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> areas = new Dictionary<int, GameObject>();
    public Dictionary<int, int[,]> active_box_map = new Dictionary<int, int[,]>();
    public GameObject all_area;

    public GameObject hp_bar_dec;
    public GameObject hp_bar;
    public GameObject hp_slash;
    public Image[] hp_fig;
    public Image[] hp_fig_max;

    public GameObject sp_bar_dec;
    public GameObject sp_bar;
    public GameObject sp_slash;
    public Image[] sp_fig;
    public Image[] spnum_fig;

    public GameObject[] twitch_level_fig_obj;
    public Image[] twitch_level_fig;
    public GameObject[] niconico_level_fig_obj;
    public Image[] niconico_level_fig;
    public GameObject[] youtube_level_fig_obj;
    public Image[] youtube_level_fig;

    public GameObject orb_menu_back;
    public Sprite[] button_orb_pushup;
    public Sprite[] button_orb_pushdown;

    public Sprite[] cage_door;
    public Sprite[] box;
    public Sprite[] box2;
    public Sprite box_action;

    public Sprite[] effect_text;

    public TextMeshProUGUI start_text;
}
