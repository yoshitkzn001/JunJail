using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataBattle : MonoBehaviour
{
    // ABC順
    public Animator ani_jun;
    public int[] battle_start_time;
    public GameObject[] battle_walk_box;
    public SpriteRenderer[] battle_walk_box_sr;
    public SpriteRenderer[] battle_enemy_sr;
    public SpriteRenderer battle_jun_sr;
    public GameObject battle_field;
    public GameObject bar_hp;
    public GameObject bar_hp_dec;
    public GameObject bar_sp;
    public GameObject bar_sp_dec;
    public GameObject buff_display;
    public Image buff_display_img;
    public Sprite[] buff_display_sprite;
    public Image[] button_first;
    public Sprite[] button_first_pushup;
    public Sprite[] button_first_pushdown;
    public Image[] button_second_arrow;
    public Sprite[] button_second_arrow_pushup;
    public Sprite[] button_second_arrow_pushdown;
    public GameObject button_first_cursor;
    public GameObject controller_guard;
    public GameObject enemy_names;
    public CanvasGroup enemy_names_alpha;
    public GameObject fade;
    public Image fade_img;
    public GameObject fig_dispay_parent;
    public CanvasGroup fig_dispay_parent_alpha;
    public GameObject[] fig_hp_display_obj;
    public Image[] fig_hp_display;
    public Image[] fig_hp;
    public Image[] fig_maxhp;
    public Image[] fig_sp;
    public Image[] fig_spnum;
    public Image[] fig_maxsp;
    public Image[] fig_page;
    public Image[] fig_maxpage;
    public GameObject hp_slash;
    public Material mat_outline_enemy;
    public Material mat_outline_jun;
    public Material mat_nomal;
    public GameObject[] menu_button_firstselect;
    public GameObject[] menu_button_leftright;
    public GameObject menu_button_return;
    public GameObject[] menu_line;
    public GameObject menu_item_message;
    public GameObject menu_message;
    public GameObject menu_message_textobj;
    public GameObject menu_name;
    public GameObject[] menu_roms;
    public GameObject[] menu_items;
    public GameObject[] menu_sps;
    public GameObject[] menu_values;
    public GameObject menu_HP;
    public GameObject menu_SP;
    public GameObject move_firstbuttons;
    public GameObject move_second;
    public GameObject move_secondbuttons_box;
    public GameObject move_secondbuttons_roms;
    public GameObject move_secondbuttons_roms_sp;
    public GameObject move_secondbuttons_status_roms_level;
    public GameObject move_secondbuttons_items;
    public GameObject move_secondbuttons_items_value;
    public GameObject move_secondbuttons_run;
    public GameObject move_returnbutton;
    public CanvasGroup run_messages_alpha;
    public GameObject[] run_messages;
    public GameObject run_buttons_box;
    public GameObject run_button_selected;
    public Image run_button_selected_image;
    public GameObject[] run_buttons;
    public Image[] run_buttons_image;
    public Sprite[] run_buttons_sprite;
    public GameObject rom_box;
    public GameObject rom_decided;
    public Sprite[] rom_get_sprite;
    public Image rom_decided_image;
    public Sprite[] rom_decided_animation;
    public Sprite[] rom_decided_done_animation;
    public GameObject rom_decided_label;
    public Image rom_decided_label_image;
    public Sprite[] rom_label_sprite;
    public Sprite[] rom_sprite;
    public Image[] rom_label_image;
    public Image[] rom_image;
    public Image[] rom_sp;
    public GameObject[] rom_status_level;
    public Image[] rom_status_level_image;
    public Sprite[] rom_status_speite;
    public Text item_text_name;
    public Text item_text_message;
    public GameObject[] item_obj;
    public Image[] item_button_image;
    public Sprite[] item_button_sprite;
    public Image[] item_image;
    public Sprite[] item_sprite;
    public Image[] item_value;
    public GameObject item_box;
    public GameObject sp_slash;
    public GameObject[] skills;
    public GameObject buttons_target;
    public GameObject win_message1;
    public TextMeshProGeometryAnimator win_message1_ani;
    public GameObject win_message2;
    public TextMeshProUGUI win_message2_text;
    public TextMeshProGeometryAnimator win_message2_ani;
    public GameObject win_message_cursor;
    public CanvasGroup win_message_alpha;
    public GameObject level_parent;
    public CanvasGroup level_parent_alpha;
    public Image level_value_text;
    public Image level_text;
    public CanvasGroup levelup_text_alpha;
    public GameObject levelup_text;
    public Text levelup_value_text;
    public GameObject level_bar;
}
