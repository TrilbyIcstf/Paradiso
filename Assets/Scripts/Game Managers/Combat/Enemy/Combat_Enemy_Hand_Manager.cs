using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
