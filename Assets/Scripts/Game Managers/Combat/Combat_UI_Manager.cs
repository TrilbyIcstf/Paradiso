using UnityEngine;

public class Combat_UI_Manager : MonoBehaviour
{
    /// <summary>
    /// The card slots the player can place cards into.
    /// </summary>
    private GameObject[] cardSlots = new GameObject[4];

    public bool isHoldingCard = false;

    public UI_Coordinator uiCoordinator;

    /// <summary>
    /// Called when player releases hold of a card with the mouse. Sets card to new position and calls associated logic
    /// </summary>
    /// <param name="card">The card that was held</param>
    public void PlayerReleasesCard(GameObject card)
    {
        int slotPosition = CheckSlotSnap(card);

        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        Card_Base cardScript = card.GetComponent<Card_Base>();

        if (slotPosition >= 0)
        {
            // Set card's resting position to field slot
            gravityScript.SetMovementType(CardMovementType.SNAP);
            gravityScript.SetGravityPoint(GetCardHolder(slotPosition).transform.position);
            gravityScript.SetPosition(GetCardHolder(slotPosition).transform.position);
            gravityScript.SetLocked(true);

            // Assign card stats to field slot
            GameManager.instance.CF.SetPlayerSpace(slotPosition, cardScript);
            GameManager.instance.CPH.RemoveCard(card);
        }
        else
        {
            gravityScript.SetMovementType(CardMovementType.FLOAT);
        }
    }

    public void ReturnToHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        gravityScript.SetGravityPoint(this.uiCoordinator.PlayerHandArea().ClosestPoint(card.transform.position));
    }

    public void DrawToHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        Vector2 closestPoint = this.uiCoordinator.PlayerHandArea().ClosestPoint(card.transform.position);
        closestPoint.y += Random.Range(-1.0f,2.5f);
        closestPoint.x += Random.Range(-1.5f, 0.0f);
        gravityScript.SetGravityPoint(closestPoint);
    }

    public void DrawToEnemyHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        Vector2 closestPoint = this.uiCoordinator.EnemyHandArea().ClosestPoint(card.transform.position);
        closestPoint.y += Random.Range(-1.0f, 2.5f);
        closestPoint.x += Random.Range(6.5f, 0.0f);
        gravityScript.SetGravityPoint(closestPoint);
    }

    /// <summary>
    /// Checks if a card should snap to a field slot
    /// </summary>
    /// <param name="card">The card to check</param>
    /// <returns>The position of the field slot to snap to, or -1 if none should be snapped to</returns>
    public int CheckSlotSnap(GameObject card)
    {
        int slotPosition = -1;
        for (int i = 0; i < this.cardSlots.Length; i++)
        {
            if (this.cardSlots[i] == null) { break; }
            Vector3 mousePosition = Input.mousePosition;
            if (GetCardHolderScript(i).shouldSnap(card.transform.position) && GameManager.instance.CF.GetPlayerSpace(i).GetCard() == null)
            {
                slotPosition = i;
            }
        }
        return slotPosition;
    }

    public GameObject GetCardHolder(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.cardSlots[pos];
    }

    public Card_Holder_Interaction GetCardHolderScript(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.cardSlots[pos].GetComponent<Card_Holder_Interaction>();
    }

    public void SetCardHolder(GameObject cardHolder, int pos)
    {
        this.cardSlots[pos] = cardHolder;
    }

    public void NotifyEnergyUpdated(float fraction)
    {
        this.uiCoordinator.energyBar.SetEnergyFill(fraction);
    }

    public void NotifyEnemyEnergyUpdated(float fraction)
    {
        this.uiCoordinator.enemyEnergyBar.SetEnergyFill(fraction);
    }

    public void InvalidEnergyCost()
    {
        this.uiCoordinator.energyBar.FlashRed();
    }
}
