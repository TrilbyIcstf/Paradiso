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

    public void SetItemTaken(bool val)
    {
        this.itemTaken = val;
    }

    /// <summary>
    /// Converts a standard room to an item room
    /// </summary>
    /// <param name="room">The standard room to be converted</param>
    /// <returns>An Item room object</returns>
    public static Item_Room_Object ConvertToItemRoom(Room_Object room)
    {
        Item_Room_Object newRoom = new Item_Room_Object(room.GetEntranceDirection(), room.GetDistance());
        newRoom.SetItem(GameManager.instance.EI.ChooseNewItem());
        newRoom.connections = room.GetConnections();
        newRoom.roomType = room.GetRoomType();
        
        return newRoom;
    }
}
