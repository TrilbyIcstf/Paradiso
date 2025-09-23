using System.Collections;
using UnityEngine;

public class Player_Deck : MonoBehaviour
{
    public GameObject card;

    public float drawCost = 10;

    private void OnMouseDown()
    {
        if (GameManager.instance.CS.CanAffordEnergy(this.drawCost))
        {
            GameManager.instance.CS.SubtractEnergy(this.drawCost, true);

            GameObject newCard = Instantiate(card, gameObject.transform.position, gameObject.transform.rotation);
            GameManager.instance.CUI.DrawToHand(newCard);
            GameManager.instance.CPH.AddCard(newCard);
        }
        else
        {
            GameManager.instance.CUI.InvalidEnergyCost();
        }
    }
}
