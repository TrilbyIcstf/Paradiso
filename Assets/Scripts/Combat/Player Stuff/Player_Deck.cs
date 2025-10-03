using System.Collections;
using UnityEngine;

public class Player_Deck : MonoBehaviour
{
    public float drawCost = 10;

    private void Awake()
    {
        GameManager.instance.CUI.SetPlayerDeck(gameObject);
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.CPD.DeckIsEmpty()) { return; }

        if (GameManager.instance.CS.CanAffordEnergy(this.drawCost))
        {
            if (!GameManager.instance.CPH.AtHandLimit())
            {
                GameManager.instance.CS.SubtractEnergy(this.drawCost, true);

                GameManager.instance.CS.DrawCard();
            }
        }
        else
        {
            GameManager.instance.CUI.InvalidEnergyCost();
        }
    }
}
