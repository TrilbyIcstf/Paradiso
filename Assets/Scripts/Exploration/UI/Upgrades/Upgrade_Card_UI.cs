using UnityEngine;

public class Upgrade_Card_UI : Card_UI_Controller
{
    public void BaseCard()
    {
        Card_Base cardBase = GetComponent<Upgrade_Card>().GetBase();
        SetCardBase(cardBase);
    }

    public void UpgradeCard()
    {
        Card_Base cardBase = GetComponent<Upgrade_Card>().GetUpgrade();
        SetCardBase(cardBase);
    }
}
