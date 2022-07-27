using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FConfig : MonoBehaviour
{
    [SerializeField] DataNomal DataNomal;
    [SerializeField] FuncMiniFigure Fig;
    [SerializeField] Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        Fig.ChangeFigure(14 - Status.stage_layer, images, true, DataNomal.figure_mini);
    }
}
