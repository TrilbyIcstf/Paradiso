using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class String_List : MonoBehaviour
{
    [Serializable]
    public class EffectDictionary : SerializableDictionary<CardEffects, Card_Effect_Description> { }
    [SerializeField]
    private EffectDictionary cardEffects;

    public Card_Effect_Description GetCardEffectDescription(CardEffects eff)
    {
        return this.cardEffects[eff];
    }
}
