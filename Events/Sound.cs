using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour
{
    public static AudioClip[] BGM;
    public static AudioSource myBGM;

    public AudioSource myBGM_parent;

    //[0] OP
    //[1] 環境音
    //[2] ステージ
    //[3] 戦闘曲
    //[4] ボス曲

    public AudioClip[] setBGM;

    private void Start()
    {
        BGM = setBGM;
        myBGM = myBGM_parent;

        if(StageCreateConfig.load == 1)
        {
            Debug.Log(true);
            StageCreateConfig.load = 2;
            SceneManager.LoadScene("test");
        }
        else if(StartStageConfig.load == 1)
        {
            StartStageConfig.load = 2;
            SceneManager.LoadScene("StageStart");
        }
        else
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
