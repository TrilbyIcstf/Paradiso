using System.Collections;
using UnityEngine;

public class Combat_Player_Stats_Manager : Stats_Manager
{
    private float energyRegen = 0.08f;

    public Combat_Player_Stats_Manager()
    {
        this.currentEnergy = 30.0f;
        this.maxEnergy = 30.0f;
    }

    private void FixedUpdate()
    {
        if (this.regenOn && !this.energyLock && EnergyFraction() < 1)
        {
            AddEnergy(this.energyRegen);
        }

        if (this.bonusCardDraw > 0 && this.bonusCardDrawer == null)
        {
            this.bonusCardDrawer = StartCoroutine(DealFreeCard());
        }
    }

    public void InitializeHealth(int max, int current)
    {
        this.maxHealth = max;
        this.currentHealth = current;
    }

    public override bool DealDamage(float amount)
    {
        this.currentHealth -= amount;
        HealthUpdate();   

        if (this.currentHealth <= 0)
        {
            Application.Quit();
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
        GM.CUI.NotifyPlayerHealthUpdate(this.currentHealth, this.maxHealth);
    }

    public override void DrawCard()
    {
        if (GM.CPD.DeckIsEmpty()) { return; }

        if (this.bonusCardDrawer == null)
        {
            FreeDrawCard();
        }
        else
        {
            AddFreeCards(1);
        }
    }

    public override void FreeDrawCard()
    {
        Transform deckPosition = GM.CUI.GetPlayerDeck().transform;
        this.card.GetComponent<Active_Card>().SetStats(GM.CPD.DrawTopCard());
        GameObject newCard = Instantiate(this.card, deckPosition.position, deckPosition.rotation);
        GM.CPH.DrawToHand(newCard);

        if (GM.CPD.DeckIsEmpty())
        {
            GM.CUI.SetDeckEmpty();
        }
    }

    protected override IEnumerator DealFreeCard()
    {
        if (GM.CPD.DeckIsEmpty()) {
            this.bonusCardDraw = 0;
            this.bonusCardDrawer = null;
            yield break; 
        }
        yield return new WaitForSeconds(0.25f);

        this.bonusCardDraw -= 1;
        FreeDrawCard();

        if (this.bonusCardDraw > 0)
        {
            this.bonusCardDrawer = StartCoroutine(DealFreeCard());
        }
        else
        {
            this.bonusCardDrawer = null;
        }
    }

    public override void CreateNewCard(Card_Base cardStats, Transform pos)
    {
        this.card.GetComponent<Active_Card>().SetStats(cardStats);
        GameObject newCard = Instantiate(this.card, pos.position, pos.rotation);
        GM.CPH.DrawToHand(newCard);
    }

    public override bool SetEnergy(float val, bool delay)
    {
        this.currentEnergy = Mathf.Min(val, this.maxEnergy);
        NotifyEnergyUpdated();
        if (delay)
        {
            StartCoroutine(RegenDelay());
        }

        return this.currentEnergy >= this.maxEnergy;
    }

    public override bool AddEnergy(float val)
    {
        this.currentEnergy = Mathf.Min(this.currentEnergy + val, this.maxEnergy);
        NotifyEnergyUpdated();
        return this.currentEnergy >= this.maxEnergy;
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

    public float EnergyFraction()
    {
        return this.currentEnergy / this.maxEnergy;
    }

    protected override void NotifyEnergyUpdated()
    {
        GM.CUI.NotifyEnergyUpdated(EnergyFraction());
    }
}
