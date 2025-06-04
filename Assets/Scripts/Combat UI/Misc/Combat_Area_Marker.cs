using System.Collections;
using System.Collections.Generic;
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
}
