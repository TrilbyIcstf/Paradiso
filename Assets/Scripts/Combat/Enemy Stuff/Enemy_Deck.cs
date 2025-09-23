using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Deck : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.CES.SetEnemyDeck(gameObject);
    }
}
