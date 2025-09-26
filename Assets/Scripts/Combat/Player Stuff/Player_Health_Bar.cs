using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health_Bar : Health_Bar
{
    private void Start()
    {
        GameManager.instance.CUI.uiCoordinator.SetPlayerHealth(this);
    }
}
