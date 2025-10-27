using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for managing stats during combat
/// </summary>
public abstract class Stats_Manager : ManagerBehavior
{
    protected float maxHealth;
    protected float currentHealth;

    protected float currentEnergy;
    protected float maxEnergy;

    protected bool regenOn = true;

    protected bool energyLock = false;

    protected int bonusCardDraw = 0;
    protected Coroutine bonusCardDrawer;
    
    [SerializeField]
    protected GameObject card;

    public abstract bool DealDamage(float amount);
    public abstract void HealDamage(float amount);
    public abstract void HealthUpdate();
    public abstract void DrawCard();
    public abstract void FreeDrawCard();
    protected abstract IEnumerator DealFreeCard();
    public abstract bool SetEnergy(float val, bool delay);
    public abstract bool AddEnergy(float val);
    public abstract bool SubtractEnergy(float val, bool delay);
    protected abstract void NotifyEnergyUpdated();
    public abstract Active_Card CreateNewCard(Card_Base cardStats, Transform pos);

    public bool CanAffordEnergy(float cost)
    {
        if (this.energyLock) { return false; }
        return cost <= this.currentEnergy;
    }

    public bool SetRegen(bool regen)
    {
        bool oldRegen = this.regenOn;
        this.regenOn = regen;
        return oldRegen == this.regenOn;
    }

    public void SetEnergyLock(bool val)
    {
        this.energyLock = val;
    }

    public IEnumerator RegenDelay(float delay = 0.25f)
    {
        SetRegen(false);
        yield return new WaitForSeconds(delay);
        SetRegen(true);
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    public float GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public void AddFreeCards(int val)
    {
        this.bonusCardDraw += val;
    }

    public float GetEnergyFraction(float energy)
    {
        return energy / this.maxEnergy;
    }
}
