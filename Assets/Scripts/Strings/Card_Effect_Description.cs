using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Effect Description", menuName = "ScriptObjects/New Card Effect", order = 1)]
[Serializable]
public class Card_Effect_Description: ScriptableObject
{
    [SerializeField]
    public CardEffects effect;

    [SerializeField]
    public string title;

    [SerializeField]
    [TextArea]
    public string playerDescription;

    [SerializeField]
    [TextArea]
    public string EnemyDescription;

    [SerializeField]
    public Sprite effectSprite;
}
