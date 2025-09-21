using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Item : MonoBehaviour
{
    public Item_Active item;

    private void OnMouseDown()
    {
        if (item == null)
        {
            return;
        }

        if (GameManager.instance.CS.CanAffordEnergy(this.item.energyCost))
        {
            GameManager.instance.CS.SubtractEnergy(this.item.energyCost, true);
            item.Activate();
        }
        else
        {
            GameManager.instance.CUI.InvalidEnergyCost();
        }
    }
}
