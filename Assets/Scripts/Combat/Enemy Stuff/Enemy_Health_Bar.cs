using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Minor class to set the enemy's health bar in the game manager
/// </summary>
public class Enemy_Health_Bar : Health_Bar
{
    private void Start()
    {
        GameManager.instance.CUI.uiCoordinator.SetEnemyHealth(this);
    }
}
