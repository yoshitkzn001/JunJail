using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpaceDebug : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] sprites;
    [SerializeField] StageCreateDebug scd;

    private bool onJun = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(onJun == false)
        {
            if (collision.tag == "Player")
            {
                sr.sprite = sprites[1];
                onJun = true;
                scd.check_space_count += 1;
            }
        }
    }
}
