using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_UI_Manager : MonoBehaviour
{
    /// <summary>
    /// The card slots the player can place cards into.
    /// </summary>
    private GameObject[] cardSlots = new GameObject[4];

    public bool isHoldingCard = false;

    public UI_Coordinator uiCoordinator;

    public void PlayerReleasesCard(GameObject card)
    {
        int slotPosition = -1;
        for (int i = 0; i < this.cardSlots.Length; i++)
        {
            if (this.cardSlots[i] == null) { break; }
            Vector3 mousePosition = Input.mousePosition;
            if (this.GetCardHolderScript(i).shouldSnap(Camera.main.ScreenToWorldPoint(mousePosition)))
            {
                slotPosition = i;
            }
        }

        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();

        if (slotPosition >= 0)
        {
            gravityScript.SetMovementType(CardMovementType.SNAP);
            gravityScript.SetGravityPoint(this.GetCardHolder(slotPosition).transform.position);
        } else
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
}
