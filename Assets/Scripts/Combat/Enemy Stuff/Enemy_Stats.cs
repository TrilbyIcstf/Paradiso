using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "ScriptableObjects/New Enemy", order = 2)]
[System.Serializable]
public class Enemy_Stats : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
}
