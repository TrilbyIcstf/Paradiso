using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base stats for an item
/// </summary>
public class Item_Base : ScriptableObject
{
    [SerializeField]
    protected Item item;

    [SerializeField]
    protected string itemName;

    [SerializeField]
    protected Sprite sprite;

    public Item GetItemType()
    {
        return this.item;
    }

    public string GetName()
    {
        return this.itemName;
    }

    public Sprite GetSprite()
    {
        return this.sprite;
    }
}
