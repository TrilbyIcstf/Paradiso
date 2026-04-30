using UnityEngine;

public class Exploration_Floor_Data : MonoBehaviour
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

    public Exploration_Floor_Data(int floorNum, Room_Object[,] floorLayout, Vector2Int startingPos, FloorType floorType, float enemyCount, float itemCount)
    {
        FloorNum = floorNum;
        FloorLayout = floorLayout;
        StartingPos = startingPos;
        FloorType = floorType;
        EnemyCount = enemyCount;
        ItemCount = itemCount;
    }
}
