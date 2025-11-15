using System.Collections.Generic;
using UnityEngine;

public static class Starting_Decks
{
    /// <summary>
    /// The basic starting deck.
    /// 30 card, 6 of each element.
    /// 2 of each power/defense values 5-18, 1 value of 19 and 20.
    /// 2 of each basic elemental effect. 4 random split effects.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, Card_Base> BasicStartingDeck()
    {
        Dictionary<string, Card_Base> deck = new Dictionary<string, Card_Base>();
        List<int> powerList = new List<int>();
        List<int> defenseList = new List<int>();
        List<CardElement> elementList = new List<CardElement>();
        int flowCount = 0;
        int incinerateCount = 0;
        int tremorCount = 0;
        int quillCount = 0;
        int spreadCount = 0;

        // Build stat lists
        for (int i = 5; i < 19; i++)
        {
            powerList.Add(i);
            powerList.Add(i);
            defenseList.Add(i);
            defenseList.Add(i);
        }
        powerList.Add(19);
        powerList.Add(20);
        defenseList.Add(19);
        defenseList.Add(20);

        // Build element list
        for (int i = 0; i < 6; i++)
        {
            elementList.Add(CardElement.Earth);
            elementList.Add(CardElement.Fire);
            elementList.Add(CardElement.Nil);
            elementList.Add(CardElement.Water);
            elementList.Add(CardElement.Wind);
        }

        for (int i = 0; i < 30; i++)
        {
            int randAttack = Random.Range(0, powerList.Count);
            int randDefense = Random.Range(0, defenseList.Count);
            int randElement = Random.Range(0, elementList.Count);
            int cardAttack = powerList[randAttack];
            powerList.RemoveAt(randAttack);
            int cardDefense = defenseList[randDefense];
            defenseList.RemoveAt(randDefense);
            CardElement cardElement = elementList[randElement];
            elementList.RemoveAt(randElement);
            CardEffects? cardEffect = null;
            switch(cardElement)
            {
                case CardElement.Earth:
                    if (tremorCount < 2)
                    {
                        cardEffect = CardEffects.Tremor;
                        tremorCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffects.Spread;
                        spreadCount++;
                    }
                    break;
                case CardElement.Fire:
                    if (incinerateCount < 2)
                    {
                        cardEffect = CardEffects.Incinerate;
                        incinerateCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffects.Spread;
                        spreadCount++;
                    }
                    break;
                case CardElement.Water:
                    if (flowCount < 2)
                    {
                        cardEffect = CardEffects.Flow;
                        flowCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffects.Spread;
                        spreadCount++;
                    }
                    break;
                case CardElement.Wind:
                    if (quillCount < 2)
                    {
                        cardEffect = CardEffects.Quills;
                        quillCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffects.Spread;
                        spreadCount++;
                    }
                    break;
                default:
                    break;
            }

            List<CardEffects> cardEffects = new List<CardEffects>();
            if (cardEffect is { } effectValue)
            {
                cardEffects.Add(effectValue);
            }
            Card_Base card = Card_Base.NewCard(i.ToString(), "Scroll", cardAttack, cardDefense, cardElement, cardEffects);
            deck[i.ToString()] = card;
        }

        return deck;
    }
}