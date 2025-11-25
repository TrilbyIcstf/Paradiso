using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the player's deck object in combat
/// </summary>
public class Player_Deck : ManagerBehavior
{
    public float drawCost = 10;

    private void Awake()
    {
        GM.CUI.SetPlayerDeck(gameObject);
    }

    private void OnMouseEnter()
    {
        CostPreviewOn();
    }

    private void OnMouseExit()
    {
        CostPreviewOff();
    }

    private void OnMouseDown()
    {
        if (GM.CPD.DeckIsEmpty()) { return; }

        if (GM.CPS.CanAffordEnergy(this.drawCost))
        {
            if (!GM.CPH.AtHandLimit())
            {
                GM.CPS.SubtractEnergy(this.drawCost, true);

                GM.CPS.DrawCard();
            }
        }
        else
        {
            GM.CUI.InvalidManaCost();
        }
    }

    /// <summary>
    /// Turns on the preview for the energy cost of drawing a card
    /// </summary>
    private void CostPreviewOn()
    {
        GM.CUI.SetManaPreview(this.drawCost);
    }

    /// <summary>
    /// Turns off the preview for the energy cost of drawing a card
    /// </summary>
    private void CostPreviewOff()
    {
        GM.CUI.RemoveManaPreview();
    }
}
