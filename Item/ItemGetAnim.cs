using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemGetAnim : MonoBehaviour
{
    const int SET_TIME = 100;

    [SerializeField] TextMeshProUGUI myText;
    Vector2 targetPos;
    int t_set;

    void OnDisable()
    {
        DesetThis();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(t_set > 0)
        {
            SetPosition();
        }
    }

    void FixedUpdate()
    {
        if(t_set > 0)
        {
            t_set -= 1;

            if(t_set < 20)
            {
                myText.alpha = t_set / 20f;

                if(t_set == 0)
                {
                    DesetThis();
                }
            }
        }
    }

    private void SetPosition()
    {
        Transform transform = gameObject.transform;
        Vector3 mypos = transform.localPosition;
        mypos.x = (targetPos.x - Camera.main.transform.position.x);
        mypos.y = (targetPos.y - Camera.main.transform.position.y);
        transform.localPosition = mypos;
    }

    public void SetThis(Vector2 pos, string name)
    {
        DesetThis();
        targetPos = pos;
        SetPosition();
        myText.text = name;
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
