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
        int slotPosition = -1;
        for (int i = 0; i < this.cardSlots.Length; i++)
        {
            if (this.cardSlots[i] == null) { break; }
            Vector3 mousePosition = Input.mousePosition;
            if (GetCardHolderScript(i).shouldSnap(Camera.main.ScreenToWorldPoint(mousePosition)) && GameManager.instance.CF.GetPlayerSpace(i).GetCard() == null)
            {
                slotPosition = i;
            }
        }

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
        }
        else
        {
            gravityScript.ResetPosition();
        }
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

    public void NotifyEnergyUpdated()
    {
        this.uiCoordinator.energyBar.SetEnergyFill();
    }

    public void InvalidEnergyCost()
    {
        this.uiCoordinator.energyBar.FlashRed();
    }
}
