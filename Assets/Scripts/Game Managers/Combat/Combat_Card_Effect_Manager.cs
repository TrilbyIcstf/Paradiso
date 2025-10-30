using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages card effects that trigger during combat
/// </summary>
public class Combat_Card_Effect_Manager : ManagerBehavior
{
    /// <summary>
    /// Triggers a single effect of a card
    /// </summary>
    /// <param name="effect">The effect triggering</param>
    /// <param name="card">The card GameObject triggering the effect</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <param name="isPlayer">If it's the player triggering the effect</param>
    /// <returns></returns>
    public IEnumerator TriggerCardEffect(CardEffects effect, GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        switch (effect)
        {
            case CardEffects.Quills:
                yield return StartCoroutine(QuillsEffect(card, effParams, isPlayer));
                break;
            case CardEffects.Incinerate:
                IncinerateEffect(card, effParams, isPlayer);
                break;
            case CardEffects.Tremor:
                TremorEffect(card, effParams, isPlayer);
                break;
            case CardEffects.Flow:
                FlowEffect(card, effParams, isPlayer);
                break;
            case CardEffects.Spread:
                SpreadEffect(card, effParams, isPlayer);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.0f);
    }

    /// <summary>
    /// Quills spawns a quill (2/2 wind) card for each adjacent match
    /// </summary>
    /// <param name="card">The card GameObject triggering the effect</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <param name="isPlayer">If it's the player triggering the effect</param>
    private IEnumerator QuillsEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        Card_Base quill = Static_Object_Manager.instance.GetQuillCard();
        for (int i = 0; i < effParams.adjacency; i++)
        {
            if (isPlayer)
            {
                GM.CPS.CreateNewCard(quill, card.transform);
            } else
            {
                GM.CES.CreateNewCard(quill, card.transform);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Incinerate debuffs random cards in the opponent's hand for each adjacent match
    /// </summary>
    /// <param name="card">The card GameObject triggering the effect</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <param name="isPlayer">If it's the player triggering the effect</param>
    private void IncinerateEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        List<int> randCards = GM.CEH.PickRandomCardsPos(effParams.adjacency);
        foreach (int pos in randCards)
        {
            GameObject targetCard = (isPlayer ? (Hand_Manager)GM.CEH : GM.CPH).GetCard(pos);
            targetCard.GetComponent<Active_Card>().AddPowerBuff(-5);
            targetCard.GetComponent<Card_UI>().EmphasizeCardCo();
        }
    }

    /// <summary>
    /// Tremor adds a delay to energy regen on the enemy, or subtracts some energy from the player for each adjacent match
    /// </summary>
    /// <param name="card">The card GameObject triggering the effect</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <param name="isPlayer">If it's the player triggering the effect</param>
    private void TremorEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        if (isPlayer)
        {
            GM.CES.AddEnergyDelay(1.5f * effParams.adjacency);
        } else
        {
            GM.CPS.SubtractEnergy(10 * effParams.adjacency, true);
        }
    }

    /// <summary>
    /// Flow draws a card for each adjacent match
    /// </summary>
    /// <param name="card">The card GameObject triggering the effect</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <param name="isPlayer">If it's the player triggering the effect</param>
    private void FlowEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        (isPlayer ? (Stats_Manager)GM.CPS : GM.CES).AddFreeCards(effParams.adjacency);
    }

    /// <summary>
    /// Spread converts a random card in the hand to match the triggering element for each adjacent match
    /// </summary>
    /// <param name="card">The card GameObject triggering the effect</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <param name="isPlayer">If it's the player triggering the effect</param>
    private void SpreadEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        CardElement elem = card.GetComponent<Active_Card>().GetElement();
        List<int> randCards = (isPlayer ? (Hand_Manager)GM.CPH : GM.CEH).PickRandomCardsPos(effParams.adjacency, c => c.GetComponent<Active_Card>().GetElement() != elem);
        foreach (int pos in randCards)
        {
            GameObject targetCard = (isPlayer ? (Hand_Manager)GM.CPH : GM.CEH).GetCard(pos);
            targetCard.GetComponent<Active_Card>().SetElement(elem);
            targetCard.GetComponent<Card_UI>().EmphasizeCardCo();
        }
    }

    /// <summary>
    /// Checks if an effect from the list will be triggered
    /// </summary>
    /// <param name="effects">List of effects to check</param>
    /// <param name="effParams">Parameters for effect calcs</param>
    /// <returns>True if an effect will trigger, false otherwise</returns>
    public bool EffectIsTriggered(List<CardEffects> effects, CardEffectParameters effParams)
    {
        foreach (CardEffects effect in effects)
        {
            switch (effect)
            {
                case CardEffects.Quills:
                case CardEffects.Flow:
                case CardEffects.Tremor:
                    if (effParams.adjacency > 0)
                    {
                        return true;
                    }
                    break;
                case CardEffects.Incinerate:
                    if (effParams.adjacency > 0 && effParams.opponentHandSize > 0)
                    {
                        return true;
                    }
                    break;
                case CardEffects.Spread:
                    if (effParams.adjacency > 0 && effParams.handSize > 0)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
        }
        return false;
    }
}

public class CardEffectParameters {
    // Number of adjacent cards with the same element
    public int adjacency;

    // Power an defense of the triggering card
    public float power;
    public float defense;

    // Hand size of the user and opponent
    public int handSize;
    public int opponentHandSize;
}