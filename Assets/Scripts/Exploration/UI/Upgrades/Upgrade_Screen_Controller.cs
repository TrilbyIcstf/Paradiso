using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Screen_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] upgradeCards = new GameObject[3];
    private Upgrade_Slot[] upgradeSlots = new Upgrade_Slot[3];

    public void SetupUpgrade(List<Card_Base> cards)
    {
        Upgrade_Stats[] upgrades = DecideUpgrades(cards);

        for (int i = 0; i < this.upgradeCards.Length; i++)
        {
            Card_Base upgradedCard = cards[i].ApplyUpgrade(upgrades[i]);
            this.upgradeSlots[i] = new Upgrade_Slot(cards[i], upgradedCard, upgrades[i]);

            Upgrade_Card_Hover cardScript = upgradeCards[i].GetComponent<Upgrade_Card_Hover>();
            cardScript.SetCards(cards[i], upgradedCard);
        }
    }

    private Upgrade_Stats[] DecideUpgrades(List<Card_Base> cards)
    {
        Upgrade_Stats[] upgrades = new Upgrade_Stats[cards.Count];

        for (int i = 0; i < cards.Count; i++)
        {
            Upgrade_Stats upgrade = new Upgrade_Stats(1, 1, CardEffect.SynergyLeft);
            upgrades[i] = upgrade;
        }

        return upgrades;
    }
}

public class Upgrade_Slot
{
    private Card_Base cardBase;
    private Card_Base cardUpgrade;
    private Upgrade_Stats upgradeStats;

    public Upgrade_Slot(Card_Base cardBase, Card_Base cardUpgrade, Upgrade_Stats upgradeStats)
    {
        this.cardBase = cardBase;
        this.cardUpgrade = cardUpgrade;
        this.upgradeStats = upgradeStats;
    }

    public Card_Base GetCardBase()
    {
        return this.cardBase;
    }

    public Card_Base GetCardUpgrade()
    {
        return this.cardUpgrade;
    }

    public Upgrade_Stats GetUpgradeStats()
    {
        return this.upgradeStats;
    }

    public void SetCardBase(Card_Base val)
    {
        this.cardBase = val;
    }

    public void SetCardUpgrade(Card_Base val)
    {
        this.cardUpgrade = val;
    }

    public void SetUpgradeStats(Upgrade_Stats val)
    {
        this.upgradeStats = val;
    }
}

public class Upgrade_Stats
{
    private int powerBuff = 0;
    private int defenseBuff = 0;

    private CardEffect newEffect = CardEffect.None;

    public Upgrade_Stats(int powerBuff = 0, int defenseBuff = 0, CardEffect newEffect = CardEffect.None)
    {
        this.powerBuff = powerBuff;
        this.defenseBuff = defenseBuff;
        this.newEffect = newEffect;
    }

    public int GetPowerBuff()
    {
        return this.powerBuff;
    }

    public int GetDefenseBuff()
    {
        return this.defenseBuff;
    }

    public CardEffect GetNewEffect()
    {
        return this.newEffect;
    }
}