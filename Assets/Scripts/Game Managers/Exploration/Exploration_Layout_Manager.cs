 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages generating and tracking the layout of a floor during exploration
/// </summary>
public class Exploration_Layout_Manager : ManagerBehavior
{
    private Room_Object[,] floorLayout;

    private Vector2Int startingPos;

    private Vector2Int currentPos;

    private int enemyCount = 0;

    private Floors currentFloor = Floors.Demo;

    public void RandomizeFloor(int width, int height)
    {
        this.floorLayout = new Room_Object[width, height];
        this.startingPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        this.currentPos = this.startingPos;
        float startingContinuePower = GetAverageFloorSize() * 50;
        RecursiveAddRoom(this.startingPos, startingContinuePower, 80.0f, Directions.None, 0);

        for (int i = floorLayout.GetLength(1) - 1; i >= 0; i--)
        {
            string mapRow = "";
            for (int j = 0; j < floorLayout.GetLength(0); j++)
            {
                if (this.floorLayout[j, i] != null)
                {
                    RoomTypes type = this.floorLayout[j, i].GetRoomType();
                    switch (type)
                    {
                        case RoomTypes.Starting:
                            mapRow += "V";
                            break;
                        case RoomTypes.Enemy:
                            mapRow += "F";
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

    private Room_Object RecursiveAddRoom(Vector2Int pos, float continuePercent, float splitPercent, Directions entranceDirection, int distance)
    {
        Room_Object thisRoom = new Room_Object(entranceDirection, distance);
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

                Directions nextDir = GetEmptyDirection(pos);

                if (nextDir == Directions.None) { break; }

                Vector2Int dir = nextDir.NumericalDirection();
                Room_Object newRoom = RecursiveAddRoom(pos + dir, continuePercent - 50.0f, splitPercent, nextDir.OppositeDirection(), distance + 1);
                thisRoom.AddConnection(newRoom, nextDir);
                newRoom.AddConnection(thisRoom, nextDir.OppositeDirection());

                if (shouldSplit)
                {
                    Directions splitDir = GetEmptyDirection(pos);

                    if (splitDir == Directions.None) { break; }

                    Vector2Int split = splitDir.NumericalDirection();
                    Room_Object splitRoom = RecursiveAddRoom(pos + split, continuePercent - 50.0f, splitPercent, splitDir.OppositeDirection(), distance + 1);
                    thisRoom.AddConnection(splitRoom, splitDir);
                    splitRoom.AddConnection(thisRoom, splitDir.OppositeDirection());
                }
            }
        } while (false);

        RoomTypes roomType = DetermineRoomType(pos);
        thisRoom.SetRoomType(roomType);
        if (roomType == RoomTypes.Enemy)
        {
            this.floorLayout[pos.x, pos.y] = Enemy_Room_Object.ConvertToEnemyRoom(thisRoom);
        }

        return GetRoom(pos);
    }

    private Directions GetEmptyDirection(Vector2Int pos)
    {
        List<Directions> directions = new List<Directions>();

        if (pos.x > 0 && this.floorLayout[pos.x - 1, pos.y] == null)
        {
            directions.Add(Directions.Left);
        }

        if (pos.x < floorLayout.GetLength(0) - 1 && this.floorLayout[pos.x + 1, pos.y] == null)
        {
            directions.Add(Directions.Right);
        }

        if (pos.y > 0 && this.floorLayout[pos.x, pos.y - 1] == null)
        {
            directions.Add(Directions.Down);
        }

        if (pos.y < floorLayout.GetLength(1) - 1 && this.floorLayout[pos.x, pos.y + 1] == null)
        {
            directions.Add(Directions.Up);
        }

        if (directions.Count == 0)
        {
            return Directions.None;
        }

        int randDir = Random.Range(0, directions.Count);
        return directions[randDir];
    }

    public Room_Object MoveInDirection(Directions dir)
    {
        Vector2Int dirVect = dir.NumericalDirection();
        this.currentPos += dirVect;
        GM.ER.EnteredDirection = dir;
        return GetCurrentRoom();
    }

    private RoomTypes DetermineRoomType(Vector2Int pos)
    {
        if (pos == this.startingPos) { return RoomTypes.Starting; }
        if (GetRoom(pos).GetConnectionCount() <= 1) {
            return RoomTypes.Empty;
        }
        float enemyFillRatio = 1.0f - (this.enemyCount / GetAverageFloorSize());
        float enemyChance = 80.0f * enemyFillRatio;
        float randAmount = Random.Range(0.0f, 100.0f);
        if (randAmount <= enemyChance)
        {
            this.enemyCount++;
            return RoomTypes.Enemy;
        } else
        {
            return RoomTypes.Empty;
        }
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

    public Room_Object GetCurrentRoom()
    {
        return GetRoom(this.currentPos);
    }

    public Room_Object GetRoomInDirection(Directions dir)
    {
        Vector2Int dirVect = dir.NumericalDirection();
        return this.floorLayout[this.currentPos.x + dirVect.x, this.currentPos.y + dirVect.y];
    }

    private float GetAverageFloorSize()
    {
        return (this.floorLayout.GetLength(0) + this.floorLayout.GetLength(1)) / 2;
    }

    public Floors GetFloor()
    {
        return this.currentFloor;
    }
}
