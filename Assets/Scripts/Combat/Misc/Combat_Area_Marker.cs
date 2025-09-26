using UnityEngine;

public class Combat_Area_Marker : MonoBehaviour
{
    public float width = 0.0f;
    public float height = 0.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(this.width, this.height, 0));
    }

    public Vector2 ClosestPoint(Vector2 pos)
    {
        float halfX = width / 2;
        float halfY = height / 2;

        float clampedX = Mathf.Clamp(pos.x, this.transform.position.x - halfX, this.transform.position.x + halfX);
        float clampedY = Mathf.Clamp(pos.y, this.transform.position.y - halfY, this.transform.position.y + halfY);

        return new Vector2(clampedX, clampedY);
    }

    public Vector2 RandomPoint()
    {
        float halfX = width / 2;
        float halfY = height / 2;

        float randX = Random.Range(-halfX, halfX);
        float randY = Random.Range(-halfY, halfY);

        return new Vector2(gameObject.transform.position.x + randX, gameObject.transform.position.y + randY);
    }

    public bool WithinArea(Vector2 pos)
    {
        float halfX = width / 2;
        float halfY = height / 2;
        if (Mathf.Abs(pos.x - this.transform.position.x) > halfX || Mathf.Abs(pos.y - this.transform.position.y) > halfY)
        {
            return false;
        }
        return true;
    }

    public float DistanceFromHand(Vector2 pos)
    {
        return Vector2.Distance(pos, ClosestPoint(pos));
    }
}
