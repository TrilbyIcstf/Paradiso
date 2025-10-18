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
                break;
            case Items.DamageTest:
                Active_Card randCard = GameManager.instance.CPH.PickRandomCard()?.GetComponent<Active_Card>();
                if (randCard == null) { break; }
                randCard.AddMultPower(0.2f);
                randCard.GetComponent<Card_UI>().EmphasizeCardCo();
                break;
            case Items.CheapDrawTest:
                GameManager.instance.CPS.DrawCard();
                break;
            case Items.AGun:
                GameManager.instance.CES.DealDamage(25.0f);
                break;
            default:
                break;
        }
    }

    public static bool CanTriggerActive(Items item)
    {
        switch (item)
        {
            case Items.Default:
                return true;
            case Items.DamageTest:
                return GameManager.instance.CPH.HandSize() > 0;
            case Items.CheapDrawTest:
                return !GameManager.instance.CPD.DeckIsEmpty() && !GameManager.instance.CPH.AtHandLimit();
            case Items.AGun:
                return true;
            default:
                return true;
        }
    }
}
