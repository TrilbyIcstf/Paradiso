using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat_UI_Manager : ManagerBehavior
{
    /// <summary>
    /// The card spaces the player can place cards into
    /// </summary>
    private GameObject[] cardSpace = new GameObject[4];

    /// <summary>
    /// The card spaces the enemy uses
    /// </summary>
    private GameObject[] enemySpace = new GameObject[4];

    /// <summary>
    /// The deck object the player uses
    /// </summary>
    [SerializeField]
    private GameObject playerDeck;

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

            // Alter card UI as necessary
            card.GetComponent<Card_UI>().SetToDefaultSorting(1);

            // Assign card stats to field space
            GM.CF.SetPlayerSpace(spacePosition, card);
            GM.CPH.RemoveCard(card);
        }
        else
        {
            gravityScript.SetMovementType(CardMovementType.FLOAT);
        }
    }

    public void EnemyReleasesCard(GameObject card, int position)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();

        if (position >= 0 && !GM.CF.FieldLocked())
        {
            // Set card's resting position to field space
            gravityScript.SetMovementType(CardMovementType.SNAP);
            gravityScript.SetGravityPoint(GetEnemyHolder(position).transform.position);
            gravityScript.SetPosition(GetEnemyHolder(position).transform.position);
            gravityScript.SetLocked(true);

            // Alter card UI as necessary
            card.GetComponent<Card_UI>().SetToDefaultSorting(1);

            // Assign card stats to field space
            GM.CF.SetEnemySpace(position, card);
            GM.CEH.RemoveCard(card);
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
        if (GM.CF.FieldLocked()) { return -1; }

        int spacePosition = -1;
        for (int i = 0; i < this.cardSpace.Length; i++)
        {
            if (this.cardSpace[i] == null) { break; }
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetCardHolderScript(i).shouldSnap(mousePosition) && GM.CF.GetPlayerSpace(i).GetCard() == null)
            {
                spacePosition = i;
            }
        }
        return spacePosition;
    }

    public IEnumerator PlayFieldResultAnimations(int pos, Combat_Field_Manager.Field_Card_Results playerResults, Combat_Field_Manager.Field_Card_Results enemyResults)
    {
        List<Coroutine> animations = new List<Coroutine>();
        bool emphasizePlayerCard = playerResults.flashLeft || playerResults.flashRight || playerResults.flashMiddle || GM.CCE.EffectIsTriggered(playerResults.effect, playerResults.effParams);
        bool emphasizeEnemyCard = enemyResults.flashLeft || enemyResults.flashRight || enemyResults.flashMiddle || GM.CCE.EffectIsTriggered(enemyResults.effect, enemyResults.effParams);

        if (playerResults.flashLeft)
        {
            animations.Add(StartCoroutine(FlashPlayerLeft(pos)));
        }

        if (playerResults.flashRight)
        {
            animations.Add(StartCoroutine(FlashPlayerRight(pos)));
        }

        if (enemyResults.flashLeft)
        {
            animations.Add(StartCoroutine(FlashEnemyLeft(pos)));
        }

        if (enemyResults.flashRight)
        {
            animations.Add(StartCoroutine(FlashEnemyRight(pos)));
        }

        if (playerResults.flashMiddle)
        {
            animations.Add(StartCoroutine(FlashMiddle(pos, playerResults.advantage)));
        }

        if (emphasizePlayerCard)
        {
            Card_UI cardScript = playerResults.card.GetComponent<Card_UI>();
            animations.Add(StartCoroutine(cardScript.EmphasizeCard()));
            cardScript.SetPower((int)playerResults.totalAttack);
            cardScript.SetDefense((int)playerResults.totalDefense);
            StartCoroutine(GM.CCE.TriggerCardEffect(playerResults.effect, playerResults.card, playerResults.effParams, true));
        }

        if (emphasizeEnemyCard)
        {
            Card_UI cardScript = enemyResults.card.GetComponent<Card_UI>();
            animations.Add(StartCoroutine(cardScript.EmphasizeCard()));
            cardScript.SetPower((int)enemyResults.totalAttack);
            cardScript.SetDefense((int)enemyResults.totalDefense);
            StartCoroutine(GM.CCE.TriggerCardEffect(enemyResults.effect, enemyResults.card, enemyResults.effParams, false));
        }

        foreach (Coroutine c in animations)
        {
            yield return c;
        }
    }

    public IEnumerator FlashMiddle(int pos, bool playerAdvantage)
    {
        yield return StartCoroutine(GetCardHolderScript(pos).FlashMiddle(playerAdvantage));
    }

    public IEnumerator FlashPlayerLeft(int pos)
    {
        yield return StartCoroutine(GetCardHolderScript(pos).FlashLeft());
    }

    public IEnumerator FlashPlayerRight(int pos)
    {
        yield return StartCoroutine(GetCardHolderScript(pos).FlashRight());
    }

    public IEnumerator FlashEnemyLeft(int pos)
    {
        yield return StartCoroutine(GetEnemyHolderScript(pos).FlashLeft());
    }

    public IEnumerator FlashEnemyRight(int pos)
    {
        yield return StartCoroutine(GetEnemyHolderScript(pos).FlashRight());
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

    public Player_Card_Holder GetCardHolderScript(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.cardSpace[pos].GetComponent<Player_Card_Holder>();
    }

    public Enemy_Card_Holder GetEnemyHolderScript(int pos)
    {
        if (pos < 0 || pos > 4)
        {
            return null;
        }
        return this.enemySpace[pos].GetComponent<Enemy_Card_Holder>();
    }

    public GameObject GetPlayerDeck()
    {
        return this.playerDeck;
    }

    public void SetCardHolder(GameObject cardHolder, int pos)
    {
        this.cardSpace[pos] = cardHolder;
    }

    public void SetEnemyHolder(GameObject cardHolder, int pos)
    {
        this.enemySpace[pos] = cardHolder;
    }

    public void SetPlayerDeck(GameObject deck)
    {
        this.playerDeck = deck;
    }

    public void NotifyEnergyUpdated(float fraction)
    {
        this.uiCoordinator.energyBar.SetEnergyFill(fraction);
    }

    public void NotifyEnemyEnergyUpdated(float fraction)
    {
        this.uiCoordinator.enemyEnergyBar.SetEnergyFill(fraction);
    }

    public void NotifyPlayerHealthUpdate(float current, float max)
    {
        float fraction = current / max;

        float incremented = Mathf.Ceil(fraction * (healthMaxSize / healthIncrement));
        int maskAmount = healthMaxSize - (int)(incremented * (healthIncrement));

        this.uiCoordinator.playerHealth.SetMaskAmount(maskAmount);
        this.uiCoordinator.playerHealth.UpdateHealthText((int)current, (int)max);
    }
    
    public void NotifyEnemyHealthUpdate(float current, float max)
    {
        float fraction = current / max;

        float incremented = Mathf.Ceil(fraction * (healthMaxSize / healthIncrement));
        int maskAmount = healthMaxSize - (int)(incremented * (healthIncrement));

        this.uiCoordinator.enemyHealth.SetMaskAmount(maskAmount);
        this.uiCoordinator.enemyHealth.UpdateHealthText((int)current, (int)max);
    }

    public void SetDeckEmpty()
    {
        this.playerDeck.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void InvalidEnergyCost()
    {
        this.uiCoordinator.energyBar.FlashRed();
    }
}