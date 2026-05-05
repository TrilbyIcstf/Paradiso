using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Exploration_Floor_Data
{
    public int FloorNum { get; set; } = 0;

    /// <summary>
    /// 2D array of the floor's layout
    /// [0,y ... x,y]
    /// [.         .]
    /// [.         .]
    /// [.         .]
    /// [0,0 ... x,0]
    /// </summary>
    public Room_Object[,] FloorLayout {  get; private set; }

    /// <summary>
    /// The starting position for the floor
    /// </summary>
    private Vector2Int StartingPos { get; set; }

    public FloorType FloorType { get; set; }

    /// <summary>
    /// Counts the number of enemies spawned on the floor
    /// </summary>
    public float EnemyCount { get; private set; } = 0;

    /// <summary>
    /// Counts the number of passive items spawned on the floor
    /// </summary>
    public float ItemCount { get; private set; } = 0;

    public List<Item> ItemList { get; private set; }

    public Exploration_Floor_Data(int floorNum, Room_Object[,] floorLayout, Vector2Int startingPos, FloorType floorType, float enemyCount, float itemCount)
    {
        FloorNum = floorNum;
        FloorLayout = floorLayout;
        StartingPos = startingPos;
        FloorType = floorType;
        EnemyCount = enemyCount;
        ItemCount = itemCount;

        ItemList = GetFloorItems();
    }

    private List<Item> GetFloorItems()
    {
        List<Item> list = new List<Item>();

        for (int i = 0; i < this.FloorLayout.GetLength(0); i++)
        {
            for (int j = 0; j < this.FloorLayout.GetLength(1); j++)
            {
                Room_Object room = this.FloorLayout[i, j];
                if (room is Item_Room_Object)
                {
                    Item item = ((Item_Room_Object)room).GetItem();
                    list.Add(item);
                }
            }
        }

        return list;
    }

    public void DebugPrintLayout()
    {
        for (int i = FloorLayout.GetLength(1) - 1; i >= 0; i--)
        {
            string mapRow = "";
            for (int j = 0; j < FloorLayout.GetLength(0); j++)
            {
                if (this.FloorLayout[j, i] != null)
                {
                    RoomType type = this.FloorLayout[j, i].GetRoomType();
                    switch (type)
                    {
                        case RoomType.Starting:
                            mapRow += "V";
                            break;
                        case RoomType.Enemy:
                            mapRow += "F";
                            break;
                        case RoomType.Item:
                            mapRow += "I";
                            break;
                        case RoomType.Stairs:
                            mapRow += "S";
                            break;
                        default:
                            mapRow += "O";
                            break;
                    }
                }
                else
                {
                    mapRow += "X";
                }
            }
            Debug.Log(mapRow);
        }

        Debug.Log("|-----------------|");
    }
}
