using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class representing a room the contains an enemy.
/// The enemy will be in the center, and all doors not linked towards the entrance will be locked until the enemy is defeated.
/// </summary>
public class Enemy_Room_Object : Room_Object
{
    private Enemy_Stats enemy;

    private bool enemyIsDefeated = false;

    public Enemy_Room_Object(Directions entranceDirection, int distance) : base(entranceDirection, distance) { }

    public Enemy_Stats GetEnemy() {
        return this.enemy;
    }

    public void SetEnemy(Enemy_Stats val)
    {
        this.enemy = val;
    }

    public bool IsEnemyDefeated()
    {
        return this.enemyIsDefeated;
    }

    public void SetEnemyDefeated(bool val)
    {
        this.enemyIsDefeated = val;
    }

    /// <summary>
    /// Converts a standard room to an enemy room
    /// </summary>
    /// <param name="room">The standard room to be converted</param>
    /// <returns>An enemy room object</returns>
    public static Enemy_Room_Object ConvertToEnemyRoom(Room_Object room)
    {
        Enemy_Room_Object newRoom = new Enemy_Room_Object(room.GetEntranceDirection(), room.GetDistance());
        newRoom.connections = room.GetConnections();
        newRoom.roomType = room.GetRoomType();
        return newRoom;
    }
}
