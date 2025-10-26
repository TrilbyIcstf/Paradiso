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
    private int maxHealth;

    [SerializeField]
    private float regenRate;

    [SerializeField]
    private EnemyDrawTypes drawType;

    public string GetName()
    {
        return this.enemyName;
    }

    public int GetHealth()
    {
        return this.maxHealth;
    }

    public float GetEnergyRegen()
    {
        return this.regenRate;
    }

    public EnemyDrawTypes GetDrawType()
    {
        return this.drawType;
    }
}
