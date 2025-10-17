using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Player_Item_Manager : ManagerBehavior
{
    [SerializeField]
    private GameObject[] itemSlots = new GameObject[3];

    public void SetPlayersItems()
    {
        List<Item_Active> activeItems = GM.PM.GetActiveItems();

        for (int i = 0; i < activeItems.Count; i++)
        {
            // Should probably throw an error here eventually
            if (i >= 3) { break; }

            this.itemSlots[i].GetComponent<Combat_Item>().SetItem(activeItems[i]);
        }
    }

    public void SetItemSlot(GameObject slot, int pos)
    {
        List<Item_Active> activeItems = GM.PM.GetActiveItems();
        this.itemSlots[pos] = slot;
        if (pos < activeItems.Count)
        {
            slot.GetComponent<Combat_Item>().SetItem(activeItems[pos]);
        }
    }
}
