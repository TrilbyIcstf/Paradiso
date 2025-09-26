using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField]
    RectMask2D mask;

    public void SetMaskAmount(int val)
    {
        Vector4 tempV = this.mask.padding;
        tempV.w = val;
        this.mask.padding = tempV;
    }
}
