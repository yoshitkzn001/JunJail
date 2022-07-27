using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class StageCreateConfig : MonoBehaviour
{
    public bool stage_create_test;
    public int battle_test;
    public bool map_test;
    public bool talk_test;

    //debug
    public static int load = 0;

    [SerializeField] MiniMapConfig Minimap;
    [SerializeField] MasStageConfig Master;
    [SerializeField] DataStage Data;
    [SerializeField] FuncMove Move;
    [SerializeField] GameObject jun;
    [SerializeField] JunConfig junC;
    [SerializeField] GameObject map_all_obj;
    [SerializeField] GameObject map_resources;
    [SerializeField] GameObject map_movespace_resources;

    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject obj_fire;

    //Tile
    [SerializeField] Tile[] tile_floors;
    [SerializeField] Tile[] tile_edges_line;
    [SerializeField] Tile[] tile_edges_corner;
    [SerializeField] Tile[] tile_edges_line_corner;
    [SerializeField] Tile[] tile_shadows;
    [SerializeField] Tile[] tile_other;
    [SerializeField] Tile[] tile_area_line;
    [SerializeField] Tile[] tile_area_corner;
    [SerializeField] Tile[] tile_area_line_corner;
    [SerializeField] Tile[] tile_area_cage;
    [SerializeField] Tile[] tile_area_cage_opened;

    [SerializeField] Sprite[] sprite_walls;
    [SerializeField] Sprite[] sprite_walllines;
    [SerializeField] Sprite[] sprite_wallshadow;

    //Obj
    [SerializeField] GameObject obj_cage;
    [SerializeField] GameObject obj_cage_door;
    [SerializeField] GameObject obj_box;
    [SerializeField] GameObject obj_stair;
    [SerializeField] GameObject obj_orb;
    [SerializeField] GameObject obj_decoration_1to1;
    [SerializeField] GameObject obj_decoration_1to2;
    [SerializeField] GameObject obj_decoration_wall_1to1;
    public Sprite[] obj_decoration_1to1_sprite;
    public Sprite[] obj_decoration_1to2_sprite;
    public Sprite[] obj_decoration_wall_1to1_sprite;
    [SerializeField] GameObject obj_chara_1to1;
    public GameObject obj_chara_1to2;
    [SerializeField] GameObject[] obj_chara_boss;

    int[,] four_dir = new int[4, 2]
    {
        {1, 0 },
        {0, 1 },
        {-1, 0 },
        {0, -1 },
    };
    List<int> dir_range = new List<int>() { 1, 2, 4, 8 };
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

    //item
    public List<(int, int, int, GameObject, int)> item_canplace = new List<(int, int, int, GameObject, int)>();
    private List<int> items_count = new List<int>() //階ごとのアイテム
    {
        1, 1, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 5
    };
    private List<int> items_done = new List<int>() //階ごとの確定アイテム 100 = アイテムなし
    {
        7, 7, 0, 7, 2, 0, 7, 0, 7, 0, 7, 0, 7, 0
    };
    private bool item_done_bool = false;

    //talk
    public List<(int, int, int, GameObject, int)> talk_canplace = new List<(int, int, int, GameObject, int)>();
    [SerializeField] Sprite[] talk_emon_sp;
    public List<float> talk_pos = new List<float>();
    public List<int> talk_speed = new List<int>();
    public List<Sprite[]> talk_sp = new List<Sprite[]>();
    private List<int> talk_count = new List<int>() //階ごとのトークキャラ数
    {
        1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0
    };
    private List<int> talk_kind = new List<int>()
    {
        1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0
    };

    //Enemy
    private List<Dictionary<int, int>> StageEnemy = new List<Dictionary<int, int>>()
    {
        new Dictionary<int, int> //15階の敵　(id, count)
        {
            {1, 1},
        }
        ,
        new Dictionary<int, int> //14
        {
            {0, 1},
            {1, 1},
        }
        ,
        new Dictionary<int, int> //13
        {
            {0, 1},
            {1, 1},
            {4, 1},
        }
        ,
        new Dictionary<int, int> //12
        {
            {0, 3},
            {1, 1},
            {4, 1},
        }
        ,
        new Dictionary<int, int> //11
        {
            {1, 3},
            {4, 2},
        }
        ,
        new Dictionary<int, int> //10
        {
            {0, 2},
            {1, 2},
            {2, 2},
        }
        ,
        new Dictionary<int, int> //9
        {
            {0, 2},
            {1, 1},
            {2, 1},
            {4, 2},
        }
        ,
        new Dictionary<int, int> //8
        {
            {0, 1},
            {1, 2},
            {2, 2},
            {4, 1},
        }
        ,
        new Dictionary<int, int> //7
        {
            {0, 2},
            {1, 2},
            {2, 2},
            {4, 2},
        }
        ,
        new Dictionary<int, int> //6
        {
            {0, 3},
            {1, 1},
            {2, 3},
            {4, 1},
        }
        ,
        new Dictionary<int, int> //5
        {
            {0, 2},
            {1, 1},
            {2, 1},
            {3, 2},
            {4, 2},
        }
        ,
        new Dictionary<int, int> //4
        {
            {0, 1},
            {1, 1},
            {2, 1},
            {3, 3},
            {4, 2},
        }
        ,
        new Dictionary<int, int> //3
        {
            {0, 2},
            {1, 2},
            {2, 1},
            {3, 2},
            {4, 1},
        }
        ,
        new Dictionary<int, int> //2
        {
            {0, 2},
            {1, 2},
            {2, 2},
            {3, 2},
            {4, 2},
        }
        ,
    };

    //int[2] {id, level}
    private Dictionary<int, List<List<int[]>>> StageEnemy_list_15 = new Dictionary<int, List<List<int[]>>>()
    {
        {1,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {1, 0},
            }
        }
        }
        ,
    };
    private Dictionary<int, List<List<int[]>>> StageEnemy_list_14 = new Dictionary<int, List<List<int[]>>>()
    {
        {0,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {0, 0},
            },
        }
        }
        ,
        {1,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {1, 0},
                new int[2] {0, 0},
            },
        }
        }
        ,
    };
    private Dictionary<int, List<List<int[]>>> StageEnemy_list_13_11 = new Dictionary<int, List<List<int[]>>>()
    {
        {0,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {0, 0},
                new int[2] {1, 0},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {0, 0},
            },
            new List<int[]>
            {
                new int[2] {0, 0},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 0},
                new int[2] {4, 0},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 0},
                new int[2] {0, 0},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 0},
                new int[2] {1, 1},
            },
        }
        }
        ,
        {1,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {1, 0},
                new int[2] {1, 0},
                new int[2] {1, 0},
            },
            new List<int[]>
            {
                new int[2] {1, 0},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 0},
                new int[2] {4, 0},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {0, 0},
            },
            new List<int[]>
            {
                new int[2] {1, 0},
                new int[2] {0, 0},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 0},
                new int[2] {0, 0},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {1, 0},
            },
        }
        }
        ,
        {4,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {4, 0},
                new int[2] {4, 0},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 0},
                new int[2] {0, 1},
                new int[2] {4, 0},
            },
            new List<int[]>
            {
                new int[2] {4, 0},
                new int[2] {1, 0},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 0},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {1, 0},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {4, 0},
            },
        }
        }
        ,
    };
    private Dictionary<int, List<List<int[]>>> StageEnemy_list_10_6 = new Dictionary<int, List<List<int[]>>>()
    {
        {0,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {1, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {4, 1},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {4, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {2, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {2, 1},
                new int[2] {2, 1},
            },
        }
        }
        ,
        {1,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {1, 1},
                new int[2] {1, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {1, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {2, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {0, 1},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {1, 1},
                new int[2] {2, 1},
                new int[2] {4, 1},
            },
        }
        }
        ,
        {2,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {2, 1},
                new int[2] {2, 1},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {1, 1},
            },
            new List<int[]>
            {
                new int[2] {2, 1},
                new int[2] {0, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {2, 1},
                new int[2] {4, 1},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {2, 1},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 1},
                new int[2] {1, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {2, 1},
                new int[2] {0, 1},
                new int[2] {4, 1},
            },
        }
        }
        ,
        {4,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {2, 1},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {1, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {1, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {1, 1},
                new int[2] {1, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {0, 1},
                new int[2] {0, 1},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 1},
                new int[2] {2, 1},
                new int[2] {2, 1},
            },
        }
        }
        ,
    };
    private Dictionary<int, List<List<int[]>>> StageEnemy_list_5_2 = new Dictionary<int, List<List<int[]>>>()
    {
        {0,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {0, 2},
                new int[2] {1, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {4, 2},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 2},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 2},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {1, 2},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {0, 1},
                new int[2] {1, 2},
                new int[2] {1, 2},
            },
        }
        }
        ,
        {1,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {1, 2},
                new int[2] {1, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {4, 2},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {0, 1},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {3, 2},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {1, 2},
                new int[2] {4, 2},
            },
        }
        }
        ,
        {2,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {1, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {0, 1},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {3, 2},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {3, 2},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {2, 2},
                new int[2] {4, 2},
                new int[2] {4, 2},
            },
        }
        }
        ,
        {3,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {3, 2},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {2, 2},
                new int[2] {2, 1},
            },
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {1, 2},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {4, 2},
                new int[2] {4, 1},
            },
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {3, 2},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {3, 2},
                new int[2] {4, 2},
            },
        }
        }
        ,
        {4,
        new List<List<int[]>>
        {
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {3, 2},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {0, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {4, 2},
                new int[2] {4, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {1, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {1, 2},
                new int[2] {1, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {0, 1},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {3, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {3, 2},
                new int[2] {2, 2},
            },
            new List<int[]>
            {
                new int[2] {4, 2},
                new int[2] {2, 2},
            },
        }
        }
        ,
    };

    //広さと長さを決めて、その広さの中で4方向探索をその長さ分繰り返す
    private List<int[]> map_list = new List<int[]>()  //y × x
    {
        new int[] //15
        {
            1, 1, 0  //1×1の広さ、2の長さ
        }
        ,
        new int[] //14
        {
            1, 1, 0
        }
        ,
        new int[] //13
        {
            1, 1, 0
        }
        ,
        new int[] //12
        {
            1, 2, 1
        }
        ,
        new int[] //11
        {
            2, 1, 1
        }
        ,
        new int[] //10
        {
            1, 2, 1
        }
        ,
        new int[] //9
        {
            1, 3, 2
        }
        ,
        new int[] //8
        {
            3, 1, 2
        }
        ,
        new int[] //7
        {
            2, 2, 2
        }
        ,
        new int[] //6
        {
            2, 2, 2
        }
        ,
        new int[] //5
        {
            2, 2, 2
        }
        ,
        new int[] //4
        {
            2, 2, 3
        }
        ,
        new int[] //3
        {
            2, 2, 3
        }
        ,
        new int[] //2
        {
            2, 2, 3
        }
        ,
    };
    private List<int> box_kind_list = new List<int>()
    {
        0, //box 1×1
        3, //box 1×2
        15, //box 2×1
        54, //box 2×2
        18, //box 2×2　左上L
        22, //box 2×2　右上L
        38, //box 2×2　左下L
        42, //box 2×2　右下L
        10, //box 1×3
        50, //box 3×1
        105, //box 2×3
        165, //box 3×2
        73, //box 3×2 テト凸右
        81, //box 3×2 テト凸左
        69, //box 2×3 テト凸上
        29, //box 2×3 テト凸下
        33, //box 2×3 テトL右上
        25, //box 2×3 テトL左上
        300, //box 3×3
    };
    public List<int> box_enemies_count_list = new List<int>() //box_kind_listと連結
    {
        1,  //box 1×1
        2,  //box 1×2
        2,  //box 2×1
        3,  //box 2×2
        2,  //box 2×2 左上L
        2,  //box 2×2 右上L
        2,  //box 2×2 左下L
        2,  //box 2×2 右下L
        2,  //box 1×3
        2,  //box 3×1
        4,  //box 2×3
        4,  //box 3×2
        3,  //box 3×2 テト凸右
        3,  //box 3×2 テト凸左
        3,  //box 2×3 テト凸上
        3,  //box 2×3 テト凸下
        3,  //box 2×3 テトL右上
        3,  //box 2×3 テトL左上
        6,  //box 3×3
    };
    private const int BOX_MAP_SIZE = 5;
    private List<List<int[,]>> box_map = new List<List<int[,]>>()
    {
        new List<int[,]>()  //[0] 道なし
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,1,5},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,0,0,3},
                {0,0,4,1,3},
                {0,0,4,0,0},
                {4,4,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,3,3,3},
                {4,1,3,3,3},
                {0,0,0,0,2},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,3,3,0,4},
                {3,3,3,1,4},
                {2,0,0,0,2},
                {5,5,5,1,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,0,0,2},
                {3,0,0,0,5},
                {2,0,0,0,2},
                {6,6,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,3,3,3},
                {4,1,3,3,3},
                {0,0,0,0,2},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,0},
                {5,1,3,1,6},
                {0,0,0,0,2},
                {7,7,7,7,7},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {2,0,0,0,4},
                {5,1,6,1,4},
                {0,0,6,0,2},
                {6,6,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,0,0,2},
                {3,1,4,4,4},
                {0,0,2,0,2},
                {0,0,5,0,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,4},
                {0,0,2,0,4},
                {5,5,5,0,4},
                {0,0,2,0,2},
                {0,0,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,2,0,2},
                {3,0,5,5,5},
                {2,0,5,5,5},
                {6,1,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,3,0,0},
                {4,1,3,1,5},
                {4,0,2,0,5},
                {4,0,6,0,5},
            }
            ,
        }  //[0] ・
        ,
        new List<int[,]>()  //[1] 右へ道が続く
        {
            
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0, 0, 3, 1, 4},
                {0, 0, 0, 0, 4},
                {0, 0, 5, 1, 4},
                {0, 0, 5, 0, 2},
                {6, 1, 5, 5, 5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0, 0, 3, 3, 3},
                {0, 0, 2, 0, 0},
                {4, 4, 4, 4, 4},
                {0, 0, 2, 0, 0},
                {0, 0, 5, 0, 0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0, 0, 3, 0, 0},
                {0, 0, 2, 0, 0},
                {4, 4, 4, 4, 4},
                {4, 0, 2, 0, 2},
                {4, 0, 5, 0, 6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0, 0, 3, 1, 4},
                {0, 0, 2, 0, 4},
                {5, 5, 5, 1, 4},
                {5, 0, 0, 0, 0},
                {5, 0, 0, 0, 0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3, 1, 4, 0, 0},
                {3, 0, 0, 0, 0},
                {3, 1, 5, 1, 6},
                {0, 0, 2, 0, 6},
                {0, 0, 7, 1, 6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3, 0, 0, 0, 0},
                {3, 0, 0, 0, 0},
                {3, 3, 3, 1, 4},
                {3, 0, 0, 0, 0},
                {3, 0, 0, 0, 0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0, 0, 3, 1, 4},
                {0, 0, 3, 0, 4},
                {0, 0, 3, 0, 4},
                {0, 0, 2, 0, 4},
                {5, 5, 5, 0, 4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3, 3, 3, 1, 4},
                {2, 0, 0, 0, 4},
                {5, 5, 5, 0, 4},
                {2, 0, 0, 0, 2},
                {6, 6, 6, 1, 7},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3, 3, 3, 0, 0},
                {2, 0, 2, 0, 0},
                {4, 4, 4, 4, 4},
                {0, 0, 2, 0, 2},
                {0, 0, 5, 5, 5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3, 1, 5, 1, 6},
                {2, 0, 5, 0, 6},
                {4, 0, 5, 0, 6},
                {4, 0, 5, 0, 0},
                {4, 1, 5, 0, 0},
            }
            ,
        }  //[1] →  d
        ,
        new List<int[,]>()  //[2] 下へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,0,0},
                {3,0,0,0,0},
                {3,0,5,0,0},
                {3,0,5,0,0},
                {3,1,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,0,0,0},
                {3,0,0,0,0},
                {3,3,3,0,0},
                {2,0,2,0,0},
                {4,1,5,1,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {4,1,3,1,5},
                {4,0,0,0,5},
                {4,0,6,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,0,0,2},
                {3,1,4,4,4},
                {0,0,2,0,0},
                {0,0,5,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,0,0,4},
                {5,5,5,1,4},
                {5,0,0,0,0},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,3,0,0},
                {4,1,3,1,5},
                {4,0,2,0,5},
                {4,0,6,0,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,1,5},
                {3,0,2,0,2},
                {3,0,6,6,6},
                {2,0,0,0,2},
                {7,0,8,8,8},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,3,3,0,4},
                {3,3,3,0,4},
                {0,0,2,0,2},
                {0,0,5,1,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {2,0,0,0,2},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,4,4},
                {2,0,4,4,4},
                {5,0,4,4,4},
                {0,0,0,0,2},
                {0,0,6,6,6},
            }
        }  //[2] ↓  dd
        ,
        new List<int[,]>()  //[3] 右と下へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,0,0,3},
                {0,0,4,1,3},
                {0,0,4,0,0},
                {4,4,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {3,0,2,0,0},
                {3,1,4,1,5},
                {0,0,2,0,5},
                {0,0,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,0,0,2},
                {0,0,4,4,4},
                {0,0,0,0,2},
                {5,5,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,4,4,4},
                {3,0,0,0,2},
                {3,3,3,1,5},
                {0,0,2,0,5},
                {0,0,6,0,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {3,0,2,0,0},
                {3,1,4,4,4},
                {0,0,4,0,0},
                {0,0,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,0,0,4},
                {3,0,0,0,2},
                {3,1,5,5,5},
                {0,0,5,5,5},
                {0,0,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,0,0,3},
                {0,0,0,0,3},
                {4,4,4,1,3},
                {4,4,4,0,3},
                {4,4,4,1,3},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,2,0,0},
                {3,0,4,1,5},
                {0,0,4,0,0},
                {0,0,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,0,0,2},
                {3,0,0,0,5},
                {2,0,0,0,2},
                {6,6,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,2,0,4},
                {3,1,5,1,4},
                {2,0,2,0,2},
                {6,6,6,6,6},
            }
            ,
        }  //[3] →↓  dd
        ,
        new List<int[,]>()  //[4] 左へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,0,0,0},
                {3,0,0,0,0},
                {3,3,3,0,0},
                {2,0,2,0,0},
                {4,1,5,1,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,3,3,3},
                {4,1,3,3,3},
                {0,0,0,0,2},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,0,0,2},
                {3,1,4,4,4},
                {0,0,2,0,2},
                {0,0,5,0,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,4,4,4},
                {3,0,0,0,4},
                {3,3,3,1,4},
                {3,0,0,0,2},
                {3,0,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,2,0,3},
                {4,4,4,1,3},
                {4,4,4,0,0},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,0},
                {5,1,3,1,6},
                {0,0,0,0,2},
                {7,7,7,7,7},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,2,0,2},
                {4,1,5,5,5},
                {0,0,5,5,5},
                {0,0,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,4,4},
                {0,0,4,4,4},
                {5,1,4,4,4},
                {0,0,4,4,4},
                {0,0,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,0,0,4},
                {5,5,5,1,4},
                {5,0,0,0,0},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,0,0,3},
                {0,0,0,0,3},
                {4,4,4,1,3},
                {0,0,0,0,3},
                {0,0,0,0,3},
            }
        }  //[4] ←  dd
        ,
        new List<int[,]>()  //[5] 右と左へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {3,3,3,3,3},
                {0,0,2,0,0},
                {0,0,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,2,0,0},
                {4,4,4,4,4},
                {0,0,0,0,0},
                {0,0,0,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,0},
                {5,1,3,1,6},
                {0,0,0,0,2},
                {7,7,7,7,7},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,2,0,3},
                {4,1,5,1,3},
                {4,0,2,0,0},
                {4,4,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {4,1,3,1,5},
                {4,0,0,0,5},
                {4,0,6,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,0,0,3},
                {0,0,0,0,3},
                {4,4,4,1,3},
                {4,4,4,0,3},
                {4,4,4,1,3},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,2,0,3},
                {4,4,4,1,3},
                {4,4,4,0,0},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,0,0},
                {3,0,0,0,0},
                {3,0,6,6,6},
                {2,0,6,6,6},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,4,4},
                {2,0,4,4,4},
                {5,0,4,4,4},
                {0,0,0,0,2},
                {0,0,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {3,3,3,3,3},
                {0,0,2,0,0},
                {0,0,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,2,0,0},
                {4,4,4,4,4},
                {0,0,4,0,0},
                {0,0,4,0,0},
            }
            ,
        }  //[5] →←  dd
        ,
        new List<int[,]>()  //[6] 左と下へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,0,0,2},
                {3,1,4,4,4},
                {0,0,2,0,2},
                {0,0,5,0,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,0,0,0},
                {3,0,0,0,0},
                {3,3,3,0,0},
                {2,0,2,0,0},
                {4,1,5,1,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {3,3,3,0,4},
                {2,0,3,0,4},
                {5,1,3,1,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {4,1,3,3,3},
                {4,0,2,0,2},
                {4,4,4,0,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,0,0,3},
                {0,0,0,0,2},
                {4,4,4,4,4},
                {0,0,0,0,4},
                {5,5,5,1,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,3,3,0,0},
                {3,3,3,0,0},
                {0,0,2,0,0},
                {5,5,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,0,0,0},
                {3,0,0,0,0},
                {3,1,4,4,4},
                {0,0,4,4,4},
                {0,0,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {2,0,0,0,0},
                {4,4,4,4,4},
                {4,4,4,4,4},
                {4,4,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {2,0,0,0,4},
                {5,5,5,0,4},
                {0,0,5,0,4},
                {0,0,5,1,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,4},
                {0,0,2,0,4},
                {5,5,5,0,4},
                {0,0,2,0,2},
                {0,0,6,6,6},
            }
            ,
        }  //[6] ←↓  dd
        ,
        new List<int[,]>()  //[7] 右と下と左へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
            }
        }  //[7] →←↓
        ,
        new List<int[,]>()  //[8] 上へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,0},
                {5,1,3,1,6},
                {0,0,0,0,2},
                {7,7,7,7,7},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {3,0,2,0,0},
                {3,1,4,1,5},
                {0,0,2,0,5},
                {0,0,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,2,0,3},
                {4,1,5,1,3},
                {4,0,2,0,0},
                {4,4,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {0,0,3,1,4},
                {0,0,2,0,2},
                {5,5,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,0,0},
                {3,0,0,0,0},
                {3,0,6,6,6},
                {2,0,6,6,6},
                {5,1,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {3,3,3,3,3},
                {0,0,0,0,2},
                {0,0,0,0,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,0,0,3},
                {4,4,4,1,3},
                {4,4,4,0,0},
                {4,4,4,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {2,0,0,0,2},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,4,4},
                {2,0,4,4,4},
                {5,0,4,4,4},
                {0,0,0,0,2},
                {0,0,6,6,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,0,0},
                {3,0,4,0,0},
                {3,1,4,4,4},
                {0,0,2,0,0},
                {0,0,5,0,0},
            }
        }  //[8] ↑  d
        ,
        new List<int[,]>()  //[9] 右と上へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {4,1,3,3,3},
                {4,0,2,0,2},
                {4,4,4,0,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,3,3,3},
                {4,1,3,3,3},
                {4,0,0,0,2},
                {4,1,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,0,0,2},
                {3,0,0,0,5},
                {2,0,0,0,5},
                {6,1,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {0,0,3,0,0},
                {0,0,3,1,4},
                {0,0,2,0,4},
                {5,5,5,0,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {2,0,2,0,0},
                {4,4,4,4,4},
                {0,0,2,0,4},
                {0,0,5,1,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,0,0},
                {3,0,0,0,0},
                {3,1,5,1,6},
                {3,0,5,0,0},
                {3,1,5,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,2,0,2},
                {4,4,4,4,4},
                {2,0,0,0,0},
                {5,0,0,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,2},
                {5,1,3,0,6},
                {5,0,0,0,0},
                {5,5,5,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,3,3},
                {0,0,2,0,2},
                {4,4,4,0,5},
                {4,4,4,0,5},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,4,0,0},
                {3,0,4,0,0},
                {3,1,4,4,4},
                {2,0,4,0,0},
                {5,0,4,0,0},
            }
            ,
        }  //[9] →↑  d
        ,
        new List<int[,]>()  //[10] 下と上へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {0,0,3,1,4},
                {0,0,3,0,0},
                {0,0,3,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {4,1,3,0,0},
                {0,0,3,0,0},
                {0,0,3,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {4,1,3,3,3},
                {0,0,3,0,0},
                {0,0,3,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {3,3,3,1,4},
                {0,0,3,0,0},
                {0,0,3,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,0,0,2},
                {3,1,4,4,4},
                {0,0,2,0,2},
                {0,0,5,0,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,0},
                {5,1,3,1,6},
                {0,0,0,0,2},
                {7,7,7,7,7},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,2,0,3},
                {4,4,4,1,3},
                {4,4,4,0,0},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {2,0,0,0,0},
                {4,4,4,4,4},
                {4,4,4,4,4},
                {4,4,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,0,4,4,4},
                {3,0,4,4,4},
                {3,1,4,4,4},
                {0,0,4,4,4},
                {0,0,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,3,3,0,2},
                {3,3,3,1,5},
                {0,0,0,0,5},
                {0,0,5,5,5},
            }
            ,
        }  //[10] ↓↑  d
        ,
        new List<int[,]>()  //[11] 右と下と上へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
            }
        }  //[11] ↓↑→
        ,
        new List<int[,]>()  //[12] 上と左へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {0,0,2,0,3},
                {4,4,4,1,3},
                {4,4,4,0,0},
                {4,4,4,1,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,0,0},
                {2,0,0,0,0},
                {4,4,4,4,4},
                {4,4,4,4,4},
                {4,4,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {2,0,0,0,4},
                {5,5,5,0,4},
                {0,0,5,0,4},
                {0,0,5,1,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,0,0},
                {0,0,3,0,0},
                {3,3,3,3,3},
                {0,0,0,0,2},
                {0,0,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {0,0,3,1,4},
                {0,0,3,0,4},
                {3,3,3,0,4},
                {0,0,2,0,4},
                {0,0,5,1,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,0,2,0,0},
                {3,1,4,0,0},
                {2,0,4,0,0},
                {4,4,4,4,4},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,2,0,2},
                {3,0,5,5,5},
                {0,0,5,5,5},
                {0,0,5,5,5},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,1,4,4,4},
                {0,0,2,0,2},
                {5,5,5,0,6},
                {5,5,5,0,6},
                {5,5,5,1,6},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {2,0,2,0,0},
                {5,1,6,0,0},
                {5,0,0,0,0},
                {5,0,0,0,0},
            }
            ,
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,1,4},
                {3,0,2,0,4},
                {3,0,5,1,4},
                {2,0,5,0,0},
                {5,5,5,0,0},
            }
            ,
        }  //[12] ↑←  d
        ,
        new List<int[,]>()  //[13] 上と左と右へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
            }
        }  //[13] ↑←→
        ,
        new List<int[,]>()  //[14] 上と左と下へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
            }
        }  //[14] ↑↓←
        ,
        new List<int[,]>()  //[15] 上と左と下と右へ道が続く
        {
            new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
            {
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
                {3,3,3,3,3},
            }
        }  //[15] ↑↓←→
    };
    public List<int[,,]> box_list = new List<int[,,]>()
    {
        new int[15, 9, 7]  //box 1×1
        {
            {
                {3,3,3,3,3,3,3,},
                {3,3,3,2,3,3,3,},
                {3,3,3,2,3,3,3,},
                {3,4,3,1,3,4,3,},
                {3,2,2,1,2,2,3,},
                {3,1,2,1,2,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,0,3,3,3,3,3,},
                {0,0,3,2,2,4,3,},
                {3,3,3,2,2,2,3,},
                {3,4,22,1,1,1,3,},
                {3,2,2,7,9,7,3,},
                {3,1,17,17,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,17,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,2,2,2,4,22,3,},
                {3,2,2,2,2,2,3,},
                {3,17,17,1,1,11,3,},
                {3,17,1,1,1,1,3,},
                {3,1,1,43,1,1,3,},
                {3,1,1,1,1,17,3,},
                {3,21,21,1,17,3,3,},
                {3,3,3,3,3,3,0,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,11,1,1,1,1,3,},
                {3,7,7,7,9,7,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,17,3,},
                {3,3,1,1,1,3,3,},
                {0,3,3,3,3,3,0,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,22,2,2,3,22,3,},
                {3,2,2,2,3,2,3,},
                {3,21,21,1,4,11,3,},
                {3,17,1,1,2,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,17,17,1,17,17,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,3,4,3,},
                {3,2,2,2,3,2,3,},
                {3,1,1,1,2,11,3,},
                {3,7,9,7,2,1,3,},
                {3,1,1,1,17,1,3,},
                {3,17,1,1,1,1,3,},
                {3,21,21,1,3,3,3,},
                {3,3,3,3,3,0,0,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,3,4,3,},
                {3,2,2,2,3,2,3,},
                {3,21,21,1,2,11,3,},
                {3,1,1,1,2,1,3,},
                {3,1,3,1,1,1,3,},
                {3,3,3,1,1,17,3,},
                {0,0,3,1,17,17,3,},
                {0,0,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,0,0,},
                {3,4,22,2,3,0,0,},
                {3,2,2,2,3,3,3,},
                {3,1,1,1,2,4,3,},
                {3,7,9,7,2,2,3,},
                {3,1,1,17,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,17,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,3,2,2,4,3,},
                {3,2,3,2,2,2,3,},
                {3,11,3,1,31,35,3,},
                {3,1,24,1,31,39,3,},
                {3,1,2,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,27,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,2,3,2,2,22,3,},
                {3,2,3,2,2,2,3,},
                {3,11,4,1,1,1,3,},
                {3,1,2,7,9,7,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,17,3,},
                {3,21,21,1,17,17,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,21,21,1,1,17,3,},
                {3,1,1,3,1,1,3,},
                {3,1,1,4,1,1,3,},
                {3,1,1,2,1,1,3,},
                {3,17,1,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,1,1,1,21,21,3,},
                {3,1,3,3,1,1,3,},
                {3,1,22,4,1,1,3,},
                {3,1,2,2,1,1,3,},
                {3,1,1,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,0,},
                {3,3,2,2,2,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,17,1,17,2,3,},
                {3,11,1,1,21,21,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,3,17,1,17,3,3,},
                {0,3,3,3,3,3,0,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,21,21,1,17,11,3,},
                {3,1,1,1,1,1,3,},
                {3,1,3,1,3,1,3,},
                {3,3,3,1,3,3,3,},
                {0,0,3,1,3,0,0,},
                {0,0,3,3,3,0,0,},
            }
            ,
            {
                {0,0,3,3,3,0,0,},
                {0,0,3,2,3,0,0,},
                {3,3,3,2,3,3,3,},
                {3,4,3,1,3,4,3,},
                {3,2,2,1,2,2,3,},
                {3,1,2,1,2,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
        },  //box 1×1
        new int[11, 9, 15]  //box 1×2
        {
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,4,2,3,2,2,3,3,3,2,2,2,2,3,},
                {3,2,2,2,3,2,2,3,3,4,2,2,2,2,3,},
                {3,2,1,1,3,17,11,3,2,2,1,1,1,11,3,},
                {3,1,1,1,4,1,1,2,2,1,1,3,1,1,3,},
                {3,1,1,1,2,9,7,2,17,1,1,2,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,2,1,1,3,},
                {3,3,1,1,1,1,1,3,17,21,21,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,2,4,2,2,2,4,2,2,2,2,3,},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,},
                {3,11,1,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,1,1,1,3,3,3,3,3,3,3,3,1,1,3,},
                {3,1,1,1,4,2,36,28,32,36,2,4,1,1,3,},
                {3,17,17,17,2,2,2,2,2,2,2,2,1,1,3,},
                {3,21,21,1,1,1,1,1,1,1,1,1,1,11,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,4,2,2,2,2,4,2,22,2,3,2,2,2,3,},
                {3,2,2,2,2,2,2,2,2,2,3,2,2,2,3,},
                {3,11,1,1,1,1,1,1,1,1,4,1,11,17,3,},
                {3,7,7,7,7,7,7,7,9,7,2,1,1,1,3,},
                {3,1,1,1,21,21,1,1,1,1,17,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,3,3,3,},
                {3,3,1,1,1,1,17,17,17,1,1,1,3,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,22,2,3,2,22,2,2,2,4,3,},
                {3,2,2,2,2,2,2,3,2,2,2,2,2,2,3,},
                {3,1,1,1,1,1,11,3,11,1,1,1,1,1,3,},
                {3,7,7,9,7,7,7,4,7,7,7,9,7,7,3,},
                {3,1,1,1,1,1,17,2,21,21,1,1,1,1,3,},
                {3,17,1,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,3,1,1,1,3,3,3,3,3,1,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {0,3,3,2,3,2,2,4,2,2,3,2,3,3,3,},
                {3,3,22,2,3,2,2,2,2,2,3,2,22,3,3,},
                {3,4,2,1,3,21,21,1,1,11,3,1,2,4,3,},
                {3,2,1,1,4,1,1,1,1,1,4,1,1,2,3,},
                {3,1,1,1,2,7,7,9,7,7,2,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,1,1,1,17,17,11,17,17,1,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,2,2,4,2,2,2,2,4,2,3,},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,},
                {3,17,17,1,1,1,1,17,1,1,1,1,17,17,3,},
                {3,17,1,1,3,1,1,1,1,1,3,1,1,17,3,},
                {3,1,1,1,3,1,1,1,1,1,3,1,1,1,3,},
                {3,1,1,1,3,7,7,7,9,7,3,1,1,1,3,},
                {3,21,21,1,3,11,1,1,1,1,3,1,1,11,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,4,2,2,2,22,2,3,2,2,4,3,},
                {3,2,2,2,2,2,2,2,2,2,3,2,2,2,3,},
                {3,17,1,1,1,1,1,17,17,11,3,1,1,1,3,},
                {3,1,1,1,3,1,1,1,1,1,3,7,9,7,3,},
                {3,1,1,1,3,1,1,1,1,1,4,1,1,1,3,},
                {3,1,1,1,3,7,9,7,7,7,2,1,1,1,3,},
                {3,21,21,1,3,1,1,1,1,1,1,1,17,11,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,3,3,3,3,2,4,2,2,3,3,3,},
                {3,4,2,2,3,3,3,2,2,2,2,2,3,3,3,},
                {3,2,17,1,3,3,3,2,21,21,1,1,2,4,3,},
                {3,11,1,1,2,4,2,1,1,1,1,1,2,2,3,},
                {3,1,1,1,2,2,2,1,3,3,3,1,1,1,3,},
                {3,3,3,1,1,1,1,1,3,3,3,1,1,11,3,},
                {3,3,3,1,1,1,17,3,3,3,3,1,17,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,3,4,2,22,2,4,3,2,2,3,3,},
                {3,3,2,2,3,2,2,2,2,2,3,2,2,3,3,},
                {3,4,17,1,3,11,1,17,1,11,3,1,17,4,3,},
                {3,2,1,1,3,3,1,1,1,3,3,1,1,2,3,},
                {3,1,1,1,4,2,1,1,1,2,4,1,1,1,3,},
                {3,1,1,1,2,2,7,9,7,2,2,1,1,1,3,},
                {3,21,21,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,4,2,2,3,2,2,4,2,2,3,2,2,4,3,},
                {3,2,2,2,3,2,2,2,2,2,3,2,2,2,3,},
                {3,1,1,1,3,11,17,17,17,11,3,1,1,1,3,},
                {3,7,9,7,4,1,1,1,1,1,4,7,9,7,3,},
                {3,1,1,1,2,1,1,3,1,1,2,1,1,1,3,},
                {3,1,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,3,1,1,1,17,3,3,3,21,21,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,4,2,2,3,3,3,3,3,3,3,2,2,4,3,},
                {3,2,2,2,3,3,3,3,3,3,3,2,2,2,3,},
                {3,21,21,1,2,3,3,3,3,3,2,1,1,17,3,},
                {3,1,1,1,2,2,3,3,3,2,2,1,1,1,3,},
                {3,1,1,1,11,2,2,4,2,2,11,1,1,1,3,},
                {3,3,1,1,1,17,2,2,2,17,1,1,1,3,3,},
                {3,3,3,1,1,1,1,1,1,1,1,1,3,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 1×2
        new int[11, 18, 7]  //box 2×1
        {
            {
                {0,0,3,3,3,3,3,},
                {0,3,3,2,2,2,3,},
                {3,3,2,2,2,2,3,},
                {3,4,2,1,1,17,3,},
                {3,2,21,21,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,},
                {3,1,1,3,2,4,3,},
                {3,1,1,3,2,2,3,},
                {3,1,1,3,11,23,3,},
                {3,1,1,4,1,1,3,},
                {3,17,1,2,7,9,3,},
                {3,17,1,1,1,1,3,},
                {3,11,1,1,3,1,3,},
                {3,1,1,1,4,1,3,},
                {3,1,1,1,2,1,3,},
                {3,3,1,1,1,1,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,},
                {3,3,2,2,2,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,1,1,1,2,3,},
                {3,1,1,3,1,1,3,},
                {3,1,1,3,1,1,3,},
                {3,1,1,3,1,1,3,},
                {3,1,1,4,1,1,3,},
                {3,7,7,2,9,7,3,},
                {3,11,1,1,1,17,3,},
                {3,1,1,1,1,17,3,},
                {3,3,1,1,1,3,3,},
                {3,4,1,1,1,4,3,},
                {3,2,9,7,7,2,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,22,4,2,3,3,3,},
                {3,2,2,2,2,3,3,},
                {3,11,1,1,2,4,3,},
                {3,1,1,1,17,2,3,},
                {3,1,1,1,1,1,3,},
                {3,3,1,1,1,1,3,},
                {3,3,3,1,1,1,3,},
                {3,3,3,3,1,17,3,},
                {3,3,3,3,1,11,3,},
                {3,3,3,2,1,1,3,},
                {3,3,4,2,1,17,3,},
                {3,22,2,1,1,1,3,},
                {3,2,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,17,3,3,},
                {3,21,21,1,3,3,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,0,3,3,3,3,3,},
                {0,3,3,2,22,4,3,},
                {3,3,2,2,2,2,3,},
                {3,4,2,1,1,17,3,},
                {3,2,17,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,3,3,},
                {3,31,1,1,3,3,3,},
                {3,11,1,3,3,3,3,},
                {3,1,1,3,3,3,3,},
                {3,17,1,4,3,3,3,},
                {3,1,1,2,22,3,3,},
                {3,1,1,1,2,4,3,},
                {3,1,1,1,17,2,3,},
                {3,1,1,1,1,1,3,},
                {3,3,1,1,1,1,3,},
                {3,3,3,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,21,21,1,1,11,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,3,1,1,3,},
                {3,3,1,3,1,3,3,},
                {3,4,1,3,1,4,3,},
                {3,2,1,3,1,2,3,},
                {3,17,1,3,1,17,3,},
                {3,1,1,3,1,1,3,},
                {3,1,1,3,1,1,3,},
                {3,1,3,3,3,1,3,},
                {3,1,2,4,2,1,3,},
                {3,1,2,2,2,1,3,},
                {3,1,1,11,1,1,3,},
                {3,17,1,1,1,17,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,2,3,2,4,2,3,},
                {3,2,3,2,2,2,3,},
                {3,11,2,1,1,1,3,},
                {3,1,2,7,9,7,3,},
                {3,1,1,1,1,1,3,},
                {3,3,3,3,3,1,3,},
                {3,2,4,2,2,1,3,},
                {3,2,2,2,2,1,3,},
                {3,17,17,11,17,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,3,3,3,3,3,},
                {3,1,2,2,4,22,3,},
                {3,1,2,2,2,2,3,},
                {3,1,1,17,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,3,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,1,1,1,1,1,3,},
                {3,7,7,9,7,7,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,},
                {3,1,17,22,3,3,3,},
                {3,1,17,2,4,3,3,},
                {3,1,1,1,2,22,3,},
                {3,21,21,1,11,2,3,},
                {3,3,1,1,1,17,3,},
                {3,4,1,1,1,1,3,},
                {3,2,7,9,7,7,3,},
                {3,1,1,1,1,1,3,},
                {3,17,1,1,1,11,3,},
                {3,3,1,1,1,3,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,21,21,1,1,17,3,},
                {3,1,1,1,1,1,3,},
                {3,1,3,3,3,1,3,},
                {3,1,3,22,3,1,3,},
                {3,1,3,2,3,1,3,},
                {3,1,4,11,4,1,3,},
                {3,1,2,1,2,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,3,1,3,1,3,},
                {3,1,3,11,3,1,3,},
                {3,1,3,3,3,1,3,},
                {3,1,2,4,2,1,3,},
                {3,1,2,2,2,1,3,},
                {3,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,0,3,3,3,3,3,},
                {0,3,3,2,2,4,3,},
                {3,3,3,2,2,2,3,},
                {3,4,2,1,17,11,3,},
                {3,2,2,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,},
                {3,1,1,2,4,22,3,},
                {3,9,7,2,2,2,3,},
                {3,1,1,17,11,17,3,},
                {3,1,1,1,1,1,3,},
                {3,3,3,3,1,1,3,},
                {3,22,4,2,1,1,3,},
                {3,2,2,2,9,7,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,3,3,3,},
                {3,21,21,1,3,3,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,1,1,1,17,11,3,},
                {3,1,3,1,1,1,3,},
                {3,1,3,1,1,1,3,},
                {3,1,3,3,3,3,3,},
                {3,1,2,2,4,2,3,},
                {3,1,2,2,2,2,3,},
                {3,1,1,1,1,17,3,},
                {3,1,1,3,1,1,3,},
                {3,1,17,3,1,1,3,},
                {3,1,11,2,7,9,3,},
                {3,7,7,2,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,3,3,3,},
                {3,21,21,1,3,3,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,},
                {3,3,2,2,2,3,3,},
                {3,3,2,2,2,3,3,},
                {3,2,1,1,1,2,3,},
                {3,2,7,9,7,2,3,},
                {3,1,1,1,1,1,3,},
                {3,1,17,3,17,1,3,},
                {3,1,1,3,1,1,3,},
                {3,1,1,3,1,1,3,},
                {3,7,9,4,7,9,3,},
                {3,1,1,2,1,1,3,},
                {3,1,17,11,17,1,3,},
                {3,1,1,1,1,1,3,},
                {3,7,7,9,7,7,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,21,21,1,1,11,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×1
        new int[12, 18, 15]  //box 2×2
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,2,4,2,2,3,3,2,4,2,3,},
                {3,2,2,2,2,2,2,2,2,3,3,2,2,2,3,},
                {3,1,1,1,1,1,1,1,11,3,2,1,1,11,3,},
                {3,25,25,1,1,1,1,1,1,3,2,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,3,1,1,1,1,3,},
                {3,25,25,1,1,1,1,1,1,3,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,4,1,1,1,1,3,},
                {3,25,25,1,1,1,1,1,1,2,17,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,25,25,1,1,1,1,1,1,3,21,21,1,1,3,},
                {3,1,1,1,1,1,1,1,1,3,1,1,1,1,3,},
                {3,25,25,1,1,1,1,1,1,3,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,3,1,1,1,1,3,},
                {3,25,25,1,1,1,1,1,1,3,3,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,3,3,1,1,11,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,2,2,3,3,3,3,2,4,2,3,},
                {3,2,2,2,2,2,2,3,3,3,3,2,2,2,3,},
                {3,11,1,1,1,1,1,3,3,3,3,1,1,11,3,},
                {3,1,1,1,3,3,1,3,3,3,3,1,1,1,3,},
                {3,1,1,17,3,3,1,3,3,3,3,17,1,1,3,},
                {3,3,3,3,3,3,1,3,3,3,3,3,3,1,3,},
                {3,22,2,2,22,4,1,3,2,2,2,22,4,1,3,},
                {3,2,2,2,2,2,1,3,2,2,2,2,2,1,3,},
                {3,17,27,27,1,1,1,3,17,27,27,1,1,1,3,},
                {3,17,1,1,1,1,1,3,21,21,1,1,1,1,3,},
                {3,11,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,4,1,1,1,1,1,1,3,},
                {3,7,7,7,7,9,7,2,7,7,7,7,9,7,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,1,1,1,3,3,3,3,3,1,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,2,3,2,2,2,3,3,2,2,2,3,},
                {3,2,2,2,2,3,2,2,2,2,2,2,2,2,3,},
                {3,1,1,1,1,2,21,21,17,2,2,1,1,11,3,},
                {3,7,9,7,7,2,1,1,1,17,17,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,1,1,3,4,2,2,2,22,22,4,3,1,1,3,},
                {3,1,1,3,2,2,2,2,2,2,2,3,1,1,3,},
                {3,1,1,3,11,17,1,17,17,17,11,3,1,1,3,},
                {3,17,1,3,1,1,1,1,1,1,1,3,1,17,3,},
                {3,1,1,3,3,1,1,1,1,1,3,3,1,1,3,},
                {3,1,1,22,3,3,1,1,1,3,3,22,1,1,3,},
                {3,1,1,2,2,4,1,1,1,4,2,2,1,1,3,},
                {3,1,1,1,2,2,7,9,7,2,2,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,1,1,1,1,1,1,1,1,1,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,2,4,2,3,2,4,2,2,2,2,3,},
                {3,2,2,2,2,2,2,3,2,2,2,2,2,2,3,},
                {3,21,21,1,1,1,11,3,11,1,1,1,1,17,3,},
                {3,1,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,17,3,17,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,1,1,3,4,2,2,2,22,22,4,3,1,1,3,},
                {3,1,1,3,2,2,2,2,2,2,2,3,1,1,3,},
                {3,1,1,3,11,17,1,17,17,17,17,3,1,1,3,},
                {3,9,7,3,1,1,1,1,1,17,17,3,9,7,3,},
                {3,1,1,3,3,1,1,1,1,17,3,3,1,1,3,},
                {3,1,1,22,3,3,1,1,1,3,3,22,1,1,3,},
                {3,1,1,2,2,4,1,1,1,4,2,2,1,1,3,},
                {3,1,1,1,2,2,7,9,7,2,2,1,1,1,3,},
                {3,1,1,1,17,1,1,1,1,1,17,1,1,1,3,},
                {3,3,1,1,1,1,1,1,1,1,1,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,2,4,36,28,32,4,2,2,2,2,3,},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,17,11,3,},
                {3,1,3,3,3,3,3,3,1,1,1,1,1,1,3,},
                {3,1,2,2,4,22,2,3,1,1,1,1,1,1,3,},
                {3,1,2,2,2,2,2,3,17,1,1,1,1,1,3,},
                {3,1,1,1,1,1,11,3,17,17,17,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,3,3,3,2,4,2,3,2,4,2,3,1,1,3,},
                {3,3,3,3,2,2,2,3,2,2,2,3,1,1,3,},
                {3,2,2,3,21,21,17,3,17,17,11,3,1,1,3,},
                {3,2,2,2,1,1,1,4,1,1,1,2,1,1,3,},
                {3,1,1,2,7,9,7,2,7,9,7,2,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,4,2,2,2,2,4,2,2,2,2,4,3,3,},
                {3,22,2,2,2,2,2,2,2,2,2,2,2,22,3,},
                {3,2,1,1,1,1,1,1,1,1,1,1,1,2,3,},
                {3,1,1,1,3,3,3,3,3,3,3,1,1,1,3,},
                {3,1,1,1,2,4,2,3,2,4,2,1,1,1,3,},
                {3,17,1,1,2,2,2,3,2,2,2,1,1,17,3,},
                {3,17,17,1,1,1,11,3,11,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,3,3,3,2,2,2,4,2,2,2,3,1,1,3,},
                {3,3,3,3,2,2,2,2,2,2,2,3,1,1,3,},
                {3,2,2,3,21,21,1,1,17,17,11,3,1,1,3,},
                {3,2,2,4,1,1,1,1,1,1,1,4,1,1,3,},
                {3,1,1,2,7,7,7,9,7,7,7,2,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,1,1,1,1,1,1,1,1,1,1,1,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,2,3,3,3,3,3,2,2,2,3,3,},
                {3,4,2,2,2,2,3,3,3,2,2,2,2,4,3,},
                {3,2,1,1,17,2,2,4,2,2,17,1,1,2,3,},
                {3,1,1,1,1,1,2,2,2,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,3,3,3,3,3,3,3,3,3,3,3,1,3,},
                {3,1,2,2,4,2,2,22,2,2,4,2,2,1,3,},
                {3,1,2,2,2,2,2,2,2,2,2,2,2,1,3,},
                {3,1,1,11,17,1,1,1,1,1,17,11,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,1,3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,2,1,2,4,2,2,2,4,3,},
                {3,2,2,2,2,2,2,1,2,2,2,2,2,2,3,},
                {3,1,1,1,1,1,17,1,17,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,21,21,1,17,3,3,3,3,3,17,1,1,11,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,2,3,3,3,3,3,2,2,2,3,3,},
                {3,4,2,2,2,2,3,3,3,2,2,2,2,4,3,},
                {3,2,1,1,17,2,2,4,2,2,17,1,1,2,3,},
                {3,1,1,1,1,1,2,2,2,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,3,3,3,3,3,1,3,3,3,3,3,1,3,},
                {3,1,2,2,2,2,3,1,3,2,2,2,2,1,3,},
                {3,1,2,2,2,2,3,1,3,2,2,2,2,1,3,},
                {3,1,1,1,17,11,3,1,3,11,17,1,1,1,3,},
                {3,1,1,1,1,1,3,1,3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,1,3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,2,1,2,1,1,1,1,1,3,},
                {3,7,7,7,7,7,2,1,2,7,7,7,7,7,3,},
                {3,1,1,1,17,17,17,1,17,17,17,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,21,21,1,1,1,1,1,1,1,1,1,1,11,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,22,2,2,4,2,3,2,4,2,2,22,3,3,},
                {3,4,2,2,2,2,2,3,2,2,2,2,2,4,3,},
                {3,2,17,1,1,1,11,3,11,1,1,1,17,2,3,},
                {3,17,1,1,1,1,1,3,1,1,1,1,1,17,3,},
                {3,1,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,1,1,1,3,3,3,3,3,1,1,1,1,3,},
                {3,1,1,1,1,4,2,3,2,4,1,1,1,1,3,},
                {3,7,9,7,7,2,2,3,2,2,7,7,9,7,3,},
                {3,1,1,1,17,17,17,3,17,17,17,1,1,1,3,},
                {3,1,1,1,1,1,17,3,17,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,4,1,1,1,1,1,1,3,},
                {3,7,7,7,7,9,7,2,7,9,7,7,7,7,3,},
                {3,1,1,1,1,1,1,11,1,1,1,1,1,1,3,},
                {3,17,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,21,21,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,22,2,2,4,2,3,2,4,2,2,22,3,3,},
                {3,4,2,2,2,2,2,3,2,2,2,2,2,4,3,},
                {3,2,17,1,1,1,1,3,1,1,1,1,17,2,3,},
                {3,17,1,1,1,3,1,3,1,3,1,1,1,17,3,},
                {3,1,1,1,1,3,11,3,11,3,1,1,1,1,3,},
                {3,1,1,1,1,3,3,3,3,3,1,1,1,1,3,},
                {3,1,1,1,1,4,2,3,2,4,1,1,1,1,3,},
                {3,7,9,7,7,2,2,2,2,2,7,7,9,7,3,},
                {3,1,1,1,17,17,17,2,17,17,17,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,1,1,2,4,24,24,4,24,24,4,2,1,1,3,},
                {3,1,1,2,2,2,2,2,2,2,2,2,1,1,3,},
                {3,1,1,1,1,1,1,11,1,1,1,1,1,1,3,},
                {3,17,1,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,21,21,1,17,3,3,3,3,3,17,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {0,3,3,2,2,2,2,3,2,2,2,2,3,3,3,},
                {3,3,2,2,2,2,2,2,2,2,2,2,2,3,3,},
                {3,2,2,1,1,1,11,2,21,21,1,1,2,2,3,},
                {3,2,17,1,1,1,1,1,1,1,1,1,17,2,3,},
                {3,1,1,1,1,3,3,3,3,3,1,1,1,1,3,},
                {3,1,1,1,3,3,3,3,3,3,3,1,1,1,3,},
                {3,1,1,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,1,1,3,3,2,2,4,2,2,3,3,1,1,3,},
                {3,1,17,3,3,2,2,2,2,2,3,3,17,1,3,},
                {3,1,1,3,3,11,1,1,1,11,3,3,1,1,3,},
                {3,1,1,3,3,3,3,1,3,3,3,3,1,1,3,},
                {3,1,1,2,3,3,3,1,3,3,3,2,1,1,3,},
                {3,17,1,2,2,4,2,1,2,4,2,2,1,17,3,},
                {3,1,1,1,2,2,2,1,2,2,2,1,1,1,3,},
                {3,3,1,1,1,17,1,1,1,17,1,1,1,3,3,},
                {3,3,3,1,1,1,1,1,1,1,1,1,3,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,4,2,2,22,3,3,3,3,3,22,2,2,4,3,},
                {3,2,2,2,2,2,2,4,2,2,2,2,2,2,3,},
                {3,17,1,1,1,2,2,2,2,2,1,1,1,17,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,1,3,3,3,3,1,1,3,},
                {3,1,1,3,2,2,3,1,3,2,2,3,1,1,3,},
                {3,3,1,4,2,2,2,1,2,2,2,4,1,3,3,},
                {3,3,1,2,11,17,2,1,2,11,17,2,1,3,3,},
                {3,3,1,1,1,1,1,1,1,1,1,1,1,3,3,},
                {3,3,1,1,1,1,1,1,1,1,1,1,1,3,3,},
                {3,22,1,3,3,3,3,1,3,3,3,3,1,22,3,},
                {3,2,1,3,2,2,3,1,3,2,2,3,1,2,3,},
                {3,1,1,4,2,2,2,1,2,2,2,4,1,1,3,},
                {3,1,1,2,11,17,2,1,2,21,21,2,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,17,1,1,1,3,3,3,3,3,1,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
        },  //box 2×2
        new int[1, 18, 15]  //box 2×2 左上L
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,2,2,4,2,2,2,2,4,2,3,},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,},
                {3,21,21,1,17,17,17,1,1,1,1,1,1,17,3,},
                {3,1,1,1,1,1,1,1,3,3,3,3,1,1,3,},
                {3,1,1,1,1,1,1,1,2,4,22,2,1,1,3,},
                {3,1,1,1,1,1,3,1,2,2,2,2,11,1,3,},
                {3,1,1,1,1,1,3,1,1,1,1,1,1,1,3,},
                {3,7,7,9,7,7,3,3,3,3,3,3,3,3,3,},
                {3,11,1,1,1,17,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,3,1,1,1,3,3,0,0,0,0,0,0,0,0,},
                {3,4,1,1,1,4,3,0,0,0,0,0,0,0,0,},
                {3,2,7,9,7,2,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,17,1,1,1,17,3,0,0,0,0,0,0,0,0,},
                {3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,},
            }
            ,
        },  //box 2×2 左上L
        new int[1, 18, 15]  //box 2×2 右上L
        {
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,2,2,2,3,2,2,2,2,4,2,3,},
                {3,3,2,2,2,2,2,3,2,2,2,2,2,2,3,},
                {3,4,1,1,1,1,1,4,11,17,1,1,1,17,3,},
                {3,2,7,9,7,7,7,2,1,1,1,1,1,1,3,},
                {3,1,1,1,1,17,17,17,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,21,21,1,3,3,3,3,3,1,3,3,3,1,3,},
                {3,3,3,3,3,3,3,3,3,1,2,4,22,1,3,},
                {0,0,0,0,0,0,0,0,3,1,2,2,2,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,3,3,3,1,3,},
                {0,0,0,0,0,0,0,0,3,1,22,4,2,1,3,},
                {0,0,0,0,0,0,0,0,3,1,2,2,2,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,17,1,1,1,11,3,},
                {0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×2 右上L
        new int[1, 18, 15]  //box 2×2 左下L
        {
            {
                {3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,},
                {3,4,2,2,2,4,3,0,0,0,0,0,0,0,0,},
                {3,2,2,2,2,2,3,0,0,0,0,0,0,0,0,},
                {3,21,21,1,1,17,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,3,1,1,1,3,3,0,0,0,0,0,0,0,0,},
                {3,4,1,1,1,4,3,0,0,0,0,0,0,0,0,},
                {3,2,7,9,7,2,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,11,3,3,3,3,3,3,3,3,3,},
                {3,1,1,1,1,1,2,2,2,4,2,2,2,4,3,},
                {3,7,9,7,7,7,2,2,2,2,2,2,2,2,3,},
                {3,1,1,1,17,17,1,1,1,17,17,1,1,11,3,},
                {3,1,3,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,4,1,1,1,3,3,3,1,1,1,1,1,3,},
                {3,1,2,1,1,3,3,3,3,3,1,1,1,17,3,},
                {3,1,1,1,3,3,3,3,3,3,3,1,17,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
        },  //box 2×2 左下L
        new int[1, 18, 15]  //box 2×2 右下L
        {
            {
                {0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,},
                {0,0,0,0,0,0,0,0,3,4,2,2,2,4,3,},
                {0,0,0,0,0,0,0,0,3,2,2,2,2,2,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,7,7,9,7,7,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,3,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,4,1,1,3,},
                {0,0,0,0,0,0,0,0,3,17,1,2,1,11,3,},
                {3,3,3,3,3,3,3,3,3,3,1,1,1,3,3,},
                {3,2,2,2,2,2,2,2,2,3,1,1,1,2,3,},
                {3,2,2,2,2,2,2,2,2,3,7,9,7,2,3,},
                {3,21,21,1,1,1,1,17,17,4,1,1,1,17,3,},
                {3,1,1,1,1,3,1,1,1,2,1,1,1,1,3,},
                {3,1,1,1,1,4,1,1,1,1,1,1,1,1,3,},
                {3,1,1,1,1,2,1,1,1,3,1,1,1,1,3,},
                {3,11,1,1,1,1,1,17,17,3,1,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×2 右下L
        new int[1, 9, 23]  //box 1×3
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,4,2,2,2,2,2,4,2,2,2,2,2,2,2,4,2,2,2,2,2,4,3,},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,},
                {3,11,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,11,3,},
                {3,1,1,1,1,3,3,3,3,3,1,1,1,3,3,3,3,3,1,1,1,1,3,},
                {3,1,1,1,1,2,22,22,4,2,1,17,1,2,4,22,22,2,1,1,1,1,3,},
                {3,17,1,1,1,2,2,2,2,2,1,17,1,2,2,2,2,2,1,1,1,17,3,},
                {3,21,21,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 1×3
        new int[1, 27, 7]  //box 3×1
        {
            {
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,1,1,1,1,1,3,},
                {3,7,7,9,7,7,3,},
                {3,1,1,1,1,1,3,},
                {3,1,3,3,3,3,3,},
                {3,1,2,22,4,2,3,},
                {3,1,2,2,2,2,3,},
                {3,1,17,11,17,17,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,7,7,7,9,7,3,},
                {3,21,21,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,3,3,3,3,1,3,},
                {3,3,4,22,2,1,3,},
                {3,3,2,2,2,1,3,},
                {3,3,17,11,17,1,3,},
                {3,3,1,1,1,1,3,},
                {3,3,1,1,1,1,3,},
                {3,4,7,9,7,7,3,},
                {3,2,1,1,1,17,3,},
                {3,1,1,1,1,1,3,},
                {3,1,1,1,1,1,3,},
                {3,17,17,1,17,17,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
        },  //box 3×1
        new int[1, 18, 23]  //box 2×3
        {
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,2,22,2,4,2,22,2,2,2,22,2,4,2,22,2,2,2,3,3,},
                {3,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,3,},
                {3,2,1,1,1,1,17,17,17,1,1,1,1,1,17,17,17,1,1,1,11,2,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,},
                {3,1,1,3,3,2,2,2,3,3,2,2,2,2,2,2,2,2,3,3,1,1,3,},
                {3,1,1,3,4,2,2,2,3,2,2,2,2,2,2,2,2,2,4,3,1,1,3,},
                {3,1,1,3,2,11,1,11,3,2,17,17,17,17,17,17,17,17,2,3,1,1,3,},
                {3,17,1,3,1,1,1,1,4,17,21,21,1,1,1,1,1,1,1,3,1,17,3,},
                {3,17,1,3,1,1,1,1,2,1,1,1,1,17,17,17,17,1,1,3,1,17,3,},
                {3,1,1,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,1,1,3,},
                {3,1,1,3,3,3,3,3,3,17,1,1,1,17,3,3,3,3,3,3,1,1,3,},
                {3,1,1,2,2,2,4,2,2,1,1,1,1,1,2,2,4,2,2,2,1,1,3,},
                {3,1,1,2,2,2,2,2,2,7,7,9,7,7,2,2,2,2,2,2,1,1,3,},
                {3,17,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,17,3,},
                {3,17,11,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,17,17,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×3
        new int[1, 27, 15]  //box 3×2
        {
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,2,3,3,3,3,3,2,2,2,3,3,},
                {3,4,2,2,2,4,3,3,3,4,2,2,2,4,3,},
                {3,2,1,1,1,2,2,3,2,2,1,1,1,2,3,},
                {3,1,1,1,1,1,2,3,2,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,17,3,21,21,1,1,1,1,3,},
                {3,1,3,3,3,3,3,3,3,3,3,3,3,1,3,},
                {3,1,2,22,22,4,2,3,2,4,22,22,2,1,3,},
                {3,1,2,2,2,2,2,3,2,2,2,2,2,1,3,},
                {3,1,1,17,17,11,17,3,17,11,17,17,1,1,3,},
                {3,3,1,1,1,1,1,3,1,1,1,1,1,3,3,},
                {3,4,1,1,1,1,1,3,1,1,1,1,1,4,3,},
                {3,2,7,7,7,9,7,3,7,9,7,7,7,2,3,},
                {3,1,1,1,1,1,1,3,1,1,1,1,1,1,3,},
                {3,1,3,3,3,3,1,3,1,3,3,3,3,1,3,},
                {3,1,3,3,3,3,1,3,1,3,3,3,3,1,3,},
                {3,1,3,3,3,3,1,3,1,3,3,3,3,1,3,},
                {3,1,3,3,3,3,1,3,1,3,3,3,3,1,3,},
                {3,1,3,3,3,3,1,3,1,3,3,3,3,1,3,},
                {3,1,3,3,3,3,1,3,1,3,3,3,3,1,3,},
                {3,1,4,22,22,2,1,3,1,2,22,22,4,1,3,},
                {3,1,2,2,2,2,1,4,1,2,2,2,2,1,3,},
                {3,1,1,17,17,11,1,2,1,11,17,17,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 3×2
        new int[1, 27, 15]  //box 3×2 テト右
        {
            {
                {3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,},
                {3,4,2,2,2,4,3,0,0,0,0,0,0,0,0,},
                {3,2,2,2,2,2,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,1,1,11,1,1,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,3,1,1,1,3,3,0,0,0,0,0,0,0,0,},
                {3,4,1,1,1,4,3,0,0,0,0,0,0,0,0,},
                {3,2,7,9,7,2,3,0,0,0,0,0,0,0,0,},
                {3,17,1,1,1,17,3,3,3,3,3,3,3,3,3,},
                {3,1,1,1,1,1,3,3,3,3,2,2,4,3,3,},
                {3,1,1,3,1,1,3,3,3,2,2,2,2,2,3,},
                {3,1,1,3,1,1,22,4,22,2,17,1,1,2,3,},
                {3,1,1,3,1,1,2,2,2,21,21,1,1,11,3,},
                {3,1,1,3,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,4,1,1,3,3,3,1,1,1,1,17,3,},
                {3,1,1,2,1,1,3,3,3,3,1,1,1,3,3,},
                {3,1,1,1,1,1,3,3,3,3,3,3,3,3,3,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,3,1,1,1,3,3,0,0,0,0,0,0,0,0,},
                {3,4,1,1,1,4,3,0,0,0,0,0,0,0,0,},
                {3,2,7,9,7,2,3,0,0,0,0,0,0,0,0,},
                {3,17,1,1,1,17,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,1,1,11,1,1,3,0,0,0,0,0,0,0,0,},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,},
                {3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,},
            }
            ,
        },  //box 3×2 テト凸右
        new int[1, 27, 15]  //box 3×2 テト左
        {
            {
                {0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,},
                {0,0,0,0,0,0,0,0,3,4,2,2,2,4,3,},
                {0,0,0,0,0,0,0,0,3,2,2,2,2,2,3,},
                {0,0,0,0,0,0,0,0,3,21,21,1,1,17,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,3,3,1,3,3,3,},
                {0,0,0,0,0,0,0,0,3,4,2,1,2,4,3,},
                {0,0,0,0,0,0,0,0,3,2,2,1,2,2,3,},
                {3,3,3,3,3,3,3,3,3,17,1,1,1,11,3,},
                {3,4,2,2,2,2,4,2,2,1,1,1,1,1,3,},
                {3,2,2,2,2,2,2,2,2,7,7,9,7,7,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,1,1,3,3,3,3,3,3,3,1,1,1,1,3,},
                {3,1,1,4,2,22,22,22,2,4,1,1,1,1,3,},
                {3,1,11,2,2,2,2,2,2,2,1,1,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,1,3,},
                {0,0,0,0,0,0,0,0,3,2,4,2,2,1,3,},
                {0,0,0,0,0,0,0,0,3,2,2,2,2,1,3,},
                {0,0,0,0,0,0,0,0,3,17,11,17,17,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,3,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,4,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,2,1,1,3,},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 3×2 テト凸左
        new int[1, 18, 23]  //box 2×3 テト上
        {
            {
                {0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,4,2,2,2,4,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,2,2,2,2,2,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,17,1,1,1,11,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,1,3,1,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,1,4,1,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,1,2,1,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,17,1,1,1,17,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,17,17,1,17,17,3,0,0,0,0,0,0,0,0},
                {3,3,3,3,3,3,3,3,3,3,3,1,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,2,2,3,4,22,2,1,2,22,4,3,2,2,2,2,4,2,3,},
                {3,2,2,2,2,2,2,3,2,2,2,1,2,2,2,3,2,2,2,2,2,2,3,},
                {3,21,21,1,1,17,17,3,1,1,1,1,1,1,1,3,17,17,1,1,1,17,3,},
                {3,1,1,1,1,1,11,2,1,1,1,1,1,1,1,2,11,1,1,1,1,1,3,},
                {3,1,1,17,17,1,1,2,7,7,7,9,7,7,7,2,1,1,17,17,1,1,3,},
                {3,3,1,1,1,1,1,17,17,17,1,1,1,17,17,17,1,1,1,1,1,3,3,},
                {3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×3 テト凸上
        new int[1, 18, 23]  //box 2×3 テト下
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,2,2,2,3,2,2,2,2,2,4,2,2,2,3,2,2,4,3,},
                {3,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,3,2,2,2,3,},
                {3,11,1,1,1,1,1,17,3,17,1,1,1,1,1,1,17,17,3,1,1,11,3,},
                {3,1,1,3,3,3,1,1,3,1,1,3,1,1,3,1,1,1,3,1,1,1,3,},
                {3,1,1,22,4,22,1,1,4,1,1,3,1,1,3,1,1,1,4,1,1,1,3,},
                {3,1,1,2,2,2,1,1,2,1,1,3,1,17,3,7,9,7,2,1,1,1,3,},
                {3,21,21,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,1,1,3,1,1,3,3,3,3,3,3,3,3,3,},
                {0,0,0,0,0,0,0,0,3,1,1,4,17,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,7,7,2,1,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,11,1,1,1,17,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,3,3,3,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,22,4,22,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,2,2,2,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0},
            }
            ,
        },  //box 2×3 テト凸下
        new int[1, 18, 23]  //box 2×3 テトL右上
        {
            {
                {0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,2,2,2,3,3,3,3,3,2,2,2,3,3,4,3,3,2,2,2,3,3,},
                {3,2,2,2,2,4,2,22,2,4,2,2,2,3,2,2,2,3,2,2,2,4,3,},
                {3,2,1,1,1,2,2,2,2,2,1,1,1,2,2,11,2,2,1,1,17,2,3,},
                {3,11,1,3,1,1,1,1,1,1,1,3,1,2,1,1,1,2,1,1,1,17,3,},
                {3,1,1,4,1,1,1,3,1,1,1,4,1,1,1,3,1,1,1,1,1,1,3,},
                {3,17,1,2,1,1,3,3,3,1,1,2,1,1,3,3,3,3,3,3,1,1,3,},
                {3,3,1,1,1,3,3,3,3,3,1,1,1,3,3,3,3,3,3,3,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,22,4,2,9,7,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,1,1,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,17,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,17,1,3,3,3,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,11,1,2,4,22,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,2,2,2,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,21,21,1,1,17,3,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×3 テトL右上
        new int[1, 18, 23]  //box 2×3 テトL左上
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,4,2,2,22,2,2,4,2,3,2,3,3,3,3,3,3,3,2,3,3,3,},
                {3,2,2,2,2,2,2,2,2,2,3,2,3,3,3,3,3,3,3,2,22,4,3,},
                {3,1,1,1,1,1,1,1,1,1,3,1,2,4,2,22,2,4,2,1,2,2,3,},
                {3,1,3,3,3,3,3,3,3,1,22,1,2,2,2,2,2,2,2,1,1,1,3,},
                {3,1,2,4,22,2,3,3,3,1,2,1,1,1,1,1,1,1,1,1,11,1,3,},
                {3,1,2,2,2,2,3,3,3,1,11,1,3,3,3,3,3,3,3,1,1,1,3,},
                {3,1,1,1,1,1,3,3,3,1,1,1,3,3,3,3,3,3,3,1,3,3,3,},
                {3,3,1,3,3,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,3,1,3,3,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,3,11,3,3,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,3,3,3,3,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,22,4,22,2,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,2,2,2,2,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,21,21,1,1,17,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,3,3,1,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            }
            ,
        },  //box 2×3 テトL左上
        new int[1, 27, 23]  //box 3×3
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3},
                {3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3},
                {3,21,21,1,11,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,21,21,1,11,1,11,1,11,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
            }
        },  //box 3×3

    }; //4:ライト 6:壁家具(設置済み) 7:檻　9:扉 11:ボックス 13:出口 15:家具(設置済み) 17:家具なんでも(設置前) 21:トークキャラ 
       //23～?奇数:家具種類確定(設置前) //22:壁家具なんでも(設置前)  //24～?偶数:壁家具種類確定(設置前)
    const int WALL_DECORATION = 6;
    const int TILE_CAGE = 7;
    const int TILE_DOOR = 9;
    const int TILE_BOX = 11;
    const int TILE_EXIT = 13;
    const int TILE_DECORATION = 15;
    const int TILE_DECORATION_RANDOM = 17;
    const int TILE_OBJECT = 19;
    const int TILE_TALK = 21;
    const int TILE_DECORATION_DECIDED_START = 23;
    const int WALL_DECORATION_RANDOM = 22;
    const int WALL_DECORATION_DECIDED_START = 24;

    private int[,] tutorial_box_map = new int[BOX_MAP_SIZE, BOX_MAP_SIZE]
    {
        {3,0,0,0,0},
        {3,0,0,0,0},
        {3,3,3,1,4},
        {0,0,0,0,0},
        {0,0,0,0,0},
    };
    public List<int[,,]> tutorial_box_list = new List<int[,,]>()
    {
        new int[1, 9, 7]  //box 1×1
        {
            {
                {0,3,3,3,3,3,3,},
                {3,3,3,3,3,3,3,},
                {3,3,3,3,3,3,3,},
                {3,4,2,2,2,4,3,},
                {3,2,2,2,2,2,3,},
                {3,1,1,1,1,11,3,},
                {3,3,3,3,3,3,3,},
                {3,3,3,3,3,3,3,},
                {3,3,3,3,3,3,3,},
            }
            ,
        },  //box 1×1
        new int[1, 18, 15]  //box 2×2 左下L
        {
            {
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,2,2,3,3,3,3,3,3,3,3,3,},
                {3,2,2,2,2,2,3,3,3,3,3,3,3,3,3,},
                {3,1,1,1,1,1,3,3,3,3,3,3,3,3,3,},
                {3,7,7,9,7,7,3,3,3,3,3,3,3,3,3,},
                {3,1,1,1,1,1,3,3,3,3,3,3,3,3,3,},
                {3,1,1,1,1,1,3,3,3,3,3,3,3,3,3,},
                {3,17,1,3,1,17,3,3,3,3,3,3,3,3,3,},
                {3,17,1,4,1,17,3,3,3,3,3,3,3,3,3,},
                {3,1,1,2,1,1,3,3,3,3,3,3,3,3,3,},
                {3,1,1,11,1,1,3,3,3,2,2,2,2,2,3,},
                {3,1,1,1,1,1,3,3,3,2,2,2,2,2,3,},
                {3,1,1,1,1,1,2,4,2,17,1,1,1,17,3,},
                {3,7,7,9,7,7,2,2,2,1,1,3,1,1,3,},
                {3,1,1,1,1,1,17,1,17,1,1,4,1,1,3,},
                {3,1,1,1,1,1,1,1,1,1,1,2,1,1,3,},
                {3,3,1,1,1,1,1,1,1,1,1,1,1,1,3,},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,},
            }
            ,
        },  //box 2×2 左下L
    };

    public Dictionary<int, GameObject> active_map_obj = new Dictionary<int, GameObject>();

    public Dictionary<int, Vector2Int> active_box_map_pos = new Dictionary<int, Vector2Int>();
    public Dictionary<int, Vector2Int> active_box_size = new Dictionary<int, Vector2Int>();
    public Dictionary<int, List<int[]>> active_box_move = new Dictionary<int, List<int[]>>(); //[0]x [1]y [2]dir_x [3]dir_y

    private bool set_orb = false;
    private int first_room = 3;
    private Vector2Int first_pos;

    //AdjustStageUI
    [SerializeField] GameObject stageUI_items_buttons;
    [SerializeField] GameObject stageUI_items_text;
    [SerializeField] GameObject stageUI_left;
    [SerializeField] GameObject stageUI_action_buttons;
    [SerializeField] GameObject stageUI_move_buttons;

    public bool tutorial;

    private void Awake()
    {
        if(Master.start_from_this == true)
        {
            if(load == 0)
            {
                load = 1;
                SceneManager.LoadScene("LoadDataScene");
            }
        }

        SetTalkFirst();
    }

    void Start()
    {
        if (stage_create_test == false)
        {
            Data.start_text.text = "衛門の間 " + (14 - Status.stage_layer) + "F";
            int[] thih_map_list;
            if (tutorial == false)
            {
                thih_map_list = map_list[Status.stage_layer];
            }
            else
            {
                thih_map_list = map_list[0];
            }
            Minimap.minimap_roomnums = new int[thih_map_list[0] * 3, thih_map_list[1] * 3];

            //create map
            bool[,] set_hypo_map = new bool[thih_map_list[0], thih_map_list[1]];
            set_hypo_map[Random.Range(0, thih_map_list[0]), Random.Range(0, thih_map_list[1])] = true;
            for (int i = 0; i < thih_map_list[2]; i++)
            {
                List<Vector2Int> able_pos = new List<Vector2Int>();
                for (int y = 0; y < thih_map_list[0]; y++)
                {
                    for (int x = 0; x < thih_map_list[1]; x++)
                    {
                        if (set_hypo_map[y, x] == false)
                        {
                            bool able = false;
                            int blocks = 0;
                            for (int d = 0; d < 4; d++)
                            {
                                int searchX = x + four_dir[d, 0];
                                int searchY = y + four_dir[d, 1];

                                if ((0 <= searchX & searchX < thih_map_list[1]) & (0 <= searchY & searchY < thih_map_list[0]))
                                {
                                    if (set_hypo_map[searchY, searchX] == true)
                                    {
                                        able = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    blocks += 1;
                                }
                            }

                            if (able == true | blocks == 4)
                            {
                                able_pos.Add(new Vector2Int(x, y));
                            }
                        }
                    }
                }
                Vector2Int pos = able_pos[Random.Range(0, able_pos.Count)];
                set_hypo_map[pos.y, pos.x] = true;
                Debug.Log(i);
            } //仮マップの生成
            
            //search box
            Dictionary<(int, int, bool), List<int[]>> move_info = new Dictionary<(int, int, bool), List<int[]>>(); //key: pos, pos, horizontal
            int y_range = 0;
            for (int map_y = thih_map_list[0] - 1; map_y >= 0; map_y--)
            {
                int x_range = 0;
                for (int map_x = thih_map_list[1] - 1; map_x >= 0; map_x--)
                {
                    int range = thih_map_list[1] * 10 * y_range + 10 * x_range;
                    if (set_hypo_map[map_y, map_x] == true)
                    {
                        List<int> dir = new List<int>();
                        for (int d = 0; d < 4; d++)
                        {
                            int searchX = map_x + four_dir[d, 0];
                            int searchY = map_y + four_dir[d, 1];

                            if ((0 <= searchX & searchX < thih_map_list[1]) & (0 <= searchY & searchY < thih_map_list[0]))
                            {
                                if (set_hypo_map[searchY, searchX] == true)
                                {
                                    dir.Add(dir_range[d]);
                                }
                            }
                        }
                        int sum_dir = Sum(dir);
                        int[,] this_box_map;
                        if(tutorial == true)
                        {
                            this_box_map = tutorial_box_map;
                        }
                        else
                        {
                            if(map_test == false)
                            {
                                this_box_map = box_map[sum_dir][Random.Range(0, box_map[sum_dir].Count)];
                            }
                            else
                            {
                                this_box_map = box_map[sum_dir][0];
                            }
                        }

                        int map_pos_y = thih_map_list[0] - map_y - 1;

                        if (dir.Contains(1))
                        {
                            move_info[(map_y * 100 + map_x, map_y * 100 + map_x + 1, true)].Add
                                (new int[5] { this_box_map[0, 4] + range, this_box_map[2, 4] + range, this_box_map[4, 4] + range, map_x, map_y });
                        }
                        if (dir.Contains(2))
                        {
                            move_info[(map_y * 100 + map_x, (map_y + 1) * 100 + map_x, false)].Add
                                (new int[5] { this_box_map[4, 0] + range, this_box_map[4, 2] + range, this_box_map[4, 4] + range, map_x, map_y });
                        }
                        if (dir.Contains(4))
                        {
                            move_info[(map_y * 100 + map_x - 1, map_y * 100 + map_x, true)] = new List<int[]>()
                        {
                            new int[5] { this_box_map[0, 0] + range, this_box_map[2, 0] + range, this_box_map[4, 0] + range, map_x, map_y }
                        };
                        }
                        if (dir.Contains(8))
                        {
                            move_info[((map_y - 1) * 100 + map_x, map_y * 100 + map_x, false)] = new List<int[]>()
                        {
                            new int[5] {this_box_map[0, 0] + range, this_box_map[0, 2] + range, this_box_map[0, 4] + range, map_x, map_y }
                        };
                        }

                        SearchBox(this_box_map, map_x, map_y, range, thih_map_list[0]);
                    }
                    x_range += 1;
                }

                y_range += 1;
            }

            foreach ((int pos1, int pos2, bool hol) in move_info.Keys)
            {
                int[] pos1_boxes = move_info[(pos1, pos2, hol)][1];
                int[] pos2_boxes = move_info[(pos1, pos2, hol)][0];

                List<int> move_num = new List<int>();
                for (int i = 0; i < 3; i++) //つなぐ部屋があるかどうかの確認
                {
                    if (pos1_boxes[i] % 10 != 0 & pos2_boxes[i] % 10 != 0)
                    {
                        move_num.Add(i);
                    }
                }

                int move_num_range = Random.Range(0, move_num.Count);
                for (int num = 0; num < move_num_range; num++)
                {
                    int remove_num = Random.Range(0, move_num.Count);
                    move_num.RemoveAt(remove_num);
                }

                int posY = pos2 / 100;
                int posX = pos2 - posY * 100;

                foreach (int num in move_num)
                {
                    int room1 = 0;
                    int room1_x;
                    int room1_y;
                    int room2 = 0;
                    int room2_x;
                    int room2_y;
                    float movespace_x = 0;
                    float movespace_y = 0;
                    if (hol == true)
                    {
                        (room1, room1_x, room1_y) = SearchBoxMove(5, num * 2, -1, 0, new int[0, 0], 0, pos1_boxes[num], pos1_boxes[3], pos1_boxes[4]);
                        (room2, room2_x, room2_y) = SearchBoxMove(-1, num * 2, 1, 0, new int[0, 0], 0, pos2_boxes[num], pos2_boxes[3], pos2_boxes[4]);
                        movespace_x = -25f + posX * 600f + 12.5f;
                        movespace_y = ((9 * ((4 - num * 2) / 2)) + 3) * 25f + (thih_map_list[0] - posY - 1) * 675f + 12.5f;
                    }
                    else
                    {
                        (room1, room1_x, room1_y) = SearchBoxMove(num * 2, 5, 0, -1, new int[0, 0], 0, pos1_boxes[num], pos1_boxes[3], pos1_boxes[4]);
                        (room2, room2_x, room2_y) = SearchBoxMove(num * 2, -1, 0, 1, new int[0, 0], 0, pos2_boxes[num], pos2_boxes[3], pos2_boxes[4]);
                        movespace_x = (3 + 8 * ((num * 2) / 2)) * 25f + posX * 600f + 12.5f;
                        movespace_y = -50f + (thih_map_list[0] - posY) * 675f + 12.5f;
                    }

                    GameObject movespace = Instantiate(map_movespace_resources);
                    Move.MoveLocalPos(movespace, movespace_x, movespace_y);
                    MoveroomConfig moveroomC = movespace.GetComponent<MoveroomConfig>();
                    moveroomC.toRoom = new int[2] { room1, room2 };
                    SetRoomInfo(room1, room1_x, room1_y, room2, room2_x, room2_y);
                    movespace.transform.parent = map_all_obj.transform;
                }
            }

            CreateBox();

            if(tutorial == false)
            {
                SetEnemies();
            }

            Master.t_nav = 3;

            AdjustStageUI();

            Destroy(this);
        }
        else
        {
            set_orb = true;
        }
    }

    private void SearchBox(int[,] this_box_map, int map_x, int map_y, int range, int max_map_y)
    {
        List<int> searched = new List<int>();
        for (int box_map_y = 0; box_map_y < 5; box_map_y++)
        {
            for (int box_map_x = 0; box_map_x < 5; box_map_x++)
            {
                int room_num = this_box_map[box_map_y, box_map_x];

                if (room_num >= 3)
                {
                    if (!searched.Contains(room_num))
                    {
                        int box_kind = SearchBoxKind(box_map_y, this_box_map, room_num, map_x, map_y, range, max_map_y);
                        int[,] box = SearchBox(box_kind, room_num + range);
                        Data.active_box_map[room_num + range] = box;
                        searched.Add(room_num);
                    }

                    Minimap.minimap_roomnums[(box_map_y / 2) + map_y * 3, (box_map_x / 2) + map_x * 3] = room_num + range;
                }
                else if(room_num > 0)
                {
                    int room1;
                    int room1_x;
                    int room1_y;
                    int room2;
                    int room2_x;
                    int room2_y;
                    float movespace_x;
                    float movespace_y;

                    if (room_num == 2)
                    {
                        (room1, room1_x, room1_y) = SearchBoxMove(box_map_x, box_map_y, 0, -1, this_box_map, range, -1, map_x, map_y);
                        (room2, room2_x, room2_y) = SearchBoxMove(box_map_x, box_map_y, 0, 1, this_box_map, range, -1, map_x, map_y);
                        movespace_x = (3 + 8 * (box_map_x / 2)) * 25f + map_x * 600f + 12.5f;
                        movespace_y = ((9 * ((5 - box_map_y) / 2)) - 2) * 25f + (max_map_y - map_y - 1) * 675f + 12.5f;
                    }
                    else
                    {
                        (room1, room1_x, room1_y) = SearchBoxMove(box_map_x, box_map_y, -1, 0, this_box_map, range, -1, map_x, map_y);
                        (room2, room2_x, room2_y) = SearchBoxMove(box_map_x, box_map_y, 1, 0, this_box_map, range, -1, map_x, map_y);
                        movespace_x = (7 + 8 * ((box_map_x) / 2)) * 25f + map_x * 600f + 12.5f;
                        movespace_y = (3 + 9 * ((4 - box_map_y) / 2)) * 25f + (max_map_y - map_y - 1) * 675f + 12.5f;
                    }

                    GameObject movespace = Instantiate(map_movespace_resources);
                    Move.MoveLocalPos(movespace, movespace_x, movespace_y);
                    MoveroomConfig moveroomC = movespace.GetComponent<MoveroomConfig>();
                    moveroomC.toRoom = new int[2] { room1, room2 };
                    SetRoomInfo(room1, room1_x, room1_y, room2, room2_x, room2_y);
                    movespace.transform.parent = map_all_obj.transform;
                }
            }
        }
    }
    private (int, int, int) SearchBoxMove(int x, int y, int dirX, int dirY, int[,] map, int range, int num, int map_x, int map_y)
    {
        int searchX = x + dirX;
        int searchY = y + dirY;
        int this_room_num;
        if (num == -1)
        {
            this_room_num = map[searchY, searchX] + range;
        }
        else
        {
            this_room_num = num;
        }
        if (!active_box_move.ContainsKey(this_room_num))
        {
            active_box_move[this_room_num] = new List<int[]> { new int[4] { searchX, searchY, -dirX, -dirY } };
        }
        else
        {
            active_box_move[this_room_num].Add(new int[4] { searchX, searchY, -dirX, -dirY });
        }

        return (this_room_num, (searchX / 2) + map_x * 3, (searchY / 2) + map_y * 3);
    }
    private void CreateBox()
    {
        List<int> rooms_num = new List<int>();

        foreach (int key in Data.active_box_map.Keys)
        {
            rooms_num.Add(key);
        }

        int exit_room;
        if (tutorial == false)
        {
            first_room = rooms_num[Random.Range(0, rooms_num.Count)];
            exit_room = rooms_num[Random.Range(0, rooms_num.Count)];
        }
        else
        {
            first_room = rooms_num[0];
            exit_room = rooms_num[1];
        }
        int[,] box = Data.active_box_map[exit_room];
        int maxX = box.GetLength(1);
        int maxY = box.GetLength(0);

        Vector2Int exit_room_pos;
        if (tutorial == false)
        {
            List<Vector2Int> exit_poses = new List<Vector2Int>();
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (box[y, x] == 1)
                    {
                        int count = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            int searchX = x + eight_dir[i, 0];
                            int searchY = y + eight_dir[i, 1];

                            int searchnum = box[searchY, searchX];
                            if (searchnum != 7 & searchnum != 9 & searchnum != 11 & searchnum != 21)
                            {
                                if (i != 2)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (searchnum != TILE_DECORATION)
                                    {
                                        count += 1;
                                    }
                                }
                            }
                        }

                        if (y < maxY - 2)
                        {
                            int searchnum = box[y + 2, x];
                            if (searchnum != 7 & searchnum != 9)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count += 1;
                        }

                        if (count == 9)
                        {
                            exit_poses.Add(new Vector2Int(x, y));
                        }
                    }
                }
            } //設置できるか判定
            exit_room_pos = exit_poses[Random.Range(0, exit_poses.Count)];
        }
        else
        {
            exit_room_pos = new Vector2Int(3, 5);
        }
        Data.active_box_map[exit_room][exit_room_pos.y, exit_room_pos.x] = 13;

        foreach (int key in rooms_num)
        {
            SetRoom(key);
        }

        if (!Status.rom_get_layer.Contains(Status.stage_layer))
        {
            set_orb = true;
        }

        //item
        int max_item = items_count[Status.stage_layer];
        if(set_orb == false)
        {
            max_item += 1;
        }
        if (items_done[Status.stage_layer] != 100)
        {
            max_item += 1;
            item_done_bool = true;
        }

        if(tutorial == true)
        {
            max_item = 2;
            set_orb = false;
            item_done_bool = false;
        }

        if (max_item != 0)
        {
            if(tutorial == false)
            {
                for (int i = 0; i < max_item; i++)
                {
                    int item_canplace_num = Random.Range(0, item_canplace.Count);
                    (int x, int y, int mymaxY, GameObject parent, int key) = item_canplace[item_canplace_num];
                    SetObj(x, y, mymaxY, 19, parent, null, null, key);

                    item_canplace.RemoveAt(item_canplace_num);
                }
            }
            else
            {
                (int x, int y, int mymaxY, GameObject parent, int key) = item_canplace[0];
                Debug.Log(key);
                if(key == 3)
                {
                    SetObj(x, y, mymaxY, 19, parent, null, null, key);
                    (x, y, mymaxY, parent, key) = item_canplace[1];
                    SetObj(x, y, mymaxY, 19, parent, null, null, key);
                }
                else
                {
                    (x, y, mymaxY, parent, key) = item_canplace[1];
                    SetObj(x, y, mymaxY, 19, parent, null, null, key);
                    (x, y, mymaxY, parent, key) = item_canplace[0];
                    SetObj(x, y, mymaxY, 19, parent, null, null, key);
                }
                item_canplace.RemoveAt(0);
                item_canplace.RemoveAt(0);
            }
        }

        if(tutorial == false)
        {
            //talk
            for (int i = 0; i < talk_count[Status.stage_layer]; i++)
            {
                int kind = talk_kind[Status.stage_layer] - 1; //0:solo 1: duo

                if (kind == 0)
                {
                    (List<List<(int, int, int, string)>> list, int chara, bool flip) = Status.random_message_info_1to1[Status.message_1to1_num];
                    int talk_canplace_num = Random.Range(0, talk_canplace.Count);
                    (int x, int y, int mymaxY, GameObject parent, int key) = talk_canplace[talk_canplace_num];
                    if (talk_test == true)
                    {
                        if (i == 0)
                        {
                            for (int t = 0; t < talk_canplace.Count; t++)
                            {
                                (x, y, mymaxY, parent, key) = talk_canplace[t];
                                Debug.Log("a");
                                if (key == first_room)
                                {
                                    Debug.Log("b");
                                    talk_canplace_num = t;
                                    break;
                                }
                            }
                        }
                    }

                    GameObject obj = Instantiate(obj_chara_1to1);
                    obj.transform.parent = parent.transform;
                    float posX = x * 25f + 12.5f;
                    float posY = (mymaxY - y - 1) * 25f + 12.5f;
                    Move.MoveLocalPosZ(obj, posX, posY, posY / 10f - 0.5f);
                    obj.SetActive(true);

                    GameObject chara_obj = obj.transform.GetChild(0).gameObject;
                    CharaAnimationScript chara_anim = chara_obj.GetComponent<CharaAnimationScript>();
                    Move.MoveLocalPosY(chara_obj, talk_pos[chara]);
                    Sprite[] chara_sprite = talk_sp[chara];
                    chara_anim.speed = talk_speed[chara];
                    for (int c = 0; c < chara_sprite.Length / 2; c++)
                    {
                        chara_anim.nomal_chara.Add(chara_sprite[c]);
                    }
                    for (int c = chara_sprite.Length / 2; c < chara_sprite.Length; c++)
                    {
                        chara_anim.action_chara.Add(chara_sprite[c]);
                    }
                    if (Random.Range(0, 2) == 0)
                    {
                        chara_anim.sr.flipX = true;
                    }

                    ActionTalkConfig chara_talk = obj.transform.GetChild(1).GetComponent<ActionTalkConfig>();
                    chara_talk.talk_info = list;
                    chara_talk.flip = flip;

                    Status.message_1to1_num += 1;
                    talk_canplace.RemoveAt(talk_canplace_num);

                    Data.active_box_map[key][y, x + 1] = 1;
                }
                else if (kind == 1)
                {
                    (List<List<(int, int, int, string)>> list, int chara1, int chara2, bool flip) = Status.random_message_info_1to2[Status.message_1to2_num];
                    int talk_canplace_num = Random.Range(0, talk_canplace.Count);
                    (int x, int y, int mymaxY, GameObject parent, int key) = talk_canplace[talk_canplace_num];

                    GameObject obj = Instantiate(obj_chara_1to2);
                    obj.transform.parent = parent.transform;
                    float posX = x * 25f + 25f;
                    float posY = (mymaxY - y - 1) * 25f + 12.5f;
                    Move.MoveLocalPosZ(obj, posX, posY, posY / 10f - 0.5f);
                    obj.SetActive(true);

                    GameObject chara1_obj = obj.transform.GetChild(0).gameObject;
                    CharaAnimationScript chara1_anim = chara1_obj.GetComponent<CharaAnimationScript>();
                    Move.MoveLocalPosY(chara1_obj, talk_pos[chara1]);
                    Sprite[] chara1_sprite = talk_sp[chara1];
                    chara1_anim.speed = talk_speed[chara1];
                    for (int c = 0; c < chara1_sprite.Length / 2; c++)
                    {
                        chara1_anim.nomal_chara.Add(chara1_sprite[c]);
                    }
                    for (int c = chara1_sprite.Length / 2; c < chara1_sprite.Length; c++)
                    {
                        chara1_anim.action_chara.Add(chara1_sprite[c]);
                    }

                    GameObject chara2_obj = obj.transform.GetChild(1).gameObject;
                    CharaAnimationScript chara2_anim = chara2_obj.GetComponent<CharaAnimationScript>();
                    Move.MoveLocalPosY(chara2_obj, talk_pos[chara2]);
                    Sprite[] chara2_sprite = talk_sp[chara2];
                    chara2_anim.speed = talk_speed[chara2];
                    for (int c = 0; c < chara2_sprite.Length / 2; c++)
                    {
                        chara2_anim.nomal_chara.Add(chara2_sprite[c]);
                    }
                    for (int c = chara2_sprite.Length / 2; c < chara2_sprite.Length; c++)
                    {
                        chara2_anim.action_chara.Add(chara2_sprite[c]);
                    }

                    ActionTalkConfig chara_talk = obj.transform.GetChild(2).GetComponent<ActionTalkConfig>();
                    chara_talk.talk_info = list;
                    chara_talk.flip = flip;

                    Status.message_1to2_num += 1;
                    talk_canplace.RemoveAt(talk_canplace_num);
                }
            }
        }

        //boss
        if (Status.boss_layer.Contains(Status.stage_layer) | tutorial == true)
        {
            int boss_index = Status.boss_message_num;
            if(tutorial == true)
            {
                boss_index = 3;
            }

            (List<List<(int, int, int, string)>> list, int chara, bool flip) = Status.boss_message_info[boss_index];

            GameObject obj = Instantiate(obj_chara_boss[boss_index]);
            obj.transform.parent = active_map_obj[exit_room].transform;
            float posX = exit_room_pos.x * 25f + 12.5f;
            float posY = (maxY - exit_room_pos.y - 1) * 25f + 12.5f;
            Move.MoveLocalPosZ(obj, posX, posY, posY / 10f - 0.5f);
            obj.SetActive(true);

            GameObject chara_obj = obj.transform.GetChild(0).gameObject;
            Move.MoveLocalPosY(chara_obj, talk_pos[0]);

            ActionTalkConfig chara_talk = obj.transform.GetChild(1).GetComponent<ActionTalkConfig>();
            chara_talk.talk_info = list;
            chara_talk.flip = flip;

            if(tutorial == false)
            {
                Status.boss_message_num += 1;
            }
        }

        for (int i = 0; i < item_canplace.Count; i++)
        {
            (int x, int y, int mymaxY, GameObject parent, int key) = item_canplace[i];
            SetDecoration(x * 25f + 12.5f, (mymaxY - y - 1) * 25f + 25f, obj_decoration_1to1, parent, obj_decoration_1to1_sprite);
        }
        for (int i = 0; i < talk_canplace.Count; i++)
        {
            (int x, int y, int mymaxY, GameObject parent, int key) = talk_canplace[i];
            SetDecoration(x * 25f + 12.5f, (mymaxY - y - 1) * 25f + 25f, obj_decoration_1to1, parent, obj_decoration_1to1_sprite);
            Data.active_box_map[key][y, x + 1] = 1;
        }

        foreach (int key in rooms_num)
        {
            GameObject area = Data.rooms[key].transform.GetChild(4).gameObject;
            SetAreas(key, area);
            area.transform.parent = Data.all_area.transform;
        }
    }
    private void SetEnemies()
    {
        //かぶらせないためにいったんキャラの位置変更
        Data.active_box_map[first_room][first_pos.y, first_pos.x] = -1;

        List<int> room_nums = new List<int>();
        foreach (int key in Data.active_box_map.Keys)
        {
            room_nums.Add(key);
        }

        List<int> enemies_ids = new List<int>();
        foreach (KeyValuePair<int, int> kvp in StageEnemy[Status.stage_layer])
        {
            for(int i=0; i<kvp.Value; i++)
            {
                enemies_ids.Add(kvp.Key);
            }
        }

        int enemies_count = enemies_ids.Count;
        for(int i=0; i <  enemies_count; i++)
        {
            if(room_nums.Count != 0)
            {
                int room = room_nums[Random.Range(0, room_nums.Count)];
                int enemy_id_index = Random.Range(0, enemies_ids.Count);
                int enemy_id = enemies_ids[enemy_id_index];

                //create
                int[,] box = Data.active_box_map[room];
                for(int c=0; c < 100; c++)
                {
                    int x = Random.Range(0, box.GetLength(1));
                    int y = Random.Range(0, box.GetLength(0));
                    if(box[y, x] == 1)
                    {
                        GameObject enemy = Instantiate(enemies[enemy_id]);
                        enemy.transform.parent = Data.rooms[room].transform;
                        float posX = x * 25f + 12.5f;
                        float posY = (box.GetLength(0) - y - 1) * 25f + 12.5f;
                        Move.MoveLocalPos(enemy, posX, posY);
                        StageEnemyConfig eneC = enemy.GetComponent<StageEnemyConfig>();
                        eneC.myroom = room;
                        eneC.my_id = enemy_id;
                        SetEnemyList(enemy_id, eneC);
                        enemy.SetActive(true);

                        break;
                    }
                }

                (int count, int max) = Master.enemies_count[room];
                count += 1;
                Master.enemies_count[room] = (count, max);
                enemies_ids.RemoveAt(enemy_id_index);
                if (count == max)
                {
                    room_nums.Remove(room);
                }
            }
        }

        if (battle_test != -1)
        {
            GameObject enemy = Instantiate(enemies[battle_test]);
            enemy.transform.parent = Data.rooms[first_room].transform;
            float posX = (first_pos.x + 1) * 25f + 12.5f;
            float posY = (Data.active_box_map[first_room].GetLength(0) - first_pos.y - 1) * 25f + 12.5f;
            Move.MoveLocalPos(enemy, posX, posY);
            StageEnemyConfig eneC = enemy.GetComponent<StageEnemyConfig>();
            eneC.myroom = first_room;
            eneC.my_id = battle_test;
            enemy.SetActive(true);
        }

        //変更したのを戻す
        Data.active_box_map[first_room][first_pos.y, first_pos.x] = 1;
    }
    private void SetEnemyList(int id, StageEnemyConfig eneC)
    {
        int layer = Status.stage_layer;
        List<List<int[]>> enemy_list_all = new List<List<int[]>>();
        List<int> enemy_id = new List<int>();
        List<int> enemy_level = new List<int>();

        if (layer == 0)
        {
            enemy_list_all = StageEnemy_list_15[id];
        }
        else if(layer == 1)
        {
            enemy_list_all = StageEnemy_list_14[id];
        }
        else if(2 <= layer & layer <= 4)
        {
            enemy_list_all = StageEnemy_list_13_11[id];
        }
        else if (5 <= layer & layer <= 9)
        {
            enemy_list_all = StageEnemy_list_10_6[id];
        }
        else if (10 <= layer)
        {
            enemy_list_all = StageEnemy_list_5_2[id];
        }

        int list_count = enemy_list_all.Count;
        if(list_count > 0)
        {
            List<int[]> enemy_list = enemy_list_all[Random.Range(0, list_count)];
            foreach(int[] enemy in enemy_list)
            {
                enemy_id.Add(enemy[0]);
                enemy_level.Add(enemy[1]);
            }

            eneC.battle_id = enemy_id;
            eneC.battle_level = enemy_level;
        }
    }
    private int SearchBoxKind(int minY, int[,] get_box_map, int num, int map_x, int map_y, int range, int max_map_y)
    {
        List<Vector2Int> pos_list = new List<Vector2Int>();
        int minX = 4;
        int maxY = 4;
        for (int y = 0 + minY; y < 5; y++)
        {
            int zero_lengthX = 0;
            for (int x = 0; x < 5; x++)
            {
                if(get_box_map[y, x] == num)
                {
                    pos_list.Add(new Vector2Int(x, y));
                    if(x < minX)
                    {
                        minX = x;
                    }
                }
                else
                {
                    zero_lengthX += 1;
                }
            }

            if(zero_lengthX == 5)
            {
                maxY = y - 1;
                break;
            }
        }

        int kind_box = 0;
        foreach(Vector2Int pos in pos_list)
        {
            kind_box += (pos.y - minY) * BOX_MAP_SIZE + (pos.x - minX);
        }

        int box_posX = (minX / 2) * 200 + map_x * 600;
        int box_posY = ((4 - maxY) / 2) * 225 + (max_map_y - map_y - 1) * 675;
        active_box_map_pos[num + range] = new Vector2Int(box_posX, box_posY);
        active_box_size[num + range] = new Vector2Int(minX, minY);

        return kind_box;
    }
    private int[,] SearchBox(int box_kind, int room_num)
    {
        int index = box_kind_list.IndexOf(box_kind);

        int[,,] boxes;
        if (tutorial == false)
        {
            boxes = box_list[index];
        }
        else
        {
            if(index == 6)
            {
                boxes = tutorial_box_list[1];
            }
            else
            {
                boxes = tutorial_box_list[0];
            }
        }

        Master.enemies_count[room_num] = (0, box_enemies_count_list[index]);
        int box_num = Random.Range(0, boxes.GetLength(0));
        Debug.Log("index: " + index);
        Debug.Log(box_num);
        int lenY = boxes.GetLength(1);
        int lenX = boxes.GetLength(2);
        int[,] box = new int[lenY, lenX];

        for (int y=0; y < lenY; y++)
        {
            for(int x=0; x < lenX; x++)
            {
                box[y, x] = boxes[box_num, y, x];
            }
        }

        return box;
    }
    private void SetRoomInfo(int room1, int room1_x, int room1_y, int room2, int room2_x, int room2_y)
    {
        int[] room1_to_room2 = new int[5] {room1_x, room1_y, room2, room2_x, room2_y};
        int[] room2_to_room1 = new int[5] {room2_x, room2_y, room1, room1_x, room1_y };

        if (Minimap.map_ways.ContainsKey(room1))
        {
            Minimap.map_ways[room1].Add(room1_to_room2);
        }
        else
        {
            Minimap.map_ways[room1] = new List<int[]>() { room1_to_room2 };
        }
        if (Minimap.map_ways.ContainsKey(room2))
        {
            Minimap.map_ways[room2].Add(room2_to_room1);
        }
        else
        {
            Minimap.map_ways[room2] = new List<int[]>() { room2_to_room1 };
        }
    }
    private void SetActions(int room)
    {
        if (Minimap.map_actions.ContainsKey(room))
        {
            Minimap.map_actions[room] += 1;
        }
        else
        {
            Minimap.map_actions[room] = 1;
        }
    }

    public void SetRoom(int key)
    {
        GameObject map_obj = Instantiate(map_resources);
        active_map_obj[key] = map_obj;
        Transform map_transform = map_obj.transform;
        map_transform.parent = map_all_obj.transform;
        Vector2Int room_pos = active_box_map_pos[key];
        Move.MoveLocalPos(map_obj, room_pos.x, room_pos.y);
        map_obj.SetActive(true);
        int[,] box = Data.active_box_map[key];
        int maxX = box.GetLength(1);
        int maxY = box.GetLength(0);
        List<Vector2Int> move_up = new List<Vector2Int>();
        List<Vector2Int> move_list = new List<Vector2Int>();

        int[,] shadow_map = new int[box.GetLength(0), box.GetLength(1)];

        Tilemap tilemap_floor = map_transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap tilemap_edge = map_transform.GetChild(1).GetComponent<Tilemap>();
        Tilemap tilemap_shadow = map_transform.GetChild(2).GetComponent<Tilemap>();
        Tilemap tilemap_ceil = map_transform.GetChild(3).GetComponent<Tilemap>();
        GameObject area = map_transform.GetChild(4).gameObject;
        Tilemap tilemap_area = area.GetComponent<Tilemap>();
        GameObject walls = map_transform.GetChild(5).gameObject;

        //キャラ設置
        if (first_room == key)
        {
            if(stage_create_test == false)
            {
                if(tutorial == false)
                {
                    while (true)
                    {
                        int pos_x = Random.Range(0, maxX);
                        int pos_y = Random.Range(0, maxY);
                        if (box[pos_y, pos_x] == 1)
                        {
                            float jun_pos_x = room_pos.x + pos_x * 25 + 12;
                            float jun_pos_y = room_pos.y + (maxY - pos_y - 1) * 25 + 30;
                            Move.MoveLocalPos(jun, jun_pos_x, jun_pos_y);

                            //Enemyのためにいったん変更
                            first_pos = new Vector2Int(pos_x, pos_y);

                            break;
                        }
                    }
                }
                else
                {
                    int pos_x = 3;
                    int pos_y = 4;
                    float jun_pos_x = room_pos.x + pos_x * 25 + 12;
                    float jun_pos_y = room_pos.y + (maxY - pos_y - 1) * 25 + 30;
                    Move.MoveLocalPos(jun, jun_pos_x, jun_pos_y);

                    //Enemyのためにいったん変更
                    first_pos = new Vector2Int(pos_x, pos_y);
                }

                jun.transform.parent = map_obj.transform;
                junC.myroom = key;
                junC.myroom_obj = map_obj;
            }
        }

        //出口設置
        if (active_box_move.ContainsKey(key))
        {
            int box_minX = active_box_size[key].x;
            int box_minY = active_box_size[key].y;
            foreach (int[] info in active_box_move[key])
            {
                int x = info[0];
                int y = info[1];
                x = (x - box_minX) / 2;
                y = (y - box_minY) / 2;
                x = 3 + x * 8;
                y = 5 + y * 9;
                int dirX = info[2];
                int dirY = info[3];
                x += dirX * 3;
                y += dirY * 3;

                box[y, x] = 1;
                move_list.Add(new Vector2Int(x, y));

                if (dirX != 0)
                {
                    SetBlock(tilemap_floor, tile_other, 0, x + dirX, y, maxY);
                    SetBlock(tilemap_floor, tile_other, 0, x + dirX * 2, y, maxY);
                    box[y - 1, x] = 2;
                    box[y - 2, x] = 2;
                    box[y - 3, x] = 3;

                    for(int i=0; i<3; i++)
                    {
                        SetBlock(tilemap_ceil, tile_other, 0, x + dirX * (i + 1), y, maxY);
                        SetBlock(tilemap_ceil, tile_other, 0, x + dirX * (i + 1), y - 1, maxY);
                        SetBlock(tilemap_ceil, tile_other, 0, x + dirX * (i + 1), y - 2, maxY);
                    }
                }
                else
                {
                    SetBlock(tilemap_floor, tile_other, 0, x, y + dirY, maxY);
                    SetBlock(tilemap_floor, tile_other, 0, x, y + dirY * 2, maxY);
                    SetBlock(tilemap_floor, tile_other, 0, x, y + dirY * 3, maxY);

                    if (dirY == -1)
                    {
                        box[y - 1, x] = 5;
                        move_up.Add(new Vector2Int(x, y - 1));

                        for(int i=0; i<2; i++)
                        {
                            SetBlock(tilemap_ceil, tile_other, 0, x, y - (3+i), maxY);
                            SetBlock(tilemap_ceil, tile_other, 0, x + 1, y - (3 + i), maxY);
                            SetBlock(tilemap_ceil, tile_other, 0, x - 1, y - (3 + i), maxY);
                        }
                    }
                    else if (dirY == 1)
                    {
                        for(int i = 0; i<3; i++)
                        {
                            SetBlock(tilemap_ceil, tile_other, 0, x, y + (i + 1), maxY);
                            SetBlock(tilemap_ceil, tile_other, 0, x + 1, y + (i + 1), maxY);
                            SetBlock(tilemap_ceil, tile_other, 0, x - 1, y + (i + 1), maxY);
                        }
                    }
                }
            }
        }

        //家具設置
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                int tile_num = box[y, x];

                if (tile_num != 0)
                {
                    if (tile_num != 3)
                    {
                        if (tile_num % 2 == 1)
                        {
                            if(tile_num != 1)
                            {
                                //obj
                                if (tile_num != 7 & tile_num != 9)
                                {
                                    SetObj(x, y, maxY, tile_num, map_obj, box, tilemap_area, key);
                                }
                            }
                        }
                    }
                }
            }
        }

        //tile設置
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                int tile_num = box[y, x];

                if (tile_num != 0)
                {
                    if (tile_num != 3)
                    {
                        if (tile_num % 2 == 1)
                        {
                            int index = Random.Range(-3, 8);
                            if(tile_num == 13)
                            {
                                index = 8;
                            }

                            SetBlock(tilemap_floor, tile_floors, index, x, y, maxY);

                            if (shadow_map[y, x] == 0)
                            {
                                SetShadow(box, shadow_map, x, y, maxX, maxY, tilemap_shadow, tile_shadows, walls);
                            }

                            //obj
                            if (tile_num == 7 | tile_num == 9)
                            {
                                SetObj(x, y, maxY, tile_num, map_obj, box, tilemap_area, key);
                            }
                        }
                        else
                        {
                            int index = Random.Range(-3, 6);
                            if(index < 0)
                            {
                                index = 0;
                            }
                            SetWall(x, y, box, maxX, maxY, index, walls, tile_num);
                        }
                    }
                    else
                    {
                        SetWalledge(x, y, box, tilemap_edge, maxX, maxY, tilemap_ceil);
                    }
                }
            }
        }

        //上への出口設置
        foreach(Vector2Int pos in move_up)
        {
            box[pos.y, pos.x] = 0;
            SetBlock(tilemap_edge, tile_other, 2, pos.x, pos.y, maxY);
            SetBlock(tilemap_floor, tile_other, 0, pos.x, pos.y, maxY);
            SetBlock(tilemap_shadow, tile_other, 1, pos.x, pos.y + 1, maxY);
        }
        foreach(Vector2Int pos in move_list)
        {
            box[pos.y, pos.x] = 5;
        }

        Data.rooms[key] = map_obj;

        if (first_room != key)
        {
            map_obj.SetActive(false);
        }
    }
    private void SetAreas(int key, GameObject area)
    {
        Tilemap tilemap_area = area.GetComponent<Tilemap>();
        int[,] box = Data.active_box_map[key];
        int maxX = box.GetLength(1);
        int maxY = box.GetLength(0);

        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                int tile_num = box[y, x];

                if (tile_num != 0)
                {
                    if (tile_num != 3)
                    {
                        if (tile_num % 2 == 1 & tile_num != TILE_BOX & tile_num != TILE_TALK & tile_num != TILE_DECORATION & tile_num != TILE_DECORATION_RANDOM & tile_num != TILE_OBJECT)
                        {
                            SetArea(x, y, box, tilemap_area, maxX, maxY, tile_num, 0);
                        }
                    }
                }
            }
        }

        Data.areas[key] = area;
    }
    private void SetBlock(Tilemap tilemap, Tile[] tile, int index, int x, int y, int maxY)
    {
        int setY = maxY - y - 1;
        if (index < 0)
        {
            index = 0;
        }

        tilemap.SetTile(new Vector3Int(x, setY, 0), tile[index]);
    }
    private void SetWall(int x, int y, int[,] box, int maxX, int maxY, int wall_index, GameObject parent, int num)
    {
        int index = 0;

        if(box[y - 1, x] % 2 == 0)
        {
            index = 7; //上に壁
        }
        else
        {
            index = 3; //上に空白
        }

        int check_right = CheckBlock(x, y, maxX, maxY, 1, 0, box, 1, 2);
        if (check_right == 1) //右に空白
        {
            if(box[y, x + 1] != 3)
            {
                index -= 1;
            }
        }
        else if (check_right == 0) //右に壁
        {
            if(index <= 3)
            {
                if (box[y - 1, x + 1] % 2 == 0)
                {
                    index -= 1;
                }
            }
        }

        int check_left = CheckBlock(x, y, maxX, maxY, -1, 0, box, 1, 2);
        if (check_left == 1) //左に空白
        {
            if (box[y, x - 1] != 3)
            {
                index -= 2;
            }
        }
        else if (check_left == 0)//左に壁
        {
            if (index <= 3)
            {
                if (box[y - 1, x - 1] % 2 == 0)
                {
                    index -= 2;
                }
            }
        }

        GameObject wall = new GameObject();
        wall.AddComponent<SpriteRenderer>();
        SpriteRenderer wall_sr = wall.GetComponent<SpriteRenderer>();
        wall_sr.sprite = sprite_walls[wall_index];
        wall_sr.sortingLayerName = "entity";
        wall.transform.parent = parent.transform;
        float wall_posX = x * 25f + 12.5f;
        float wall_posY = (maxY - y - 1) * 25f + 12.5f;
        float wall_posZ;
        if(box[y + 1, x] % 2 == 0)
        {
            wall_posZ = wall_posY - 37.5f;
        }
        else
        {
            wall_posZ = wall_posY - 12.5f;
        }
        Move.MoveLocalPosZ(wall, wall_posX, wall_posY, wall_posZ / 10f + 0.05f);

        if (index != 3)
        {
            GameObject wallline = new GameObject();
            wallline.AddComponent<SpriteRenderer>();
            SpriteRenderer wallline_sr = wallline.GetComponent<SpriteRenderer>();
            wallline_sr.sprite = sprite_walllines[index];
            wallline_sr.sortingLayerName = "entity";
            wallline.transform.parent = wall.transform;
            Move.MoveLocalPosZ(wallline, 0, 0, -0.01f);
        }

        //objs
        if (num == 4)
        {
            GameObject fire = Instantiate(obj_fire);
            fire.transform.parent = wall.transform;
            Move.MoveLocalPosZ(fire, 0, 0.5f, -0.03f);
            fire.SetActive(true);
        }
        else if(num == WALL_DECORATION_RANDOM | num >= WALL_DECORATION_DECIDED_START)
        {
            int set = Random.Range(0, 100);

            if (stage_create_test == true)
            {
                set = Random.Range(16, 100);
            }

            if (set > 15)
            {
                GameObject decoration = Instantiate(obj_decoration_wall_1to1);
                decoration.transform.parent = wall.transform;
                Move.MoveLocalPosZ(decoration, 0, -12.5f, -0.03f);
                int sp_index = 0;
                if(num >= WALL_DECORATION_DECIDED_START)
                {
                    if ((num - WALL_DECORATION_DECIDED_START) % 4 == 0)
                    {
                        sp_index = (num - WALL_DECORATION_DECIDED_START) / 4;
                    }
                    Debug.Log(num);
                    Debug.Log(sp_index);
                }
                else
                {
                    sp_index = Random.Range(0, obj_decoration_wall_1to1_sprite.Length);
                }
                decoration.GetComponent<SpriteRenderer>().sprite = obj_decoration_wall_1to1_sprite[sp_index];
                decoration.SetActive(true);
                box[y, x] = WALL_DECORATION;
            }
            else
            {
                box[y, x] = 2;
            }
        }
    }
    private void SetArea(int x, int y, int[,] box, Tilemap tilemap, int maxX, int maxY, int tile_num, int cage)
    {
        if ((tile_num != 7 & tile_num != 9) | cage > 0)
        {
            List<int> four_dir_count = new List<int>();
            List<int> eight_dir_count = new List<int>();

            if(!(box[y, x] == 15)) //家具じゃない場合
            {
                for (int i = 0; i < 8; i++)
                {
                    int searchX = x + eight_dir[i, 0];
                    int searchY = y + eight_dir[i, 1];

                    if ((0 <= searchX & searchX < maxX) & (0 <= searchY & searchY < maxY))
                    {
                        int searchbox_num = box[searchY, searchX];

                        if (searchbox_num == 3 | searchbox_num % 2 == 0 | (searchbox_num >= 15 & searchbox_num < 30) | searchbox_num == 11)
                        {
                            if (i % 2 == 0)
                            {
                                four_dir_count.Add(dir_range[i / 2]);
                            }
                            else
                            {
                                eight_dir_count.Add(dir_range[i / 2]);
                            }
                        }
                        else if (searchbox_num == 7)
                        {
                            if (i == 5 | i == 7)
                            {
                                if (cage == 2)
                                {
                                    eight_dir_count.Add(dir_range[i / 2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            four_dir_count.Add(dir_range[i / 2]);
                        }
                        else
                        {
                            eight_dir_count.Add(dir_range[i / 2]);
                        }
                    }
                }
            }
            else
            {
                return;
            }

            int four_count = four_dir_count.Count;
            int eight_count = eight_dir_count.Count;

            if (four_count != 0 & eight_count == 0)
            {
                int index = Sum(four_dir_count);
                SetBlock(tilemap, tile_area_line, index, x, y, maxY);
            }

            if (four_count != 0 & eight_count != 0)
            {
                int index = Sum(four_dir_count);

                if (index != 5 & index != 10)
                {
                    int four_zero = dir_range.IndexOf(four_dir_count[0]) - 1;
                    if (index == 9)
                    {
                        four_zero = 2;
                    }
                    if (four_zero < 0)
                    {
                        four_zero = 3;
                    }
                    for (int i = 0; i < four_count + 1; i++)
                    {
                        int num = dir_range[four_zero];
                        if (eight_dir_count.Contains(num))
                        {
                            eight_dir_count.Remove(num);
                        }

                        four_zero += 1;
                        if (four_zero > 3)
                        {
                            four_zero = 0;
                        }
                    }
                }
                else
                {
                    eight_dir_count = new List<int>();
                }

                if (eight_dir_count.Count == 0)
                {
                    SetBlock(tilemap, tile_area_line, index, x, y, maxY);
                }
                else
                {
                    index = SearchWalledge_linecorner(four_dir_count, eight_dir_count);
                    SetBlock(tilemap, tile_area_line_corner, index, x, y, maxY);
                }

                //int a = Sum(eight_dir_count);
                //Debug.Log("x: " + x + "  y: " + y + "  index: " + a);
            }

            if (four_count == 0 & eight_count != 0)
            {
                int index = Sum(eight_dir_count);
                SetBlock(tilemap, tile_area_corner, index, x, y, maxY);
            }

            if (four_count == 0 & eight_count == 0)
            {
                SetBlock(tilemap, tile_area_line, 0, x, y, maxY);
            }
        }
        else
        {
            int index = 0;
            if (box[y, x + 1] == 7 | box[y, x + 1] == 9)
            {
                index += 1;
            }
            if (box[y, x - 1] == 7 | box[y, x - 1] == 9)
            {
                index += 2;
            }
            if(box[y - 1, x] != 1)
            {
                index += 4;
            }
            else
            {
                int sub_index = 0;
                if (box[y - 1, x - 1] != 1)
                {
                    if (index == 2)
                    {
                        index = 9;
                    }
                    else if(index != 1)
                    {
                        index = 10;
                        sub_index += 1;
                    }
                }
                if (box[y - 1, x + 1] != 1)
                {
                    if (index == 1)
                    {
                        index = 8;
                    }
                    else if (index != 2)
                    {
                        index = 11;
                        sub_index += 1;
                    }
                }
                if(sub_index == 2)
                {
                    index = 12;
                }
            }
            if (tile_num == TILE_CAGE)
            {
                SetBlock(tilemap, tile_area_cage, index, x, y, maxY);
            }
            else if(tile_num == TILE_DOOR)
            {
                SetBlock(tilemap, tile_area_cage_opened, index, x, y, maxY);
            }
        }
    }
    private void SetWalledge(int x, int y, int[,] box, Tilemap tilemap, int maxX, int maxY, Tilemap ceil)
    {
        List<int> four_dir_count = new List<int>();
        List<int> eight_dir_count = new List<int>();
        for (int i=0; i<8; i++)
        {
            int searchX = x + eight_dir[i, 0];
            int searchY = y + eight_dir[i, 1];

            if((0 <= searchX & searchX < maxX) & (0 <= searchY & searchY < maxY))
            {
                if(box[searchY, searchX] != 3 & box[searchY, searchX] != 0)
                {
                    if (i % 2 == 0)
                    {
                        four_dir_count.Add(dir_range[i / 2]);
                    }
                    else
                    {
                        eight_dir_count.Add(dir_range[i / 2]);
                    }
                }
            }
        }

        int four_count = four_dir_count.Count;
        int eight_count = eight_dir_count.Count;

        if (four_count != 0 & eight_count == 0)
        {
            int index = Sum(four_dir_count);
            SetBlock(tilemap, tile_edges_line, index, x, y, maxY);
        }

        if (four_count != 0 & eight_count != 0)
        {
            int index = Sum(four_dir_count);

            if (index != 5 & index != 10)
            {
                int four_zero = dir_range.IndexOf(four_dir_count[0]) - 1;
                if(index == 9)
                {
                    four_zero = 2;
                }
                if (four_zero < 0)
                {
                    four_zero = 3;
                }
                for (int i = 0; i < four_count + 1; i++)
                {
                    int num = dir_range[four_zero];
                    if (eight_dir_count.Contains(num))
                    {
                        eight_dir_count.Remove(num);
                    }

                    four_zero += 1;
                    if (four_zero > 3)
                    {
                        four_zero = 0;
                    }
                }
            }
            else
            {
                eight_dir_count = new List<int>();
            }

            if (eight_dir_count.Count == 0)
            {
                SetBlock(tilemap, tile_edges_line, index, x, y, maxY);
            }
            else
            {
                index = SearchWalledge_linecorner(four_dir_count, eight_dir_count);
                SetBlock(tilemap, tile_edges_line_corner, index, x, y, maxY);
            }

            //int a = Sum(eight_dir_count);
            //Debug.Log("x: " + x + "  y: " + y + "  index: " + a);
        }

        if (four_count == 0 & eight_count != 0)
        {
            int index = Sum(eight_dir_count);
            SetBlock(tilemap, tile_edges_corner, index, x, y, maxY);
        }

        if(four_count != 0 | eight_count != 0)
        {
            SetBlock(ceil, tile_other, 0, x, y, maxY);
        }
    }
    private int SearchWalledge_linecorner(List<int> four_dir_count, List<int> right_dir_count)
    {
        int index_four = Sum(four_dir_count);
        int index_eight = Sum(right_dir_count);

        int index;
        if (index_four == 1)
        {
            if (index_eight == 2)
            {
                index = 0;
            }
            else if (index_eight == 4)
            {
                index = 1;
            }
            else
            {
                index = 2;
            }
        }
        else if (index_four == 3)
        {
            index = 3;
        }
        else if (index_four == 2)
        {
            if (index_eight == 4)
            {
                index = 4;
            }
            else if (index_eight == 8)
            {
                index = 5;
            }
            else
            {
                index = 6;
            }
        }
        else if (index_four == 6)
        {
            index = 7;
        }
        else if (index_four == 4)
        {
            if (index_eight == 8)
            {
                index = 8;
            }
            else if (index_eight == 1)
            {
                index = 9;
            }
            else
            {
                index = 10;
            }
        }
        else if (index_four == 12)
        {
            index = 11;
        }
        else if (index_four == 8)
        {
            if (index_eight == 1)
            {
                index = 12;
            }
            else if (index_eight == 2)
            {
                index = 13;
            }
            else
            {
                index = 14;
            }
        }
        else
        {
            index = 15;
        }

        return index;
    }
    private void SetShadow(int[,] box, int[,] map, int x, int y, int maxX, int maxY, Tilemap tilemap, Tile[] tile, GameObject wall)
    {
        if(box[y - 1, x] % 2 == 0)
        {
            int check_wall_shadow = CheckBlock(x, y, maxX, maxY, 1, -2, box, 3, 0);
            if (check_wall_shadow == 1)
            {

                GameObject wallshadow = new GameObject();
                wallshadow.AddComponent<SpriteRenderer>();
                SpriteRenderer wallshadow_sr = wallshadow.GetComponent<SpriteRenderer>();
                wallshadow_sr.sprite = sprite_wallshadow[0];
                wallshadow_sr.sortingLayerName = "entity";
                wallshadow.transform.parent = wall.transform;
                float wall_posX = x * 25f + 12.5f;
                float wall_posY = (maxY - (y - 1) - 1) * 25f + 12.5f;
                float wall_posZ = wall_posY - 12.5f;
                Move.MoveLocalPosZ(wallshadow, wall_posX, wall_posY, wall_posZ / 10f + 0.03f);

                GameObject wallshadow2 = new GameObject();
                wallshadow2.AddComponent<SpriteRenderer>();
                SpriteRenderer wallshadow_sr2 = wallshadow2.GetComponent<SpriteRenderer>();
                wallshadow_sr2.sprite = sprite_wallshadow[1];
                wallshadow_sr2.sortingLayerName = "entity";
                wallshadow_sr2.transform.parent = wall.transform;
                Move.MoveLocalPosZ(wallshadow2, wall_posX, wall_posY + 25f, wall_posZ / 10f + 0.03f);

                SetBlock(tilemap, tile, 3, x, y, maxY);
                map[y , x] = 1;
                (int len, bool exit) = SearchShadowFloor(box, x, y + 1, 1, 0, maxX, maxY);
                for(int d=0; d<len; d++)
                {
                    if(d == len - 1 & exit == true)
                    {
                        SetBlock(tilemap, tile, 5, x, y + (d + 1), maxY);
                    }
                    else
                    {
                        SetBlock(tilemap, tile, 4, x, y + (d + 1), maxY);
                    }
                    map[y + (d + 1), x] = 1;
                }
            }
            else
            {
                SetBlock(tilemap, tile, 0, x, y, maxY);
                map[y, x] = 1;
            }

            int check_outofrange = CheckBlock(x, y, maxX, maxY, -1, -1, box, 3, 0);
            if(check_outofrange != 2)
            {
                if (map[y - 1, x - 1] == 0 & (box[y - 1, x - 1] % 2 == 1 & box[y - 1, x - 1] != 3))
                {
                    (int len, bool exit) = SearchShadowFloor(box, x - 1, y, -1, 0, maxX, maxY);
                    for (int d = 0; d < len; d++)
                    {
                        if (d == len - 1)
                        {
                            SetBlock(tilemap, tile, 6, x - 1, y - d, maxY);
                        }
                        else if (d == 0)
                        {
                            SetBlock(tilemap, tile, 5, x - 1, y - d, maxY);
                        }
                        else
                        {
                            SetBlock(tilemap, tile, 4, x - 1, y - d, maxY);
                        }
                        map[y - d, x - 1] = 1;
                    }
                }
            }
        }
        else
        {
            int check_up = CheckBlock(x, y, maxX, maxY, 0, 1, box, 3, 0);
            int check_right = CheckBlock(x, y, maxX, maxY, 1, 0, box, 3, 0);
            if (check_up > 0 & check_right > 0)
            {
                (int len, bool exit) = SearchShadowFloor(box, x, y, -1, 0, maxX, maxY);
                for (int d = 0; d < len; d++)
                {
                    if (d == len - 1 & exit == true)
                    {
                        SetBlock(tilemap, tile, 6, x, y - d, maxY);
                    }
                    else
                    {
                        SetBlock(tilemap, tile, 4, x, y - d, maxY);
                    }
                    map[y - d, x] = 1;
                }
            }
        }
    }
    private (int length, bool exit) SearchShadowFloor(int[,] box, int x, int y, int dir, int len, int maxX, int maxY)
    {
        bool search = true;
        bool end = false;

        if (CheckBlock(x, y, maxX, maxY, 0, 0, box, 1, 2) == 1 & CheckBlock(x, y, maxX, maxY, 0, 0, box, 3, 1) == 1)
        {
            len += 1;
            if (dir == 1)
            {
                if (CheckBlock(x, y, maxX, maxY, 1, 0, box, 1, 2) > 0 & CheckBlock(x, y, maxX, maxY, 1, 0, box, 3, 1) > 0)
                {
                    end = true;
                    search = false;
                }
            }
            else
            {
                if (CheckBlock(x, y, maxX, maxY, 1, -1, box, 1, 2) > 0 & CheckBlock(x, y, maxX, maxY, 1, -1, box, 3, 1) > 0)
                {
                    end = true;
                    search = false;
                }
            }
        }
        else
        {
            search = false;
        }

        if(search == true)
        {
            (int length, bool exit) = SearchShadowFloor(box, x, y + dir, dir, len, maxX, maxY);
            end = exit;
            len = length;
        }

        return (len, end);
    }
    private int CheckBlock(int x, int y, int maxX, int maxY, int plusX, int plusY, int[,] box, int num, int search_kind) //[0]不一致 [1]一致 [2]範囲外
    {
        int check = 0;
        int checkX = x + plusX;
        int checkY = y + plusY;

        if ((0 <= checkX & checkX < maxX) & (0 <= checkY & checkY < maxY))
        {
            if(search_kind == 0)
            {
                if (box[checkY, checkX] == num)
                {
                    check = 1;
                }
            }
            else if(search_kind == 1)
            {
                if (box[checkY, checkX] != num)
                {
                    check = 1;
                }
            }
            else if (search_kind == 2)
            {
                if (box[checkY, checkX] % 2 == num)
                {
                    check = 1;
                }
            }
        }
        else
        {
            check = 2;
        }

        return check;
    }
    public void SetObj(int x, int y, int maxY, int num, GameObject parent, int[,] box, Tilemap area, int key)
    {
        if(num == TILE_CAGE)
        {
            GameObject obj = Instantiate(obj_cage);
            obj.transform.parent = parent.transform;
            float wall_posX = x * 25f + 12.5f;
            float wall_posY = (maxY - y - 1) * 25f + 26.5f;
            Move.MoveLocalPosZ(obj, wall_posX, wall_posY, (wall_posY - 22.5f) / 10f + 0.05f);
            obj.SetActive(true);
        }
        else if(num == TILE_DOOR)
        {
            GameObject obj = Instantiate(obj_cage_door);
            obj.transform.parent = parent.transform;
            float wall_posX = x * 25f + 12.5f;
            float wall_posY = (maxY - y - 1) * 25f + 26.5f;
            Move.MoveLocalPosZ(obj, wall_posX, wall_posY, (wall_posY - 22.5f) / 10f + 0.05f);
            obj.SetActive(true);
            ActionCageDoorConfig obj_config = obj.GetComponent<ActionCageDoorConfig>();
            if (box[y, x + 1] == 7)
            {
                obj_config.dir = 1;
            }
            else
            {
                obj_config.dir = -1;
            }

            int index = 0;
            if (box[y, x + 1] == 7 | box[y, x + 1] == 9)
            {
                index += 1;
            }
            if (box[y, x - 1] == 7 | box[y, x - 1] == 9)
            {
                index += 2;
            }
            obj_config.area_x = x;
            obj_config.area_y = maxY - y - 1;
        }

        else if(num == TILE_BOX)
        {
            item_canplace.Add((x, y, maxY, parent, key));
        }

        else if(num == TILE_EXIT)
        {
            GameObject obj = Instantiate(obj_stair);
            obj.transform.parent = parent.transform;
            float wall_posX = x * 25f + 12.5f;
            float wall_posY = (maxY - y - 1) * 25f + 12.5f;
            Move.MoveLocalPos(obj, wall_posX, wall_posY);
            obj.SetActive(true);

            Transform transform = obj.transform;
            Vector3 pos = transform.position;
            int goalX = (int)(pos.x / 200);
            int goalY = (int)((pos.y + 25f) / 225);
            Minimap.goal_pos = new Vector2Int(goalX, goalY);
        }

        else if(num == TILE_DECORATION_RANDOM)
        {
            int set = Random.Range(0, 100);

            if(stage_create_test == true)
            {
                set = Random.Range(16, 100);
            }

            if(set > 15)
            {
                if (box[y, x + 1] == 17)
                {
                    if(set < 55)
                    {
                        SetDecoration(x * 25f + 25f, (maxY - y - 1) * 25f + 25f, obj_decoration_1to2, parent, obj_decoration_1to2_sprite);
                        box[y, x + 1] = 15;
                    }
                    else
                    {
                        SetDecoration(x * 25f + 12.5f, (maxY - y - 1) * 25f + 25f, obj_decoration_1to1, parent, obj_decoration_1to1_sprite);
                    }
                }
                else
                {
                    SetDecoration(x * 25f + 12.5f, (maxY - y - 1) * 25f + 25f, obj_decoration_1to1, parent, obj_decoration_1to1_sprite);
                }

                box[y, x] = 15;
            }
            else
            {
                box[y, x] = 1;
            }
        }

        else if (num == TILE_OBJECT)
        {
            if (set_orb == false)
            {
                GameObject obj = Instantiate(obj_orb);
                obj.transform.parent = parent.transform;
                float posX = x * 25f + 12.5f;
                float posY = (maxY - y - 1) * 25f + 17f;
                Move.MoveLocalPosZ(obj, posX, posY, (posY - 17f) / 10f);
                obj.SetActive(true);

                ActionBoxConfig config = obj.GetComponent<ActionBoxConfig>();
                int rom;
                if(tutorial == false)
                {
                    rom = Status.myskill_inStage[0];
                    Status.myskill_inStage.RemoveAt(0);
                }
                else
                {
                    rom = 0;
                }
                
                config.rom_num = rom;
                config.myroom = key;

                set_orb = true;
            }
            else
            {
                GameObject obj = Instantiate(obj_box);
                obj.transform.parent = parent.transform;
                float posX = x * 25f + 12.5f;
                float posY = (maxY - y - 1) * 25f + 17f;
                Move.MoveLocalPosZ(obj, posX, posY, (posY - 17f) / 10f);
                obj.SetActive(true);

                int item_num = 0;
                if (item_done_bool == true)
                {
                    item_num = items_done[Status.stage_layer];
                    item_done_bool = false;
                }
                else
                {
                    if(tutorial == false)
                    {
                        List<int> items = new List<int>() { 0 };
                        for (int i = 0; i < Status.stage_items.Length; i++)
                        {
                            items.Add(items[i] + Status.stage_items[i]);
                        }
                        int item = Random.Range(0, items[items.Count - 1]);

                        for (int i = 1; i < items.Count; i++)
                        {
                            if (items[i - 1] <= item & item < items[i])
                            {
                                item_num = i - 1;
                                Status.stage_items[item_num] -= 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        item_num = 0;
                    }
                }
                ActionBoxConfig config = obj.GetComponent<ActionBoxConfig>();
                config.item_num = item_num;
                config.myroom = key;
            }
            SetActions(key);
        }

        else if(num == TILE_TALK)
        {
            if(box[y, x - 1] != TILE_TALK)
            {
                talk_canplace.Add((x, y, maxY, parent, key));
            }
        }

        else if(num >= TILE_DECORATION_DECIDED_START)
        {
            if(num % 2 == 1)
            {
                box[y, x] = 15;
                int next_num = box[y, x + 1];
                if (next_num == num)
                {
                    if((next_num / 2) % 2 == 0)
                    {
                        SetDecorationDecided(x * 25f + 25f, (maxY - y - 1) * 25f + 25f, obj_decoration_1to2, parent, obj_decoration_1to2_sprite, (num - (TILE_DECORATION_DECIDED_START + 2)) / 4);
                        box[y, x + 1] = 15;
                    }
                    else
                    {
                        SetDecorationDecided(x * 25f + 12.5f, (maxY - y - 1) * 25f + 25f, obj_decoration_1to1, parent, obj_decoration_1to1_sprite, (num - TILE_DECORATION_DECIDED_START) / 4);
                    }
                }
                else
                {
                    SetDecorationDecided(x * 25f + 12.5f, (maxY - y - 1) * 25f + 25f, obj_decoration_1to1, parent, obj_decoration_1to1_sprite, (num - TILE_DECORATION_DECIDED_START) / 4);
                }
            }
        }
    }

    private void SetDecoration(float x, float y, GameObject new_obj, GameObject parent, Sprite[] sprites)
    {
        GameObject obj = Instantiate(new_obj);
        obj.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        obj.transform.parent = parent.transform;
        Move.MoveLocalPosZ(obj, x, y, (y - 25f) / 10f);
        obj.SetActive(true);
    }

    private void SetDecorationDecided(float x, float y, GameObject new_obj, GameObject parent, Sprite[] sprites, int index)
    {
        Debug.Log(index);
        GameObject obj = Instantiate(new_obj);
        obj.GetComponent<SpriteRenderer>().sprite = sprites[index];
        obj.transform.parent = parent.transform;
        Move.MoveLocalPosZ(obj, x, y, (y - 25f) / 10f);
        obj.SetActive(true);
    }

    private void SetTalkFirst()
    {
        talk_sp.Add(talk_emon_sp);
    }

    private void AdjustStageUI()
    {
        float sideX = (int)(Status.SCREEN_WIDTH - 200);
        Move.MoveLocalPosX(Master.stageUI_map, sideX);
        Move.MoveLocalPosX(Master.stageUI_status, sideX);
        Move.MoveLocalPosX(stageUI_items_text, sideX);
        Move.MoveLocalPosX(stageUI_items_buttons, -sideX);
        Move.MoveLocalPosX(stageUI_left, -sideX);

        if(sideX > 0)
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
    }

    private int Sum(List<int> list)
    {
        int sum = 0;
        foreach (int num in list)
        {
            sum += num;
        }
        return sum;
    }
    public void printmap(int[,] pmap)
    {
        string text = "";

        for (int y = 0; y < pmap.GetLength(0); y++)
        {
            for (int x = 0; x < pmap.GetLength(1); x++)
            {
                string value = "";
                if (pmap[y, x] >= 10 | pmap[y, x] < 0)
                {
                    value += "." + pmap[y, x];
                }
                else
                {
                    value += ".." + pmap[y, x];
                }

                text += ConvertToFullWidth(value);
            }

            text += "\n";
        }

        Debug.Log(text);
    }
    private string ConvertToFullWidth(string halfWidthStr)
    {
        string fullWidthStr = null;

        for (int i = 0; i < halfWidthStr.Length; i++)
        {
            fullWidthStr += (char)(halfWidthStr[i] + 65248);
        }

        return fullWidthStr;
    }
}
