using System.Collections.Generic;
using System;
using UnityEngine;

public enum CardElement
{
    Fire,
    Water,
    Earth,
    Wind,
    Nill
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
        new ElementEffectiveness(CardElement.Nill, CardElement.Nill, 1.0f),
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

public enum Items
{
    Default,
    DamageTest,
    CheapDrawTest
}

[Serializable]
public enum CardEffects
{
    None,
    Quills,
    Incinerate,
    Flow,
    Tremor
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