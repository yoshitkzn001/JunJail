using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpBallConfig : MonoBehaviour
{
    public MasBattleConfig mbc;

    public void PutInBall()
    {
        mbc.spballs.Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
