using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill6EmonCardConfig : MonoBehaviour
{
    [SerializeField] GameObject card_fig;

    public void DesetFig()
    {
        card_fig.SetActive(false);
    }
}
