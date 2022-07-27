using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Status : MonoBehaviour
{
    public bool tutorial;

    private const int TARGET_ENEMY = 0;
    private const int TARGET_ALL = -1;
    private const int TARGET_ME = -2;

    private const int USE_ALL = 0;
    private const int USE_BATTLE = 1;
    private const int USE_STAGE = 2;

    //stage status
    public static int stage_tips;
    public static int stage_layer;
    public static int[] stage_items;
    public static List<(List<List<(int, int, int, string)>>, int, bool)> random_message_info_1to1 = new List<(List<List<(int, int, int, string)>>, int, bool)>()
    {
        //キャラ一体のランダムトークイベント
        //( 会話情報, キャラの種類1 , 会話初めにじゅんいちの方向を向くかどうか )

        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "やあ"),
                    (1, 1, 0, "かっかかっかかかか、かかっか\n加藤さんですか！？"),
                    (-1, 0, 0, "俺　加藤だけど"),
                    (1, 1, 0, "一か月間　どこでなにしてたんですか！？"),
                    (-1, 0, 0, "独房で寝てたわ！\nそれよりお前　おめーは正気なのか？"),
                    (1, 1, 0, "え？ ああ　僕は大丈夫ですよ！"),
                    (1, 1, 0, "その質問をするということは加藤さん\nやつらと遭遇したんですね\n「ROM狂い」に"),
                    (-1, 0, 0, "ROM狂い？\n俺　加藤だけどなんだその名前"),
                    (1, 1, 0, "この一か月で色々ありまして...\nROMはご存じですか？"),
                    (-1, 0, 0, "さっき拾ったやつのことか？\n「右ストレート」って書いてたな"),
                    (1, 1, 0, "それは　持つ人の潜在能力を吸いだして\n技として使えるようにしてくれる優れモノなんですよ"),
                    (1, 1, 0, "それが　この一か月で唐突に監獄内に出現しはじめまして..."),
                    (1, 1, 0, "そのROMを求めて人を襲うやつらのことを\n「ROM狂い」って呼んでるんです"),
                    (-1, 0, 0, "なるほどな　大体わかったわ\nおめーも気をつけろよ"),
                    (1, 1, 1, "奴らの特徴は目が赤いことです\n注意してくださいね！"),
                }
                ,
                new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "おめーここにいて大丈夫なのか？\n襲われたらやべぇだろ"),
                    (1, 1, 1, "僕はROM持ってないから　大丈夫です！"),
                    (1, 1, 0, "あいつら　ROM持ってなさそうな奴には興味ないんすよね..."),
                }
                ,
                new List<(int, int, int, string)>()
                {
                    (1, 1, 0, "持ってもしょうがないし..."),
                }
            },
            0, true
        )
        ,
        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "やあ"),
                    (1, 1, 1, "あ、加藤さん！聞きましたよ、1ヶ月間も寝込んで一体何が..."),
                    (-1, 0, 0, "それはいいじゃねーかよおめぇ、聞くんじゃねぇ！"),
                    (-1, 0, 0, "そんなことより　どこを回っても看守が見当たらないんだけどよ\n何があったんだ？"),
                    (1, 1, 0, "え、知らないんですか？\nもうこの監獄は完全に無法地帯ですよ"),
                    (1, 1, 0, "ROM狂い達が暴れたせいで　看守が逃げ出しちゃったんです"),
                    (-1, 0, 1, "それはすごいことだねぇ！じゃあ脱獄し放題じゃねぇかよ！"),
                    (1, 1, 0, "あ"),
                    (1, 1, 0, "あの、加藤さん、その話は監獄内ではNGでてるんですよね..."),
                    (-1, 0, 0, "は？なんでだよ"),
                    (1, 1, 0, "..."),
                }
                ,
                new List<(int, int, int, string)>()
                {
                    (1, 1, 0, "..."),
                }
            },
            0, true
        )
    };
    public static int message_1to1_num;
    public static List<(List<List<(int, int, int, string)>>, int, int, bool)> random_message_info_1to2 = new List<(List<List<(int, int, int, string)>>, int, int, bool)>()
    {
        //キャラ二体のランダムトークイベント
        //( 会話情報, キャラの種類1, キャラの種類2, 会話初めにじゅんいちの方向を向くかどうか )

        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (1, 1, 2, "いや、お前さ、頭悪くねーか！？"),
                    (1, 1, 2, "どう考えても笑いの表現として「wwww」の方がにぎやかに見えるからいいじゃねーか！"),
                    (-1, 1, 2, "いやいやいや、お前の方が頭悪くねーか？"),
                    (-1, 1, 2, "「草」の方が短くて打ちやすくて見やすいから優れてるよなぁ！？"),
                    (1, 1, 2, "は？お前それでも衛門かよ！新参のgmがよぉ！"),
                    (-1, 1, 2, "古参の老害がぁ！"),
                }
                ,
                new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "(そっとしておこう)"),
                }
                ,
            },
            0, 0, false
        )
        ,
        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (1, 1, 0, "なあ、知ってるか？"),
                    (-1, 1, 0, "なんだ？"),
                    (1, 1, 0, "ROM狂いども、もともとはあそこまでROMを求めてなかったんだどさ"),
                    (-1, 1, 0, "なにかあったのか？"),
                    (1, 1, 0, "自分が持ってるROMを取られないように ROMを飲み込む文化ができてな"),
                    (1, 1, 0, "それが原因で 体が変形してROMを過剰に求めるようになったんだと"),
                    (-1, 1, 0, "普通に気持ち悪いな"),
                }
                ,
                new List<(int, int, int, string)>()
                {
                    (-1, 1, 0, "お前の顔が変なのは ROMを飲み込んだせいか？"),
                    (1, 1, 0, "うるｾｲｰ"),
                }
                ,
            },
            0, 0, false
        )
        ,
    };
    public static int message_1to2_num;

    //boss
    public static int boss_message_num;
    public static List<(List<List<(int, int, int, string)>>, int, bool)> boss_message_info = new List<(List<List<(int, int, int, string)>>, int, bool)>()
    {
        //キャラ一体のランダムトークイベント
        //( 会話情報, キャラの種類1 , 会話初めにじゅんいちの方向を向くかどうか )

        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (1, 2, 0, "ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ...ｿﾞｳｻﾝ..."),
                }
            },
            2, true
        )
        ,
        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (1, 3, 0, "あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ　あ"),
                }
            },
            3, true
        )
        ,
        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (1, 4, 0, "ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ ﾁﾝﾁﾝがﾓﾁﾓﾁ "),
                }
            },
            4, true
        )
        ,
        (
            new List<List<(int, int, int, string)>>()
            {
                new List<(int, int, int, string)>()
                {
                    (-1, 0, 0, "お前　俺の視聴者だよなぁ！？\nちょうどよかった"),
                    (-1, 0, 0, "看守が一人もいねぇんだけどよ\nおめーなんか知ってるか？"),
                    (1, 2, 0, "..............."),
                    (-1, 0, 0, "機会見計らって\n脱獄しようと思ってんだけどなぁ？"),
                    (1, 2, 0, "..............."),
                    (-1, 0, 0, "おい　おめーなんか変だぞ\n大丈夫か？"),
                    (1, 2, 0, ".......オレ　オマエノ\nROM　ウバウ"),
                    (-1, 0, 0, "...あぁ！？\nあに言ってんだおめー？"),
                    (-1, 0, 0, "ROMってさっき拾ったやつのことかぁ？"),
                    (1, 2, 0, "ヨコセ！！！！！"),
                }
            },
            2, true
        )
    };
    public static List<int> boss_layer = new List<int>() { 4, 9, 13 };

    //computer status
    public static float SCREEN_WIDTH;

    //character status
    public static int level;
    public static int exp;
    public static int hp;
    public static int maxhp;
    public static int sp;
    public static int maxsp;
    public static int atk;
    public static int guard_hp;
    public static int guard_maxhp;
    public static int guard_sp;
    public static int st;
    public static int st_max;
    public static int baggage_count;
    public static int baggage_count_max;
    public static int twitch_level;
    public static int youtube_level;
    public static int niconico_level;

    //skills
    public static int[] skills_sp = new int[18]
    {
        6, 12, 20,
        10, 25, 40,  //ここで大声出すよなぁ!?
        8, 18, 28,
        30, 20, 10,
        10, 15, 20,
        20, 30, 40
    };
    public static int[] myskill_level;
    public static string[] skills_name = new string[6]
    {
        "右ストレート", 
        "大きい声出すよなぁ!?",
        "ジャガもどき投げ",
        "そしてついにドロー",
        "ちょっと休むぞ！",
        "集まれ45組",
    };
    public static string[] skills_message = new string[6]
    {
        "衝撃的な一発をぶっ放して敵を堕とす。", 
        "ここで大きい声を出して敵全体にダメージを与える。",
        "オクレイマンの嫌いな「ジャガもどき」を敵全体に投げつける。",
        "所持しているROMのどれかをランダムで発動させる。",
        "ゆっくり休んでHPを回復させる。ひん。",
        "カードゲームで稼いだコインを敵全体に投げつける。\n次はたかい？ひくい？",
    };
    public static int[] skill_target = new int[6] // enemy, enemy_all, me
    {
        TARGET_ENEMY,
        TARGET_ALL,
        TARGET_ALL,
        TARGET_ENEMY,
        TARGET_ME,
        TARGET_ALL,
    };
    public static List<int> myskill;
    public static List<int> myskill_all;
    public static List<int> myskill_inStage;

    public static bool load = false;

    //items
    public static int[] items_value;
    public static int[] items_use = new int[9]
    {
        USE_ALL,
        USE_ALL,
        USE_BATTLE,
        USE_BATTLE,
        USE_BATTLE,
        USE_BATTLE,
        USE_STAGE,
        USE_ALL,
        USE_STAGE,
    };
    public static int[] items_target = new int[9] // enemy, enemy_all, me
    {
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
        TARGET_ME,
    };
    public static string[] items_name = new string[9]
    {
        "ライフガーダー缶",
        "ライフガーダーボトル",
        "甘いパン",
        "辛いパン",
        "硬いパン",
        "アスパラ",
        "ワープ飯",
        "SPゴールドジョッキ",
        "ROMキット",
    };
    public static string[] items_message = new string[9]
    {
        "極生命体飲料を内包した缶。HPが30回復する。",
        "極生命体飲料を内包したボトル。HPが100回復する。",
        "甘くて美味しいふっくらもちもちパン。ATKステータスとガードステータスがしばらくの間上昇する。",
        "辛くて火を噴くまっかなあつあつパン。ATKステータスがしばらくの間上昇する。",
        "硬くて嚙めないがっちりカチカチパン。ガードステータスがしばらくの間上昇する。",
        "岩手の農家が育てた至極の一品。しばらくの間HPが毎ターン30回復する。",
        "食べると階段までワープする。",
        "SPが20回復する。競艇を見ながら飲むと捗る。",
        "ROMをアップグレードすることができる。",
    };

    //run
    public static string[] run_name = new string[2]
    {
        "逃げる",
        "全力で逃げる"
    };
    public static string[] run_message = new string[2]
    {
        "走力Gの速度で敵から逃げる。逃げ切れるかどうかは運次第。",
        "30SPで走力を上げて敵から逃げる。確実に逃げ切れる。"
    };

    //status rom
    public static List<int> status_roms_level;
    public static string[] status_roms_name = new string[4]
    {
        "ヒットポイントROM",
        "スピリットROM",
        "アタックROM",
        "ガードROM",
    };
    public static string[] status_roms_message = new string[4]
    {
        "HPステータスに影響するROM",
        "SPステータスに影響するROM",
        "ATKステータスに影響するROM",
        "ガードステータスに影響するROM"
    };
    public static string[] status_roms_message_up = new string[4]
    {
        "最大HPが30増える。",
        "SPが80増える。",
        "攻撃力が強化される。",
        "ガードの耐久度が上昇し、パリィ時に貰えるSPが増える。"
    };

    public static int[] status_roms_up = new int[4]
    {
        30,
        80,
        1,
        1
    };

    public static List<int> rom_get_layer = new List<int>()
    {
        1, 4, 6, 8
    };

    public static int[] get_ext_list = new int[5]
    {
        8,
        18,
        24,
        36,
        4
    };

    public static int[] exp_list = new int[10]
    {
        0,
        32,
        104,
        204,
        354,
        554,
        804,
        1104,
        1454,
        14540,
    };

    // Start is called before the first frame update
    void Awake()
    {
        if(load == false)
        {
            level = 1;
            exp = 0;
            maxhp = 32;
            hp = maxhp;
            guard_maxhp = 10;
            guard_sp = 3;
            guard_hp = guard_maxhp;
            atk = 8;
            baggage_count_max = 10;
            twitch_level = 1;
            youtube_level = 1;
            niconico_level = 1;

            if(tutorial == false)
            {
                st = 99;
                sp = 190;
                myskill = new List<int>
                {
                    0
                };
            }
            else
            {
                st = 99;
                sp = 199;
                myskill = new List<int>();
            }
            st_max = st;

            myskill_all = new List<int>
            {
                1, 2, 3, 4
            };
            myskill_inStage = new List<int>();

            Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            Vector2 pos2 = transform.InverseTransformPoint(pos);
            SCREEN_WIDTH = pos2.x;

            for (int i = 0; i < 4; i++)
            {
                int skill;
                if (i == 0)
                {
                    skill = 1;
                }
                else if(i == 1)
                {
                    skill = 2;
                }
                else
                {
                    skill = myskill_all[Random.Range(0, myskill_all.Count)];
                }
                myskill_inStage.Add(skill);
                myskill_all.Remove(skill);

                if (myskill_all.Count == 0)
                {
                    break;
                }
            }

            //reset
            stage_items = new int[]
            {
                15,
                5,
                4,
                4,
                4,
                5,
                0,
                10,
                0,
            };
            message_1to1_num = 0;
            message_1to2_num = 0;
            boss_message_num = 0;
            stage_layer = 0;

            myskill_level = new int[6]
            {
                1,
                1,
                1,
                1,
                1,
                1,
            };

            items_value = new int[9]
            {
                0, //エネルギードリンク缶
                0, //エネルギードリンクボトル
                0,
                0,
                0,
                0,
                0,
                0,
                0,
            };

            status_roms_level = new List<int>()
            {
                1, 1, 1, 1
            };

            load = true;
        }
    }
}
