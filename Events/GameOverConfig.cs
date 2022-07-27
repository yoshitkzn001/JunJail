using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverConfig : MonoBehaviour
{
    [SerializeField] Image EndFade;

    int t_end;
    int t_fade;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        EndFade.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(t_end == 0)
        {
            if(Input.GetMouseButtonDown(0) | Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.E))
            {
                EndFade.gameObject.SetActive(true);
                t_fade = 70;
            }
        }
    }

    private void FixedUpdate()
    {
        if(t_end > 0)
        {
            t_end -= 1;
        }

        if(t_fade > 0)
        {
            t_fade -= 1;
            EndFade.color = new Color(0f, 0f, 0f, (70 - t_fade) / 70f);

            if(t_fade == 0)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    public void GameOver(AudioSource bgm, CriAtomSource se, CriAtomSource voice)
    {
        voice.Stop();
        se.Play("damage3");
        gameObject.SetActive(true);
        bgm.Stop();
        Sound.myBGM.Stop();
        t_end = 30;
    }
}
