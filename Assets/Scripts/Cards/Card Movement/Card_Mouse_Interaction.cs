using UnityEngine;

public class Card_Mouse_Interaction : ManagerBehavior
{
    /// <summary>
    /// Flag marking when this card is currently being held by the mouse
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
                GM.CUI.isHoldingCard = false;
                GM.CUI.PlayerReleasesCard(this.cardGravity.gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GM.CUI.isHoldingCard && !this.cardGravity.GetLocked())
        {
            this.isHoldingCard = true;
            GM.CPH.SetSortingOrder(this.cardGravity.gameObject);
            GM.CUI.isHoldingCard = true;
            this.cardGravity.SetMovementType(CardMovementType.LOOSESNAP);
        }
    }
}
