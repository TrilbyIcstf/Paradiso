using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single room within the map
/// </summary>
public class Room_Object
{
    // Tracks which rooms this one is connected to and in which direction
    protected Dictionary<Directions, Vector2Int> connections = new Dictionary<Directions, Vector2Int>();

    protected RoomTypes roomType;

    // Tracks what direction in the room leads towards the floor entrance
    protected Directions entranceDirection;

    // Number of rooms from the floor entrance
    protected int distance;

    // Tracks if the player has been in the room
    protected bool entered = false;

    public Room_Object(Directions entranceDirection, int distance)
    {
        this.entranceDirection = entranceDirection;
        this.distance = distance;
    }

    public void AddConnection(Vector2Int pos, Directions dir)
    {
        this.connections[dir] = pos;
    }

    public Dictionary<Directions, Vector2Int> GetConnections()
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

    public bool GetEntered()
    {
        return this.entered;
    }

    public void SetRoomType(RoomTypes val)
    {
        this.roomType = val;
    }

    public void SetEntered(bool val)
    {
        this.entered = val;
    }
}
