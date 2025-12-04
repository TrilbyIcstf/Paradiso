using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Static misc string list
/// </summary>
public class String_List : MonoBehaviour
{
    private Static_Strings statics = new English_Static();

    [Serializable]
    public class EffectDictionary : SerializableDictionary<CardEffect, Card_Effect_Description> { }
    [SerializeField]
    private EffectDictionary cardEffects;
    [Serializable]
    public class ItemDictionary : SerializableDictionary<Item, Item_Description> { }
    [SerializeField]
    private ItemDictionary items;
    [Serializable]
    public class ElementDictionary : SerializableDictionary<CardElement, Element_Description> { }
    [SerializeField]
    private ElementDictionary elements;
    [Serializable]
    public class AffinityDictionary : SerializableDictionary<CardAffinity, Affinity_Description> { }
    [SerializeField]
    private AffinityDictionary affinities;


    public Card_Effect_Description GetCardEffectDescription(CardEffect eff)
    {
        return this.cardEffects[eff];
    }

    public Item_Description GetItemDescription(Item item)
    {
        return this.items[item];
    }

    public Element_Description GetElement(CardElement element)
    {
        return this.elements[element];
    }

    public Affinity_Description GetAffinity(CardAffinity affinity)
    {
        return this.affinities[affinity];
    }

    public Static_Strings Statics()
    {
        return this.statics;
    }
}
