using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement for the enemy's cards
/// </summary>
public class Enemy_Card_Movement : MonoBehaviour
{
    /// <summary>
    /// Flag marking when this card is currently being held by the enemy
    /// </summary>
    private bool isHoldingCard = false;

    [SerializeField]
    private SpriteRenderer sr;
    private Card_Gravity cardGravity;

    private GameObject enemyMouse;

    private void Awake()
    {
        this.cardGravity = GetComponent<Card_Gravity>();
    }

    private void Update()
    {
        if (this.isHoldingCard)
        {
            Vector2 mousePosition = enemyMouse.transform.position;
            this.cardGravity.SetGravityPoint(mousePosition);
        }
    }

    public void OnHold(GameObject mouse)
    {
        if (!this.cardGravity.GetLocked())
        {
            this.isHoldingCard = true;
            this.enemyMouse = mouse;
            GameManager.instance.CEH.SetSortingOrder(gameObject);
            this.cardGravity.SetMovementType(CardMovementType.SNAP);
        }
    }

    public void OnRelease(int position)
    {
        this.isHoldingCard = false;
        GameManager.instance.CUI.EnemyReleasesCard(this.cardGravity.gameObject, position);
    }
}
