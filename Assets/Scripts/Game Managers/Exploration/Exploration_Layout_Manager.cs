 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages generating and tracking the layout of a floor during exploration
/// </summary>
public class Exploration_Layout_Manager : ManagerBehavior
{
    /// <summary>
    /// 2D array of the floor's layout
    /// [0,y ... x,y]
    /// [.         .]
    /// [.         .]
    /// [.         .]
    /// [0,0 ... x,0]
    /// </summary>
    private Room_Object[,] floorLayout;

    /// <summary>
    /// The starting position for the floor
    /// </summary>
    private Vector2Int startingPos;

    /// <summary>
    /// The player's current position on the floor
    /// </summary>
    private Vector2Int currentPos;

    /// <summary>
    /// Counts the number of enemies spawned on the floor
    /// </summary>
    private float enemyCount = 0;

    /// <summary>
    /// Counts the number of passive items spawned on the floor
    /// </summary>
    private float itemCount = 0;

    /// <summary>
    /// Tracks if the stairs have been generated on the floor yet
    /// </summary>
    private bool stairsGenerated = false;

    private Floor currentFloor = Floor.Demo;

    private int floorNumber = 1;

    public void ResetFloorGenVars()
    {
        this.enemyCount = 0;
        this.itemCount = 0;
        this.stairsGenerated = false;
    }

    public void RandomizeFloor(int width, int height)
    {
        ResetFloorGenVars();
        this.floorLayout = new Room_Object[width, height];
        this.startingPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        this.currentPos = this.startingPos;
        float startingContinuePower = GetAverageFloorSize() * 50;
        RecursiveAddRoom(this.startingPos, startingContinuePower, 80.0f, Direction.None, 0);
        GetRoom(this.currentPos).SetEntered(true);

        for (int i = floorLayout.GetLength(1) - 1; i >= 0; i--)
        {
            string mapRow = "";
            for (int j = 0; j < floorLayout.GetLength(0); j++)
            {
                if (this.floorLayout[j, i] != null)
                {
                    RoomType type = this.floorLayout[j, i].GetRoomType();
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
                } else
                {
                    mapRow += "X";
                }
            }
            Debug.Log(mapRow);
        }
    }

    private Room_Object RecursiveAddRoom(Vector2Int pos, float continuePercent, float splitPercent, Direction entranceDirection, int distance)
    {
        Room_Object thisRoom = new Room_Object(entranceDirection, distance);
        if (entranceDirection != Direction.None)
        {
            thisRoom.AddConnection(pos + entranceDirection.NumericalDirection(), entranceDirection);
        }

        this.floorLayout[pos.x, pos.y] = thisRoom;
        float randPerc = Random.Range(0.0f, 100.0f);
        do
        {
            if (randPerc <= continuePercent)
            {
                bool shouldSplit = false;
                float randSplit = Random.Range(0.0f, 100.0f);
                if (randSplit <= splitPercent)
                {
                    shouldSplit = true;
                    splitPercent -= 40;
                }

                Direction nextDir = GetEmptyDirection(pos);

                if (nextDir == Direction.None) { break; }

                Vector2Int dir = nextDir.NumericalDirection();
                Room_Object newRoom = RecursiveAddRoom(pos + dir, continuePercent - 50.0f, splitPercent, nextDir.OppositeDirection(), distance + 1);
                thisRoom.AddConnection(pos + dir, nextDir);

                if (shouldSplit)
                {
                    Direction splitDir = GetEmptyDirection(pos);

                    if (splitDir == Direction.None) { break; }

                    Vector2Int split = splitDir.NumericalDirection();
                    Room_Object splitRoom = RecursiveAddRoom(pos + split, continuePercent - 50.0f, splitPercent, splitDir.OppositeDirection(), distance + 1);
                    thisRoom.AddConnection(pos + split, splitDir);
                }
            }
        } while (false);

        RoomType roomType = DetermineRoomType(pos);
        thisRoom.SetRoomType(roomType);
        this.floorLayout[pos.x, pos.y] = ConvertToRoomType(thisRoom, roomType);

        return GetRoom(pos);
    }

