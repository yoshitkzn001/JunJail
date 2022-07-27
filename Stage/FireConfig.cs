using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireConfig : MonoBehaviour
{
    private const int LIGHT_SPEED_FIRST = 5;
    private const int LIGHT_SPEED = 20;

    [SerializeField] MasStageConfig Master;
    [SerializeField] JunConfig junC;
    GameObject jun;
    Animator ani;

    int lightT;
    int light_anim_speed;

    AnimatorStateInfo aniinfo;
    private bool lightup;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer under_sr;
    [SerializeField] private Sprite[] light_sprites;
    [SerializeField] private Sprite[] underlight_sprites;
    public bool firstlightup;

    // Start is called before the first frame update
    void Awake()
    {
        jun = GameObject.Find("Jun");
        ani = GetComponent<Animator>();

        if(firstlightup == true)
        {
            lightup = true;
            ani.enabled = true;
            ani.Play("fire_play", 0, Random.Range(0f, 1f));

            sr.sprite = light_sprites[1];
            under_sr.sprite = underlight_sprites[1];
            lightT = LIGHT_SPEED * 4;
            light_anim_speed = LIGHT_SPEED;
        }
    }

    private void OnEnable()
    {
        if(lightup == true)
        {
            ani.Play("fire_play", 0, Random.Range(0f, 1f));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lightup == false)
        {
            Transform transform = gameObject.transform;
            Vector3 pos = transform.position;
            Transform juntransform = jun.transform;
            Vector3 junpos = juntransform.position;

            float rangeX = Mathf.Pow(pos.x - junpos.x, 2);
            float rangeY = Mathf.Pow((pos.y) - junpos.y, 2);
            float range = Mathf.Sqrt(rangeX + rangeY);

            if(range < 90)
            {
                if(junC.canmove == true)
                {
                    ani.enabled = true;
                    lightup = true;
                    lightT = LIGHT_SPEED_FIRST * 4;
                    light_anim_speed = LIGHT_SPEED_FIRST;
                    sr.sprite = light_sprites[3];
                    Master.nomalSE.Play("fire");
                }
            }
        }

        if(lightT > 0)
        {
            lightT -= 1;

            if (lightT % light_anim_speed == 0)
            {
                if(lightT == 0)
                {
                    sr.sprite = light_sprites[1];
                    under_sr.sprite = underlight_sprites[1];
                    lightT = LIGHT_SPEED * 4;
                    light_anim_speed = LIGHT_SPEED;
                }
                else
                {
                    int num = (lightT / light_anim_speed) - 1;
                    under_sr.sprite = underlight_sprites[num];
                    sr.sprite = light_sprites[num];
                }
            }
        }
    }
}
