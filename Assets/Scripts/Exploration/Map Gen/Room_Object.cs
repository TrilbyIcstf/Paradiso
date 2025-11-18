using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single room within the map
/// </summary>
public class Room_Object
{
    // Tracks which rooms this one is connected to and in which direction
    protected Dictionary<Direction, Vector2Int> connections = new Dictionary<Direction, Vector2Int>();

    protected RoomType roomType;

    // Tracks what direction in the room leads towards the floor entrance
    protected Direction entranceDirection;

    // Number of rooms from the floor entrance
    protected int distance;

    // Tracks if the player has been in the room
    protected bool entered = false;

    public Room_Object(Direction entranceDirection, int distance)
    {
        this.entranceDirection = entranceDirection;
        this.distance = distance;
    }

    public void AddConnection(Vector2Int pos, Direction dir)
    {
        this.connections[dir] = pos;
    }

    public Dictionary<Direction, Vector2Int> GetConnections()
    {
        return this.connections;
    }

    public int GetConnectionCount()
    {
        return this.connections.Count;
    }

    public RoomType GetRoomType()
    {
        return this.roomType;
    }

    public Direction GetEntranceDirection()
    {
        return this.entranceDirection;
    }

    public int GetDistance()
    {
        return this.distance;
    }

    public bool GetEntered()
    {
        return this.entered;
    }

    public void SetRoomType(RoomType val)
    {
        this.roomType = val;
    }

    public void SetEntered(bool val)
    {
        this.entered = val;
    }
}
