using UnityEngine;

public class UI_Coordinator : MonoBehaviour
{
    public Energy_Bar_UI energyBar;
    private GameObject playerHandArea;

    private void Awake()
    {
        GameManager.instance.CUI.uiCoordinator = this;
    }

    public Combat_Area_Marker PlayerHandArea()
    {
        return this.playerHandArea.GetComponent<Combat_Area_Marker>();
    }

    public void SetPlayerHandArea(GameObject val)
    {
        this.playerHandArea = val;
    }
}
