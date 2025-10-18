using UnityEngine;
using UnityEngine.UI;

public class UI_Coordinator : ManagerBehavior
{
    public Energy_Bar_UI energyBar;
    public Energy_Bar_UI enemyEnergyBar;

    [SerializeField]
    private Image previewBar;

    private GameObject playerHandArea;
    private GameObject enemyHandArea;

    public Player_Health_Bar playerHealth;
    public Enemy_Health_Bar enemyHealth;

    private void Awake()
    {
        GameManager.instance.CUI.uiCoordinator = this;
    }

    public void SetPreviewFill(float val)
    {
        this.previewBar.fillAmount = val;
    }

    public Combat_Area_Marker PlayerHandArea()
    {
        return this.playerHandArea.GetComponent<Combat_Area_Marker>();
    }

    public Combat_Area_Marker EnemyHandArea()
    {
        return this.enemyHandArea.GetComponent<Combat_Area_Marker>();
    }

    public void SetPlayerHandArea(GameObject val)
    {
        this.playerHandArea = val;
    }

    public void SetEnemyHandArea(GameObject val)
    {
        this.enemyHandArea = val;
    }

    public void SetPlayerHealth(Player_Health_Bar val)
    {
        this.playerHealth = val;
        GM.CPS.HealthUpdate();
    }

    public void SetEnemyHealth(Enemy_Health_Bar val)
    {
        this.enemyHealth = val;
        GM.CES.HealthUpdate();
    }
}
