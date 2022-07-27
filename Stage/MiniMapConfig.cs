using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(1)]
public class MiniMapConfig : MonoBehaviour
{
    public Dictionary<int, List<int[]>> map_ways = new Dictionary<int, List<int[]>>();
    public Dictionary<int, int> map_actions = new Dictionary<int, int>();

    public int[,] minimap_roomnums;
    [SerializeField] JunConfig junC;
    [SerializeField] FuncMove Move;

    [SerializeField] Tilemap minimap_tilemap;
    [SerializeField] Tile[] minimap_tiles;
    [SerializeField] Tile[] minimap_action_tiles;
    [SerializeField] Tilemap minimap_way_tilemap;
    [SerializeField] Tile[] minimap_way_tiles;
    [SerializeField] Tile[] minimap_action_way_tiles;
    [SerializeField] GameObject minimap;
    [SerializeField] GameObject allminimap_parent;
    [SerializeField] GameObject allminimap;
    [SerializeField] Tilemap allminimap_tilemap;
    [SerializeField] Tilemap allminimap_way_tilemap;
    [SerializeField] Tile goal_tile;

    int[,] four_dir = new int[4, 2]
    {
        {1, 0 },
        {0, 1 },
        {-1, 0 },
        {0, -1 },
    };
    int[] dir = new int[4] { 1, 2, 4, 8 };
    List<int> num_in_box;
    public List<int> num_jun_done;
    public List<int> pos_jun_done;

    bool open = false;

    int mapX = -99;
    int mapY = -99;
    public Vector2Int goal_pos;

    private void Update()
    {
        Transform transform = junC.transform;
        Vector3 pos = transform.position;
        int posX = (int)((pos.x + 12.5f) / 200);
        int posY = (int)((pos.y - 20.5f + 25f) / 225);
        
        if(mapX != posX | mapY != posY)
        {
            mapX = posX;
            mapY = posY;
            MiniMapSetTile(junC.myroom);
        }
    }

    public void ChangeActionValue(int room, int value)
    {
        map_actions[room] += value;

        if(map_actions[room] <= 0)
        {
            MiniMapSetTile(junC.myroom);
        }
    }

    public void MiniMapSetTile(int junroom)
    {
        if (!num_jun_done.Contains(junroom))
        {
            num_jun_done.Add(junroom);
        }

        int pos = mapY * 100 + mapX;
        if (!pos_jun_done.Contains(pos))
        {
            pos_jun_done.Add(pos);
        }

        num_in_box = new List<int>();

        if(open == true)
        {
            Move.MoveLocalPos(allminimap, -((mapX + 1) * 12 + 6), -((mapY + 1) * 12 + 6) + 0.5f);
            SetTile(true, 0, 0, minimap_roomnums.GetLength(1), minimap_roomnums.GetLength(0), allminimap_tilemap, allminimap_way_tilemap);
        }
        else
        {
            SetTile(false, -1, -1, 2, 2, minimap_tilemap, minimap_way_tilemap);
        }
    }

    private void SetTile(bool all, int minrangeX, int minrangeY, int maxrangeX, int maxrangeY, Tilemap minimap_tilemap, Tilemap minimap_way_tilemap)
    {
        int maxY = minimap_roomnums.GetLength(0);
        int maxX = minimap_roomnums.GetLength(1);

        for (int y = minrangeY; y < maxrangeY; y++)
        {
            for (int x = minrangeX; x < maxrangeX; x++)
            {
                int setX;
                int setY;
                int maxsetY;
                if(all == false)
                {
                    setX = mapX + x;
                    setY = (maxY - mapY - 1) + y;
                    maxsetY = 1 - y;
                }
                else
                {
                    setX = x;
                    setY = y;
                    maxsetY = maxY - y;
                }

                int done_pos = (maxY - setY - 1) * 100 + setX;

                if (pos_jun_done.Contains(done_pos))
                {
                    if ((0 <= setX & setX < maxX) & (0 <= setY & setY < maxY))
                    {
                        int room_num = minimap_roomnums[setY, setX];

                        if (room_num >= 3)
                        {
                            if (num_jun_done.Contains(room_num))
                            {
                                //goal
                                if (goal_pos.x == setX & (maxY - goal_pos.y - 1) == setY)
                                {
                                    minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 1), goal_tile);
                                }
                                else
                                {
                                    minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 1), null);
                                }

                                if (!num_in_box.Contains(room_num))
                                {
                                    num_in_box.Add(room_num);
                                }

                                int way_index = 0;
                                int tile_index = 0;
                                for (int i = 0; i < 4; i++)
                                {
                                    int searchX = setX + four_dir[i, 0];
                                    int searchY = setY + four_dir[i, 1];
                                    if (0 <= searchX & searchX < maxX)
                                    {
                                        if (0 <= searchY & searchY < maxY)
                                        {
                                            int nextto_room_num = minimap_roomnums[searchY, searchX];
                                            if (nextto_room_num == room_num)
                                            {
                                                tile_index += dir[i];
                                            }
                                            else if (nextto_room_num >= 3)
                                            {
                                                if (map_ways.ContainsKey(room_num))
                                                {
                                                    foreach (int[] info in map_ways[room_num])
                                                    {
                                                        if (info[0] == setX & info[1] == setY)
                                                        {
                                                            if (info[2] == nextto_room_num)
                                                            {
                                                                if (info[3] == searchX & info[4] == searchY)
                                                                {
                                                                    way_index += dir[i];
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (way_index == 0)
                                {
                                    minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                                }
                                else
                                {
                                    if (map_actions.ContainsKey(room_num))
                                    {
                                        if (map_actions[room_num] > 0)
                                        {
                                            minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), minimap_action_way_tiles[way_index]);
                                        }
                                        else
                                        {
                                            minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), minimap_way_tiles[way_index]);
                                        }
                                    }
                                    else
                                    {
                                        minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), minimap_way_tiles[way_index]);
                                    }
                                }

                                if (map_actions.ContainsKey(room_num))
                                {
                                    if (map_actions[room_num] > 0)
                                    {
                                        minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), minimap_action_tiles[tile_index]);
                                    }
                                    else
                                    {
                                        minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), minimap_tiles[tile_index]);
                                    }
                                }
                                else
                                {
                                    minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), minimap_tiles[tile_index]);
                                }
                            }
                            else
                            {
                                minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 1), null);
                                minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                                minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                            }
                        }
                        else
                        {
                            minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 1), null);
                            minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                            minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                        }
                    }
                    else
                    {
                        minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 1), null);
                        minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                        minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                    }
                }
                else
                {
                    minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 1), null);
                    minimap_way_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                    minimap_tilemap.SetTile(new Vector3Int(x + 1, maxsetY, 0), null);
                }
            }
        }
    }

    //全体マップ表示を作れ
    public void AllMapSetTile()
    {
        if(junC.canmove == true & junC.Moveroom == false)
        {
            if(open == false)
            {
                minimap.SetActive(false);
                allminimap_parent.SetActive(true);
                Move.MoveLocalPos(allminimap, -((mapX + 1) * 12 + 6), -((mapY + 1) * 12 + 6) + 0.5f);
                SetTile(true, 0, 0, minimap_roomnums.GetLength(1), minimap_roomnums.GetLength(0), allminimap_tilemap, allminimap_way_tilemap);
                open = true;
            }
            else
            {
                minimap.SetActive(true);
                allminimap_parent.SetActive(false);
                SetTile(false, -1, -1, 2, 2, minimap_tilemap, minimap_way_tilemap);
                open = false;
            }
        }
    }
}
