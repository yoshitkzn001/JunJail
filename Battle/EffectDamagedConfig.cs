using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamagedConfig : MonoBehaviour
{
    [SerializeField] Animator ani;
    public BattleEnemyConfig bec;

    private void OnEnable()
    {
        ani.SetTrigger("blow");
    }

    public void EnqueueEffect()
    {
        gameObject.SetActive(false);
        bec.effects_damaged.Enqueue(gameObject);
    }
}
