using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single room within the map
/// </summary>
public class Room_Object
{
    // Tracks which rooms this one is connected to and in which direction
    protected Dictionary<Directions, Room_Object> connections = new Dictionary<Directions, Room_Object>();

    protected RoomTypes roomType;

    // Tracks what direction in the room leads towards the floor entrance
    protected Directions entranceDirection;

    // Number of rooms from the floor entrance
    protected int distance;

    public Room_Object(Directions entranceDirection, int distance)
    {
        this.entranceDirection = entranceDirection;
        this.distance = distance;

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

    public Directions GetEntranceDirection()
    {
        return this.entranceDirection;
    }

    public int GetDistance()
    {
        return this.distance;
    }

    public void SetRoomType(RoomTypes val)
    {
        this.roomType = val;
    }
}
