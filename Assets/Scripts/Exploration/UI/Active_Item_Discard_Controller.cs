using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the box used to select an item to discard when over the limit
/// </summary>
public class Active_Item_Discard_Controller : MonoBehaviour
{
    /// <summary>
    /// 1-3 are player's active item images.
    /// 4 is the image for the new item.
    /// </summary>
    [SerializeField]
    private Active_Item_Discard_Holder[] activeItemHolders = new Active_Item_Discard_Holder[4];

    public void SetupDiscard(Item_Active newItem)
    {
        List<Item_Active> activeItems = GameManager.instance.PM.GetActiveItems();

        for (int i = 0; i < 3; i++)
        {
            this.activeItemHolders[i].SetupItem(activeItems[i]);
        }
        this.activeItemHolders[3].SetupItem(newItem);

        gameObject.SetActive(true);
    }
}
