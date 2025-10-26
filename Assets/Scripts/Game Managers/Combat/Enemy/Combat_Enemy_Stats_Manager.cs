using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the enemy's stats during combat
/// </summary>
public class Combat_Enemy_Stats_Manager : Stats_Manager
{
    [SerializeField]
    private Enemy_Stats enemy;

    private float energyDelay = 0.0f;

    private float regenMultiplier = 1.0f;

    private GameObject enemyDeck;

    public Combat_Enemy_Stats_Manager()
    {
        this.currentEnergy = 0.0f;
        this.maxEnergy = 3.0f;
    }

    private void FixedUpdate()
    {
        if (this.regenOn && !this.energyLock && EnergyFraction() < 1)
        {
            AddEnergy(Time.deltaTime * this.regenMultiplier);
        }

        if (EnergyFraction() >= 1)
        {
            if (!GM.CEH.AtHandLimit())
            {
                DrawCard();
            }
        }

        if (this.bonusCardDraw > 0 && this.bonusCardDrawer == null)
        {
            this.bonusCardDrawer = StartCoroutine(DealFreeCard());
        }
    }

    public void SetEnemy(Enemy_Stats val)
    {
        this.enemy = val;
        this.maxHealth = this.enemy.GetHealth();
        this.currentHealth = this.enemy.GetHealth();
    }

    public override bool DealDamage(float amount)
    {
        this.currentHealth -= amount;
        GM.CUI.NotifyEnemyHealthUpdate(this.currentHealth, this.maxHealth);

        if (this.currentHealth <= 0)
        {
            Enemy_Mouse_Controller.testPause = true;
            GM.EndCombat();
        }

        return this.currentHealth <= 0;
    }

    public override void HealDamage(float amount)
    {
        this.currentHealth += amount;
        this.currentHealth = Mathf.Min(this.currentHealth, this.maxHealth);
        HealthUpdate();
    }

    public override void HealthUpdate()
    {
        GM.CUI.NotifyEnemyHealthUpdate(this.currentHealth, this.maxHealth);
    }

    public override void DrawCard()
    {
        SetEnergy(0, true);
        ResetDelay();
        if (this.bonusCardDrawer == null)
        {
            FreeDrawCard();
        } else
        {
            AddFreeCards(1);
        }
    }

    public override void FreeDrawCard()
    {
        if (this.enemyDeck != null)
        {
            this.card.GetComponent<Active_Card>().SetStats(Card_Base.RandomizeStats());
            GameObject newCard = Instantiate(this.card, enemyDeck.transform.position, enemyDeck.transform.rotation);
            GM.CEH.DrawToEnemyHand(newCard);
        }
    }

    protected override IEnumerator DealFreeCard()
    {
        yield return new WaitForSeconds(0.25f);

        this.bonusCardDraw -= 1;
        FreeDrawCard();

        if (this.bonusCardDraw > 0)
        {
            this.bonusCardDrawer = StartCoroutine(DealFreeCard());
        } else
        {
            this.bonusCardDrawer = null;
        }
    }

    public void SetEnemyDeck(GameObject deck)
    {
        this.enemyDeck = deck;
    }

    public override Active_Card CreateNewCard(Card_Base cardStats, Transform pos)
    {
        this.card.GetComponent<Active_Card>().SetStats(cardStats);
        GameObject newCard = Instantiate(this.card, pos.position, pos.rotation);
        GM.CEH.DrawToHand(newCard);
        return newCard.GetComponent<Active_Card>();
    }

    public override bool SetEnergy(float val, bool delay)
    {
        this.currentEnergy = Mathf.Min(val, RequiredEnergy());
        NotifyEnergyUpdated();
        if (delay)
        {
            StartCoroutine(RegenDelay());
        }

        return this.currentEnergy >= RequiredEnergy();
    }

    public override bool AddEnergy(float val)
    {
        this.currentEnergy = Mathf.Min(this.currentEnergy + val, RequiredEnergy());
        NotifyEnergyUpdated();
        return this.currentEnergy >= RequiredEnergy();
    }

    public void AddEnergyDelay(float val)
    {
        this.energyDelay += val;
    }

    public void ResetDelay()
    {
        this.energyDelay = 0.0f;
    }

    public override bool SubtractEnergy(float val, bool delay)
    {
        this.currentEnergy = Mathf.Max(this.currentEnergy - val, 0);
        NotifyEnergyUpdated();
        if (delay)
        {
            StartCoroutine(RegenDelay());
        }

        return this.currentEnergy <= 0;
    }

    public float RequiredEnergy()
    {
        return this.maxEnergy + this.energyDelay;
    }

    public float EnergyFraction()
    {
        return this.currentEnergy / RequiredEnergy();
    }

    protected override void NotifyEnergyUpdated()
    {
        GM.CUI.NotifyEnemyEnergyUpdated(EnergyFraction());
    }
}
