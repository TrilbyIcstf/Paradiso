using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hand_Area : Combat_Area_Marker
{
    private void Start()
    {
        GameManager.instance.CUI.uiCoordinator.SetEnemyHandArea(this.gameObject);
        GameManager.instance.CEH.SetHandArea(this);
    }
}
