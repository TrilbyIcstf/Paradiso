using UnityEngine;

public class UI_Coordinator : MonoBehaviour
{
    public Energy_Bar_UI energyBar;
    public Energy_Bar_UI enemyEnergyBar;
    private GameObject playerHandArea;
    private GameObject enemyHandArea;

    private void Awake()
    {
        GameManager.instance.CUI.uiCoordinator = this;
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
}
