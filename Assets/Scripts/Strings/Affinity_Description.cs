using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Affinity Description", menuName = "ScriptObjects/New Affinity", order = 4)]
[Serializable]
public class Affinity_Description : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string UpgradeDescription { get; set; }
}
