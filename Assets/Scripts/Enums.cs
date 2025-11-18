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
    DamageTest,
    CheapDrawTest,
    AGun,
    PageRipper,
    Antiquifier,
    Solidifier,
    Bandage,
    CrystalBall
}

public static class ItemMethods { 
    public static bool IsActive(this Item item)
    {
        switch (item)
        {
            case Item.DamageTest:
            case Item.CheapDrawTest:
            case Item.AGun:
            case Item.PageRipper:
                return true;
            case Item.Antiquifier:
            case Item.Solidifier:
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
    Spread
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