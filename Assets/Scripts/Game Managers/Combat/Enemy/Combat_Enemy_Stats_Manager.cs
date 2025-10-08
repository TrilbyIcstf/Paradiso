using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Enemy_Stats_Manager : ManagerBehavior
{
    [SerializeField]
    private Enemy_Stats enemy;

    private float maxHealth;
    private float currentHealth;

    private float currentEnergy = 0.0f;
    private float maxEnergy = 3.0f;

    private float energyDelay = 0.0f;

    private bool regenOn = true;
    private float regenMultiplier = 1.0f;

    private int bonusCardDraw = 0;
    private Coroutine bonusCardDrawer;

    private GameObject enemyDeck;
    public GameObject card;

    private void FixedUpdate()
    {
        if (this.regenOn && EnergyFraction() < 1)
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
        this.maxHealth = this.enemy.maxHealth;
        this.currentHealth = this.enemy.maxHealth;
    }

    public bool DealDamage(float amount)
    {
        this.currentHealth -= amount;
        GM.CUI.NotifyEnemyHealthUpdate(this.currentHealth, this.maxHealth);

        if (this.currentHealth <= 0)
        {
            Enemy_Mouse_Controller.testPause = true;
        }

        return this.currentHealth <= 0;
    }

    public void DrawCard()
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

    public void FreeDrawCard()
    {
        if (this.enemyDeck != null)
        {
            this.card.GetComponent<Active_Card>().SetStats(Card_Base.RandomizeStats());
            GameObject newCard = Instantiate(this.card, enemyDeck.transform.position, enemyDeck.transform.rotation);
            GM.CEH.DrawToEnemyHand(newCard);
        }
    }

    public void AddFreeCards(int val)
    {
        this.bonusCardDraw += val;
    }

    private IEnumerator DealFreeCard()
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

    public bool SetEnergy(float val, bool delay)
    {
        this.currentEnergy = Mathf.Min(val, RequiredEnergy());
        NotifyEnergyUpdated();
        if (delay)
        {
            StartCoroutine(RegenDelay());
        }

        return this.currentEnergy >= RequiredEnergy();
    }

    public bool AddEnergy(float val)
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

    public bool SubtractEnergy(float val, bool delay)
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

    public bool CanAffordEnergy(float cost)
    {
        return cost <= this.currentEnergy;
    }

    private IEnumerator RegenDelay()
    {
        ToggleRegen(false);
        yield return new WaitForSeconds(0.25f);
        ToggleRegen(true);
    }

    private void NotifyEnergyUpdated()
    {
        GM.CUI.NotifyEnemyEnergyUpdated(EnergyFraction());
    }

    public bool ToggleRegen(bool regen)
    {
        bool oldRegen = this.regenOn;
        this.regenOn = regen;
        return oldRegen == this.regenOn;
    }
}
