using UnityEngine;

public class Tile_Click : ManagerBehavior
{
    private void OnMouseDown()
    {
        Room_Tile roomScript = GetComponent<Room_Tile>();
        Vector2Int roomPos = roomScript.GetPos();

        if (GM.EL.IsConnected(roomPos))
        {
            Vector3 pos = Camera.main.transform.position;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            Camera.main.transform.position = pos;

            GameObject.FindGameObjectWithTag("MapGen").GetComponent<Map_Generation>().MoveTo(roomPos);
        }
    }
}
