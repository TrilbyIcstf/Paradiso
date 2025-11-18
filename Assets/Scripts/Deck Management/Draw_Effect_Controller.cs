using UnityEngine;

public class Draw_Effect_Controller
{
    public static CardEffect TestBasicEffect(CardElement elem)
    {
        switch (elem)
        {
            case CardElement.Earth:
                return CardEffect.Tremor;
            case CardElement.Fire:
                return CardEffect.Incinerate;
            case CardElement.Water:
                return CardEffect.Flow;
            case CardElement.Wind:
                return CardEffect.Quills;
            default:
                return CardEffect.None;
        }
    }
}
