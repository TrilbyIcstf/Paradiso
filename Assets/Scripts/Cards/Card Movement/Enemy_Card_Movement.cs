using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Card_Movement : MonoBehaviour
{
    private static int baseSortingOrder = 2;

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
            this.sr.sortingOrder = baseSortingOrder + 1;
            this.cardGravity.SetMovementType(CardMovementType.SNAP);
        }
    }

    public void OnRelease(int position)
    {
        this.isHoldingCard = false;
        this.sr.sortingOrder = baseSortingOrder;
        GameManager.instance.CUI.EnemyReleasesCard(this.cardGravity.gameObject, position);
    }
}
