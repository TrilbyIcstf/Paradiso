using UnityEngine;

public class English_Static : Static_Strings
{
    public override string PowerUpgrade(int val)
    {
        return $"+{val} Power";
    }

    public override string DefenseUpgrade(int val)
    {
        return $"+{val} Defense";
    }
}
