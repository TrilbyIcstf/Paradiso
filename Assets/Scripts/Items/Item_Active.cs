using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Active Item", menuName = "ScriptableObjects/New Active Item", order = 1)]
[System.Serializable]
public class Item_Active : Item_Base
{
    public float energyCost = 1;

    [SerializeField]
    private float cooldown = 0;

    public void Activate()
    {
        Item_Behavior.TriggerActiveEffect(this.item);
    }

    public bool CanActivate()
    {
        return Item_Behavior.CanTriggerActive(this.item);
    }

    public float GetCooldown()
    {
        return this.cooldown;
    }
}
