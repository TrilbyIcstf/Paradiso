using System.Collections.Generic;
using UnityEngine;

public class Field_Card_Results
{
    internal GameObject card;

    internal List<CardEffect> effects = new List<CardEffect>();
    internal CardEffectParameters effParams;
    internal Active_Card stats;

    internal bool flashLeft = false;
    internal bool flashMiddle = false;
    internal bool flashRight = false;

    internal bool advantage = false;

    internal int position = 0;

    internal float totalDamage = 0;
    internal float finalPower = 0;
    internal float finalDefense = 0;
}

public class Field_Full_Results
{
    private Field_Card_Results[] playerResults;
    private Field_Card_Results[] enemyResults;

    private int size;

    private bool playerAffinity = false;
    private bool enemyAffinity = false;

    public Field_Full_Results(int size)
    {
        this.playerResults = new Field_Card_Results[size];
        this.enemyResults = new Field_Card_Results[size];
        this.size = size;
    }

    public Field_Card_Results GetPlayerResult(int pos)
    {
        return this.playerResults[pos];
    }

    public Field_Card_Results GetEnemyResult(int pos)
    {
        return this.enemyResults[pos];
    }

    public bool GetPlayerAffinity()
    {
        return this.playerAffinity;
    }

    public bool GetEnemyAffinity()
    {
        return this.enemyAffinity;
    }

    public int GetSize()
    {
        return this.size;
    }

    public void SetPlayerResult(int pos, Field_Card_Results val)
    {
        this.playerResults[pos] = val;
    }

    public void SetEnemyResult(int pos, Field_Card_Results val)
    {
        this.enemyResults[pos] = val;
    }

    public void SetPlayerAffinity(bool val)
    {
        this.playerAffinity = val;
    }

    public void SetEnemyAffinity(bool val)
    {
        this.enemyAffinity = val;
    }
}