using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomDecidedConfig : MonoBehaviour
{
    [SerializeField] private TaptostartConfig Taptostart;
    [SerializeField] private DataBattle Data;
    [SerializeField] private MasBattleConfig Master;
    [SerializeField] private FuncMove Move;
    [SerializeField] private GameObject Jun;

    [SerializeField] CriAtomSource nomalSE;

    private const int ANIM_UP_ROM_SPEED = 5;
    private const int ANIM_DONE_ROM_SPEED = 6;
    private const int UP_ROM = 36;
    private const int DOWN_ROM = 7;
    private const int DONE_ROM = 30; //ANIM_DONE_ROM_SPEED * num(3)
    private const int SKILL_MOVET = 25;

    private int level;
    private int skill;

    private bool move_rom;
    private bool second_set;
    private float posY_center;
    private float posY_up;
    private float posY_down;
    private int t_up_rom;
    private int t_down_rom;
    private int t_done_rom;
    private int t_open_skill;
    private float[] pos_taptostart = new float[12] { 0.5f, -54.5f, 0.5f, -74.5f, 0.5f, -79.5f , 0f, 0f, 0f, 0f, 0.5f, -72.5f};

    // Update is called once per frame
    void FixedUpdate()
    {
        //rom
        if(t_up_rom > 0)
        {
            MoveUpRom();
        }
        if(t_down_rom > 0)
        {
            MoveDonwRom();
        }
        if(t_done_rom > 0)
        {
            DoneRom();
        }

        //skill
        if(t_open_skill > 0)
        {
            MoveSkill();
        }

        //skill ready
        if(move_rom == true)
        {
            if(skill == 5)
            {
                Move.MoveLocalPosPlusX(Jun, -1);
            }
        }
    }

    //move
    private void MoveUpRom()
    {
        t_up_rom -= 1;
        if(t_up_rom % 2 == 0)
        {
            Move.DeccelMoveY(t_up_rom, UP_ROM, posY_center, posY_up, Data.rom_decided);
        }

        if (t_up_rom == UP_ROM - ANIM_UP_ROM_SPEED)
        {
            Data.rom_decided_image.sprite = Data.rom_decided_animation[(level - 1) * 3 + 1];
        }

        if (t_up_rom == UP_ROM - (ANIM_UP_ROM_SPEED * 2))
        {
            Data.rom_decided_image.sprite = Data.rom_decided_animation[(level - 1) * 3 + 2];
            Data.rom_decided_label.SetActive(true);
        }

        if (t_up_rom == 0)
        {
            t_down_rom = DOWN_ROM;
        }
    }
    private void MoveDonwRom()
    {
        t_down_rom -= 1;
        Move.AccelMoveY(t_down_rom, DOWN_ROM, posY_up, posY_down, Data.rom_decided);

        if(t_down_rom == 0)
        {
            Data.rom_decided_image.sprite = Data.rom_decided_done_animation[0];
            Data.rom_decided_label.SetActive(false);
            nomalSE.Play("romload1");
            t_done_rom = DONE_ROM;
            move_rom = false;

            if(skill != 5)
            {
                Data.ani_jun.SetInteger("skill", skill + 1);
            }

            if (skill == 4)
            {
                Master.voiceSE.Play("voice_skill5_" + Random.Range(1, 3));
            }

            if(skill == 5)
            {
                Data.ani_jun.SetInteger("step", 1);
            }
        }
    }
    private void DoneRom()
    {
        t_done_rom -= 1;

        if(t_done_rom % ANIM_DONE_ROM_SPEED == 0)
        {
            int T = (DONE_ROM - t_done_rom) / ANIM_DONE_ROM_SPEED;
            Data.rom_decided_image.sprite = Data.rom_decided_done_animation[T];

            if(t_done_rom / ANIM_DONE_ROM_SPEED == DONE_ROM / ANIM_DONE_ROM_SPEED - 1)
            {
                Data.skills[skill].SetActive(true);
                t_open_skill = SKILL_MOVET;
                if (second_set == false)
                {
                    Master.St_obj_config.CloseST(SKILL_MOVET);
                }
            }
        }
    }
    private void MoveSkill()
    {
        t_open_skill -= 1;
        Move.DeccelMoveY(t_open_skill, SKILL_MOVET, 0, 80, Data.skills[skill]);
        if (second_set == false)
        {
            Move.AccelMoveX(t_open_skill, SKILL_MOVET, Master.pos_move_HPSP[0], Master.pos_move_HPSP[1], Data.menu_SP);
            Move.AccelMoveX(t_open_skill, SKILL_MOVET, -Master.pos_move_HPSP[0], -Master.pos_move_HPSP[1], Data.menu_HP);
        }

        if (t_open_skill == 0)
        {
            if(skill != 3 & skill != 4)
            {
                Taptostart.SetStart(pos_taptostart[skill * 2], pos_taptostart[skill * 2 + 1]);
                Master.ReadyAttack();
            }
            else
            {
                Master.step = 8;
            }
        }
    }

    //config
    public void SetRomDecided(int myskill, int mylevel, float center, float up, float down, bool set = false)
    {
        second_set = set;
        posY_center = center;
        posY_up = up;
        posY_down = down;
        level = mylevel;
        skill = myskill;

        Data.rom_decided_label_image.sprite = Data.rom_label_sprite[myskill * 2];
        Data.rom_decided_image.sprite = Data.rom_decided_animation[(level - 1) * 3];

        t_up_rom = UP_ROM;

        move_rom = true;

        if(skill == 5)
        {
            Data.ani_jun.SetInteger("skill", skill + 1);
        }
    }
}
