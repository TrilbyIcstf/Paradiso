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
        this.ui.SetEffect(GetEffect());
    }

    public void AddPowerBuff(float val)
    {
        this.powerBuff += val;
        this.ui.SetPower(GetPower());
    }

    public void AddDefenseBuff(float val)
    {
        this.defenseBuff += val;
        this.ui.SetDefense(GetDefense());
    }

    public void AddMultPower(float val)
    {
        this.powerMult += val;
        this.ui.SetPower(GetPower());
    }

    public void AddMultDefense(float val)
    {
        this.defenseBuff += val;
        this.ui.SetDefense(GetDefense());
    }

    public Card_Base GetBase()
    {
        return this.cardStats;
    }

    public int GetPower()
    {
        float power = (this.cardStats.power + this.powerBuff) * this.powerMult;
        return Mathf.Max(0, Mathf.CeilToInt(power));
    }

    public int GetDefense()
    {
        float defense = (this.cardStats.defense + this.defenseBuff) * this.defenseMult;
        return Mathf.Max(0, Mathf.CeilToInt(defense));
    }

    public CardElement GetElement()
    {
        return this.cardStats.element;
    }

    public CardEffects GetEffect()
    {
        return this.cardStats.effect;
    }

    public void SetStats(Card_Base stats)
    {
        this.cardStats = stats;
    }
}
