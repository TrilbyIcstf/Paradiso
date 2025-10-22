using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Card : MonoBehaviour
{
    [SerializeField]
    private Card_Base cardStats;

    private Card_UI ui;

    private float activePower = 0;
    private float activeDefense = 0;

    // Element override
    private CardElement? elementOverride;

    private void Awake()
    {
        this.activePower = this.cardStats.power;
        this.activeDefense = this.cardStats.defense;

        this.ui = GetComponent<Card_UI>();

        this.ui.SetPower(GetPower());
        this.ui.SetDefense(GetDefense());
        this.ui.SetElement(GetElement());
        this.ui.SetEffect(GetEffect());
    }

    public void SetPower(float val)
    {
        this.activePower = val;
        this.ui.SetPower(GetPower());
    }

    public void SetDefense(float val)
    {
        this.activeDefense = val;
        this.ui.SetDefense(GetDefense());
    }

    public void AddPowerBuff(float val)
    {
        this.activePower += val;
        this.ui.SetPower(GetPower());
    }

    public void AddDefenseBuff(float val)
    {
        this.activeDefense += val;
        this.ui.SetDefense(GetDefense());
    }

    public void AddMultPower(float val)
    {
        this.activePower *= val;
        this.ui.SetPower(GetPower());
    }

    public void AddMultDefense(float val)
    {
        this.activeDefense *= val;
        this.ui.SetDefense(GetDefense());
    }

    public void AddElementOverride(CardElement val)
    {
        this.elementOverride = val;
        this.ui.SetElement(GetElement());
    }

    public Card_Base GetBase()
    {
        return this.cardStats;
    }

    public int GetPower()
    {
        return Mathf.Max(0, Mathf.CeilToInt(this.activePower));
    }

    public int GetDefense()
    {
        return Mathf.Max(0, Mathf.CeilToInt(this.activeDefense));
    }

    public CardElement GetElement()
    {
        return this.elementOverride ?? this.cardStats.element;
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
