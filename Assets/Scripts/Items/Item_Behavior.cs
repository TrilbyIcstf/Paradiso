using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Behavior
{
    public static void TriggerActiveEffect(Items item)
    {
        switch(item)
        {
            case Items.Default:
                return;
            case Items.DamageTest:
                Active_Card randCard = GameManager.instance.CPH.PickRandomCard().GetComponent<Active_Card>();
                randCard.AddMultPower(0.2f);
                return;
            default:
                return;
        }
    }
}
