using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Effects: MonoBehaviour
{
    public static IEnumerator TriggerCardEffect(CardEffects effect, GameObject card, CardEffectParameters effParams)
    {
        switch (effect)
        {
            case CardEffects.Quills:
                Card_Base quill = Static_Object_Manager.instance.QuillCard;
                for (int i = 0; i < effParams.adjacency; i++)
                {
                    GameManager.instance.CS.CreateNewCard(quill, card.transform);
                    yield return new WaitForSeconds(0.1f);
                }
                break;
            case CardEffects.Incinerate:
                List<int> randCards = GameManager.instance.CEH.PickRandomCardsPos(effParams.adjacency);
                foreach (int pos in randCards)
                {
                    GameObject targetCard = GameManager.instance.CEH.GetCard(pos);
                    targetCard.GetComponent<Active_Card>().AddPowerBuff(-5);
                    targetCard.GetComponent<Card_UI>().EmphasizeCardCo();   
                }
                break;
            case CardEffects.Tremor:
                GameManager.instance.CES.AddEnergyDelay(0.75f * effParams.adjacency);
                break;
            case CardEffects.Flow:
                GameManager.instance.CS.AddFreeCards(effParams.adjacency);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.0f);
    }

    public static bool EffectIsTriggered(CardEffects effect, CardEffectParameters effParams)
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