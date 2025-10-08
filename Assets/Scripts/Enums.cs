using System.Collections.Generic;
using System;

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
