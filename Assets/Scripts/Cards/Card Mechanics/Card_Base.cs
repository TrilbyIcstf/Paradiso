using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base stats and effects for a card. Used to track contents in a deck.
/// </summary>
[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 3)]
[System.Serializable]
public class Card_Base : ScriptableObject
{
    // Unique ID used when making changes to the content of a deck.
    private string cardID;

    [SerializeField]
    private string cardName;

    [SerializeField]
    private int power;
    [SerializeField]
    private int defense;

    [SerializeField]
    private CardElement element;

    [SerializeField]
    private List<CardEffects> effects = new List<CardEffects>();

    public static Card_Base NewCard(string id, string name, int power, int defense, CardElement element, List<CardEffects> effects)
    {
        Card_Base cardBase = ScriptableObject.CreateInstance<Card_Base>();
        cardBase.cardID = id;
        cardBase.cardName = name;
        cardBase.power = power;
        cardBase.defense = defense;
        cardBase.element = element;

        cardBase.effects = effects != null ? effects : new List<CardEffects>();

        return cardBase;
    }

    public void BuffPower(int buff)
    {
        this.power += buff;
    }

    public void BuffDefense(int buff)
    {
        this.defense += buff;
    }

    public string GetID()
    {
        return this.cardID;
    }

    public void SetID(string val)
    {
        this.cardID = val;
    }

    /// <summary>
    /// Randomizes a card's stats for testing
    /// </summary>
    /// <returns>The randomized stats</returns>
    public static Card_Base RandomizeStats()
    {
        Card_Base cardBase = ScriptableObject.CreateInstance<Card_Base>();
        cardBase.cardName = "Random Card";
        cardBase.power = Random.Range(5, 20);
        cardBase.defense = Random.Range(5, 20);
        cardBase.element = RandomElement();

        int effectChance = Random.Range(1, 10);
        if (effectChance >= 8)
        {
            if (cardBase.element == CardElement.Wind)
            {
                cardBase.effects.Add(CardEffects.Quills);
            }
            else if (cardBase.element == CardElement.Fire)
            {
                cardBase.effects.Add(CardEffects.Incinerate);
            }
            else if (cardBase.element == CardElement.Water)
            {
                cardBase.effects.Add(CardEffects.Flow);
            }
            else if (cardBase.element == CardElement.Earth)
            {
                cardBase.effects.Add(CardEffects.Tremor);
            }
            else
            {
                cardBase.effects.Add(CardEffects.None);
            }
        } else if (effectChance == 7 && cardBase.element != CardElement.Nil)
        {
            cardBase.effects.Add(CardEffects.Spread);
        }

        return cardBase;
    }

    /// <summary>
    /// Generates a random element for testing
    /// </summary>
    /// <returns>A random element</returns>
    private static CardElement RandomElement()
    {
        int elementIndex = Random.Range(0, 5);
        return elementIndex switch
        {
            0 => CardElement.Nil,
            1 => CardElement.Fire,
            2 => CardElement.Wind,
            3 => CardElement.Earth,
            4 => CardElement.Water,
            _ => CardElement.Nil,
        };
    }

    public string GetName()
    {
        return this.cardName;
    }

    public int GetPower()
    {
        return this.power;
    }

    public int GetDefense()
    {
        return this.defense;
    }

    public CardElement GetElement()
    {
        return this.element;
    }

    public List<CardEffects> GetEffects()
    {
        return this.effects;
    }
}
