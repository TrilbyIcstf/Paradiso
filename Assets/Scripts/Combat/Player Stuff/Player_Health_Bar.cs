using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Child class to set the player's health bar in the game manager
/// </summary>
public class Player_Health_Bar : Health_Bar
{
    private void Start()
    {
        GameManager.instance.CUI.uiCoordinator.SetPlayerHealth(this);
    }
}
