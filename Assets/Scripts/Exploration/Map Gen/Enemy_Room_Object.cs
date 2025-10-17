using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Room_Object : Room_Object
{
    private Enemy_Stats enemy;

    private bool enemyIsDefeated = false;

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

    public static Enemy_Room_Object ConvertToEnemyRoom(Room_Object room)
    {
        Enemy_Room_Object newRoom = new Enemy_Room_Object();
        newRoom.connections = room.GetConnections();
        newRoom.roomType = room.GetRoomType();
        return newRoom;
    }
}
