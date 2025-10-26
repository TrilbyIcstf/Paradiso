using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class for representing the player's hand area in combat
/// </summary>
public class Player_Hand_Area : Combat_Area_Marker
{
    private void Start()
    {
        GameManager.instance.CUI.uiCoordinator.SetPlayerHandArea(this.gameObject);
        GameManager.instance.CPH.SetHandArea(this);
    }
}
