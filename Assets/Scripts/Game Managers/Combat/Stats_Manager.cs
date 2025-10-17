using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats_Manager : ManagerBehavior
{
    protected float maxHealth;
    protected float currentHealth;

    protected float currentEnergy;
    protected float maxEnergy;

    protected bool regenOn = true;

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
    public abstract void CreateNewCard(Card_Base cardStats, Transform pos);

    public bool CanAffordEnergy(float cost)
    {
        return cost <= this.currentEnergy;
    }

    public bool ToggleRegen(bool regen)
    {
        bool oldRegen = this.regenOn;
        this.regenOn = regen;
        return oldRegen == this.regenOn;
    }

    public IEnumerator RegenDelay(float delay = 0.25f)
    {
        ToggleRegen(false);
        yield return new WaitForSeconds(delay);
        ToggleRegen(true);
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
}
