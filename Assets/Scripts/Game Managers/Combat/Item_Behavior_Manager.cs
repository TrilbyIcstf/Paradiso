using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the effects of items
/// </summary>
public class Item_Behavior_Manager : ManagerBehavior
{
    public void TriggerActiveEffect(Items item)
    {
        switch(item)
        {
            case Items.Default:
                break;
            case Items.DamageTest:
                Active_Card randCard = GM.CPH.PickRandomCard()?.GetComponent<Active_Card>();
                if (randCard == null) { break; }
                randCard.AddMultPower(1.2f);
                randCard.GetComponent<Card_UI>().EmphasizeCardCo();
                break;
            case Items.CheapDrawTest:
                GM.CPS.DrawCard();
                break;
            case Items.AGun:
                GM.CES.DealDamage(25.0f);
                break;
            case Items.PageRipper:
                PageRipperActive();
                break;
            default:
                break;
        }
    }

    public void TriggerPassiveItem(Items item, PassiveEffectParameters passParams)
    {
        switch (item)
        {
            case Items.Antiquifier:
                {
                    Active_Card cardScript = passParams.triggeredCard.GetComponent<Active_Card>();
                    Card_Base cardStats = cardScript.GetBase();
                    string cardID = cardStats.GetID();

                    cardScript.AddPowerBuff(1);
                    cardStats.BuffPower(1);
                    GM.PM.UpdateCard(cardStats.GetID(), cardStats);
                }
                break;
            case Items.Solidifier:
                {
                    Active_Card cardScript = passParams.triggeredCard.GetComponent<Active_Card>();
                    Card_Base cardStats = cardScript.GetBase();
                    string cardID = cardStats.GetID();

                    cardScript.AddDefenseBuff(1);
                    cardStats.BuffDefense(1);
                    GM.PM.UpdateCard(cardStats.GetID(), cardStats);
                }
                break;
            case Items.Bandage:
                GM.PM.HealHealth(25);
                break;
            default:
                break;
        }
    }

    public void TriggerItemPickup(Items item, PassiveEffectParameters passParams)
    {
        switch (item)
        {
            case Items.CrystalBall:
                GM.PM.AddMaxEnergy(10);
                break;
            default:
                break;
        }
    }

    private void PageRipperActive()
    {
        GameObject randCard = GM.CPH.PickRandomCard();
        Active_Card cardScript = randCard.GetComponent<Active_Card>();
        Card_Base cardStats = cardScript.GetBase();
        Active_Card newCard = GM.CPS.CreateNewCard(cardStats, randCard.transform);
        int halvedPower = Mathf.CeilToInt(cardScript.GetPower() / 2.0f);
        int halvedDefense = Mathf.CeilToInt(cardScript.GetDefense() / 2.0f);
        cardScript.SetPower(halvedPower);
        cardScript.SetDefense(halvedDefense);
        newCard.SetPower(halvedPower);
        newCard.SetDefense(halvedDefense);
    }

    public bool CanTriggerActive(Items item)
    {
        switch (item)
        {
            case Items.Default:
                return true;
            case Items.DamageTest:
                return GM.CPH.HandSize() > 0;
            case Items.CheapDrawTest:
                return !GM.CPD.DeckIsEmpty() && !GM.CPH.AtHandLimit();
            case Items.AGun:
                return true;
            case Items.PageRipper:
                return GM.CPH.HandSize() > 0;
            default:
                return false;
        }
    }

    public bool WillTriggerPassive(Items item, PassiveEffectParameters passParams)
    {
        switch (item)
        {
            case Items.CrystalBall:
            case Items.Bandage:
                return true;
            case Items.Antiquifier:
                return passParams.triggeredCard != null;
            case Items.Solidifier:
                return passParams.triggeredCard != null;
            default:
                return false;
        }
    }
}

public class PassiveEffectParameters
{
    public GameObject triggeredCard;

    public static PassiveEffectParameters TriggeredCard(GameObject triggeredCard)
    {
        PassiveEffectParameters ret = new PassiveEffectParameters();
        ret.triggeredCard = triggeredCard;
        return ret;
    }
}