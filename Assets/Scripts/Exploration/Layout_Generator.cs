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
        return null;
    }

    public void ResetFloorGenVars()
    {
        this.enemyNum = 0;
        this.itemNum = 0;
        this.stairsGenerated = false;
    }

    private float GetAverageFloorSize()
    {
        return (this.floorLayout.GetLength(0) + this.floorLayout.GetLength(1)) / 2;
    }
}
