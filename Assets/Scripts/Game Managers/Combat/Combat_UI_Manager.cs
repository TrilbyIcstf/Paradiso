using UnityEngine;
using UnityEngine.UI;

public class Combat_UI_Manager : MonoBehaviour
{
    /// <summary>
    /// The card spaces the player can place cards into
    /// </summary>
    private GameObject[] cardSpace = new GameObject[4];

    /// <summary>
    /// The card spaces the enemy uses
    /// </summary>
    private GameObject[] enemySpace = new GameObject[4];

    private static int healthMaxSize = 400;
    private static int healthIncrement = 5;

    public bool isHoldingCard = false;

    public UI_Coordinator uiCoordinator;

    /// <summary>
    /// Called when player releases hold of a card with the mouse. Sets card to new position and calls associated logic
    /// </summary>
    /// <param name="card">The card that was held</param>
    public void PlayerReleasesCard(GameObject card)
    {
        int spacePosition = CheckSpaceSnap(card);

        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();

        if (spacePosition >= 0)
        {
            // Set card's resting position to field space
            gravityScript.SetMovementType(CardMovementType.SNAP);
            gravityScript.SetGravityPoint(GetCardHolder(spacePosition).transform.position);
            gravityScript.SetPosition(GetCardHolder(spacePosition).transform.position);
            gravityScript.SetLocked(true);

            // Assign card stats to field space
            GameManager.instance.CF.SetPlayerSpace(spacePosition, card);
            GameManager.instance.CPH.RemoveCard(card);
        }
        else
        {
            gravityScript.SetMovementType(CardMovementType.FLOAT);
        }
    }

    public void EnemyReleasesCard(GameObject card, int position)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();

        if (position >= 0)
        {
            // Set card's resting position to field space
            gravityScript.SetMovementType(CardMovementType.SNAP);
            gravityScript.SetGravityPoint(GetEnemyHolder(position).transform.position);
            gravityScript.SetPosition(GetEnemyHolder(position).transform.position);
            gravityScript.SetLocked(true);

            // Assign card stats to field space
            GameManager.instance.CF.SetEnemySpace(position, card);
            GameManager.instance.CEH.RemoveCard(card);
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

    public void ReturnToEnemyHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        gravityScript.SetGravityPoint(this.uiCoordinator.EnemyHandArea().ClosestPoint(card.transform.position));
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
        closestPoint.y += Random.Range(-2.5f, 1.0f);
        closestPoint.x += Random.Range(6.5f, 0.0f);
        gravityScript.SetGravityPoint(closestPoint);
    }

    /// <summary>
    /// Checks if a card should snap to a field space
    /// </summary>
    /// <param name="card">The card to check</param>
    /// <returns>The position of the field space to snap to, or -1 if none should be snapped to</returns>
    public int CheckSpaceSnap(GameObject card)
    {
        if (GameManager.instance.CF.FieldLocked()) { return -1; }

        int spacePosition = -1;
        for (int i = 0; i < this.cardSpace.Length; i++)
        {
            if (this.cardSpace[i] == null) { break; }
            Vector3 mousePosition = Input.mousePosition;
            if (GetCardHolderScript(i).shouldSnap(card.transform.position) && GameManager.instance.CF.GetPlayerSpace(i).GetCard() == null)
            {
                spacePosition = i;
            }
        }
        return spacePosition;
    }

    public GameObject GetCardHolder(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.cardSpace[pos];
    }

    public GameObject GetEnemyHolder(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.enemySpace[pos];
    }

    public Card_Holder_Interaction GetCardHolderScript(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.cardSpace[pos].GetComponent<Card_Holder_Interaction>();
    }

    public void SetCardHolder(GameObject cardHolder, int pos)
    {
        this.cardSpace[pos] = cardHolder;
    }

    public void SetEnemyHolder(GameObject cardHolder, int pos)
    {
        this.enemySpace[pos] = cardHolder;
    }

    public void NotifyEnergyUpdated(float fraction)
    {
        this.uiCoordinator.energyBar.SetEnergyFill(fraction);
    }

    public void NotifyEnemyEnergyUpdated(float fraction)
    {
        this.uiCoordinator.enemyEnergyBar.SetEnergyFill(fraction);
    }

    public void NotifyPlayerHealthUpdate(float fraction)
    {
        float incremented = Mathf.Ceil(fraction * (healthMaxSize / healthIncrement));
        int maskAmount = healthMaxSize - (int)(incremented * (healthIncrement));

        this.uiCoordinator.playerHealth.SetMaskAmount(maskAmount);
    }
    
    public void NotifyEnemyHealthUpdate(float fraction)
    {
        float incremented = Mathf.Ceil(fraction * (healthMaxSize / healthIncrement));
        int maskAmount = healthMaxSize - (int)(incremented * (healthIncrement));

        this.uiCoordinator.enemyHealth.SetMaskAmount(maskAmount);
    }

    public void InvalidEnergyCost()
    {
        this.uiCoordinator.energyBar.FlashRed();
    }
}
