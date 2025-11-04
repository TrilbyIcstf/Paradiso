using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_Holder : MonoBehaviour
{
    protected Item_Base item;

    public Item_Base GetItem()
    {
        return this.item;
    }

    public Items GetItemType()
    {
        return this.item?.GetItem() ?? Items.Default;
    }

    public bool HasItem()
    {
        return this.item != null;
    }

    public void SetItem(Item_Base val)
    {
        this.item = val;
    }
}
