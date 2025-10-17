using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Description", menuName = "ScriptObjects/New Item", order = 2)]
[Serializable]
public class Item_Description : ScriptableObject
{
    [SerializeField]
    public Items item;

    [SerializeField]
    public string title;

    [SerializeField]
    [TextArea]
    public string description;

    [SerializeField]
    public Sprite itemSprite;
}
