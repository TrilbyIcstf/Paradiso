using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages UI elements during a combat scene
/// </summary>
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

    /// <summary>
    /// The max size of the health bar (not its health value)
    /// </summary>
    private static int healthMaxSize = 400;

    /// <summary>
    /// The size increment the health bar will increase and decrease in (trying to match 1 pixel)
    /// </summary>
    private static int healthIncrement = 5;

    /// <summary>
    /// Tracks if the player is holding a card
    /// </summary>
    public bool isHoldingCard = false;

    public UI_Coordinator uiCoordinator;

    [SerializeField]
    private Combat_Pip_Controller pipController;

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
            card.GetComponent<Card_UI_Controller>().SetToDefaultSorting(1);

            // Assign card stats to field space
            GM.CF.SetPlayerSpace(spacePosition, card);
            GM.CPH.RemoveCard(card);
        }
        else
        {
            gravityScript.SetMovementType(CardMovementType.FLOAT);
        }
    }

    /// <summary>
    /// Releases the card from the enemy's grip and snaps it to the field if needed
    /// </summary>
    /// <param name="card">The card being released</param>
    /// <param name="position">The position of the field space being played to (-1 for none)</param>
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
            card.GetComponent<Card_UI_Controller>().SetToDefaultSorting(1);

            // Assign card stats to field space
            GM.CF.SetEnemySpace(position, card);
            GM.CEH.RemoveCard(card);
        }
        else
        {
            gravityScript.SetMovementType(CardMovementType.FLOAT);
        }
    }

    /// <summary>
    /// Returns a card to the player's hand
    /// </summary>
    /// <param name="card">The card to return</param>
    public void ReturnToHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        gravityScript.SetGravityPoint(this.uiCoordinator.PlayerHandArea().ClosestPoint(card.transform.position));
    }

    /// <summary>
    /// Returns a card to the enemy's hand
    /// </summary>
    /// <param name="card">The card to return</param>
    public void ReturnToEnemyHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        gravityScript.SetGravityPoint(this.uiCoordinator.EnemyHandArea().ClosestPoint(card.transform.position));
    }

    /// <summary>
    /// Sends a newly drawn card to the player's hand with slight variation
    /// </summary>
    /// <param name="card">The drawn card</param>
    public void DrawToHand(GameObject card)
    {
        Card_Gravity gravityScript = card.GetComponent<Card_Gravity>();
        Vector2 closestPoint = this.uiCoordinator.PlayerHandArea().ClosestPoint(card.transform.position);
        closestPoint.y += Random.Range(-1.0f,2.5f);
        closestPoint.x += Random.Range(-1.5f, 0.0f);
        gravityScript.SetGravityPoint(closestPoint);
    }

    /// <summary>
    /// Sends a newly drawn card to the enemy's hand with slight variation
    /// </summary>
    /// <param name="card">The drawn card</param>
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
            if ((GetCardHolderScript(i).shouldSnap(mousePosition) || GetCardHolderScript(i).shouldSnap(card.transform.position)) && GM.CF.GetPlayerSpace(i).GetCard() == null)
            {
                spacePosition = i;
            }
        }
        return spacePosition;
    }

    /// <summary>
    /// Checks if a card should be emphasized during field calcs
    /// </summary>
    /// <param name="results">Result holder</param>
    /// <returns>If the card should be emphasized</returns>
    private bool WillEmphasize(Field_Card_Results results)
    {
        if (results.flashLeft || results.flashRight || results.flashMiddle)
        {
            return true;
        }
        if (GM.CCE.EffectIsTriggered(results.effects, results.effParams))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a card should be emphasized during field calcs
    /// </summary>
    /// <param name="results">Result holder</param>
    /// <param name="passParams">Parameters for passives</param>
    /// <returns>If the card should be emphasized</returns>
    private bool WillEmphasize(Field_Card_Results results, PassiveEffectParameters passParams)
    {
        if (WillEmphasize(results)) {
            return true;
        }

        if (GM.PM.WillPassiveTrigger(EffectTiming.CardScoredBefore, passParams) || GM.PM.WillPassiveTrigger(EffectTiming.CardScoredAfter, passParams))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Plays all relevent animations for a field position
    /// </summary>
    /// <param name="pos">The field position</param>
    /// <param name="playerResults">The player's field results</param>
    /// <param name="enemyResults">The enemy's field results</param>
    public IEnumerator PlayCardResultAnimations(int pos, Field_Card_Results playerResults, Field_Card_Results enemyResults)
    {
        List<Coroutine> animations = new List<Coroutine>();

        PassiveEffectParameters passParams = PassiveEffectParameters.TriggeredCard(playerResults.card, enemyResults.card);
        bool emphasizePlayerCard = WillEmphasize(playerResults, passParams);
        bool emphasizeEnemyCard = WillEmphasize(enemyResults);

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
            Card_UI_Controller cardScript = playerResults.card.GetComponent<Card_UI_Controller>();
            animations.Add(StartCoroutine(cardScript.EmphasizeCard()));
            cardScript.SetPower((int)playerResults.totalAttack);
            cardScript.SetDefense((int)playerResults.totalDefense);
            foreach (CardEffect effect in playerResults.effects)
            {
                animations.Add(StartCoroutine(GM.CCE.TriggerCardEffect(effect, playerResults.card, playerResults.effParams, true)));
            }
        }

        if (emphasizeEnemyCard)
        {
            Card_UI_Controller cardScript = enemyResults.card.GetComponent<Card_UI_Controller>();
            animations.Add(StartCoroutine(cardScript.EmphasizeCard()));
            cardScript.SetPower((int)enemyResults.totalAttack);
            cardScript.SetDefense((int)enemyResults.totalDefense);
            foreach (CardEffect effect in enemyResults.effects)
            {
                animations.Add(StartCoroutine(GM.CCE.TriggerCardEffect(effect, enemyResults.card, enemyResults.effParams, false)));
            }
        }

        foreach (Coroutine c in animations)
        {
            yield return c;
        }
    }

    /// <summary>
    /// Plays relevant animations for the full field
    /// </summary>
    /// <param name="results"></param>
    /// <returns></returns>
    public IEnumerator PlayFieldResultAnimations(Field_Full_Results results)
    {
        List<Coroutine> animations = new List<Coroutine>();

        if (results.GetPlayerAffinity())
        {
            for (int i = 0; i < results.GetSize(); i++)
            {
                Field_Card_Results result = results.GetPlayerResult(i);
                Card_UI_Controller cardScript = result.card.GetComponent<Card_UI_Controller>();
                animations.Add(StartCoroutine(cardScript.EmphasizeCard()));
                cardScript.SetPower((int)result.totalAttack);
                cardScript.SetDefense((int)result.totalDefense);
            }
        }

        if (results.GetEnemyAffinity())
        {
            for (int i = 0; i < results.GetSize(); i++)
            {
                Field_Card_Results result = results.GetEnemyResult(i);
                Card_UI_Controller cardScript = result.card.GetComponent<Card_UI_Controller>();
                animations.Add(StartCoroutine(cardScript.EmphasizeCard()));
                cardScript.SetPower((int)result.totalAttack);
                cardScript.SetDefense((int)result.totalDefense);
            }
        }

        foreach (Coroutine c in animations)
        {
            yield return c;
        }
    }

    public IEnumerator PlayDamagePips(Field_Full_Results results)
    {
        for (int i = 0; i < results.GetSize(); i++)
        {
            yield return StartCoroutine(this.pipController.ReleaseThePips(results, i));
            GameObject playerCard = results.GetPlayerResult(i).card;
            GameObject enemyCard = results.GetEnemyResult(i).card;

            Destroy(playerCard);
            Destroy(enemyCard);
        }

        yield return StartCoroutine(this.pipController.WaitUntilFinished());
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

    public Combat_Pip_Controller GetPipController()
    {
        return this.pipController;
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

    public void SetEnergyPreview(float energyCost)
    {
        float fillAmount = GM.CPS.GetEnergyFraction(energyCost);
        this.uiCoordinator.SetPreviewFill(fillAmount);
    }

    public void RemoveEnergyPreview()
    {
        this.uiCoordinator.SetPreviewFill(0);
    }

    public void InvalidEnergyCost()
    {
        this.uiCoordinator.energyBar.FlashRed();
    }
}