using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles UI elements in exploration environments
/// </summary>
public class Room_UI_Coordinator : MonoBehaviour
{
    [SerializeField]
    private Item_Pickup_Box_Controller itemPickupBox;

    public void OnItemPickup(Item_Base item)
    {
        this.itemPickupBox.SetTextBox(item);
        GameManager.instance.EP.SetMovementLock(true);
    }

    public void OnDismissItemPickup()
    {
        GameManager.instance.EP.SetMovementLock(false);
    }
}
