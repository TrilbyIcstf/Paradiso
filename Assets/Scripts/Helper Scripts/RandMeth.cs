using UnityEngine;

public static class RandMeth
{
    public static bool RollDie(int sides)
    {
        int randNum = Random.Range(0, sides);
        return randNum == 0;
    }

    public static bool PercentChance(float successRate)
    {
        float randNum = Random.Range(0.0f, 100.0f);
        return randNum <= successRate;
    }
}
