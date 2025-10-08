using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 3)]
[System.Serializable]
public class Card_Base : ScriptableObject
{
    public string cardName;

    public int power;
    public int defense;

    public CardElement element;

    public CardEffects effect;

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
                cardBase.effect = CardEffects.Quills;
            }
            else if (cardBase.element == CardElement.Fire)
            {
                cardBase.effect = CardEffects.Incinerate;
            }
            else if (cardBase.element == CardElement.Water)
            {
                cardBase.effect = CardEffects.Flow;
            }
            else if (cardBase.element == CardElement.Earth)
            {
                cardBase.effect = CardEffects.Tremor;
            }
            else
            {
                cardBase.effect = CardEffects.None;
            }
        } else
        {
            cardBase.effect = CardEffects.None;
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
            0 => CardElement.Nill,
            1 => CardElement.Fire,
            2 => CardElement.Wind,
            3 => CardElement.Earth,
            4 => CardElement.Water,
            _ => CardElement.Nill,
        };
    }
}
