using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minor class to set the enemy's deck in the game manager
/// </summary>
public class Enemy_Deck : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.CES.SetEnemyDeck(gameObject);
    }
}
