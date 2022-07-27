using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StageCreateDebug : MonoBehaviour
{
    int[,] four_dir = new int[4, 2]
    {
        {1, 0 },
        {0, 1 },
        {-1, 0 },
        {0, -1 },
    };
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

    [SerializeField] StageCreateConfig scc;
    [SerializeField] FuncMove Move;
    [SerializeField] GameObject jun;
    [SerializeField] GameObject CheckSpaceParent;
    [SerializeField] GameObject CheckSpace;
    private GameObject CheckSpaceParent_Active;

    [SerializeField] GameObject grid;
    [SerializeField] GameObject undo;
    [SerializeField] GameObject[] tools;
    [SerializeField] GameObject tool;
    [SerializeField] DataStage Data;
    [SerializeField] GameObject buttons;
    [SerializeField] Text[] Map_Value;

    private List<int[,]> undo_box = new List<int[,]>();
    private List<int> undo_item_count = new List<int>();
    private List<int> undo_talk_count = new List<int>();
    private List<List<int>> undo_wall_deco_pos_list = new List<List<int>>();
    private List<List<int>> undo_wall_deco_num_list = new List<List<int>>();

    public List<int[,]> default_box_list = new List<int[,]>()
    {
        new int[9, 7]  //box 1×1
        {
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,1,1,-2,1,1,0},
            {0,1,1,1,1,1,0},
            {0,-1,1,1,1,-1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0},
        },  //box 1×1
        new int[9, 15]  //box 1×2
        {
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
             {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
             {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
             {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
             {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 1×2
        new int[18, 7]  //box 2×1
        {
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,1,1,-2,1,1,0},
            {0,1,1,1,1,1,0},
            {0,-1,1,1,1,-1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,1,1,1,0},
            {0,-1,1,1,1,-1,0},
            {0,1,1,1,1,1,0},
            {0,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0},
        },  //box 2×1
        new int[18, 15]  //box 2×2
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×2
        new int[18, 15]  //box 2×2 左上L
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,-1,1,1,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,-1,1,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×2 左上L
        new int[18, 15]  //box 2×2 右上L
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×2 右上L
        new int[18, 15]  //box 2×2 左下L
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,-2,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,-2,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×2 左上L
        new int[18, 15]  //box 2×2 左下L
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,1,-2,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,1,1,-2,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×2 右下L
        new int[9, 23]  //box 1×3
        {
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
             {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
             {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
             {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
             {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 1×3
        new int[27, 7]  //box 3×1
        {
                {0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,0},
                {0,1,1,1,1,1,0},
                {0,-1,1,1,1,-1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,-1,1,1,1,-1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,1,1,1,0},
                {0,-1,1,1,1,-1,0},
                {0,1,1,1,1,1,0},
                {0,1,1,-1,1,1,0},
                {0,0,0,0,0,0,0},
        },  //box 3×1
        new int[18, 23]  //box 2×3
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×3
        new int[27, 15]  //box 3×2
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 3×2
        new int[27, 15]  //box 3×2
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,-1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 3×2
        new int[27, 15]  //box 3×2
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,1,-2,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,1,1,-2,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,-1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0},
            {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,1,1,-1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 3×2
        new int[18, 23]  //box 2×3
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,-2,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,-1,1,1,1,1,1,1,1,-1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×3
        new int[18, 23]  //box 2×3
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,-1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×3
        new int[18, 23]  //box 2×3
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,-1,1,1,1,1,1,1,-1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,1,1,1,-1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,-1,1,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×3
        new int[18, 23]  //box 2×3
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,-1,1,1,1,1,1,1,1,1,-1,1,1,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,-1,1,1,1,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        },  //box 2×3
        new int[27, 23]  //box 3×3
        {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,-2,1,1,1,1,1,1,1,-2,1,1,1,1,1,1,1,-2,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,-1,3},
                {3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3},
                {3,1,1,-1,1,1,1,1,1,1,1,-1,1,1,1,1,1,1,1,1,-1,1,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
        },  //box 3×3
    };
    public List<int[,]> default_boxsize_list = new List<int[,]>()
    {
        new int[1, 1]  //box 1×1
        {
            {1}
        },  //box 1×1
        new int[1, 3]  //box 1×2
        {
            {1, 1, 1}
        },  //box 1×2
        new int[3, 1]  //box 1×2
        {
            {1},
            {1},
            {1},
        },  //box 1×2
        new int[3, 3]  //box 2×2
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
        },  //box 2×2
        new int[3, 3]  //box 2×2
        {
            {1, 1, 1},
            {1, 0, 0},
            {1, 0, 0},
        },  //box 2×2
        new int[3, 3]  //box 2×2
        {
            {1, 1, 1},
            {0, 0, 1},
            {0, 0, 1},
        },  //box 2×2
        new int[3, 3]  //box 2×2
        {
            {1, 0, 0},
            {1, 0, 0},
            {1, 1, 1},
        },  //box 2×2
        new int[3, 3]  //box 2×2
        {
            {0, 0, 1},
            { 0, 0, 1},
            {1, 1, 1},
        },  //box 2×2
        new int[1, 5]
        {
            {1, 1, 1, 1, 1},
        },
        new int[5, 1]
        {
            {1},
            {1},
            {1},
            {1},
            {1},
        },
        new int[3, 5]
        {
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
        },
        new int[5, 3]
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
        },
        new int[5, 3]
        {
            {1, 0, 0},
            {1, 0, 0},
            {1, 1, 1},
            {1, 0, 0},
            {1, 0, 0},
        },
        new int[5, 3]
        {
            {0, 0, 1},
            {0, 0, 1},
            {1, 1, 1},
            {0, 0, 1},
            {0, 0, 1},
        },
        new int[3, 5]
        {
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
            {1, 1, 1, 1, 1},
        },
        new int[3, 5]
        {
            {1, 1, 1, 1, 1},
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
        },
        new int[3, 5]
        {
            {1, 1, 1, 1, 1},
            {0, 0, 0, 0, 1},
            {0, 0, 0, 0, 1},
        },
        new int[3, 5]
        {
            {1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0},
            {1, 0, 0, 0, 0},
        },
        new int[5, 5]
        {
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
        },
    };
    public List<int> default_item_count = new List<int>();
    public List<int> default_talk_count = new List<int>();
    int[,] box_map;

    private int tool_num = 0;
    private int tools_num = 0;
    private int tool_kind_num = 0;
    [SerializeField] SpriteRenderer tool_sr;
    [SerializeField] Sprite[] tool_sprites;
    private float tool_height;

    private int check_space_count_max = 0;
    public int check_space_count = 0;

    public bool create_mode;
    public bool test_mode = false;
    private bool tool_select;
    private bool tool_set;
    private int camera_speed = 2;
    private int box_num;

    //item
    [SerializeField] Text item_count_text;
    private int item_count = 0;

    //talk
    [SerializeField] Text talk_count_text;
    private int talk_count = 0;

    Vector2Int exit_room_pos;

    private List<int> wall_deco_pos_list = new List<int>();
    private List<int> wall_deco_num_list = new List<int>();

    private List<int> cantset_floor = new List<int>();
    private List<int> cantset_obj = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< Map_Value.Length; i++)
        {
            int value = scc.box_list[i].GetLength(0);
            Map_Value[i].text = "" + value;
        }
    }

    private void FixedUpdate()
    {
        if(create_mode == true & test_mode == false)
        {
            float Hdirection = Input.GetAxis("Horizontal");
            float Vdirection = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                camera_speed = 4;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                camera_speed = 2;
            }

            //Camera Move
            if (Hdirection != 0 | Vdirection != 0)
            {
                int moveX = 0;
                int moveY = 0;

                if(Hdirection != 0)
                {
                    if(Hdirection > 0)
                    {
                        moveX = camera_speed;
                    }
                    else if(Hdirection < 0)
                    {
                        moveX = -camera_speed;
                    }
                }
                if(Vdirection != 0)
                {
                    if (Vdirection > 0)
                    {
                        moveY = camera_speed;
                    }
                    else if (Vdirection < 0)
                    {
                        moveY = -camera_speed;
                    }
                }

                Transform ctransform = Camera.main.transform;
                Vector3 cpos = ctransform.position;
                cpos.x += moveX;
                cpos.y += moveY;
                ctransform.position = cpos;
            }
        }
    }

    private void Update()
    {
        if(create_mode == true & test_mode == false)
        {
            Vector2 position = Input.mousePosition;
            Vector2 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

            int mouse_posX = (int)(screenToWorldPointPosition.x / 25f);
            int mouse_posY = (int)(screenToWorldPointPosition.y / 25f);

            Transform transform = tool.transform;
            Vector3 pos = transform.position;
            pos.x = mouse_posX * 25 + 12.5f;
            pos.y = mouse_posY * 25 + 12.5f + tool_height;
            transform.position = pos;

            if (position.x >= 1300)
            {
                if (tool_select == false)
                {
                    tool.SetActive(false);
                    tool_select = true;
                }
            }
            else
            {
                if (tool_select == true)
                {
                    tool.SetActive(true);
                    tool_select = false;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if(tool_select == false & tool_set == true)
                {
                    ChangeMap(mouse_posX, mouse_posY);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (tool_set == false)
                {
                    tool_set = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (tool_set == true)
                {
                    tool_set = false;
                }
            }
        }

        if (create_mode == true)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SetTest();
            }
        }

        if(test_mode == true)
        {
            if (check_space_count == check_space_count_max)
            {
                string filePath = Application.dataPath + @"\Resources\stage" + box_num + ".txt";

                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath))
                    {
                    }
                }

                File.AppendAllText(filePath, "\n            {\n");

                int map_size_y = box_map.GetLength(0);
                int map_size_x = box_map.GetLength(1);
                for (int y = 0; y < map_size_y; y++)
                {
                    string low = "                {";

                    for (int x = 0; x < map_size_x; x++)
                    {
                        low += box_map[y, x] + ",";
                    }

                    low += "},\n";
                    File.AppendAllText(filePath, low);
                }

                File.AppendAllText(filePath, "            }\n            ,");
                Debug.Log("マップ出力完了");

                SetTest();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                string filePath = Application.dataPath + @"\Resources\stage" + box_num + ".txt";

                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath))
                    {
                    }
                }

                File.AppendAllText(filePath, "\n            {\n");

                int map_size_y = box_map.GetLength(0);
                int map_size_x = box_map.GetLength(1);
                for (int y = 0; y < map_size_y; y++)
                {
                    string low = "                {";

                    for (int x = 0; x < map_size_x; x++)
                    {
                        low += box_map[y, x] + ",";
                    }

                    low += "},\n";
                    File.AppendAllText(filePath, low);
                }

                File.AppendAllText(filePath, "            }\n            ,");
                Debug.Log("マップ出力完了");

                SetTest();
            }
        }
    }

    public void SelectMap(int num)
    {
        buttons.SetActive(false);
        CreateMap(num);
        box_num = num;
        create_mode = true;
    }
    public void SelectTool(int num)
    {
        tool_num = num;
        tool_sr.sprite = tool_sprites[num];

        if(num == 2)
        {
            tool_height = 12.5f;
        }
        else if(num == 4 | num == 5)
        {
            tool_height = 14f;
        }
        else if(num == 8)
        {
            tool_height = -12.5f;
        }
        else
        {
            tool_height = 0;
        }
    }
    public void ChangeTools(int num)
    {
        tools[tools_num].SetActive(false);
        tools[num].SetActive(true);
        tools_num = num;
    }
    public void ChangeToolKind(int num)
    {
        tool_kind_num = num;

        if(num >= 23 & num % 2 == 1)
        {
            if((num - 23) % 4 == 0)
            {
                int index = (num - 23) / 4;
                tool_sr.sprite = scc.obj_decoration_1to1_sprite[index];
                tool_height = 12.5f;
            }
        }

        if (num >= 24 & num % 2 == 0)
        {
            if ((num - 24) % 4 == 0)
            {
                int index = (num - 24) / 4;
                tool_sr.sprite = scc.obj_decoration_wall_1to1_sprite[index];
                tool_height = -12.5f;
            }
        }
    }

    private void SetTest()
    {
        if(test_mode == false)
        {
            if(default_item_count[box_num] - item_count <= 0 & default_talk_count[box_num] - talk_count <= 0)
            {
                int maxY = box_map.GetLength(0);
                int maxX = box_map.GetLength(1);

                bool can_not = false;
                bool exit_space = false;
                int min_space_count = scc.box_enemies_count_list[box_num] + 1;
                int space_count = 0;
                for (int y = 0; y < maxY; y++)
                {
                    for (int x = 0; x < maxX; x++)
                    {
                        if (box_map[y, x] == 1)
                        {
                            space_count += 1;

                            int count = 0;

                            for (int i = 0; i < 8; i++)
                            {
                                int searchX = x + eight_dir[i, 0];
                                int searchY = y + eight_dir[i, 1];

                                int searchnum = box_map[searchY, searchX];
                                if (searchnum != 7 & searchnum != 9 & searchnum != 11 & searchnum != 21)
                                {
                                    if (i != 2)
                                    {
                                        count += 1;
                                    }
                                    else
                                    {
                                        if (!(searchnum == 17 | searchnum >= 23))
                                        {
                                            count += 1;
                                        }
                                    }
                                }
                            }

                            if (y < maxY - 2)
                            {
                                int searchnum = box_map[y + 2, x];
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
                                exit_space = true;
                            }
                        }
                    }
                }
                if(space_count < min_space_count)
                {
                    Debug.Log("敵(" + scc.box_enemies_count_list[box_num] + "体)とプレイヤーを置くスペースが無い");
                    can_not = true;
                }
                if(exit_space == false)
                {
                    Debug.Log("出口を置くスペースが無い");
                    can_not = true;
                }
                if(can_not == true)
                {
                    return;
                }

                test_mode = true;

                for (int i = 0; i < 100; i++)
                {
                    int pos_x = Random.Range(0, maxX);
                    int pos_y = Random.Range(0, maxY);
                    if (box_map[pos_y, pos_x] == 1)
                    {
                        float jun_pos_x = pos_x * 25 + 12;
                        float jun_pos_y = (maxY - pos_y - 1) * 25 + 30;
                        Move.MoveLocalPos(jun, jun_pos_x, jun_pos_y);
                        Transform ctransform = Camera.main.transform;
                        Vector3 cpos = ctransform.position;
                        cpos.x = jun_pos_x;
                        cpos.y = jun_pos_y;
                        ctransform.position = cpos;

                        break;
                    }
                }

                CheckSpaceParent.SetActive(true);
                jun.SetActive(true);
                jun.transform.parent = Data.rooms[3].transform;
                jun.GetComponent<JunConfig>().canmove = true;
                jun.GetComponent<JunConfig>().myroom = 3;
                jun.GetComponent<JunConfig>().myroom_obj = Data.rooms[3];

                tools[tools_num].SetActive(false);
                undo.SetActive(false);
                tool.SetActive(false);

                GameObject checkspaceparent = Instantiate(CheckSpaceParent);
                CheckSpaceParent_Active = checkspaceparent;
                checkspaceparent.transform.parent = grid.transform;

                int box_minX = scc.active_box_size[3].x;
                int box_minY = scc.active_box_size[3].y;
                foreach (int[] info in scc.active_box_move[3])
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

                    GameObject checkspace = Instantiate(CheckSpace);
                    Move.MoveLocalPos(checkspace, x * 25 + 12.5f, (maxY - y - 1) * 25 + 12.5f);
                    checkspace.SetActive(true);
                    checkspace.transform.parent = checkspaceparent.transform;

                    check_space_count_max += 1;
                }

                for (int y = 0; y < maxY; y++)
                {
                    for (int x = 0; x < maxX; x++)
                    {
                        if (box_map[y, x] == 1)
                        {
                            GameObject checkspace = Instantiate(CheckSpace);
                            Move.MoveLocalPos(checkspace, x * 25 + 12.5f, (maxY - y - 1) * 25 + 12.5f);
                            checkspace.SetActive(true);
                            checkspace.transform.parent = checkspaceparent.transform;

                            check_space_count_max += 1;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("トークキャラかボックスの設置個数が足りてない");
            }
            
        }
        else
        {
            test_mode = false;

            Destroy(CheckSpaceParent_Active);
            jun.SetActive(false);
            jun.transform.parent = grid.transform;
            tools[tools_num].SetActive(true);
            undo.SetActive(true);
            tool.SetActive(true);
            CheckSpaceParent.SetActive(false);
            check_space_count = 0;
            check_space_count_max = 0;
        }
    }

    public void UndoMap()
    {
        if(undo_box.Count > 0)
        {
            int n = undo_box.Count - 1;
            int undo_map_size_y = undo_box[n].GetLength(0);
            int undo_map_size_x = undo_box[n].GetLength(1);
            int[,] undo_map = new int[undo_map_size_y, undo_map_size_x];

            for (int y = 0; y < undo_map_size_y; y++)
            {
                for (int x = 0; x < undo_map_size_x; x++)
                {
                    undo_map[y, x] = undo_box[n][y, x];
                }
            }

            box_map = undo_map;
            item_count = undo_item_count[n];
            talk_count = undo_talk_count[n];
            wall_deco_pos_list = new List<int>(undo_wall_deco_pos_list[n]);
            wall_deco_num_list = new List<int>(undo_wall_deco_num_list[n]);

            undo_box.RemoveAt(n);
            undo_item_count.RemoveAt(n);
            undo_talk_count.RemoveAt(n);
            undo_wall_deco_pos_list.RemoveAt(n);
            undo_wall_deco_num_list.RemoveAt(n);
            CreateMap(box_num);
        }
    }
    private void SetUndoQueue()
    {
        int map_size_y = box_map.GetLength(0);
        int map_size_x = box_map.GetLength(1);
        int[,] undo_map = new int[map_size_y, map_size_x];

        for (int y = 0; y < map_size_y; y++)
        {
            for (int x = 0; x < map_size_x; x++)
            {
                undo_map[y, x] = box_map[y, x];
            }
        }

        undo_box.Add(undo_map);
        undo_item_count.Add(item_count);
        undo_talk_count.Add(talk_count);
        undo_wall_deco_pos_list.Add(new List<int>(wall_deco_pos_list));
        undo_wall_deco_num_list.Add(new List<int>(wall_deco_num_list));

        if (undo_box.Count >= 30)
        {
            undo_box.RemoveAt(0);
            undo_item_count.RemoveAt(0);
            undo_talk_count.RemoveAt(0);
            undo_wall_deco_pos_list.RemoveAt(0);
            undo_wall_deco_num_list.RemoveAt(0);
        }
        scc.printmap(undo_map);
    }
    private void ChangeMap(int world_x, int world_y)
    {
        int x = world_x;
        int y = box_map.GetLength(0) - world_y - 1;

        if((0 <= x & x < box_map.GetLength(1)) & (0 <= y & y < box_map.GetLength(0)))
        {
            //消しゴム
            if(tool_num == 0)
            {
                if(box_map[y, x] == 1)
                {
                    if(!cantset_floor.Contains(x * 100 + y) & (box_map[y - 1, x] != 11 & box_map[y - 1, x] != 21) & (box_map[y - 2, x] != 11 & box_map[y - 2, x] != 21))
                    {
                        SetUndoQueue();
                        box_map[y, x] = 0;
                        CreateMap(box_num);

                        int wall_deco_pos = x * 100 + (y - 2);
                        if (wall_deco_pos_list.Contains(wall_deco_pos))
                        {
                            int n = wall_deco_pos_list.IndexOf(wall_deco_pos);
                            wall_deco_pos_list.RemoveAt(n);
                            wall_deco_num_list.RemoveAt(n);
                            CreateMap(box_num);
                            tool_set = false;
                        }
                        wall_deco_pos = x * 100 + (y - 3);
                        if (wall_deco_pos_list.Contains(wall_deco_pos))
                        {
                            int n = wall_deco_pos_list.IndexOf(wall_deco_pos);
                            wall_deco_pos_list.RemoveAt(n);
                            wall_deco_num_list.RemoveAt(n);
                            CreateMap(box_num);
                            tool_set = false;
                        }
                        wall_deco_pos = x * 100 + (y - 4);
                        if (wall_deco_pos_list.Contains(wall_deco_pos))
                        {
                            int n = wall_deco_pos_list.IndexOf(wall_deco_pos);
                            wall_deco_pos_list.RemoveAt(n);
                            wall_deco_num_list.RemoveAt(n);
                            CreateMap(box_num);
                            tool_set = false;
                        }
                    }
                }
                else if (box_map[y, x] == 17)
                {
                    SetUndoQueue();
                    box_map[y, x] = 1;
                    tool_set = false;
                    CreateMap(box_num);
                }
                else if (box_map[y, x] == 7)
                {
                    SetUndoQueue();
                    box_map[y, x] = 1;
                    tool_set = false;
                    CreateMap(box_num);
                }
                else if (box_map[y, x] == 9)
                {
                    SetUndoQueue();
                    box_map[y, x] = 1;
                    tool_set = false;
                    CreateMap(box_num);
                }
                else if(box_map[y, x] >= 4 & box_map[y, x] % 2 == 0)
                {
                    int wall_deco_pos = x * 100 + y;
                    if (wall_deco_pos_list.Contains(wall_deco_pos))
                    {
                        int n = wall_deco_pos_list.IndexOf(wall_deco_pos);
                        SetUndoQueue();
                        wall_deco_pos_list.RemoveAt(n);
                        wall_deco_num_list.RemoveAt(n);
                        CreateMap(box_num);
                        tool_set = false;
                    }
                }

                else if(box_map[y, x] >= 23 & (box_map[y, x] - 23) % 4 == 0)
                {
                    SetUndoQueue();
                    box_map[y, x] = 1;
                    tool_set = false;
                    CreateMap(box_num);
                }

                else if(box_map[y, x] == 11)
                {
                    SetUndoQueue();
                    box_map[y, x] = 1;
                    item_count -= 1;
                    tool_set = false;
                    CreateMap(box_num);
                }

                else if (box_map[y, x] == 21)
                {
                    box_map[y, x] = 1;

                    if (box_map[y, x + 1] == 21)
                    {
                        box_map[y, x + 1] = 1;
                    }
                    else if (box_map[y, x - 1] == 21)
                    {
                        box_map[y, x - 1] = 1;
                    }

                    SetUndoQueue();
                    talk_count -= 1;
                    tool_set = false;
                    CreateMap(box_num);
                }
            }

            else if(tool_num == 1)
            {
                if((box_map[y, x] % 2 == 0 | box_map[y, x] == 3) & default_box_list[box_num][y, x] == 1)
                {
                    if(box_map[y, x] % 2 == 0)
                    {
                        if (box_map[y - 1, x] % 2 == 0)
                        {
                            SetUndoQueue();
                            box_map[y, x] = 1;

                            if (box_map[y - 3, x] != 3)
                            {
                                box_map[y - 1, x] = 1;
                                box_map[y - 2, x] = 1;
                            }
                            int wall_deco_pos = x * 100 + (y - 1);
                            if (wall_deco_pos_list.Contains(wall_deco_pos))
                            {
                                int n = wall_deco_pos_list.IndexOf(wall_deco_pos);
                                wall_deco_pos_list.RemoveAt(n);
                                wall_deco_num_list.RemoveAt(n);
                                CreateMap(box_num);
                                tool_set = false;
                            }

                            CreateMap(box_num);
                        }
                    }
                    else if (box_map[y, x] == 3)
                    {
                        if(box_map[y - 1, x] != 3 & box_map[y + 1, x] == 3)
                        {
                            SetUndoQueue();
                            box_map[y, x] = 1;
                            CreateMap(box_num);
                        }
                        else if (box_map[y - 1, x] == 3 & box_map[y + 1, x] == 3)
                        {
                            if(box_map[y - 2, x] == 3 & box_map[y - 3, x] == 3)
                            {
                                SetUndoQueue();
                                box_map[y, x] = 1;
                                CreateMap(box_num);
                            }
                        }
                    }
                }
            }

            else if(tool_num == 2)
            {
                if (!cantset_obj.Contains(x * 100 + y))
                {
                    if(box_map[y, x] == 1)
                    {
                        if (box_map[y + 1, x] != 7 & box_map[y + 1, x] != 9)
                        {
                            SetUndoQueue();
                            box_map[y, x] = tool_kind_num;
                            CreateMap(box_num);
                        }
                    }
                }
            }

            else if(tool_num == 3)
            {
                if(box_map[y, x] == 2)
                {
                    if(box_map[y + 1, x] == 2)
                    {
                        int wall_deco_pos = x * 100 + y;
                        if (!wall_deco_pos_list.Contains(wall_deco_pos))
                        {
                            SetUndoQueue();
                            wall_deco_pos_list.Add(wall_deco_pos);
                            wall_deco_num_list.Add(4);
                            CreateMap(box_num);
                        }
                    }
                }
            }

            else if(tool_num == 4)
            {
                if (!cantset_obj.Contains(x * 100 + y))
                {
                    if (box_map[y, x] == 1)
                    {
                        SetUndoQueue();
                        box_map[y, x] = 7;
                        CreateMap(box_num);
                    }
                }
            }

            else if (tool_num == 5)
            {
                if (!cantset_obj.Contains(x * 100 + y))
                {
                    if (box_map[y, x] == 1)
                    {
                        if (box_map[y, x - 1] != 9 & box_map[y, x + 1] != 9)
                        {
                            SetUndoQueue();
                            box_map[y, x] = 9;
                            CreateMap(box_num);
                        }
                    }
                }
            }

            else if(tool_num == 6)
            {
                if (!cantset_obj.Contains(x * 100 + y))
                {
                    if (box_map[y, x] % 2 == 1 & box_map[y, x] != 3)
                    {
                        if (box_map[y, x] != 11)
                        {
                            bool set = true;
                            for (int d = 0; d < 8; d++)
                            {
                                int searchX = x + eight_dir[d, 0];
                                int searchY = y + eight_dir[d, 1];
                                if (box_map[searchY, searchX] == 11 | box_map[searchY, searchX] == 21 | box_map[searchY, searchX] == 9)
                                {
                                    set = false;
                                    break;
                                }
                            }

                            if (set == true)
                            {
                                SetUndoQueue();
                                box_map[y, x] = 11;
                                item_count += 1;
                                CreateMap(box_num);
                            }
                        }
                    }
                }
            }

            else if (tool_num == 7)
            {
                if(box_map[y, x] % 2 == 1 & box_map[y, x] != 3)
                {
                    if (!cantset_obj.Contains(x * 100 + y) & !cantset_obj.Contains((x + 1) * 100 + y))
                    {
                        if (box_map[y, x + 1] % 2 == 1 & box_map[y, x + 1] != 3)
                        {
                            if (box_map[y, x + 1] != 21 & box_map[y, x] != 21)
                            {
                                bool set = true;
                                for (int d = 0; d < 8; d++)
                                {
                                    int searchX = x + eight_dir[d, 0];
                                    int searchY = y + eight_dir[d, 1];
                                    if (box_map[searchY, searchX] == 11 | box_map[searchY, searchX] == 21 | box_map[searchY, searchX] == 9)
                                    {
                                        set = false;
                                        break;
                                    }
                                }
                                for (int d = 0; d < 8; d++)
                                {
                                    int searchX = x + 1 + eight_dir[d, 0];
                                    int searchY = y + eight_dir[d, 1];
                                    if (box_map[searchY, searchX] == 11 | box_map[searchY, searchX] == 21 | box_map[searchY, searchX] == 9)
                                    {
                                        set = false;
                                        break;
                                    }
                                }

                                if (set == true)
                                {
                                    SetUndoQueue();
                                    box_map[y, x] = 21;
                                    box_map[y, x + 1] = 21;
                                    talk_count += 1;
                                    CreateMap(box_num);
                                }
                            }
                        }
                    }
                }
            }

            else if (tool_num == 8)
            {
                if (box_map[y, x] == 2)
                {
                    if(box_map[y + 1, x] == 2)
                    {
                        int wall_deco_pos = x * 100 + y;
                        if (!wall_deco_pos_list.Contains(wall_deco_pos))
                        {
                            SetUndoQueue();
                            wall_deco_pos_list.Add(wall_deco_pos);
                            wall_deco_num_list.Add(tool_kind_num);
                            CreateMap(box_num);
                        }
                    }
                }
            }
        }
    }

    private void CreateMap(int num)
    {
        scc.item_canplace = new List<(int, int, int, GameObject, int)>();
        scc.talk_canplace = new List<(int, int, int, GameObject, int)>();

        if (create_mode == false)
        {
            tool.SetActive(true);
            int[,] default_map = default_box_list[num];
            int default_map_size_y = default_map.GetLength(0);
            int default_map_size_x = default_map.GetLength(1);

            for (int y = 0; y < default_map_size_y; y++)
            {
                for (int x = 0; x < default_map_size_x; x++)
                {
                    if(default_map[y, x] == -1)
                    {
                        cantset_floor.Add(x * 100 + y);
                        cantset_obj.Add(x * 100 + y);
                        default_map[y, x] = 1;
                    }
                    else if (default_map[y, x] == -2)
                    {
                        cantset_floor.Add(x * 100 + y);
                        cantset_floor.Add(x * 100 + (y + 1));
                        cantset_floor.Add(x * 100 + (y + 2));
                        cantset_obj.Add(x * 100 + y);
                        default_map[y, x] = 1;
                    }
                }
            }

            int[,] map = new int[default_map_size_y, default_map_size_x];
            for (int y = 0; y < default_map_size_y; y++)
            {
                for (int x = 0; x < default_map_size_x; x++)
                {
                    map[y, x] = default_map[y, x];
                }
            }
            box_map = map;

            Transform ctransform = Camera.main.transform;
            Vector3 cpos = ctransform.position;
            cpos.x = 100;
            cpos.y = 100;
            ctransform.position = cpos;
        }
        else
        {
            Destroy(Data.rooms[3]);
        }

        scc.active_box_move[3] = new List<int[]>();

        int map_size_y = box_map.GetLength(0);
        int map_size_x = box_map.GetLength(1);
        for (int y = 0; y < map_size_y; y++)
        {
            for (int x = 0; x < map_size_x; x++)
            {
                if(box_map[y, x] == 3 | box_map[y, x] % 2 == 0)
                {
                    box_map[y, x] = 0;
                }
            }
        }
        for (int y = map_size_y - 1; y >= 0; y--)
        {
            for (int x = map_size_x - 1; x >= 0; x--)
            {
                if (box_map[y, x] % 2 == 1)
                {
                    if (box_map[y - 1, x] == 0)
                    {
                        box_map[y - 1, x] = 2;

                        int wall_deco_pos = x * 100 + (y - 2);
                        if (wall_deco_pos_list.Contains(wall_deco_pos))
                        {
                            int n = wall_deco_pos_list.IndexOf(wall_deco_pos);
                            box_map[y - 2, x] = wall_deco_num_list[n];
                        }
                        else
                        {
                            box_map[y - 2, x] = 2;
                        }

                        box_map[y - 3, x] = 0;
                    }
                }
            }
        }
        for (int y = 0; y < map_size_y; y++)
        {
            for (int x = 0; x < map_size_x; x++)
            {
                if (box_map[y, x] == 0)
                {
                    for(int d=0; d<8; d++)
                    {
                        int dirX = eight_dir[d, 0];
                        int dirY = eight_dir[d, 1];
                        int searchX = x + dirX;
                        int searchY = y + dirY;

                        if ((0 <= searchX & searchX < map_size_x) & (0 <= searchY & searchY < map_size_y))
                        {
                            if (box_map[searchY, searchX] != 0)
                            {
                                box_map[y, x] = 3;
                                break;
                            }
                        }
                    }
                }
            }
        }

        int exit_map_size_y = box_map.GetLength(0);
        int exit_map_size_x = box_map.GetLength(1);
        int[,] exit_map = new int[exit_map_size_y, exit_map_size_x];

        for (int y = 0; y < exit_map_size_y; y++)
        {
            for (int x = 0; x < exit_map_size_x; x++)
            {
                exit_map[y, x] = box_map[y, x];
            }
        }

        Data.active_box_map[3] = exit_map;
        scc.active_box_map_pos[3] = new Vector2Int(0, 0);

        int[,] boxsize_map = default_boxsize_list[num];
        int boxsize_y = boxsize_map.GetLength(0);
        int boxsize_x = boxsize_map.GetLength(1);
        scc.active_box_size[3] = new Vector2Int(0, 0);
        for (int y = 0; y < boxsize_y; y++)
        {
            for (int x = 0; x < boxsize_x; x++)
            {
                if (x % 2 == 0 & y % 2 == 0)
                {
                    if (boxsize_map[y, x] == 1)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            int dirX = four_dir[d, 0];
                            int dirY = four_dir[d, 1];
                            int searchX = x + dirX;
                            int searchY = y + dirY;

                            if ((0 <= searchX & searchX < boxsize_x) & (0 <= searchY & searchY < boxsize_y))
                            {
                                if (boxsize_map[searchY, searchX] == 0)
                                {
                                    CreateWay(x, y, dirX, dirY);
                                }
                            }
                            else
                            {
                                CreateWay(x, y, dirX, dirY);
                            }
                        }
                    }
                }
            }
        }

        scc.SetRoom(3);

        //item
        item_count_text.text = "" + (default_item_count[num] - item_count);
        if (scc.item_canplace.Count != 0)
        {
            for (int i = 0; i < item_count; i++)
            {
                int item_canplace_num = Random.Range(0, scc.item_canplace.Count);
                (int x, int y, int mymaxY, GameObject parent, int key) = scc.item_canplace[item_canplace_num];
                scc.SetObj(x, y, mymaxY, 19, parent, null, null, key);

                scc.item_canplace.RemoveAt(item_canplace_num);
            }
        }
        
        //talk
        talk_count_text.text = "" + (default_talk_count[num] - talk_count);
        for (int i = 0; i < talk_count; i++)
        {
            (List<List<(int, int, int, string)>> list, int chara1, int chara2, bool flip) = Status.random_message_info_1to2[0];
            int talk_canplace_num = Random.Range(0, scc.talk_canplace.Count);
            (int x, int y, int mymaxY, GameObject parent, int key) = scc.talk_canplace[talk_canplace_num];

            GameObject obj = Instantiate(scc.obj_chara_1to2);
            obj.transform.parent = parent.transform;
            float posX = x * 25f + 25f;
            float posY = (mymaxY - y - 1) * 25f + 12.5f;
            Move.MoveLocalPosZ(obj, posX, posY, posY / 10f - 0.5f);
            obj.SetActive(true);

            GameObject chara1_obj = obj.transform.GetChild(0).gameObject;
            CharaAnimationScript chara1_anim = chara1_obj.GetComponent<CharaAnimationScript>();
            Move.MoveLocalPosY(chara1_obj, scc.talk_pos[chara1]);
            Sprite[] chara1_sprite = scc.talk_sp[chara1];
            chara1_anim.speed = scc.talk_speed[chara1];
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
            Move.MoveLocalPosY(chara2_obj, scc.talk_pos[chara2]);
            Sprite[] chara2_sprite = scc.talk_sp[chara2];
            chara2_anim.speed = scc.talk_speed[chara2];
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
            scc.talk_canplace.RemoveAt(talk_canplace_num);
        }
    }

    private void CreateWay(int x, int y, int dirX, int dirY)
    {
        if (!scc.active_box_move.ContainsKey(3))
        {
            scc.active_box_move[3] = new List<int[]> { new int[4] { x, y, dirX, dirY } };
        }
        else
        {
            scc.active_box_move[3].Add(new int[4] { x, y, dirX, dirY });
        }
    }
}
