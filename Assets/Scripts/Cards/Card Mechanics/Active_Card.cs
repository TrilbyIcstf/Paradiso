using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Card : MonoBehaviour
{
    [SerializeField]
    private Card_Base cardStats;

    private Card_UI ui;

    // Additive buffs to a card's stats
    private float powerBuff = 0;
    private float defenseBuff = 0;

    // Multiplicative buffs to add to a card's stats
    private float powerMult = 1;
    private float defenseMult = 1;

    private void Awake()
    {
        this.ui = GetComponent<Card_UI>();

        this.ui.SetPower(GetPower());
        this.ui.SetDefense(GetDefense());
        this.ui.SetElement(GetElement());
    }

    /// <summary>
    /// Randomizes a card's stats for testing
    /// </summary>
    /// <returns>The randomized stats</returns>
    public Card_Base RandomizeStats()
    {
        Card_Base cardBase = ScriptableObject.CreateInstance<Card_Base>();
        cardBase.cardName = "Random Card";
        cardBase.power = Random.Range(5, 20);
        cardBase.defense = Random.Range(5, 20);
        cardBase.element = RandomElement();
        this.cardStats = cardBase;

        return cardBase;
    }

    /// <summary>
    /// Generates a random element for testing
    /// </summary>
    /// <returns>A random element</returns>
    private CardElement RandomElement()
    {
        int elementIndex = Random.Range(0, 5);
        return elementIndex switch
        {
            0 => CardElement.Nill,
            1 => CardElement.Fire,
            2 => CardElement.Wind,
            3 => CardElement.Earth,
            4 => CardElement.Water,
            _ => CardElement.Nill,
        };
    }

    public void AddMultPower(float val)
    {
        this.powerMult += val;
        this.ui.SetPower(GetPower());
    }

    public Card_Base GetBase()
    {
        return this.cardStats;
    }

    public int GetPower()
    {
        float power = (this.cardStats.power + this.powerBuff) * this.powerMult;
        return Mathf.CeilToInt(power);
    }

    public int GetDefense()
    {
        float defense = (this.cardStats.defense + this.defenseBuff) * this.defenseMult;
        return Mathf.CeilToInt(defense);
    }

    public CardElement GetElement()
    {
        return this.cardStats.element;
    }
}
