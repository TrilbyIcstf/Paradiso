using System.Collections.Generic;
using System;
using System.Linq;

public class Exploration_Item_Manager : ManagerBehavior
{
    private List<Items> availableItems = Enum.GetValues(typeof(Items)).Cast<Items>().ToList();

    public Items ChooseNewItem()
    {
        List<Items> playerItems = GM.PM.GetItems().Select(i => i.GetItemType()).ToList();
        List<Items> mapItems = GM.EL.GetFloorItems();
        List<Items> discardedItems = GM.PM.GetDiscardedItems();

        List<Items> existingItems = playerItems.Concat(mapItems).Concat(discardedItems).ToList();

        List<Items> viableItems = ItemMethods.GetItemRoomViableList();
        List<Items> remainingItems = viableItems.Except(existingItems).ToList();

        if (remainingItems.Count == 0)
        {
            return Items.Default;
        }

        int randPos = UnityEngine.Random.Range(0, remainingItems.Count);

        return remainingItems[randPos];
    }

    public void RemoveAvailableItem(Items val)
    {
        this.availableItems.Remove(val);
    }
}
