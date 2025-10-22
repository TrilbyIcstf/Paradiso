using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive Item", menuName = "ScriptableObjects/New Passive Item", order = 1)]
[System.Serializable]
public class Item_Passive : Item_Base
{
    [SerializeField]
    private EffectTiming timing;

    public bool IsCorrectTiming(EffectTiming currTiming)
    {
        return currTiming == this.timing;
    }
}
