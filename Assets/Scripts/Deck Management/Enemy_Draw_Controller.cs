using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_Draw_Controller : MonoBehaviour
{
    public static Card_Base DecideCard(EnemyDrawTypes type, Enemy_Stats_State state, List<Card_Base> drawnCards)
    {
        switch (type)
        {
            case EnemyDrawTypes.AllFire:
                return AllElementCard(state, drawnCards, CardElement.Fire);
            default:
                return RandomCard(state);
        }
    }

    public static Card_Base RandomCard(Enemy_Stats_State state)
    {
        int minPow = state.GetMinPower();
        int maxPow = state.GetMaxPower();
        int minDef = state.GetMinDefense();
        int maxDef = state.GetMaxDefense();

        string cardName = "Page";
        int power = Random.Range(minPow, maxPow) + state.GetPowerBuff();
        int defense = Random.Range(minDef, maxDef) + state.GetDefenseBuff();
        CardElement element = ElementMethods.RandomElement();
        List<CardEffects> effects = new List<CardEffects>();

        float randNum = Random.Range(0.0f, 100.0f);
        if (randNum <= state.GetEffectRate() - 10)
        {
            if (element == CardElement.Wind)
            {
                effects.Add(CardEffects.Quills);
            }
            else if (element == CardElement.Fire)
            {
                effects.Add(CardEffects.Incinerate);
            }
            else if (element == CardElement.Water)
            {
                effects.Add(CardEffects.Flow);
            }
            else if (element == CardElement.Earth)
            {
                effects.Add(CardEffects.Tremor);
            }
            else
            {
                effects.Add(CardEffects.None);
            }
        }
        else if (randNum <= state.GetEffectRate() && element != CardElement.Nil)
        {
            effects.Add(CardEffects.Spread);
        }

        Card_Base cardBase = Card_Base.NewCard("", cardName, power, defense, element, effects);

        return cardBase;
    }

    private static Card_Base AllElementCard(Enemy_Stats_State state, List<Card_Base> drawnCards, CardElement element)
    {
        int minPow = state.GetMinPower();
        int maxPow = state.GetMaxPower();
        int minDef = state.GetMinDefense();
        int maxDef = state.GetMaxDefense();

        string cardName = "Page";
        int power = Random.Range(minPow, maxPow) + state.GetPowerBuff();
        int defense = Random.Range(minDef, maxDef) + state.GetDefenseBuff();
        List<CardEffects> effects = new List<CardEffects>();

        float effectChance = state.GetEffectRate();
        if (drawnCards.Count >= 5) {
            defense = AdjustStatTowardsAverage(defense, minDef, maxDef, AverageDefense(drawnCards));
            power = AdjustStatTowardsAverage(power, minPow, maxPow, AveragePower(drawnCards));
            float rateDiff = effectChance - EffectPercent(drawnCards);
            effectChance += rateDiff; 
        }

        float randNum = Random.Range(0.0f, 100.0f);
        if (randNum <= effectChance)
        {
            effects = RandomFireEffects();
        }

        Card_Base cardBase = Card_Base.NewCard("", cardName, power, defense, element, effects);

        return cardBase;
    }

    private static List<CardEffects> RandomFireEffects()
    {
        List<CardEffects> effects = new List<CardEffects>();
        float randNum = Random.Range(0.0f, 100.0f);

        if (randNum <= 33.0f)
        {
            effects.Add(CardEffects.Incinerate);
        } else
        {
            effects.Add(CardEffects.Spread);
        }

        return effects;
    }

    private static int AdjustStatTowardsAverage(int stat, int min, int max, float drawnAverage)
    {
        float idealAverage = StatAverage(min, max);

        int adjust = Mathf.RoundToInt(idealAverage - drawnAverage);
        return Mathf.Clamp(stat + adjust, min, max);
    }

    private static float StatAverage(float first, float second)
    {
        return (first + second) / 2.0f;
    }

    private static float AveragePower(List<Card_Base> drawnCards)
    {
        return (float)drawnCards.Average(c => c.GetPower());
    }

    private static float AverageDefense(List<Card_Base> drawnCards)
    {
        return (float)drawnCards.Average(c => c.GetDefense());
    }

    private static float EffectPercent(List<Card_Base> drawnCards)
    {
        if (!drawnCards.Any()) { return 0; }
        return (drawnCards.Count(c => c.GetEffects().Any()) / drawnCards.Count) * 100;
    }
}
