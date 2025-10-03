using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Player_Item_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemSlots = new GameObject[3];

    public void SetPlayersItems()
    {
        List<Item_Active> activeItems = GameManager.instance.PM.GetActiveItems();

        for (int i = 0; i < activeItems.Count; i++)
        {
            // Should probably throw an error here eventually
            if (i >= 3) { break; }

            this.itemSlots[i].GetComponent<Combat_Item>().SetItem(activeItems[i]);
        }
    }
}
