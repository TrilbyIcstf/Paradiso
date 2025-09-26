using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestInit : MonoBehaviour
{
    public Enemy_Stats enemy;

    private void Awake()
    {
        GameManager.instance.CES.SetEnemy(enemy);
        GameManager.instance.InitCombat();

        Destroy(gameObject);
    }
}
