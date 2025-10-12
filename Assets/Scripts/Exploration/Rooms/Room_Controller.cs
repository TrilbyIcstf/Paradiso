using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : ManagerBehavior
{
    [SerializeField]
    private SerializableDictionary<Directions, GameObject> doorGrids;

    private void Awake()
    {
        GM.ER.SetCurrentRoom(this);
        GM.ER.SetupCurrentRoom();
    }

    public void SetupRoom(Room_Object room)
    {
        Room_Object thisRoom = room;

        foreach (var conn in thisRoom.GetConnections())
        {
            if (conn.Key == Directions.None) { continue; }
            this.doorGrids[conn.Key].SetActive(conn.Value == null);
        }
    }
}
