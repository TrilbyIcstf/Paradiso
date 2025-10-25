using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks information about a card actively on the field. Tracks any additional changes made to the card's base stats.
/// </summary>
public class Active_Card : MonoBehaviour
{
    [SerializeField]
    private Card_Base cardStats;

    private Card_UI ui;

    // Card's stats that can be overriden from the base
    private float activePower = 0;
    private float activeDefense = 0;

    // Card's element that can be overriden from the base
    private CardElement activeElement;

    // Card's effects that can be overriden from the base
    private List<CardEffects> activeEffects = new List<CardEffects>();

    private void Awake()
    {
        this.activePower = this.cardStats.GetPower();
        this.activeDefense = this.cardStats.GetDefense();
        this.activeElement = this.cardStats.GetElement();
        this.activeEffects = this.cardStats.GetEffects();

        this.ui = GetComponent<Card_UI>();

        this.ui.SetPower(GetPower());
        this.ui.SetDefense(GetDefense());
        this.ui.SetElement(GetElement());
        this.ui.SetEffects(GetEffects());
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

    public void SetElement(CardElement val)
    {
        this.activeElement = val;
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
        return this.activeElement;
    }

    public List<CardEffects> GetEffects()
    {
        return this.activeEffects;
    }

    public void SetStats(Card_Base stats)
    {
        this.cardStats = stats;
    }
}
