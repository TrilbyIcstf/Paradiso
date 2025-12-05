using UnityEngine;

public class Upgrade_Card : MonoBehaviour
{
    private Card_Base cardBase;
    private Card_Base cardUpgrade;

    public void SetCards(Card_Base cardBase, Card_Base cardUpgrade)
    {
        this.cardBase = cardBase;
        this.cardUpgrade = cardUpgrade;
    }

    public Card_Base GetBase()
    {
        return this.cardBase;
    }

    public Card_Base GetUpgrade()
    {
        return this.cardUpgrade;
    }
}
