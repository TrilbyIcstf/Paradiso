using UnityEngine;

public class Map_Handler : ManagerBehavior
{
    private const float roomDistance = 2.5f;

    private GameObject[,] mapLayout;

    [SerializeField]
    private GameObject mapTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int width = GM.EL.GetFloorWidth();
        int height = GM.EL.GetFloorHeight();

        this.mapLayout = new GameObject[width, height];

        Vector2Int currentPos = GM.EL.GetStartingCoords();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                Room_Object room = GM.EL.GetRoom(pos);

                if (room == null) { continue; }

                int xDist = pos.x - currentPos.x;
                int yDist = pos.y - currentPos.y;

                float xPos = transform.position.x + (xDist * roomDistance);
                float yPos = transform.position.y + (yDist * roomDistance);
                Vector3 roomPos = new Vector3(xPos, yPos, 0);

                GameObject tempTile = Instantiate(this.mapTile, roomPos, Quaternion.identity);
                Room_Tile tempScript = tempTile.GetComponent<Room_Tile>();
                tempScript.Setup(room);
                this.mapLayout[pos.x, pos.y] = tempTile;
            }
        }
    }

    public void MoveTo(Vector2Int pos)
    {
        GM.EL.MoveTo(pos);
        UpdateFloor();
        GM.EL.TriggerRoom(pos);
    }

    public void UpdateFloor()
    {
        for (int x = 0; x < this.mapLayout.GetLength(0); x++)
        {
            for (int y = 0; y < this.mapLayout.GetLength(1); y++)
            {
                if (this.mapLayout[x, y] == null) { continue; }

                Vector2Int pos = new Vector2Int(x, y);

                Room_Object room = GM.EL.GetRoom(pos);
                GameObject roomTile = this.mapLayout[x, y];
                Room_Tile roomScript = roomTile.GetComponent<Room_Tile>();
                roomScript.UpdateRoom(room);
            }
        }
    }
}
