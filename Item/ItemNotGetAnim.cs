using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemNotGetAnim : MonoBehaviour
{
    const int SET_TIME = 50;

    [SerializeField] FuncMove Move;
    [SerializeField] TextMeshProUGUI myText;
    Vector2 targetPos;
    int t_set;

    void OnDisable()
    {
        DesetThis();
    }

    void LateUpdate()
    {
        if (t_set > 0)
        {
            SetPosition();
        }
    }

    private void FixedUpdate()
    {
        if (t_set > 0)
        {
            t_set -= 1;

            if(t_set == SET_TIME - 4)
            {
                targetPos = new Vector2(targetPos.x - 6f, targetPos.y);
            }

            if (t_set == SET_TIME - 8)
            {
                targetPos = new Vector2(targetPos.x + 2f, targetPos.y);
            }

            if (t_set < 20)
            {
                myText.alpha = t_set / 20f;

                if (t_set == 0)
                {
                    DesetThis();
                }
            }
        }
    }

    public void SetPosition()
    {
        Transform transform = gameObject.transform;
        Vector3 mypos = transform.localPosition;
        mypos.x = (targetPos.x - Camera.main.transform.position.x);
        mypos.y = (targetPos.y - Camera.main.transform.position.y);
        transform.localPosition = mypos;
    }

    public void SetThis(Vector2 pos)
    {
        DesetThis();
        targetPos = new Vector2(pos.x + 4f, pos.y);
        SetPosition();
        myText.alpha = 1f;
        gameObject.SetActive(true);
        t_set = SET_TIME;
    }

    public void DesetThis()
    {
        gameObject.SetActive(false);
        t_set = 0;
    }
}
