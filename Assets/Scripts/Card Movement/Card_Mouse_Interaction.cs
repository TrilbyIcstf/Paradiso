using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Mouse_Interaction : MonoBehaviour
{
    /// <summary>
    /// Flag marking when this card is currently being held by the mouse.
    /// </summary>
    private bool isHoldingCard = false;

    [SerializeField]
    private Card_Gravity cardGravity;

    private void Update()
    {
        if (this.isHoldingCard)
        {
            Vector3 mousePosition = Input.mousePosition;
            this.cardGravity.SetGravityPoint(Camera.main.ScreenToWorldPoint(mousePosition));

            if (!Input.GetMouseButton(0))
            {
                this.isHoldingCard = false;
                GameManager.instance.CUI.isHoldingCard = false;
                GameManager.instance.CUI.PlayerReleasesCard(this.cardGravity.gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.instance.CUI.isHoldingCard)
        {
            this.isHoldingCard = true;
            GameManager.instance.CUI.isHoldingCard = true;
            this.cardGravity.SetMovementType(CardMovementType.LOOSESNAP);
            this.cardGravity.RegisterPosition();
        }   
    }
}
