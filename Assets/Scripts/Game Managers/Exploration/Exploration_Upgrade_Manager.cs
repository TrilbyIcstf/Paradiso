using UnityEngine;

public class Exploration_Upgrade_Manager : ManagerBehavior
{
    [SerializeField]
    private GameObject upgradeScreen;

    private GameObject tempScreen;

    public void TriggerUpgrade()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("RoomCanvas");
        if (canvas == null)
        {
            return;
        }

        this.tempScreen = Instantiate(this.upgradeScreen, canvas.transform);
        Upgrade_Screen_Controller controllerScript = GetController();

        controllerScript.SetupUpgrade(GM.PL.GetRandomCards(3));
    }

    private Upgrade_Screen_Controller GetController()
    {
        return this.tempScreen.GetComponent<Upgrade_Screen_Controller>();
    }
}
