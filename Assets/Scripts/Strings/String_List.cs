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
    public class EffectDictionary : SerializableDictionary<CardEffect, Card_Effect_Description> { }
    [SerializeField]
    private EffectDictionary cardEffects;
    [Serializable]
    public class ItemDictionary : SerializableDictionary<Item, Item_Description> { }
    [SerializeField]
    private ItemDictionary items;


    public Card_Effect_Description GetCardEffectDescription(CardEffect eff)
    {
        return this.cardEffects[eff];
    }

    public Item_Description GetItemDescription(Item item)
    {
        return this.items[item];
    }
}
