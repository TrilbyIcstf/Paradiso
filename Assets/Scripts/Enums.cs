using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public enum CardElement
{
    Fire,
    Water,
    Earth,
    Wind,
    Nil
}

public static class ElementMethods
{
    private class ElementEffectiveness
    {
        internal CardElement attackingType;
        internal CardElement defendingType;
        internal float mult;

        public ElementEffectiveness(CardElement attacking, CardElement defending, float val)
        {
            this.attackingType = attacking;
            this.defendingType = defending;
            this.mult = val;
        }
    }

    private static List<ElementEffectiveness> elementMultList = new List<ElementEffectiveness>()
    {
        new ElementEffectiveness(CardElement.Nil, CardElement.Nil, 1.0f),
        new ElementEffectiveness(CardElement.Fire, CardElement.Wind, Consts.SU),
        new ElementEffectiveness(CardElement.Fire, CardElement.Water, Consts.NVE),
        new ElementEffectiveness(CardElement.Water, CardElement.Fire, Consts.SU),
        new ElementEffectiveness(CardElement.Water, CardElement.Earth, Consts.NVE),
        new ElementEffectiveness(CardElement.Earth, CardElement.Water, Consts.SU),
        new ElementEffectiveness(CardElement.Earth, CardElement.Wind, Consts.NVE),
        new ElementEffectiveness(CardElement.Wind, CardElement.Earth, Consts.SU),
        new ElementEffectiveness(CardElement.Wind, CardElement.Fire, Consts.NVE)
    };

    public static float EffectivenessMult(this CardElement attacking, CardElement defending)
    {
        float mult = elementMultList.Find(m => attacking == m.attackingType && defending == m.defendingType)?.mult ?? 1.0f;

        return mult;
    }

    /// <summary>
    /// Generates a random element
    /// </summary>
    /// <returns>A random element</returns>
    public static CardElement RandomElement()
    {
        int elementIndex = UnityEngine.Random.Range(0, 5);
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
}

public enum CardAffinity
{
    Sol,
    Terra,
    Luna
}

public static class AffinityMethods
{
    public static CardAffinity RandomAffinity()
    {
        int elementIndex = UnityEngine.Random.Range(0, 5);
        return elementIndex switch
        {
            0 => CardAffinity.Sol,
            < 3 => CardAffinity.Terra,
            _ => CardAffinity.Luna
        };
    }

    public static float AffinityPowerBoost(this CardAffinity? affinity) => affinity switch
    {
        CardAffinity.Sol => 1.5f,
        CardAffinity.Terra => 1.5f,
        CardAffinity.Luna => 1.0f,
        _ => 1.0f
    };

    public static float AffinityDefenseBoost(this CardAffinity? affinity) => affinity switch
    {
        CardAffinity.Sol => 2.0f,
        CardAffinity.Terra => 1.0f,
        CardAffinity.Luna => 2.0f,
        _ => 1.0f
    };
}

public enum EffectTiming
{
    Pickup,
    Draw,
    CardScoredBefore,
    CardScoredAfter,
    ItemUsed,
    DamageDealt,
    DamageTaken,
    CombatEnd
}

public enum Item
{
    Default,
    RunicHammer, // Active: Buffs card
    ManaRune, // Active: Draw card
    AGun, // Active: Gun
    PageRipper, // Active: Splits page
    Antiquifier, // Passive: Buffs power on play
    Solidifier, // Passive: Buffs defense on play
    Bandage, // Passive: Heal after fight
    CrystalBall, // Pickup: Increases max mana
    DiamondBall, // Pickup: Increases max mana
    ManaReservoir, // Active: Refills mana
    ClairvoyantGem // Pickup: Increases mana regen
}

public static class ItemMethods { 
    public static bool IsActive(this Item item)
    {
        switch (item)
        {
            case Item.RunicHammer:
            case Item.ManaRune:
            case Item.AGun:
            case Item.PageRipper:
            case Item.ManaReservoir:
                return true;
            case Item.Antiquifier:
            case Item.Solidifier:
            case Item.Bandage:
            case Item.CrystalBall:
            case Item.DiamondBall:
            case Item.ClairvoyantGem:
                return false;
            default:
                return false;
        }
    }

    public static List<Item> GetItemRoomViableList()
    {
        return Enum.GetValues(typeof(Item)).Cast<Item>().Where(i => i.IsItemRoomViable()).ToList();
    }

    private static bool IsItemRoomViable(this Item item)
    {
        switch (item)
        {
            case Item.Default:
                return false;
            default:
                return true;
        }
    }
}

[Serializable]
public enum CardEffect
{
    None,
    Quills,
    Incinerate,
    Flow,
    Tremor,
    Spread,
    SynergyLeft,
    SynergyRight,
    SynthesisLeft,
    SynthesisRight
}

public static class EffectMethods
{
    public static Direction DirectionOverride(this CardEffect effect)
    {
        switch (effect)
        {
            case CardEffect.SynergyLeft:
            case CardEffect.SynthesisLeft:
                return Direction.Left;
            case CardEffect.SynergyRight:
            case CardEffect.SynthesisRight:
                return Direction.Right;
            default:
                return Direction.None;
        }
    }
}

public enum Direction
{
    None,
    Up,
    Left,
    Right,
    Down
}

public static class DirectionMethods
{
    public static Vector2Int NumericalDirection(this Direction dir)
    {
        switch(dir)
        {
            case Direction.None:
                return new Vector2Int(0, 0);
            case Direction.Up:
                return new Vector2Int(0, 1);
            case Direction.Left:
                return new Vector2Int(-1, 0);
            case Direction.Right:
                return new Vector2Int(1, 0);
            case Direction.Down:
                return new Vector2Int(0, -1);
            default:
                return new Vector2Int(0, 0);
        }
    }

    public static Direction OppositeDirection(this Direction dir)
    {
        switch(dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                return Direction.None;
        }
    }
}

public enum RoomType
{
    Empty,
    Starting,
    Enemy,
    Item,
    Stairs
}

public enum Floor
{
    Demo
}

public enum EnemyDrawType
{
    Random,
    AllFire,
    FireHeavy,
    AllWind,
    WindHeavy,
    AllWater,
    WaterHeavy,
    AllEarth,
    EarthHeavy,
    HighPower,
    HighDefense,
    AllNil
}

public enum Enemy
{
    Test,
    Cherub,
    FerventCrusader,
    TranquilCrusader,
    SteadfastCrusader,
    SpiritedCrusader,
    BurningSeraph
}