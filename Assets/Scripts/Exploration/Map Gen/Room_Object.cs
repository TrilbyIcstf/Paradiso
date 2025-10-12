using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Object
{
    private Dictionary<Directions, Room_Object> connections = new Dictionary<Directions, Room_Object>();

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
}
