 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploration_Layout_Manager : ManagerBehavior
{
    private Room_Object[,] floorLayout;

    private Vector2Int startingPos;

    private Vector2Int currentPos;

    public void RandomizeFloor(int width, int height)
    {
        this.floorLayout = new Room_Object[width, height];
        this.startingPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        this.currentPos = this.startingPos;
        float startingContinuePower = ((width + height) / 2) * 50;
        RecursiveAddRoom(this.startingPos, startingContinuePower, 80.0f);

        for (int i = floorLayout.GetLength(1) - 1; i >= 0; i--)
        {
            string mapRow = "";
            for (int j = 0; j < floorLayout.GetLength(0); j++)
            {
                if (i == this.startingPos.y && j == this.startingPos.x)
                {
                    mapRow += "V";
                } else if (this.floorLayout[j, i] != null)
                {
                    mapRow += "O";
                } else
                {
                    mapRow += "X";
                }
            }
            Debug.Log(mapRow);
        }
    }

    private Room_Object RecursiveAddRoom(Vector2Int pos, float continuePercent, float splitPercent)
    {
        Room_Object thisRoom = new Room_Object();
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
                Room_Object newRoom = RecursiveAddRoom(pos + dir, continuePercent - 50.0f, splitPercent);
                thisRoom.AddConnection(newRoom, nextDir);
                newRoom.AddConnection(thisRoom, nextDir.OppositeDirection());

                if (shouldSplit)
                {
                    Directions splitDir = GetEmptyDirection(pos);

                    if (splitDir == Directions.None) { break; }

                    Vector2Int split = splitDir.NumericalDirection();
                    Room_Object splitRoom = RecursiveAddRoom(pos + split, continuePercent - 50.0f, splitPercent);
                    thisRoom.AddConnection(splitRoom, splitDir);
                    splitRoom.AddConnection(thisRoom, splitDir.OppositeDirection());
                }
            }
        } while (false);

        return this.floorLayout[pos.x, pos.y];
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

    public bool MoveInDirection(Directions dir)
    {
        Vector2Int dirVect = dir.NumericalDirection();
        this.currentPos += dirVect;
        return this.floorLayout[this.currentPos.x, this.currentPos.y] != null;
    }

    public Vector2Int GetStartingCoords()
    {
        return this.startingPos;
    }

    public Room_Object GetStartingRoom()
    {
        return this.floorLayout[this.startingPos.x, this.startingPos.y];
    }

    public Room_Object GetCurrentRoom()
    {
        return this.floorLayout[this.currentPos.x, this.currentPos.y];
    }

    public Room_Object GetRoomInDirection(Directions dir)
    {
        Vector2Int dirVect = dir.NumericalDirection();
        return this.floorLayout[this.currentPos.x + dirVect.x, this.currentPos.y + dirVect.y];
    }
}
