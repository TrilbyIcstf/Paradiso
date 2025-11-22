using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_Draw_Controller : MonoBehaviour
{
    public static Card_Base DecideCard(EnemyDrawType type, Enemy_Stats_State state, List<Card_Base> drawnCards)
    {
        switch (type)
        {
            case EnemyDrawType.AllFire:
                return AllElementCard(state, drawnCards, CardElement.Fire);
            case EnemyDrawType.AllEarth:
                return AllElementCard(state, drawnCards, CardElement.Earth);
            case EnemyDrawType.AllWater:
                return AllElementCard(state, drawnCards, CardElement.Water);
            case EnemyDrawType.AllWind:
                return AllElementCard(state, drawnCards, CardElement.Wind);
            case EnemyDrawType.FireHeavy:
                return HeavyElementCard(state, drawnCards, CardElement.Fire);
            case EnemyDrawType.EarthHeavy:
                return HeavyElementCard(state, drawnCards, CardElement.Earth);
            case EnemyDrawType.WaterHeavy:
                return HeavyElementCard(state, drawnCards, CardElement.Water);
            case EnemyDrawType.WindHeavy:
                return HeavyElementCard(state, drawnCards, CardElement.Wind);
            default:
                return RandomCard(state, drawnCards);
        }
    }

    public static Card_Base RandomCard(Enemy_Stats_State state, List<Card_Base> drawnCards)
    {
        int minPow = state.GetMinPower();
        int maxPow = state.GetMaxPower();
        int minDef = state.GetMinDefense();
        int maxDef = state.GetMaxDefense();
        int powQuality = state.GetPowerQuality();
        int defQuality = state.GetDefenseQuality();
        float effectChance = state.GetEffectRate();

        string cardName = "Page";
        int power = RollForStat(minPow, maxPow, powQuality);
        int defense = RollForStat(minDef, maxDef, defQuality);
        CardElement element = ElementMethods.RandomElement();
        CardAffinity affinity = AffinityMethods.RandomAffinity();

        if (drawnCards.Count >= 5)
        {
            power = AdjustStatTowardsAverage(power, minPow, maxPow, powQuality, DrawnAveragePower(drawnCards));
            defense = AdjustStatTowardsAverage(defense, minDef, maxDef, defQuality, DrawnAverageDefense(drawnCards));
            float rateDiff = effectChance - DrawnEffectPercent(drawnCards);
            effectChance += rateDiff;
        }

        List<CardEffect> effects = new List<CardEffect>();

        float randNum = Random.Range(0.0f, 100.0f);
        if (randNum <= effectChance - 10)
        {
            if (element == CardElement.Wind)
            {
                effects.Add(CardEffect.Quills);
            }
            else if (element == CardElement.Fire)
            {
                effects.Add(CardEffect.Incinerate);
            }
            else if (element == CardElement.Water)
            {
                effects.Add(CardEffect.Flow);
            }
            else if (element == CardElement.Earth)
            {
                effects.Add(CardEffect.Tremor);
            }
        }
        else if (randNum <= effectChance && element != CardElement.Nil)
        {
            effects.Add(CardEffect.Spread);
        }

        Card_Base cardBase = Card_Base.NewCard("", cardName, power, defense, element, affinity, effects);

        return cardBase;
    }

    private static Card_Base AllElementCard(Enemy_Stats_State state, List<Card_Base> drawnCards, CardElement element)
    {
        int minPow = state.GetMinPower();
        int maxPow = state.GetMaxPower();
        int minDef = state.GetMinDefense();
        int maxDef = state.GetMaxDefense();
        int powQuality = state.GetPowerQuality();
        int defQuality = state.GetDefenseQuality();
        float effectChance = state.GetEffectRate();

        string cardName = "Page";
        int power = RollForStat(minPow, maxPow, powQuality);
        int defense = RollForStat(minDef, maxDef, defQuality);
        CardAffinity affinity = AffinityMethods.RandomAffinity();
        
        if (drawnCards.Count >= 5) {
            power = AdjustStatTowardsAverage(power, minPow, maxPow, powQuality, DrawnAveragePower(drawnCards));
            defense = AdjustStatTowardsAverage(defense, minDef, maxDef, defQuality, DrawnAverageDefense(drawnCards));
            float rateDiff = effectChance - DrawnEffectPercent(drawnCards);
            effectChance += rateDiff;
        }

        List<CardEffect> effects = new List<CardEffect>();
        float randNum = Random.Range(0.0f, 100.0f);
        if (randNum <= effectChance)
        {
            switch (element)
            {
                case CardElement.Fire:
                    effects = RandomFireEffects();
                    break;
                case CardElement.Earth:
                    effects = RandomEarthEffects();
                    break;
                case CardElement.Water:
                    effects = RandomWaterEffects();
                    break;
                case CardElement.Wind:
                    effects = RandomWindEffects();
                    break;
            }
            
        }

        Card_Base cardBase = Card_Base.NewCard("", cardName, power, defense, element, affinity, effects);

        return cardBase;
    }

    private static Card_Base HeavyElementCard(Enemy_Stats_State state, List<Card_Base> drawnCards, CardElement element)
    {
        int minPow = state.GetMinPower();
        int maxPow = state.GetMaxPower();
        int minDef = state.GetMinDefense();
        int maxDef = state.GetMaxDefense();
        int powQuality = state.GetPowerQuality();
        int defQuality = state.GetDefenseQuality();
        float effectChance = state.GetEffectRate();

        string cardName = "Page";
        int power = RollForStat(minPow, maxPow, powQuality);
        int defense = RollForStat(minDef, maxDef, defQuality);
        CardAffinity affinity = AffinityMethods.RandomAffinity();

        if (drawnCards.Count >= 5)
        {
            power = AdjustStatTowardsAverage(power, minPow, maxPow, powQuality, DrawnAveragePower(drawnCards));
            defense = AdjustStatTowardsAverage(defense, minDef, maxDef, defQuality, DrawnAverageDefense(drawnCards));
            float rateDiff = effectChance - DrawnEffectPercent(drawnCards);
            effectChance += rateDiff;
        }

        float elemChance = 65.0f;
        CardElement randElement;
        float randNum = Random.Range(0.0f, 100.0f);
        if (randNum <= elemChance)
        {
            randElement = element;
        } 
        else
        {
            do
            {
                randElement = ElementMethods.RandomElement();
            } while (randElement == element);
        }

        List<CardEffect> effects = new List<CardEffect>();
        randNum = Random.Range(0.0f, 100.0f);
        if (randNum <= effectChance)
        {
            switch (randElement)
            {
                case CardElement.Fire:
                    effects = RandomFireEffects();
                    break;
                case CardElement.Earth:
                    effects = RandomEarthEffects();
                    break;
                case CardElement.Water:
                    effects = RandomWaterEffects();
                    break;
                case CardElement.Wind:
                    effects = RandomWindEffects();
                    break;
            }

        }

        Card_Base cardBase = Card_Base.NewCard("", cardName, power, defense, randElement, affinity, effects);

        return cardBase;
    }

    private static int RollForStat(int min, int max, int quality)
    {
        int roll = 0;
        for (int i = 0; i < quality + 1; i++)
        {
            roll = Mathf.Max(Random.Range(min, max), roll);
        }
        return roll;
    }

    private static List<CardEffect> RandomFireEffects()
    {
        List<CardEffect> effects = new List<CardEffect>();
        float randNum = Random.Range(0.0f, 100.0f);

        if (randNum <= 33.0f)
        {
            effects.Add(CardEffect.Incinerate);
        } 
        else
        {
            effects.Add(CardEffect.Spread);
        }

        return effects;
    }
    private static List<CardEffect> RandomEarthEffects()
    {
        List<CardEffect> effects = new List<CardEffect>();
        float randNum = Random.Range(0.0f, 100.0f);

        if (randNum <= 33.0f)
        {
            effects.Add(CardEffect.Tremor);
        } 
        else
        {
            effects.Add(CardEffect.Spread);
        }

        return effects;
    }
    private static List<CardEffect> RandomWaterEffects()
    {
        List<CardEffect> effects = new List<CardEffect>();
        float randNum = Random.Range(0.0f, 100.0f);

        if (randNum <= 33.0f)
        {
            effects.Add(CardEffect.Flow);
        } 
        else
        {
            effects.Add(CardEffect.Spread);
        }

        return effects;
    }
    private static List<CardEffect> RandomWindEffects()
    {
        List<CardEffect> effects = new List<CardEffect>();
        float randNum = Random.Range(0.0f, 100.0f);

        if (randNum <= 33.0f)
        {
            effects.Add(CardEffect.Quills);
        } 
        else
        {
            effects.Add(CardEffect.Spread);
        }

        return effects;
    }

    private static float CalcAdjustedAverage(int min, int max, int quality)
    {
        float average = StatAverage(min, max);
        if (quality > 0)
        {
            float qualityAdjustRatio = Mathf.Pow(quality, 2) / Mathf.Pow(quality + 1, 2);
            float qualityAdjust = (max - average) * qualityAdjustRatio;
            average += qualityAdjust;
        }

        return average;
    }

    private static int AdjustStatTowardsAverage(int stat, int min, int max, int quality, float drawnAverage)
    {
        float idealAverage = CalcAdjustedAverage(min, max, quality);

        int adjust = Mathf.RoundToInt(idealAverage - drawnAverage);
        return Mathf.Clamp(stat + adjust, min, max);
    }

    private static float StatAverage(float first, float second)
    {
        return (first + second) / 2.0f;
    }

    private static float DrawnAveragePower(List<Card_Base> drawnCards)
    {
        return (float)drawnCards.Average(c => c.GetPower());
    }

    private static float DrawnAverageDefense(List<Card_Base> drawnCards)
    {
        return (float)drawnCards.Average(c => c.GetDefense());
    }

    private static float DrawnEffectPercent(List<Card_Base> drawnCards)
    {
        if (!drawnCards.Any()) { return 0; }
        return (drawnCards.Count(c => c.GetEffects().Any()) / drawnCards.Count) * 100;
    }
}
