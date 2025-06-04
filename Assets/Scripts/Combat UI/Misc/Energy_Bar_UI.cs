using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy_Bar_UI : MonoBehaviour
{
    private Image energyBar;

    private void Awake()
    {
        this.energyBar = GetComponent<Image>();
    }

    public void SetEnergyFill()
    {
        this.energyBar.fillAmount = GameManager.instance.CS.EnergyFraction();
    }
}
