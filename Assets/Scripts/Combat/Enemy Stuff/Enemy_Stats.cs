using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object to hold the stats for an enemy encounter
/// </summary>
[CreateAssetMenu(fileName = "Enemy Stats", menuName = "ScriptableObjects/New Enemy", order = 2)]
[System.Serializable]
public class Enemy_Stats : ScriptableObject
{
    [SerializeField]
    private string enemyName;

    [SerializeField]
    private EnemyDrawType drawType;

    [SerializeField]
    private Enemy_Stats_State baseStats;

    public string GetName()
    {
        return this.enemyName;
    }

    public Enemy_Stats_State GetBaseStats()
    {
        return this.baseStats;
    }

    public EnemyDrawType GetDrawType()
    {
        return this.drawType;
    }
}
