using System.Collections.Generic;
using System;
using System.Linq;

public class Exploration_Item_Manager : ManagerBehavior
{
    private List<Item> availableItems = Enum.GetValues(typeof(Item)).Cast<Item>().ToList();

    public Item ChooseNewItem()
    {
        List<Item> playerItems = GM.PL.GetItems().Select(i => i.GetItemType()).ToList();
        List<Item> mapItems = GM.EL.GetFloorItems();
        List<Item> discardedItems = GM.PL.GetDiscardedItems();

        List<Item> existingItems = playerItems.Concat(mapItems).Concat(discardedItems).ToList();

        List<Item> viableItems = ItemMethods.GetItemRoomViableList();
        List<Item> remainingItems = viableItems.Except(existingItems).ToList();

        if (remainingItems.Count == 0)
        {
            return Item.Default;
        }

        int randPos = UnityEngine.Random.Range(0, remainingItems.Count);

        return remainingItems[randPos];
    }

    public void RemoveAvailableItem(Item val)
    {
        this.availableItems.Remove(val);
    }
}
