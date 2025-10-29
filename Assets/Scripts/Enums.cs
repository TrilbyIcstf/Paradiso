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
}

public enum EffectTiming
{
    Draw,
    CardScoredBefore,
    CardScoredAfter,
    ItemUsed,
    DamageDealt,
    DamageTaken
}

public enum Items
{
    Default,
    DamageTest,
    CheapDrawTest,
    AGun,
    PageRipper,
    Antiquifier,
    Solidifier
}

public static class ItemMethods { 
    public static bool IsActive(this Items item)
    {
        switch (item)
        {
            case Items.DamageTest:
            case Items.CheapDrawTest:
            case Items.AGun:
            case Items.PageRipper:
                return true;
            case Items.Antiquifier:
            case Items.Solidifier:
                return false;
            default:
                return false;
        }
    }

    public static List<Items> GetItemRoomViableList()
    {
        return Enum.GetValues(typeof(Items)).Cast<Items>().Where(i => i.IsItemRoomViable()).ToList();
    }

    private static bool IsItemRoomViable(this Items item)
    {
        switch (item)
        {
            case Items.AGun:
            case Items.Default:
                return false;
            default:
                return true;
        }
    }
}

[Serializable]
public enum CardEffects
{
    None,
    Quills,
    Incinerate,
    Flow,
    Tremor,
    Spread
}

public enum Directions
{
    None,
    Up,
    Left,
    Right,
    Down
}

public static class DirectionMethods
{
    public static Vector2Int NumericalDirection(this Directions dir)
    {
        switch(dir)
        {
            case Directions.None:
                return new Vector2Int(0, 0);
            case Directions.Up:
                return new Vector2Int(0, 1);
            case Directions.Left:
                return new Vector2Int(-1, 0);
            case Directions.Right:
                return new Vector2Int(1, 0);
            case Directions.Down:
                return new Vector2Int(0, -1);
            default:
                return new Vector2Int(0, 0);
        }
    }

    public static Directions OppositeDirection(this Directions dir)
    {
        switch(dir)
        {
            case Directions.Up:
                return Directions.Down;
            case Directions.Down:
                return Directions.Up;
            case Directions.Left:
                return Directions.Right;
            case Directions.Right:
                return Directions.Left;
            default:
                return Directions.None;
        }
    }
}

public enum RoomTypes
{
    Empty,
    Starting,
    Enemy,
    Item,
    Stairs
}

public enum Floors
{
    Demo
}

public enum EnemyDrawTypes
{
    Random,
    AllFire,
    AllNil
}