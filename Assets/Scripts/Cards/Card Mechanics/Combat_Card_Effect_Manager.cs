using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Card_Effect_Manager : ManagerBehavior
{
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
            default:
                break;
        }
        yield return new WaitForSeconds(0.0f);
    }

    private IEnumerator QuillsEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        Card_Base quill = Static_Object_Manager.instance.QuillCard;
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

    private void FlowEffect(GameObject card, CardEffectParameters effParams, bool isPlayer)
    {
        (isPlayer ? (Stats_Manager)GM.CPS : GM.CES).AddFreeCards(effParams.adjacency);
    }

    public bool EffectIsTriggered(CardEffects effect, CardEffectParameters effParams)
    {
        switch (effect)
        {
            case CardEffects.Quills:
            case CardEffects.Flow:
            case CardEffects.Tremor:
                return effParams.adjacency > 0;
            case CardEffects.Incinerate:
                return effParams.adjacency > 0 && effParams.opponentHandSize > 0;
            default:
                return false;
        }
    }
}

public class CardEffectParameters {
    public int adjacency;

    public float power;
    public float defense;

    public int handSize;
    public int opponentHandSize;
}