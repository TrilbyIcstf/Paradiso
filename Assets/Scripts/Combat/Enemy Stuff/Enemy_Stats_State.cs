using UnityEngine;

public class Enemy_Stats_State
{
    private int minPower = 0;
    private int maxPower = 0;

    private int minDefense = 0;
    private int maxDefense = 0;

    private int powerBuff = 0;
    private int defenseBuff = 0;

    private float effectRate = 0;

    public int GetMinPower() => this.minPower;
    public void SetMinPower(int value) => this.minPower = value;

    public int GetMaxPower() => this.maxPower;
    public void SetMaxPower(int value) => this.maxPower = value;

    public int GetMinDefense() => this.minDefense;
    public void SetMinDefense(int value) => this.minDefense = value;

    public int GetMaxDefense() => this.maxDefense;
    public void SetMaxDefense(int value) => this.maxDefense = value;

    public int GetPowerBuff() => this.powerBuff;
    public void SetPowerBuff(int value) => this.powerBuff = value;

    public int GetDefenseBuff() => this.defenseBuff;
    public void SetDefenseBuff(int value) => this.defenseBuff = value;

    public float GetEffectRate() => this.effectRate;
    public void SetEffectRate(float value) => this.effectRate = value;
}
