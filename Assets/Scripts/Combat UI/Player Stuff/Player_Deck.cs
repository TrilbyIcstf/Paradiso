using System.Collections;
using UnityEngine;

public class Player_Deck : MonoBehaviour
{
    private float drawCost = 12;
    private void OnMouseDown()
    {
        if (GameManager.instance.CS.CanAffordEnergy(this.drawCost))
        {
            GameManager.instance.CS.SubtractEnergy(this.drawCost);
            StartCoroutine(RegenDelay());
        }
        else
        {
            GameManager.instance.CUI.InvalidEnergyCost();
        }
    }

    private IEnumerator RegenDelay()
    {
        GameManager.instance.CS.ToggleRegen(false);
        yield return new WaitForSeconds(0.25f);
        GameManager.instance.CS.ToggleRegen(true);
    }
}
