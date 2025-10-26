using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class representing a room with a collectable item in it.
/// </summary>
public class Item_Room_Object : Room_Object
{
    private Items containedItem;

    private bool itemTaken = false;

    public Item_Room_Object(Directions entranceDirection, int distance) : base(entranceDirection, distance) { }

    public Items GetItem()
    {
        return this.containedItem;
    }

    public bool IsItemTaken()
    {
        return this.itemTaken;
    }

    public void SetItem(Items val)
    {
        this.containedItem = val;
    }

    public void SetItemTake(bool val)
    {
        this.itemTaken = val;
    }
}
