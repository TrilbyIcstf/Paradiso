using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Static misc string list
/// </summary>
public class String_List : MonoBehaviour
{
    [Serializable]
    public class EffectDictionary : SerializableDictionary<CardEffects, Card_Effect_Description> { }
    [SerializeField]
    private EffectDictionary cardEffects;
    [Serializable]
    public class ItemDictionary : SerializableDictionary<Items, Item_Description> { }
    [SerializeField]
    private ItemDictionary items;


    public Card_Effect_Description GetCardEffectDescription(CardEffects eff)
    {
        return this.cardEffects[eff];
    }

    public Item_Description GetItemDescription(Items item)
    {
        return this.items[item];
    }
}
