using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDecidedConfig : MonoBehaviour
{
    private const int ANIM_SPEED = 6;

    [SerializeField] private FuncMove Move;
    [SerializeField] private DataBattle Data;
    [SerializeField] private Image img;
    public GameObject parent;

    private int item_index;

    private int t_anim;

    // Update is called once per frame
    void FixedUpdate()
    {
        t_anim += 1;

        if(t_anim == ANIM_SPEED)
        {
            img.sprite = Data.item_sprite[item_index * 3 + 2];
        }

        if (t_anim == ANIM_SPEED * 2)
        {
            img.sprite = Data.item_sprite[item_index * 3];
        }
    }

    public void SetItemDecided(int num, int index)
    {
        Move.SetSamePos(parent, Data.item_obj[num], 0, 0.5f);
        parent.SetActive(true);
        item_index = index;
        img.sprite = Data.item_sprite[index * 3 + 1];
        Data.menu_items[num].SetActive(false);
        t_anim = 0;
    }
}
