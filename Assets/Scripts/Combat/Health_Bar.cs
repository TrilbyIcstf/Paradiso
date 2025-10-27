using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstract class for controlling health bars during combat
/// </summary>
public abstract class Health_Bar : MonoBehaviour
{
    [SerializeField]
    private RectMask2D mask;

    [SerializeField]
    internal Text healthText;
    internal bool showingHealth = false;

    private void OnMouseEnter()
    {
        if (!this.showingHealth)
        {
            healthText.enabled = true;
            this.showingHealth = true;
        }
    }

    private void OnMouseExit()
    {
        if (this.showingHealth)
        {
            healthText.enabled = false;
            this.showingHealth = false;
        }
    }

    public void SetMaskAmount(int val)
    {
        Vector4 tempV = this.mask.padding;
        tempV.w = val;
        this.mask.padding = tempV;
    }

    public void UpdateHealthText(int current, int max)
    {
        string healthString = $"{current} / {max}";
        this.healthText.text = healthString;
    }
}
