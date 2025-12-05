using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Screen_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] upgradeCards = new GameObject[3];
    private Upgrade_Slot[] upgradeSlots = new Upgrade_Slot[3];

    [SerializeField]
    private Upgrade_Description_Controller[] upgradeDescriptions = new Upgrade_Description_Controller[3];

    public void SetupUpgrade(List<Card_Base> cards)
    {
        Upgrade_Stats[] upgrades = DecideUpgrades(cards);

        for (int i = 0; i < this.upgradeCards.Length; i++)
        {
            Upgrade_Stats upgrade = upgrades[i];
            Card_Base upgradedCard = cards[i].ApplyUpgrade(upgrade);
            this.upgradeSlots[i] = new Upgrade_Slot(cards[i], upgradedCard, upgrade);

            Upgrade_Card cardScript = this.upgradeCards[i].GetComponent<Upgrade_Card>();
            cardScript.SetCards(cards[i], upgradedCard);
            this.upgradeCards[i].GetComponent<Upgrade_Card_UI>().BaseCard();

            SetUpgradeDescription(upgrade, i);
        }
    }

    private Upgrade_Stats[] DecideUpgrades(List<Card_Base> cards)
    {
        Upgrade_Stats[] upgrades = new Upgrade_Stats[cards.Count];

        for (int i = 0; i < cards.Count; i++)
        {
            upgrades[i] = DecideUpgrade(cards[i], 1);
        }

        return upgrades;
    }

    private Upgrade_Stats DecideUpgrade(Card_Base card, int amount)
    {
        List<UpgradeType> upgradeTypes = AvailableUpgradeTypes(card);

        Upgrade_Stats upgrade = new Upgrade_Stats();

        for (int i = 0; i < amount; i++)
        {
            int randNum = Random.Range(0, upgradeTypes.Count);

            UpgradeType type = upgradeTypes[randNum];
            upgradeTypes.Remove(type);

            upgrade = ApplyUpgradeType(upgrade, card, type);
        }

        return upgrade;
    }

    private List<UpgradeType> AvailableUpgradeTypes(Card_Base card)
    {
        // TODO: MAKE THIS BETTER IN THE FUTURE
        List<UpgradeType> upgradeTypes = UpgradeTypeMethods.TestUpgradeList();
        if (card.GetEffects().Count >= 3 || card.GetElement() == CardElement.Nil)
        {
            upgradeTypes.Remove(UpgradeType.WeakEffect);
        }
        return upgradeTypes;
    }

    private Upgrade_Stats ApplyUpgradeType(Upgrade_Stats stats, Card_Base card, UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.PowerSmall:
                {
                    int buff = Mathf.CeilToInt(GameManager.instance.PL.GetAverageDeckPower() / 5);
                    stats.SetPowerBuff(buff);
                    break;
                }
            case UpgradeType.PowerBig:
                {
                    int buff = Mathf.CeilToInt(GameManager.instance.PL.GetAverageDeckPower() / 3);
                    stats.SetPowerBuff(buff);
                    break;
                }
            case UpgradeType.DefenseSmall:
                {
                    int buff = Mathf.CeilToInt(GameManager.instance.PL.GetAverageDeckDefense() / 5);
                    stats.SetDefenseBuff(buff);
                    break;
                }
            case UpgradeType.DefenseBig:
                {
                    int buff = Mathf.CeilToInt(GameManager.instance.PL.GetAverageDeckDefense() / 3);
                    stats.SetDefenseBuff(buff);
                    break;
                }
            case UpgradeType.NewElement:
                {
                    CardElement newElement;
                    do
                    {
                        int randNum = Random.Range(0, 4);
                        newElement = (CardElement)randNum;
                    } while (newElement == card.GetElement());
                    stats.SetNewElement(newElement);
                    break;
                }
            case UpgradeType.NewAffinity:
                {
                    CardAffinity newAffinity;
                    do
                    {
                        int randNum = Random.Range(0, 3);
                        newAffinity = (CardAffinity)randNum;
                    } while (newAffinity == card.GetAffinity());
                    stats.SetNewAffinity(newAffinity);
                    break;
                }
            case UpgradeType.WeakEffect:
                {
                    stats.SetNewEffect(CardEffect.Spread);
                    break;
                }
        }

        return stats;
    }

    private void SetUpgradeDescription(Upgrade_Stats upgrade, int pos)
    {
        int upgradeNum = 0;
        if (upgrade.GetNewEffect() != CardEffect.None)
        {
            Card_Effect_Description effect = GameManager.instance.STR.GetCardEffectDescription(upgrade.GetNewEffect());
            this.upgradeDescriptions[pos].SetUpgrade(effect.title, effect.effectSprite, upgradeNum);
            upgradeNum++;
        }
        if (upgrade.GetNewElement() != null)
        {
            CardElement elem = (CardElement)upgrade.GetNewElement();
            Element_Description elemDescription = GameManager.instance.STR.GetElement(elem);
            Sprite elemSprite = Static_Object_Manager.instance.GetElementIcon(elem);
            this.upgradeDescriptions[pos].SetUpgrade(elemDescription.UpgradeDescription, elemSprite, upgradeNum);
            upgradeNum++;
        }
        if (upgrade.GetNewAffinity() != null)
        {
            CardAffinity aff = (CardAffinity)upgrade.GetNewAffinity();
            Affinity_Description affDescription = GameManager.instance.STR.GetAffinity(aff);
            Sprite affSprite = Static_Object_Manager.instance.GetAffinityIcon(aff);
            this.upgradeDescriptions[pos].SetUpgrade(affDescription.UpgradeDescription, affSprite, upgradeNum);
            upgradeNum++;
        }
        if (upgrade.GetPowerBuff() > 0)
        {
            Sprite powSprite = Static_Object_Manager.instance.PowerIcon;
            string upgradeDescription = GameManager.instance.STR.Statics().PowerUpgrade(upgrade.GetPowerBuff());
            this.upgradeDescriptions[pos].SetUpgrade(upgradeDescription, powSprite, upgradeNum);
            upgradeNum++;
        }
        if (upgrade.GetDefenseBuff() > 0)
        {
            Sprite defSprite = Static_Object_Manager.instance.DefenseIcon;
            string upgradeDescription = GameManager.instance.STR.Statics().DefenseUpgrade(upgrade.GetDefenseBuff());
            this.upgradeDescriptions[pos].SetUpgrade(upgradeDescription, defSprite, upgradeNum);
            upgradeNum++;
        }
    }

    public void UpgradeSelected(int pos)
    {
        Destroy(gameObject);
        Card_Base selectedUpgrade = this.upgradeSlots[pos].GetCardUpgrade();
        GameManager.instance.PL.UpdateCard(selectedUpgrade.GetID(), selectedUpgrade);
        GameManager.instance.ER.NextPopup();
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

    private CardElement? newElement = null;

    private CardAffinity? newAffinity = null;

    public Upgrade_Stats(int powerBuff = 0, int defenseBuff = 0, CardEffect newEffect = CardEffect.None, CardElement? newElement = null, CardAffinity? newAffinity = null)
    {
        this.powerBuff = powerBuff;
        this.defenseBuff = defenseBuff;
        this.newEffect = newEffect;
        this.newElement = newElement;
        this.newAffinity = newAffinity;
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

    public CardElement? GetNewElement()
    {
        return this.newElement;
    }

    public CardAffinity? GetNewAffinity()
    {
        return this.newAffinity;
    }

    public void SetPowerBuff(int val)
    {
        this.powerBuff = val;
    }

    public void SetDefenseBuff(int val)
    {
        this.defenseBuff = val;
    }

    public void SetNewEffect(CardEffect val)
    {
        this.newEffect = val;
    }

    public void SetNewElement(CardElement val)
    {
        this.newElement = val;
    }

    public void SetNewAffinity(CardAffinity val)
    {
        this.newAffinity = val;
    } 
}