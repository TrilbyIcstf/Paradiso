using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_Holder : MonoBehaviour
{
    protected Item_Base item;

    [SerializeField]
    protected GameObject itemObject;

    public void SetupItem(Item_Base item)
    {
        SetItem(item);

        SetItemImage(item.GetSprite());
    }

    public Item_Base GetItem()
    {
        return this.item;
    }

    public Item GetItemType()
    {
        return this.item?.GetItemType() ?? Item.Default;
    }

    public bool HasItem()
    {
        return this.item != null;
    }

    public void SetItem(Item_Base val)
    {
        this.item = val;
    }

    protected abstract void SetItemImage(Sprite sprite);
}
