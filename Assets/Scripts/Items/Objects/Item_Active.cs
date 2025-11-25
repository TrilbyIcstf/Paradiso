using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that can be activated during combat
/// </summary>
[CreateAssetMenu(fileName = "Active Item", menuName = "ScriptableObjects/New Active Item", order = 1)]
[System.Serializable]
public class Item_Active : Item_Base
{
    [SerializeField]
    private float energyCost = 1;

    [SerializeField]
    private float cooldown = 0;

    [SerializeField]
    private bool startsOnCooldown = false;

    public void Activate()
    {
        GameManager.instance.IBM.TriggerActiveEffect(this.item);
    }

    public bool CanActivate()
    {
        return GameManager.instance.IBM.CanTriggerActive(this.item);
    }

    public float GetCooldown()
    {
        return this.cooldown;
    }

    public float GetCost()
    {
        return this.energyCost;
    }

    public bool GetStartsOnCooldown()
    {
        return this.startsOnCooldown;
    }
}
