using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Stats_Manager : MonoBehaviour
{
    private float currentEnergy = 36;
    private float maxEnergy = 36;

    public bool SetEnergy(float val)
    {
        this.currentEnergy = Mathf.Min(val, this.maxEnergy);
        NotifyEnergyUpdated();
        return this.currentEnergy >= this.maxEnergy;
    }

    public bool AddEnergy(float val)
    {
        this.currentEnergy = Mathf.Min(this.currentEnergy + val, this.maxEnergy);
        NotifyEnergyUpdated();
        return this.currentEnergy >= this.maxEnergy;
    }

    public bool SubtractEnergy(float val)
    {
        this.currentEnergy = Mathf.Max(this.currentEnergy - val, 0);
        NotifyEnergyUpdated();
        return this.currentEnergy <= 0;
    }

    public float EnergyFraction()
    {
        return this.currentEnergy / this.maxEnergy;
    }

    public bool CanAffordEnergy(float cost)
    {
        return cost > this.currentEnergy;
    }

    private void NotifyEnergyUpdated()
    {
        GameManager.instance.CUI.NotifyEnergyUpdated();
    }
}
