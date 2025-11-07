using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Follow_Cursor : MonoBehaviour
{
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert screen position to world position
        // Camera.main.ScreenToWorldPoint handles the conversion for 2D
        // The z-component is typically ignored in 2D or set to 0
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Set the object's position to the mouse's world position
        // Ensure the z-component is consistent with your 2D setup (e.g., 0)
        transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
    }
}
