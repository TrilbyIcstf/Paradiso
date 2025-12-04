using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Element Description", menuName = "ScriptObjects/New Element", order = 3)]
[Serializable]
public class Element_Description : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string UpgradeDescription { get; set; }
}
