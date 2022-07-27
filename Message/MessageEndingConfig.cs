using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageEndingConfig : MonoBehaviour
{
    [SerializeField] EndConfig endC;
    [SerializeField] CriAtomSource nomalSE;
    [SerializeField] FuncMove Move;
    [SerializeField] GameObject box;
    [SerializeField] GameObject chara_box;
    [SerializeField] GameObject text_obj;
    [SerializeField] GameObject text_cursor;
    [SerializeField] TextMeshProGeometryAnimator text_ani;
    [SerializeField] Image message_chara_image;
    [SerializeField] Text message_name;
    [SerializeField] TextMeshProUGUI message_text;

    [SerializeField] Sprite sprite_alpha;
    [SerializeField] Sprite sprite_box;
    [SerializeField] Sprite[] sprite_jun;
    [SerializeField] Sprite[] sprite_emon;
    [SerializeField] Sprite[] sprite_emonsniper;
    [SerializeField] Sprite[] sprite_mokokids;
    [SerializeField] Sprite[] sprite_hato;
    private List<Sprite[]> sprites = new List<Sprite[]>();

    [SerializeField] string[] chara_names;
    public List<(int, int, int, string)> message_info = new List<(int, int, int, string)>(); //位置、キャラ、表情、セリフ

    private bool text_read;
    private int text_step;
    private int text_step_max;
    private const int BOX_TIME = 3;
    private const int BOX_CLOSE = 9;
    private int t_open;
    private int t_close;

    public bool[] action_timing;

    private bool boss_battle;
    private GameObject boss_object;

    // Start is called before the first frame update
    void Start()
    {
        sprites.Add(sprite_jun);
        sprites.Add(sprite_mokokids);
        sprites.Add(sprite_hato);

        text_obj.SetActive(false);
        chara_box.SetActive(false);
        box.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t_open > 0)
        {
            t_open -= 1;

            if (t_open == BOX_TIME * 2)
            {
                BoxSizePos(-108.5f, 73f);
                chara_box.SetActive(true);
            }

            if (t_open == BOX_TIME)
            {
                BoxSizePos(-109.5f, 72f);
            }

            if (t_open == 0)
            {
                BoxSizePos(-110.5f, 71f);
                text_obj.SetActive(true);
                t_open = -1;
            }
        }

        if (t_close > 0)
        {
            t_close -= 1;

            if (t_close == BOX_TIME * (BOX_CLOSE - 1))
            {
                BoxSizePos(-108.5f, 73f);
            }

            if (t_close == BOX_TIME * (BOX_CLOSE - 2))
            {
                BoxSizePos(-113.5f, 20f);
                chara_box.SetActive(false);
            }

            if (t_close == BOX_TIME * (BOX_CLOSE - 3))
            {
                box.GetComponent<Image>().sprite = sprite_alpha;
            }

            if (t_close == BOX_TIME * (BOX_CLOSE - 5))
            {
                Close();
            }
        }
    }

    private void Update()
    {
        if (text_read == false)
        {
            if (text_ani.progress >= 1.0f)
            {
                text_read = true;
                text_cursor.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(0) | Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.E))
        {
            if (t_close == -1 & t_open == -1)
            {
                if (text_read == true)
                {
                    text_cursor.SetActive(false);
                    text_obj.SetActive(false);

                    if (text_step == text_step_max)
                    {
                        t_close = BOX_TIME * BOX_CLOSE;
                        BoxSizePos(-109.5f, 72f);
                        nomalSE.Play("message2");
                    }
                    else
                    {
                        SetText();
                        text_obj.SetActive(true);
                        nomalSE.Play("message1");
                    }
                }
                else
                {
                    text_read = true;
                    text_ani.Skip();
                    text_cursor.SetActive(true);
                }
            }
        }
    }

    private void BoxSizePos(float y, float sizeY)
    {
        RectTransform recttransform = box.GetComponent<RectTransform>();
        Vector2 size = recttransform.sizeDelta;
        size.y = sizeY;
        recttransform.sizeDelta = size;

        Move.MoveLocalPosY(box, y);
    }

    public void Open(bool boss, GameObject boss_obj)
    {
        t_close = -1;
        text_step = 0;
        t_open = BOX_TIME * 3;
        box.SetActive(true);
        BoxSizePos(-113.5f, 20f);
        text_step_max = message_info.Count;
        SetText();

        boss_battle = boss;
        boss_object = boss_obj;

        Debug.Log(true);
    }

    private void Close()
    {
        box.GetComponent<Image>().sprite = sprite_box;
        box.SetActive(false);
        t_close = 0;
        t_open = 0;
        text_step = 0;
        chara_box.SetActive(false);
        text_cursor.SetActive(false);
        text_obj.SetActive(false);
        text_read = true;

        endC.MessageClose();
    }

    private void SetText()
    {
        (int pos, int chara, int face, string text) = message_info[text_step];

        if (pos == -1 | pos == 1)
        {
            Transform transform = message_chara_image.transform;
            Vector3 scale = transform.localScale;
            scale.x = -pos;
            transform.localScale = scale;

            if (pos == -1)
            {
                Move.MoveLocalPosX(chara_box, -112f);
                Move.MoveLocalPosX(text_obj, 28f);
            }
            else if (pos == 1)
            {
                Move.MoveLocalPosX(chara_box, 112f);
                Move.MoveLocalPosX(text_obj, -28f);
            }
        }

        message_chara_image.sprite = sprites[chara][face];
        message_name.text = chara_names[chara];
        message_text.text = text;

        if(action_timing[text_step] == true)
        {
            endC.Action(text_step);
        }

        text_step += 1;
        text_read = false;
    }
}
