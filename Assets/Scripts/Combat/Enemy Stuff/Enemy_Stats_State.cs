using UnityEngine;

[System.Serializable]
public class Enemy_Stats_State
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private float regenRate;

    [SerializeField]
    private int minPower = 0;
    [SerializeField]
    private int maxPower = 0;

    [SerializeField]
    private int minDefense = 0;
    [SerializeField]
    private int maxDefense = 0;

    [SerializeField]
    private int powerQuality = 0;
    [SerializeField]
    private int defenseQuality = 0;

    [SerializeField]
    private float effectRate = 0;

    public int GetMaxHealth() => this.maxHealth;
    public void SetMaxHealth(int value) => this.maxHealth = value;

    public float GetRegenRate() => this.regenRate;
    public void SetRegenRate(float value) => this.regenRate = value;

    public int GetMinPower() => this.minPower;
    public void SetMinPower(int value) => this.minPower = value;

    public int GetMaxPower() => this.maxPower;
    public void SetMaxPower(int value) => this.maxPower = value;

    public int GetMinDefense() => this.minDefense;
    public void SetMinDefense(int value) => this.minDefense = value;

    public int GetMaxDefense() => this.maxDefense;
    public void SetMaxDefense(int value) => this.maxDefense = value;

    public int GetPowerQuality() => this.powerQuality;
    public void SetPowerQuality(int value) => this.powerQuality = value;

    public int GetDefenseQuality() => this.defenseQuality;
    public void SetDefenseQuality(int value) => this.defenseQuality = value;

    public float GetEffectRate() => this.effectRate;
    public void SetEffectRate(float value) => this.effectRate = value;
}
