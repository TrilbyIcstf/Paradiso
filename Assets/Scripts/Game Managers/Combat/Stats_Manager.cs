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

    protected float currentMana;
    protected float maxMana;

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
    public abstract bool SetMana(float val, bool delay);
    public abstract bool AddEnergy(float val);
    public abstract bool SubtractEnergy(float val, bool delay);
    protected abstract void NotifyEnergyUpdated();
    public abstract Active_Card CreateNewCard(Card_Base cardStats, Transform pos);

    public bool CanAffordEnergy(float cost)
    {
        if (this.energyLock) { return false; }
        return cost <= this.currentMana;
    }

    public bool SetRegen(bool regen)
    {
        bool oldRegen = this.regenOn;
        this.regenOn = regen;
        return oldRegen == this.regenOn;
    }

    public void SetManaLock(bool val)
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

    public float GetMaxMana()
    {
        return this.maxMana;
    }

    public float GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public float GetCurrentMana()
    {
        return this.currentMana;
    }

    public void AddFreeCards(int val)
    {
        this.bonusCardDraw += val;
    }

    public float GetManaFraction(float mana)
    {
        return mana / this.maxMana;
    }
}
