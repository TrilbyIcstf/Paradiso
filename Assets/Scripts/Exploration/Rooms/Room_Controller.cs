using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<Directions, GameObject> doorGrids;

    private void Awake()
    {
        GameManager.instance.EL.SetCurrentRoom(this);
    }

    public void SetupRoom()
    {
        Room_Object thisRoom = GameManager.instance.EL.GetStartingRoom();

        foreach (var conn in thisRoom.GetConnections())
        {
            if (conn.Key == Directions.None) { continue; }
            this.doorGrids[conn.Key].SetActive(conn.Value == null);
        }
    }
}
