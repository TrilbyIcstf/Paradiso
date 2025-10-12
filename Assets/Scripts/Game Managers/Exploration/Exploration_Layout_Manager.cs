 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploration_Layout_Manager : ManagerBehavior
{
    private Room_Object[,] floorLayout;

    private int startingX;
    private int startingY;

    private Room_Controller currentRoom;

    public void RandomizeFloor(int width, int height)
    {
        this.floorLayout = new Room_Object[width, height];
        (this.startingX, this.startingY) = (Random.Range(0, width), Random.Range(0, height));
        float startingContinuePower = ((width + height) / 2) * 50;
        RecursiveAddRoom(startingX, startingY, startingContinuePower, 80.0f);

        for (int i = floorLayout.GetLength(1) - 1; i >= 0; i--)
        {
            string mapRow = "";
            for (int j = 0; j < floorLayout.GetLength(0); j++)
            {
                if (i == startingY && j == startingX)
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

    private Room_Object RecursiveAddRoom(int x, int y, float continuePercent, float splitPercent)
    {
        this.floorLayout[x, y] = new Room_Object();
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

                Directions nextDir = GetEmptyDirection(x, y);

                if (nextDir == Directions.None) { break; }

                (int xDir, int yDir) = nextDir.NumericalDirection();
                this.floorLayout[x, y].AddConnection(RecursiveAddRoom(x + xDir, y + yDir, continuePercent - 50.0f, splitPercent), nextDir);

                if (shouldSplit)
                {
                    Directions splitDir = GetEmptyDirection(x, y);

                    if (splitDir == Directions.None) { break; }

                    (int xSplit, int ySplit) = splitDir.NumericalDirection();
                    this.floorLayout[x, y].AddConnection(RecursiveAddRoom(x + xSplit, y + ySplit, continuePercent - 50.0f, splitPercent), splitDir);
                }
            }
        } while (false);

        return this.floorLayout[x, y];
    }

    private Directions GetEmptyDirection(int x, int y)
    {
        List<Directions> directions = new List<Directions>();

        if (x > 0 && this.floorLayout[x - 1, y] == null)
        {
            directions.Add(Directions.Left);
        }

        if (x < floorLayout.GetLength(0) - 1 && this.floorLayout[x + 1, y] == null)
        {
            directions.Add(Directions.Right);
        }

        if (y > 0 && this.floorLayout[x, y - 1] == null)
        {
            directions.Add(Directions.Down);
        }

        if (y < floorLayout.GetLength(1) - 1 && this.floorLayout[x, y + 1] == null)
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

    public void SetupCurrentRoom()
    {
        this.currentRoom.SetupRoom();
    }

    public (int, int) GetStartingCoords()
    {
        return (this.startingX, this.startingY);
    }

    public Room_Object GetStartingRoom()
    {
        return this.floorLayout[this.startingX, this.startingY];
    }

    public void SetCurrentRoom(Room_Controller val)
    {
        this.currentRoom = val;
    }
}
