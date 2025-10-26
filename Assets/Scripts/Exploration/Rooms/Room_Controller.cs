using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controls objects within a room during exploration
/// </summary>
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

    /// <summary>
    /// Sets up the room to match the passed in room object
    /// </summary>
    /// <param name="room">The room to set up</param>
    /// <param name="enteredDirection">The direction the player entered from</param>
    public void SetupRoom(Room_Object room, Directions enteredDirection)
    {
        UnlockRoom(room);

        if (this.playerSpawns.ContainsKey(enteredDirection))
        {
            this.player.transform.position = this.playerSpawns[enteredDirection].transform.position;
        }

        if (room is Enemy_Room_Object)
        {
            Enemy_Room_Object enemyRoom = (Enemy_Room_Object)room;

            TileBase oldWall = GM.SOM.GetWallTile(GM.EL.GetFloor());
            TileBase newWall = GM.SOM.GetLockTile(GM.EL.GetFloor());

            if (enemyRoom.IsEnemyDefeated())
            {
                GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");
                Destroy(enemyObj);
            } else
            {
                foreach (var conn in room.GetConnections())
                {
                    if (conn.Key == Directions.None) { continue; }
                    if (conn.Key == room.GetEntranceDirection() || conn.Value == null) { continue; }
                    this.doorGrids[conn.Key].SetActive(true);
                    this.doorGrids[conn.Key].GetComponent<Tilemap>().SwapTile(oldWall, newWall);
                }
            }
        }
    }

    public void UnlockRoom(Room_Object room)
    {
        foreach (var conn in room.GetConnections())
        {
            if (conn.Key == Directions.None) { continue; }
            this.doorGrids[conn.Key].SetActive(conn.Value == null);
        }
    }
}
