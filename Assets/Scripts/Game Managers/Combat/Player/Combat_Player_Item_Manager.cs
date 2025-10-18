using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            GetPlayerItem(i).SetItem(activeItems[i]);
        }
    }

    public void SetItemSlot(GameObject slot, int pos)
    {
        List<Item_Active> activeItems = GM.PM.GetActiveItems();
        this.itemSlots[pos] = slot;
        if (pos < activeItems.Count)
        {
            GetPlayerItem(pos).SetItem(activeItems[pos]);
        }
    }

    public Combat_Item GetPlayerItem(int pos)
    {
        return this.itemSlots[pos].GetComponent<Combat_Item>();
    }

    public List<Combat_Item> GetPlayerItems()
    {
        List<Combat_Item> scriptList = this.itemSlots.OfType<GameObject>().Select(i => i.GetComponent<Combat_Item>()).ToList();
        return scriptList;
    }
}
