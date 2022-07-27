using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OpConfig : MonoBehaviour
{
    const int SLEEP_TIME = 100;
    const int OPEN_PC_TIME = 60;
    const int OPEN_NICO_TIME = 60;
    const int CLOSE_PC_TIME = 60;

    [SerializeField] TextMeshProUGUI text_txt;
    [SerializeField] TextMeshProGeometryAnimator text_ani;
    [SerializeField] GameObject text_cursor;
    [SerializeField] GameObject text_obj;

    [SerializeField] GameObject y_comment;
    [SerializeField] Sprite[] y_comment_sprites;
    [SerializeField] Text[] y_comment_texts;
    [SerializeField] Image[] y_comment_renderers;

    Queue<string> y_comment_txt = new Queue<string>();
    Queue<int> y_comment_sprites_indexs = new Queue<int>();
    int y_comment_sprites_index;

    [SerializeField] GameObject n_comment;
    [SerializeField] CanvasGroup nico_alpha;
    [SerializeField] Sprite[] n_comment_sprites;
    [SerializeField] Text[] n_comment_texts;
    [SerializeField] Image[] n_comment_renderers;

    Queue<string> n_comment_txt = new Queue<string>();
    Queue<int> n_comment_sprites_indexs = new Queue<int>();

    [SerializeField] CanvasGroup pc_alpha;

    string[] messages = new string[]
    {
        "配信",
        "インターネットを通じて\n雑談やゲームプレイなどを生放送する行為である",
        "20XX年　それが　全国的に大流行していた",
        "大流行の影響で　若者の暴徒化\n教育への悪影響などの問題が発生した",
        "問題が大きくなり　日本政府は\n配信行為とその視聴を禁じた",
        "しかし　それでもなお　配信を続ける者",
        "そして　それでもなお　配信を見続ける者が存在していた",
        "そのような者たちを　投獄している塔がある",
        "ニコニコ監獄塔",
        "その一画　怒りが爆発している男がいた"
    };

    string[] youtube_comment = new string[]
    {
        "やあ",
        "草",
        "は？おもんなスタヌ行くわ　は？おもんなスタヌ行くわ　は？おもんなスタヌ行くわ",
        "この人は何を言ってるんですか？",
        "彼は冗談を言ってます",
        "初見です",
        "ポリコレへの配慮足りて無くないですか？",
        "は？",
        "わあ！？",
        "やあ",
        "誰も聞いてなくて草",
        "大将が背を向けて逃げるな",
        "俺は俺を許せねぇよぉ！",
        "加藤殿　拙者ポテチ8袋目でござるよwwww",
        "？",
        "あ",
        "あ",
        "あ",
        "あ",
        "あ",
        "あ",
    };
    string[] niconico_comment = new string[]
    {
        "やああああああああああ",
        "やあ",
        "やあああああああ",
        "wwwwwwwwwwwwwwwww",
        "wwwwwww",
        "wwwwwwwwwwww",
        "加藤純一最強！加藤純一最強！加藤純一最強！加藤純一最強！",
        "どりゃああああああああああああああああ",
        "どりゃあああああああああああああ",
        "よーーーーーーーーーーーーーし",
        "犯罪上等ちゃねら！",
        "よーーーーーーーーーーーーーーーーーーし",
        "wwwwwwwwwwwwwwwwwwwwwwww",
        "wwwwwwwwwwww",
        "オレタチが一番だ！"
    };

    bool text_read;
    int time;
    int step;

    int t_open_pc;
    int t_close_pc;
    int t_open_nico;

    // Start is called before the first frame update
    void Start()
    {
        time = SLEEP_TIME + 30;
        step = 0;

        Sound.myBGM.clip = Sound.BGM[1];
        Sound.myBGM.volume = 0.0f;
        Sound.myBGM.Play();

        for (int i=0; i< y_comment_texts.Length; i++)
        {
            string text = youtube_comment[Random.Range(0, youtube_comment.Length)];
            y_comment_texts[i].text = text;
            y_comment_renderers[i].sprite = y_comment_sprites[y_comment_sprites_index];

            y_comment_txt.Enqueue(text);
            y_comment_sprites_indexs.Enqueue(y_comment_sprites_index);

            y_comment_sprites_index += 1;
            if (y_comment_sprites_index == 2)
            {
                y_comment_sprites_index = 0;
            }
        }

        for (int i = 0; i < n_comment_texts.Length; i++)
        {
            string text = niconico_comment[Random.Range(0, niconico_comment.Length)];
            n_comment_texts[i].text = text;
            int index = Random.Range(0, 3);
            n_comment_renderers[i].sprite = n_comment_sprites[index];

            n_comment_txt.Enqueue(text);
            n_comment_sprites_indexs.Enqueue(index);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(time > 0)
        {
            time -= 1;

            if(step == 0)
            {
                Sound.myBGM.volume = 0.5f * (((SLEEP_TIME + 30) - time) / (float)(SLEEP_TIME + 30));
            }

            if(time == 0)
            {
                if(step != 11)
                {
                    text_txt.text = messages[step];
                    text_obj.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene("TutorialScene");
                }
            }
        }
        if(t_open_pc > 0)
        {
            t_open_pc -= 1;

            pc_alpha.alpha = ((OPEN_PC_TIME - t_open_pc) / (float)OPEN_PC_TIME) * 0.6f;

            if (t_open_pc == 0)
            {
                pc_alpha.alpha = 0.6f;
            }
        }
        if(t_open_nico > 0)
        {
            t_open_nico -= 1;

            nico_alpha.alpha = ((OPEN_PC_TIME - t_open_nico) / (float)OPEN_PC_TIME);

            if (t_open_nico == 0)
            {
                nico_alpha.alpha = 1.0f;
            }
        }
        if (t_close_pc > 0)
        {
            t_close_pc -= 1;

            pc_alpha.alpha = (t_close_pc / (float)CLOSE_PC_TIME) * 0.6f;

            if (t_close_pc == 0)
            {
                pc_alpha.alpha = 0f;
            }
        }

        if (step > 0 & step < 5)
        {
            int comment = Random.Range(0, 4);

            if(comment == 0)
            {
                YComment();
            }
        }
        else if(step > 4 & step < 9)
        {
            int comment = Random.Range(0, 12);

            if (comment == 0)
            {
                NComment();
            }
        }
    }

    void YComment()
    {
        y_comment_txt.Dequeue();
        y_comment_sprites_indexs.Dequeue();
        y_comment_txt.Enqueue(youtube_comment[Random.Range(0, youtube_comment.Length)]);
        y_comment_sprites_indexs.Enqueue(y_comment_sprites_index);

        int i_text = 0;
        foreach(string text in y_comment_txt)
        {
            y_comment_texts[i_text].text = text;
            i_text += 1;
        }

        int i_index = 0;
        foreach (int index in y_comment_sprites_indexs)
        {
            y_comment_renderers[i_index].sprite = y_comment_sprites[index];
            i_index += 1;
        }

        y_comment_sprites_index += 1;
        if (y_comment_sprites_index == 2)
        {
            y_comment_sprites_index = 0;
        }
    }
    void NComment()
    {
        n_comment_txt.Dequeue();
        n_comment_sprites_indexs.Dequeue();
        n_comment_txt.Enqueue(niconico_comment[Random.Range(0, niconico_comment.Length)]);
        n_comment_sprites_indexs.Enqueue(Random.Range(0, 3));

        int i_text = 0;
        foreach (string text in n_comment_txt)
        {
            n_comment_texts[i_text].text = text;
            i_text += 1;
        }

        int i_index = 0;
        foreach (int index in n_comment_sprites_indexs)
        {
            n_comment_renderers[i_index].sprite = n_comment_sprites[index];
            i_index += 1;
        }
    }

    private void Update()
    {
        if(time == 0)
        {
            if (text_read == false)
            {
                if (text_ani.progress >= 1.0f)
                {
                    text_read = true;
                    text_cursor.SetActive(true);

                    if (step == 4)
                    {
                        y_comment.SetActive(false);
                    }
                    else if (step == 5)
                    {
                        t_open_nico = OPEN_NICO_TIME;
                        n_comment.SetActive(true);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0) | Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.E))
            {
                if (text_read == true)
                {
                    text_cursor.SetActive(false);
                    text_obj.SetActive(false);
                    text_read = false;
                    time = SLEEP_TIME;
                    step += 1;

                    if (step == 1)
                    {
                        t_open_pc = OPEN_PC_TIME;
                    }
                    else if (step == 7)
                    {
                        t_close_pc = CLOSE_PC_TIME;
                    }
                    else if (step == 10)
                    {
                        time = 50;
                        step += 1;
                    }
                }
                else
                {
                    text_cursor.SetActive(true);
                    text_read = true;
                    text_ani.Skip();

                    if (step == 4)
                    {
                        y_comment.SetActive(false);
                    }
                    else if (step == 5)
                    {
                        t_open_nico = OPEN_NICO_TIME;
                        n_comment.SetActive(true);
                    }
                }
            }
        }
    }
}
