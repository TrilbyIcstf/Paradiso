using UnityEngine;

public class Map_Generation : ManagerBehavior
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

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector2Int pos = new Vector2Int(j, i);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
