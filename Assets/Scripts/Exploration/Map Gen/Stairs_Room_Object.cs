using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class representing a room with the stairs in it
/// </summary>
public class Stairs_Room_Object : Room_Object
{
    public Stairs_Room_Object(Directions entranceDirection, int distance) : base(entranceDirection, distance) { }

    /// <summary>
    /// Converts a standard room to a stairs room
    /// </summary>
    /// <param name="room">The standard room to be converted</param>
    /// <returns>A Stairs room object</returns>
    public static Stairs_Room_Object ConvertToStairsRoom(Room_Object room)
    {
        Stairs_Room_Object newRoom = new Stairs_Room_Object(room.GetEntranceDirection(), room.GetDistance());
        newRoom.connections = room.GetConnections();
        newRoom.roomType = room.GetRoomType();

        return newRoom;
    }
}