    private Direction GetEmptyDirection(Vector2Int pos)
    {
        List<Direction> directions = new List<Direction>();

        if (pos.x > 0 && this.floorLayout[pos.x - 1, pos.y] == null)
        {
            directions.Add(Direction.Left);
        }

        if (pos.x < floorLayout.GetLength(0) - 1 && this.floorLayout[pos.x + 1, pos.y] == null)
        {
            directions.Add(Direction.Right);
        }

        if (pos.y > 0 && this.floorLayout[pos.x, pos.y - 1] == null)
        {
            directions.Add(Direction.Down);
        }

        if (pos.y < floorLayout.GetLength(1) - 1 && this.floorLayout[pos.x, pos.y + 1] == null)
        {
            directions.Add(Direction.Up);
        }

        if (directions.Count == 0)
        {
            return Direction.None;
        }

        int randDir = Random.Range(0, directions.Count);
        return directions[randDir];
    }

    public Room_Object MoveInDirection(Direction dir)
    {
        Vector2Int dirVect = dir.NumericalDirection();
        this.currentPos += dirVect;
        GM.ER.EnteredDirection = dir;
        GetRoom(this.currentPos).SetEntered(true);
        return GetCurrentRoom();
    }

    private RoomType DetermineRoomType(Vector2Int pos)
    {
        float randAmount;
        if (pos == this.startingPos) { return RoomType.Starting; }
        if (GetRoom(pos).GetConnectionCount() <= 1) {
            if (!this.stairsGenerated)
            {
                this.stairsGenerated = true;
                return RoomType.Stairs;
            }

            float itemFillRatio = 1.0f - (this.itemCount / 2);
            float itemChance = 100.0f * itemFillRatio;
            randAmount = Random.Range(0.0f, 100.0f);
            if (randAmount < itemChance)
            {
                this.itemCount++;
                return RoomType.Item;
            }

            return RoomType.Empty;
        }
        float enemyFillRatio = 1.0f - (this.enemyCount / GetAverageFloorSize());
        float enemyChance = 60.0f * enemyFillRatio;
        randAmount = Random.Range(0.0f, 100.0f);
        if (randAmount < enemyChance)
        {
            this.enemyCount++;
            return RoomType.Enemy;
        } else
        {
            return RoomType.Empty;
        }
    }

    private Room_Object ConvertToRoomType(Room_Object room, RoomType type)
    {
        switch (type)
        {
            case RoomType.Enemy:
                return Enemy_Room_Object.ConvertToEnemyRoom(room);
            case RoomType.Item:
                return Item_Room_Object.ConvertToItemRoom(room);
            case RoomType.Stairs:
                return Stairs_Room_Object.ConvertToStairsRoom(room);
            default:
                return room;
        }
    }

    /// <summary>
    /// Finds all the items contained on the current floor
    /// </summary>
    /// <returns>A list of items on the current floor</returns>
    public List<Item> GetFloorItems()
    {
        List<Item> list = new List<Item>();

        for (int i = 0; i < this.floorLayout.GetLength(0); i++)
        {
            for (int j = 0; j < this.floorLayout.GetLength(1); j++)
            {
                Room_Object room = this.floorLayout[i, j];
                if (room is Item_Room_Object)
                {
                    Item item = ((Item_Room_Object)room).GetItem();
                    list.Add(item);
                }
            }
        }

        return list;
    }

    public Vector2Int GetStartingCoords()
    {
        return this.startingPos;
    }

    public Room_Object GetRoom(Vector2Int pos)
    {
        return this.floorLayout[pos.x, pos.y];
    }

    public Room_Object GetStartingRoom()
    {
        return GetRoom(this.startingPos);
    }

    public Vector2Int GetPos()
    {
        return this.currentPos;
    }

    public Room_Object GetCurrentRoom()
    {
        return GetRoom(this.currentPos);
    }

    public Room_Object GetRoomInDirection(Direction dir)
    {
        Vector2Int dirVect = dir.NumericalDirection();
        return this.floorLayout[this.currentPos.x + dirVect.x, this.currentPos.y + dirVect.y];
    }

    private float GetAverageFloorSize()
    {
        return (this.floorLayout.GetLength(0) + this.floorLayout.GetLength(1)) / 2;
    }

    public int GetFloorWidth()
    {
        return this.floorLayout.GetLength(0);
    }

    public int GetFloorHeight()
    {
        return this.floorLayout.GetLength(1);
    }

    public Floor GetFloor()
    {
        return this.currentFloor;
    }

    public int GetFloorNumber()
    {
        return this.floorNumber;
    }

    public void IncrementFloorNumber()
    {
        this.floorNumber++;
    }
}
