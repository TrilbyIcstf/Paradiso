using UnityEngine;

/// <summary>
/// Used to mark areas for particular use in a scene (i.e. marks the player's hand area) and also adds visual indicator to editor
/// </summary>
public class Combat_Area_Marker : MonoBehaviour
{
    public float width = 0.0f;
    public float height = 0.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(this.width, this.height, 0));
    }

    /// <summary>
    /// Finds the closets point within the area to passed position
    /// </summary>
    /// <param name="pos">Position to find the closest point to</param>
    /// <returns>The closest point within area to pos</returns>
    public Vector2 ClosestPoint(Vector2 pos)
    {
        float halfX = width / 2;
        float halfY = height / 2;

        float clampedX = Mathf.Clamp(pos.x, this.transform.position.x - halfX, this.transform.position.x + halfX);
        float clampedY = Mathf.Clamp(pos.y, this.transform.position.y - halfY, this.transform.position.y + halfY);

        return new Vector2(clampedX, clampedY);
    }

    /// <summary>
    /// Returns a random point within the area
    /// </summary>
    /// <returns>A random point within the area</returns>
    public Vector2 RandomPoint()
    {
        float halfX = width / 2;
        float halfY = height / 2;

        float randX = Random.Range(-halfX, halfX);
        float randY = Random.Range(-halfY, halfY);

        return new Vector2(gameObject.transform.position.x + randX, gameObject.transform.position.y + randY);
    }

    /// <summary>
    /// Checks if the passed in pos is within the area
    /// </summary>
    /// <param name="pos">The pos to check</param>
    /// <returns>True if within area, false otherwise</returns>
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

    /// <summary>
    /// Finds the distance to the closest point in the area
    /// </summary>
    /// <param name="pos">Position to check distance from</param>
    /// <returns>Distance to the closest poin</returns>
    public float DistanceFromArea(Vector2 pos)
    {
        return Vector2.Distance(pos, ClosestPoint(pos));
    }
}
