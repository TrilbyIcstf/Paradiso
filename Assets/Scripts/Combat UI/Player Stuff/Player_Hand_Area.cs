using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand_Area : Combat_Area_Marker
{
    private void Start()
    {
        GameManager.instance.CUI.uiCoordinator.SetPlayerHandArea(this.gameObject);
    }
}
