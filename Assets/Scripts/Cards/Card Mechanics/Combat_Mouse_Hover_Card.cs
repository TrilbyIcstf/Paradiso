using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles displaying additional information when the player hovers over a card with the mouse
/// </summary>
public class Combat_Mouse_Hover_Card : Mouse_Hover_Card
{
    [SerializeField]
    private Active_Card cardStats;

    private void OnMouseEnter()
    {
        MouseEnter();
    }

    private void OnMouseExit()
    {
        MouseExit();
    }

    protected override List<CardEffects> GetEffects()
    {
        return this.cardStats.GetEffects(); ;
    }
}
