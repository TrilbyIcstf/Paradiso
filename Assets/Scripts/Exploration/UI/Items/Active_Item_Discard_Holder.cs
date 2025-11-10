using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Active_Item_Discard_Holder : Item_Holder
{
    [SerializeField]
    int pos;

    public int GetPos()
    {
        return this.pos;
    }

    protected override void SetItemImage(Sprite sprite)
    {
        this.itemObject.GetComponent<Image>().sprite = sprite;
    }
}
