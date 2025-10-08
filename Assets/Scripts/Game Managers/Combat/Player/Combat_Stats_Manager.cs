using System.Collections;
using UnityEngine;

public class Combat_Stats_Manager : ManagerBehavior
{
    private float maxHealth;
    private float currentHealth;

    private float currentEnergy = 30;
    private float maxEnergy = 30;

    private bool regenOn = true;
    private float energyRegen = 0.08f;

    private int bonusCardDraw = 0;
    private Coroutine bonusCardDrawer;

    [SerializeField]
    private GameObject card;

    private void FixedUpdate()
    {
        if (this.regenOn && EnergyFraction() < 1)
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

    public bool DealDamage(float amount)
    {
        this.currentHealth -= amount;
        GM.CUI.NotifyPlayerHealthUpdate(this.currentHealth, this.maxHealth);

        return this.currentHealth <= 0;
    }

    public float GetHealth()
    {
        return this.currentHealth;
    }

    public float GetCurrentHealth()
    {
        return this.maxHealth;
    }

    public void DrawCard()
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

    public void FreeDrawCard()
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

    public void AddFreeCards(int val)
    {
        this.bonusCardDraw += val;
    }

    private IEnumerator DealFreeCard()
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

    public void CreateNewCard(Card_Base cardStats, Transform pos)
    {
        this.card.GetComponent<Active_Card>().SetStats(cardStats);
        GameObject newCard = Instantiate(this.card, pos.position, pos.rotation);
        GM.CPH.DrawToHand(newCard);
    }

    public bool SetEnergy(float val, bool delay)
    {
        this.currentEnergy = Mathf.Min(val, this.maxEnergy);
        NotifyEnergyUpdated();
        if (delay)
        {
            StartCoroutine(RegenDelay());
        }

        return this.currentEnergy >= this.maxEnergy;
    }

    public bool AddEnergy(float val)
    {
        this.currentEnergy = Mathf.Min(this.currentEnergy + val, this.maxEnergy);
        NotifyEnergyUpdated();
        return this.currentEnergy >= this.maxEnergy;
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

    public float EnergyFraction()
    {
        return this.currentEnergy / this.maxEnergy;
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
        GM.CUI.NotifyEnergyUpdated(EnergyFraction());
    }

    public bool ToggleRegen(bool regen)
    {
        bool oldRegen = this.regenOn;
        this.regenOn = regen;
        return oldRegen == this.regenOn;
    }
}
