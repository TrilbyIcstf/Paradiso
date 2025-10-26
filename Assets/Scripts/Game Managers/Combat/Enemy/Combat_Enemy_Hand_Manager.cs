using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the enemy's hand during combat
/// </summary>
public class Combat_Enemy_Hand_Manager : Hand_Manager
{
    private Combat_Area_Marker handArea;

    public void SetHandArea(Combat_Area_Marker script)
    {
        this.handArea = script;
    }

    internal override Combat_Area_Marker GetHandArea()
    {
        return this.handArea;
    }

    internal GameObject RandomCard()
    {
        if (this.hand.Count == 0) { return null; }
        int randCard = Random.Range(0, this.hand.Count);
        return this.hand[randCard];
    }
}
