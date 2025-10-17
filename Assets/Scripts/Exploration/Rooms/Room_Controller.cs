using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : ManagerBehavior
{
    [SerializeField]
    private SerializableDictionary<Directions, GameObject> doorGrids;
    [SerializeField]
    private SerializableDictionary<Directions, GameObject> playerSpawns;
    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        GM.ER.SetCurrentRoom(this);
        GM.ER.SetupCurrentRoom();
    }

    public void SetupRoom(Room_Object room, Directions enteredDirection)
    {
        Room_Object thisRoom = room;

        foreach (var conn in thisRoom.GetConnections())
        {
            if (conn.Key == Directions.None) { continue; }
            this.doorGrids[conn.Key].SetActive(conn.Value == null);
        }

        if (this.playerSpawns.ContainsKey(enteredDirection))
        {
            this.player.transform.position = this.playerSpawns[enteredDirection].transform.position;
        }

        if (room is Enemy_Room_Object)
        {
            Enemy_Room_Object enemyRoom = (Enemy_Room_Object)room;
            
            if (enemyRoom.IsEnemyDefeated())
            {
                GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");
                Destroy(enemyObj);
            }
        }
    }
}
