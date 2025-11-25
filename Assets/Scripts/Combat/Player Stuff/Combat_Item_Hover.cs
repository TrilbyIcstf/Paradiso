using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interaction of the player hovering over an item in combat
/// </summary>
public class Combat_Item_Hover : Mouse_Hover_Item
{
    [SerializeField]
    protected Combat_Item itemHolder;

    private void OnMouseDown()
    {
        DestroyBox();
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    private void OnMouseEnter()
    {
        CostPreviewOn();
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    private void OnMouseExit()
    {
        CostPreviewOff();
        DestroyBox();
    }

    /// <summary>
    /// Turns on the preview showing the item's energy cost on the player's energy bar
    /// </summary>
    private void CostPreviewOn()
    {
        if (this.itemHolder.GetBase() != null)
        {
            GameManager.instance.CUI.SetManaPreview(this.itemHolder.GetBase().GetCost());
        }
    }

    /// <summary>
    /// Turns off the preview showing the item's energy cost on the player's energy bar
    /// </summary>
    private void CostPreviewOff()
    {
        GameManager.instance.CUI.RemoveManaPreview();
    }

    protected override Item_Base GetBase()
    {
        return this.itemHolder.GetBase();
    }
}
