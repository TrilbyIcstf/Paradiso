using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Enemy_Stats_Manager : MonoBehaviour
{
    [SerializeField]
    private Enemy_Stats enemy;

    private float maxHealth;
    private float currentHealth;

    private float currentEnergy = 0;
    private float maxEnergy = 2.5f;

    private bool regenOn = true;
    private float regenMultiplier = 1;

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
            if (!GameManager.instance.CEH.AtHandLimit())
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
        this.maxHealth = val.maxHealth;
        this.currentHealth = val.maxHealth;
    }

    public bool DealDamage(float amount)
    {
        this.currentHealth -= amount;
        GameManager.instance.CUI.NotifyEnemyHealthUpdate(this.currentHealth, this.maxHealth);

        return this.currentHealth <= 0;
    }

    public void DrawCard()
    {
        SetEnergy(0, true);
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
            GameManager.instance.CUI.DrawToEnemyHand(newCard);
            GameManager.instance.CEH.AddCard(newCard);
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
        GameManager.instance.CUI.NotifyEnemyEnergyUpdated(EnergyFraction());
    }

    public bool ToggleRegen(bool regen)
    {
        bool oldRegen = this.regenOn;
        this.regenOn = regen;
        return oldRegen == this.regenOn;
    }
}
