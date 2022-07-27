using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingConfig : MonoBehaviour
{
    private const int TIME_SET_TIPS = 20;

    private string[] tips = new string[]
    {
        "攻撃をガードすると耐久度が下がり、\nガードの色が赤くなります。\n耐久度はターン始めに1/5ほど回復します。",
        "スタミナがマイナスになると、加藤純一に疲労が出始めてダメージを受けます。",
        "ROMを使うときspを消費します。\nspが足りない場合、代わりにhpを消費するので注意が必要です。",
        "右上のマップの黄色いエリアには、ROMボックスまたはアイテムボックスがあります。",
        "spバーの右下に書いている数字は、100spが何個あるかを示しています。\n2と書いてる場合、spバー + 200 が全spです。",
        "ガードを解除したタイミングで攻撃を受けると、パリィになります。\nパリィに成功すると、ノーダメージ・sp増加 が発生します。",
    };

    [SerializeField] private Controller Control;
    [SerializeField] FuncMove Move;
    public GameObject LoadinObj;
    public CanvasGroup TipsCanvas;
    public GameObject LoadingDoneObj;
    public JunConfig junC;
    public TextMeshProUGUI tips_text;
    public CanvasGroup myCanvas;
    public GameObject fade;
    public Image fade_img;
    private AsyncOperation async;
    public int t_load;
    private int t_fadeout;

    private bool endLoad;
    public bool startload;
    public bool tutorial;
    public bool start_scene;

    const float BGM_VOLUME = 0.55f;

    private void Start()
    {
        Move.MoveLocalPosX(LoadinObj, Status.SCREEN_WIDTH - 40);
        Move.MoveLocalPosX(LoadingDoneObj, Status.SCREEN_WIDTH - 40);
        tips_text.text = tips[Status.stage_tips];

        if(startload == true)
        {
            myCanvas.alpha = 0f;

            endLoad = true;
            t_fadeout = 70;
        }

        if(start_scene == true)
        {
            Sound.myBGM.clip = Sound.BGM[2];
            Sound.myBGM.volume = 0.0f;
            Sound.myBGM.Play();
        }
    }

    private void FixedUpdate()
    {
        if(t_fadeout > 0)
        {
            t_fadeout -= 1;
            fade_img.color = new Color(0f, 0f, 0f, t_fadeout / 25f);

            if(t_fadeout >= 15)
            {
                if(startload == false)
                {
                    myCanvas.alpha = (t_fadeout - 15) / 10f;
                }
            }
            if (start_scene == true)
            {
                Sound.myBGM.volume = BGM_VOLUME * ((70 - t_fadeout) / 70f);
            }

            if (t_fadeout == 0)
            {
                fade_img.color = new Color(0f, 0f, 0f, 0f);
                myCanvas.alpha = 1f;
                fade.SetActive(false);
                gameObject.SetActive(false);
                junC.load = true;
                if(tutorial == false)
                {
                    junC.canmove = true;
                }
                LoadingDoneObj.SetActive(false);
                LoadinObj.SetActive(true);
                TipsCanvas.alpha = 0;
            }
        }

        if(t_load > 0)
        {
            t_load -= 1;

            if(tutorial == false)
            {
                if (t_load < TIME_SET_TIPS)
                {
                    float alpha = (TIME_SET_TIPS - t_load) / (float)TIME_SET_TIPS;
                    TipsCanvas.alpha = alpha;

                    if (t_load == 0)
                    {
                        LoadNextScene("SampleScene");
                    }
                }
            }
            else
            {
                if(t_load == 0)
                {
                    if(Status.stage_layer != 14)
                    {
                        PlayerPrefs.SetInt("Tutorial", 1);
                        SceneManager.LoadScene("StageStart");
                    }
                    else
                    {
                        SceneManager.LoadScene("End");
                    }
                }
            }
        }
    }

    private void Update()
    {
        if(endLoad == false)
        {
            if (Input.GetMouseButtonDown(0) | Control.GetButtonDownDecide())
            {
                endLoad = true;
                t_fadeout = 25;
            }
        }
    }

    public void StartLoad()
    {
        t_load = TIME_SET_TIPS + 35;
        Status.stage_tips = Random.Range(0, tips.Length);
        tips_text.text = tips[Status.stage_tips];

        if(tutorial == true | Status.stage_layer == 14)
        {
            myCanvas.alpha = 0f;
            Sound.myBGM.Stop();
            tutorial = true;
        }
    }

    public void LoadNextScene(string scenename)
    {
        StartCoroutine(LoadScene(scenename));
    }

    IEnumerator LoadScene(string scenename)
    {
        async = SceneManager.LoadSceneAsync(scenename);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
