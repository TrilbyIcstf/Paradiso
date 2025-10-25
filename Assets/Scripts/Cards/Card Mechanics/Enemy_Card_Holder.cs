using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class for enemy field positions
/// </summary>
public class Enemy_Card_Holder : Card_Holder
{
    private void Awake()
    {
        GameManager.instance.CUI.SetEnemyHolder(this.gameObject, this.position);
    }
}
