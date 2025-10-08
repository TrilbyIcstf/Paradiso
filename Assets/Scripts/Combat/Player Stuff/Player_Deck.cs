using System.Collections;
using UnityEngine;

public class Player_Deck : ManagerBehavior
{
    public float drawCost = 10;

    private void Awake()
    {
        GM.CUI.SetPlayerDeck(gameObject);
    }

    private void OnMouseDown()
    {
        if (GM.CPD.DeckIsEmpty()) { return; }

        if (GM.CS.CanAffordEnergy(this.drawCost))
        {
            if (!GM.CPH.AtHandLimit())
            {
                GM.CS.SubtractEnergy(this.drawCost, true);

                GM.CS.DrawCard();
            }
        }
        else
        {
            GM.CUI.InvalidEnergyCost();
        }
    }
}
