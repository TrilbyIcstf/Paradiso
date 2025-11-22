using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the effects of items
/// </summary>
public class Item_Behavior_Manager : ManagerBehavior
{
    public void TriggerActiveEffect(Item item)
    {
        switch(item)
        {
            case Item.Default:
                break;
            case Item.RunicHammer:
                Active_Card randCard = GM.CPH.PickRandomCard()?.GetComponent<Active_Card>();
                if (randCard == null) { break; }
                randCard.AddMultPower(1.2f);
                randCard.GetComponent<Card_UI_Controller>().EmphasizeCardCo();
                break;
            case Item.ManaRune:
                GM.CPS.DrawCard();
                break;
            case Item.AGun:
                GM.CES.DealDamage(25.0f);
                break;
            case Item.PageRipper:
                PageRipperActive();
                break;
            default:
                break;
        }
    }

    public void TriggerPassiveItem(Item item, PassiveEffectParameters passParams)
    {
        switch (item)
        {
            case Item.Antiquifier:
                {
                    Active_Card cardScript = passParams.triggeredCard.GetComponent<Active_Card>();
                    Card_Base cardStats = cardScript.GetBase();
                    string cardID = cardStats.GetID();

                    cardScript.AddPowerBuff(1);
                    cardStats.BuffPower(1);
                    GM.PM.UpdateCard(cardStats.GetID(), cardStats);
                }
                break;
            case Item.Solidifier:
                {
                    Active_Card cardScript = passParams.triggeredCard.GetComponent<Active_Card>();
                    Card_Base cardStats = cardScript.GetBase();
                    string cardID = cardStats.GetID();

                    cardScript.AddDefenseBuff(1);
                    cardStats.BuffDefense(1);
                    GM.PM.UpdateCard(cardStats.GetID(), cardStats);
                }
                break;
            case Item.Bandage:
                GM.PM.HealHealth(25);
                break;
            default:
                break;
        }
    }

    public void TriggerItemPickup(Item item, PassiveEffectParameters passParams)
    {
        switch (item)
        {
            case Item.CrystalBall:
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

    public bool CanTriggerActive(Item item)
    {
        switch (item)
        {
            case Item.Default:
                return true;
            case Item.RunicHammer:
                return GM.CPH.HandSize() > 0;
            case Item.ManaRune:
                return !GM.CPD.DeckIsEmpty() && !GM.CPH.AtHandLimit();
            case Item.AGun:
                return true;
            case Item.PageRipper:
                return GM.CPH.HandSize() > 0;
            default:
                return false;
        }
    }

    public bool WillTriggerPassive(Item item, PassiveEffectParameters passParams)
    {
        switch (item)
        {
            case Item.CrystalBall:
            case Item.Bandage:
                return true;
            case Item.Antiquifier:
                return passParams.triggeredCard != null;
            case Item.Solidifier:
                return passParams.triggeredCard != null;
            default:
                return false;
        }
    }
}

public class PassiveEffectParameters
{
    public GameObject triggeredCard;

    public static PassiveEffectParameters TriggeredCard(GameObject triggeredCard, GameObject opposingCard)
    {
        PassiveEffectParameters ret = new PassiveEffectParameters();
        ret.triggeredCard = triggeredCard;
        return ret;
    }
}