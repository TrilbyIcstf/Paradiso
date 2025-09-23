using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Enemy_Stats_Manager : MonoBehaviour
{
    private float currentEnergy = 0;
    private float maxEnergy = 2.5f;

    private bool regenOn = true;
    private float regenMultiplier = 1;

    private GameObject enemyDeck;
    public GameObject card;

    private void FixedUpdate()
    {
        if (this.regenOn && EnergyFraction() < 1)
        {
            if (AddEnergy(Time.deltaTime * this.regenMultiplier))
            {
                DrawCard();
            }
        }
    }

    private void DrawCard()
    {
        SetEnergy(0, true);
        if (enemyDeck != null)
        {
            GameObject newCard = Instantiate(card, enemyDeck.transform.position, enemyDeck.transform.rotation);
            GameManager.instance.CUI.DrawToEnemyHand(newCard);
            GameManager.instance.CEH.AddCard(newCard);
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
