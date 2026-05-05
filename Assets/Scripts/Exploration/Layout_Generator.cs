using System.Collections.Generic;
using UnityEngine;

public class Layout_Generator
{
    private Vector2Int startingPos;
    private int enemyNum = 0;
    private int itemNum = 0;
    private bool stairsGenerated = false;
    private Room_Object[,] floorLayout;

    public Exploration_Floor_Data GenerateBasicFloor(int floorNum)
    {
        ResetFloorGenVars();
        int width = Random.Range(4, 7);
        int height = Random.Range(4, 7);
        this.floorLayout = new Room_Object[width, height];
        this.startingPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        float startingContinuePower = GetAverageFloorSize() * 60;
        RecursiveAddRoom(this.startingPos, startingContinuePower, 80.0f, Direction.None, 0);

        return new Exploration_Floor_Data(floorNum, this.floorLayout, this.startingPos, FloorType.Pergatorio, this.enemyNum, this.itemNum);
    }

    private Room_Object RecursiveAddRoom(Vector2Int pos, float continuePercent, float splitPercent, Direction entranceDirection, int distance)
    {
        Room_Object thisRoom = new Room_Object(entranceDirection, distance, pos);
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
                    splitPercent *= 0.5f;
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

    private RoomType DetermineRoomType(Vector2Int pos)
    {
        float randAmount;
        if (pos == this.startingPos) { return RoomType.Starting; }
        if (GetRoom(pos).GetConnectionCount() <= 1)
        {
            if (!this.stairsGenerated)
            {
                this.stairsGenerated = true;
                return RoomType.Stairs;
            }

            float itemFillRatio = 1.0f - (this.itemNum / 2);
            float itemChance = 100.0f * itemFillRatio;
            randAmount = Random.Range(0.0f, 100.0f);
            if (randAmount < itemChance)
            {
                this.itemNum++;
                return RoomType.Item;
            }

            return RoomType.Empty;
        }
        float enemyFillRatio = 1.0f - (this.enemyNum / GetAverageFloorSize());
        float enemyChance = 60.0f * enemyFillRatio;
        randAmount = Random.Range(0.0f, 100.0f);
        if (randAmount < enemyChance)
        {
            this.enemyNum++;
            return RoomType.Enemy;
        }
        else
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

    public void ResetFloorGenVars()
    {
        this.enemyNum = 0;
        this.itemNum = 0;
        this.stairsGenerated = false;
    }

    private Room_Object GetRoom(Vector2Int pos)
    {
        return this.floorLayout[pos.x, pos.y];
    }

    private float GetAverageFloorSize()
    {
        return (this.floorLayout.GetLength(0) + this.floorLayout.GetLength(1)) / 2;
    }
}
