using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Object
{
    protected Dictionary<Directions, Room_Object> connections = new Dictionary<Directions, Room_Object>();

    protected RoomTypes roomType;

    public Room_Object()
    {
        this.connections[Directions.None] = this;
    }

    public void AddConnection(Room_Object room, Directions dir)
    {
        this.connections[dir] = room;
    }

    public Dictionary<Directions, Room_Object> GetConnections()
    {
        return this.connections;
    }

    public int GetConnectionCount()
    {
        return this.connections.Count;
    }

    public RoomTypes GetRoomType()
    {
        return this.roomType;
    }

    public void SetRoomType(RoomTypes val)
    {
        this.roomType = val;
    }
}
