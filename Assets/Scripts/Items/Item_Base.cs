using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base stats for an item
/// </summary>
public class Item_Base : ScriptableObject
{
    [SerializeField]
    protected Items item;

    [SerializeField]
    protected string itemName;

    [SerializeField]
    protected Sprite sprite;

    public Items GetItemType()
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
