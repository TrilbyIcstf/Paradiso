using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Item_Holder : Item_Holder
{
    protected override void SetItemImage(Sprite sprite)
    {
        this.itemObject.GetComponent<Image>().sprite = sprite;
    }
}
