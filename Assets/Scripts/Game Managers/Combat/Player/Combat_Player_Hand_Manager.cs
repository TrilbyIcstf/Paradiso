using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the player's hand during combat
/// </summary>
public class Combat_Player_Hand_Manager : Hand_Manager
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
}
