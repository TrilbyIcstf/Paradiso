using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Exploration_Item_Manager : ManagerBehavior
{
    private List<Items> availableItems = Enum.GetValues(typeof(Items)).Cast<Items>().ToList();

    public Items ChooseNewItem()
    {
        List<Items> playerItems = GM.PM.GetItems().Select(i => i.item).ToList();
        List<Items> mapItems = GM.EL.GetFloorItems();

        List<Items> existingItems = playerItems;
        existingItems.Concat(mapItems);

        return Items.AGun;
    }

    public void RemoveAvailableItem(Items val)
    {
        this.availableItems.Remove(val);
    }
}
