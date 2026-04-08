using System.Collections.Generic;
using UnityEngine;

public class Room_Tile : ManagerBehavior
{
    [SerializeField]
    private SerializableDictionary<Direction, GameObject> doors = new SerializableDictionary<Direction, GameObject>();

    [SerializeField]
    private SpriteRenderer icon;

    private Room_Object room;

    private Vector2Int position;

    public void Setup(Room_Object room)
    {
        UpdateRoom(room);

        List<Direction> conns = new List<Direction>(room.GetConnections().Keys);
        foreach (Direction conn in conns)
        {
            doors[conn].SetActive(false);
        }

        this.position = room.GetPos();
    }

    public void UpdateRoom(Room_Object room)
    {
        this.room = room;

        if (room.GetSeen())
        {
            GetComponent<SpriteRenderer>().sprite = Static_Object_Manager.instance.GetRoomIcon("LightRoom");
            this.icon.sprite = room.GetIcon();
        } 
        else
        {
            GetComponent<SpriteRenderer>().sprite = Static_Object_Manager.instance.GetRoomIcon("DarkRoom");
        }
    }

    public Vector2Int GetPos()
    {
        return this.position;
    }
}
