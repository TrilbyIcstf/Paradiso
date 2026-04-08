using System.Collections;
using UnityEngine;

public class Tile_Click : ManagerBehavior
{
    private void OnMouseDown()
    {
        if (!GM.EM.MovementLock)
        {
            Room_Tile roomScript = GetComponent<Room_Tile>();
            Vector2Int roomPos = roomScript.GetPos();

            if (GM.EL.IsConnected(roomPos))
            {
                StartCoroutine(MovePlayer(roomPos));
            }
        }
    }

    private IEnumerator MovePlayer(Vector2Int roomPos)
    {
        Vector3 pos = Camera.main.transform.position;
        pos.x = transform.position.x;
        pos.y = transform.position.y;

        Exploration_Map_Camera cam = Camera.main.GetComponent<Exploration_Map_Camera>();

        yield return StartCoroutine(cam.MoveTo(pos, 0.5f));

        GameObject.FindGameObjectWithTag("MapGen").GetComponent<Map_Handler>().MoveTo(roomPos);
    }
}
