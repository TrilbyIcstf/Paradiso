using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles UI elements in exploration environments
/// </summary>
public class Room_UI_Coordinator : ManagerBehavior
{
    [SerializeField]
    private Item_Pickup_Box_Controller itemPickupBox;
    [SerializeField]
    private Active_Item_Discard_Controller activeDiscardBox;
    [SerializeField]
    private Minimap_UI_Controller minimap;
    [SerializeField]
    private Inventory_Button_Controller inventoryButton;
    [SerializeField]
    private Inventory_Box_Controller inventoryBox;

    public void OnItemPickup(Item_Base item)
    {
        this.itemPickupBox.SetTextBox(item);
        GM.EP.SetMovementLock(true);
    }

    public void OnAcceptItemPickup()
    {
        if (GM.PM.IsTentativeActive() && GM.PM.GetActiveItems().Count >= 3)
        {
            Item_Active pickupItem = GM.PM.GetTentativeActive();
            this.activeDiscardBox.SetupDiscard(pickupItem);
        } else
        {
            GM.PM.AddTentativeItem();
            GM.EP.SetMovementLock(false);
        }
    }

    public void OpenInventory()
    {
        this.minimap.SetVisible(false);
        this.inventoryButton.SetVisible(false);

        this.inventoryBox.SetVisible(true);
    }
}
