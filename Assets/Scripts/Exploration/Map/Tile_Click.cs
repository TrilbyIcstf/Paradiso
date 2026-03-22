using UnityEngine;

public class Tile_Click : MonoBehaviour
{
    private void OnMouseDown()
    {
        Vector3 pos = Camera.main.transform.position;
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        Camera.main.transform.position = pos;
    }
}
