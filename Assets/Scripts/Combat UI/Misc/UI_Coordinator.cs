using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Coordinator : MonoBehaviour
{
    public Energy_Bar_UI energyBar;

    private void Awake()
    {
        GameManager.instance.CUI.uiCoordinator = this;
    }
}
