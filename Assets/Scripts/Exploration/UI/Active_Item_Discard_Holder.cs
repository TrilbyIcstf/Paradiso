using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Active_Item_Discard_Holder : Item_Holder
{
    [SerializeField]
    int pos;

    [SerializeField]
    Image itemImage;

    public void SetupItem(Item_Base item)
    {
        SetItem(item);

        this.itemImage.sprite = item.GetSprite();
    }

    public int GetPos()
    {
        return this.pos;
    }
}
