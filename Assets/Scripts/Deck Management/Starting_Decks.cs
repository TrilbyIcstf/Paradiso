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
        List<CardAffinity> affinityList = new List<CardAffinity>();
        int flowCount = 0;
        int incinerateCount = 0;
        int tremorCount = 0;
        int quillCount = 0;
        int spreadCount = 0;

        int synergyLeftCard = Random.Range(0, 30);
        int synergyRightCard = Random.Range(0, 30);

        int synthesisLeftCard;
        int synthesisRightCard;

        do
        {
            synthesisLeftCard = Random.Range(0, 30);
        } while (synergyLeftCard == synthesisLeftCard);

        do
        {
            synthesisRightCard = Random.Range(0, 30);
        } while (synergyRightCard == synthesisRightCard);


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

            affinityList.Add(CardAffinity.Terra);
            affinityList.Add(CardAffinity.Terra);
            affinityList.Add(CardAffinity.Luna);
            affinityList.Add(CardAffinity.Luna);
            affinityList.Add(CardAffinity.Sol);
        }

        for (int i = 0; i < 30; i++)
        {
            int randAttack = Random.Range(0, powerList.Count);
            int randDefense = Random.Range(0, defenseList.Count);
            int randElement = Random.Range(0, elementList.Count);
            int randAffinity = Random.Range(0, affinityList.Count);
            int cardAttack = powerList[randAttack];
            powerList.RemoveAt(randAttack);
            int cardDefense = defenseList[randDefense];
            defenseList.RemoveAt(randDefense);
            CardElement cardElement = elementList[randElement];
            elementList.RemoveAt(randElement);
            CardAffinity cardAffinity = affinityList[randAffinity];
            affinityList.RemoveAt(randAffinity);
            CardEffect? cardEffect = null;
            switch(cardElement)
            {
                case CardElement.Earth:
                    if (tremorCount < 2)
                    {
                        cardEffect = CardEffect.Tremor;
                        tremorCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffect.Spread;
                        spreadCount++;
                    }
                    break;
                case CardElement.Fire:
                    if (incinerateCount < 2)
                    {
                        cardEffect = CardEffect.Incinerate;
                        incinerateCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffect.Spread;
                        spreadCount++;
                    }
                    break;
                case CardElement.Water:
                    if (flowCount < 2)
                    {
                        cardEffect = CardEffect.Flow;
                        flowCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffect.Spread;
                        spreadCount++;
                    }
                    break;
                case CardElement.Wind:
                    if (quillCount < 2)
                    {
                        cardEffect = CardEffect.Quills;
                        quillCount++;
                    } else if (spreadCount < 4)
                    {
                        cardEffect = CardEffect.Spread;
                        spreadCount++;
                    }
                    break;
                default:
                    break;
            }

            List<CardEffect> cardEffects = new List<CardEffect>();
            if (cardEffect is { } effectValue)
            {
                cardEffects.Add(effectValue);
            }

            if (i == synergyLeftCard)
            {
                cardEffects.Add(CardEffect.SynergyLeft);
            }

            if (i == synergyRightCard)
            {
                cardEffects.Add(CardEffect.SynergyRight);
            }

            if (i == synthesisLeftCard)
            {
                //cardEffects.Add(CardEffect.);
            }

            if (i == synthesisRightCard)
            {
                //cardEffects.Add(CardEffect.);
            }

            Card_Base card = Card_Base.NewCard(i.ToString(), "Scroll", cardAttack, cardDefense, cardElement, cardAffinity, cardEffects);
            deck[i.ToString()] = card;
        }

        return deck;
    }
}