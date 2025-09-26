using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Card_Holder : MonoBehaviour
{
    [SerializeField]
    private int position;

    private void Awake()
    {
        GameManager.instance.CUI.SetEnemyHolder(this.gameObject, position);
    }
}
