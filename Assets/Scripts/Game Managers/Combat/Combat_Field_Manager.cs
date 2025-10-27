using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the field spaces during combat
/// </summary>
public class Combat_Field_Manager : ManagerBehavior
{
    private Player_Field_Space[] playerSpaces = new Player_Field_Space[4];
    private Enemy_Field_Space[] enemySpaces = new Enemy_Field_Space[4];

    // Used to show how much time until the field locks
    private Field_Timer fieldTimer;

    private bool fieldLocked = false;

    private Coroutine fieldProcessing;

    private void Awake()
    {
        InitiateField();
    }

    /// <summary>
    /// Resets field in preperation for new combat
    /// </summary>
    public void NewCombat()
    {
        InitiateField();
    }

    /// <summary>
    /// Locks the field after a delay
    /// </summary>
    private IEnumerator DelayedLockField()
    {
        yield return StartCoroutine(this.fieldTimer.RunTimer());
        SetFieldLock(true);
    }

    public void SetFieldLock(bool val)
    {
        this.fieldLocked = val;
        foreach(Combat_Item item in GM.CPI.GetPlayerItems())
        {
            item.SetLock(val);
        }
        GM.CPS.SetEnergyLock(val);
        GM.CES.SetEnergyLock(val);
    }

    /// <summary>
    /// Essentially completes a 'turn' of the combat. Compares the damage values of the player's and 
    /// opponent's played cards and applies effects and damage, then resets field
    /// </summary>
    private IEnumerator ProcessField()
    {
        // Lock the field
        yield return StartCoroutine(DelayedLockField());

        yield return new WaitForSeconds(1.0f);

        float damageToEnemy = 0;
        float damageToPlayer = 0;

        string debugPlayerAttacks = "Player attacks: ";
        string debugEnemyAttacks = "Enemy attacks: ";

        // Loop through each field space
        for (int i = 0; i < this.playerSpaces.Length; i++)
        {
            // Tracks parameters of each field space to determine damage and effects
            PassiveEffectParameters passParams = PassiveEffectParameters.TriggeredCard(this.playerSpaces[i].GetCardObject());

            // Trigger passive effects BEFORE damage calcs
            GM.PM.ActivatePassiveItems(EffectTiming.CardScoredBefore, passParams);

            // Calculate damage, buffs, etc.
            (Field_Card_Results playerResults, Field_Card_Results enemyResults) = CalculatePosition(i, this.playerSpaces, this.enemySpaces);

            // Trigger passive effects AFTER damage calcs
            GM.PM.ActivatePassiveItems(EffectTiming.CardScoredAfter, passParams);

            // Play animations from calcs
            yield return StartCoroutine(GM.CUI.PlayFieldResultAnimations(i, playerResults, enemyResults));

            debugPlayerAttacks += $"Card {i} deals {playerResults.totalDamage}, ";
            debugEnemyAttacks += $"Card {i} deals {enemyResults.totalDamage}, ";

            // Add to damage totals
            damageToEnemy += playerResults.totalDamage;
            damageToPlayer += enemyResults.totalDamage;
        }

        yield return new WaitForSeconds(0.25f);

        debugPlayerAttacks += $"dealing {damageToEnemy} in total.";
        debugEnemyAttacks += $"dealing {damageToPlayer} in total.";

        Debug.Log(debugPlayerAttacks);
        Debug.Log(debugEnemyAttacks);

        GM.CES.DealDamage(damageToEnemy);
        GM.CPS.DealDamage(damageToPlayer);

        // Reset field and resume playing
        ResetField();
        SetFieldLock(false);
        this.fieldProcessing = null;
    }

    /// <summary>
    /// Calculates the player's and enemy's cards damage, effects, etc. for a single collumn of the field
    /// </summary>
    /// <param name="pos">Position of the collumn</param>
    /// <param name="playerCards">Array of all player's cards</param>
    /// <param name="enemyCards">Array of all enemy's cards</param>
    /// <returns>Results for player's and enemy's cards</returns>
    private (Field_Card_Results, Field_Card_Results) CalculatePosition(int pos, Field_Space[] playerCards, Field_Space[] enemyCards)
    {
        // Pulls all relevent data from the field for calculations
        Field_Card_Results playerResults = CalculateCard(pos, playerCards, enemyCards);
        Field_Card_Results enemyResults = CalculateCard(pos, enemyCards, playerCards);

        // Calculates damage
        (playerResults, enemyResults) = CalculateDamage(playerResults, enemyResults);

        // Calculates effect parameters
        (playerResults, enemyResults) = CalculateEffectParametersShared(playerResults, enemyResults);

        return (playerResults, enemyResults);
    }

    /// <summary>
    /// Gets data for a single card during field calculations
    /// </summary>
    /// <param name="pos">Position of card in field</param>
    /// <param name="attackingCards">The cards on the field for the attacker</param>
    /// <param name="defendingCards">The cards on teh field for the defender</param>
    /// <returns>Calc results for the card</returns>
    private Field_Card_Results CalculateCard(int pos, Field_Space[] attackingCards, Field_Space[] defendingCards)
    {
        Field_Card_Results results = new Field_Card_Results();

        Active_Card attackingCard = attackingCards[pos].GetCard();
        Active_Card defendingCard = defendingCards[pos].GetCard();

        // Set defaults
        results.card = attackingCards[pos].GetCardObject();
        results.effects = attackingCard?.GetEffects() ?? new List<CardEffects>();
        results.effParams = new CardEffectParameters();
        results.totalAttack = attackingCard?.GetPower() ?? 0;
        results.totalDefense = attackingCard?.GetDefense() ?? 0;

        // Apply elemental and adjacency buffs (adjacency only buffs attack so defending card isn't factored in)
        results = ApplyElementalWeakness(results, attackingCard, defendingCard);
        results = ApplyAdjacencyBuffs(results, pos, attackingCards, attackingCard);

        // Rounds stats to int
        results.totalAttack = Mathf.Ceil(results.totalAttack);
        results.totalDefense = Mathf.Ceil(results.totalDefense);

        // Calculates parameters for effects on the card
        if (results.effects.Count != 0)
        {
            results = CalculateEffectParametersIndividual(results);
        }

        return results;
    }

    /// <summary>
    /// Calculates damage dealt by a card clash
    /// </summary>
    /// <param name="playerResults">Player's result holder</param>
    /// <param name="enemyResults">Enemy's result holder</param>
    /// <returns>The damage results for the player and enemy</returns>
    private (Field_Card_Results, Field_Card_Results) CalculateDamage(Field_Card_Results playerResults, Field_Card_Results enemyResults)
    {
        playerResults.totalDamage = Mathf.Max(0, playerResults.totalAttack - enemyResults.totalDefense);
        enemyResults.totalDamage = Mathf.Max(0, enemyResults.totalAttack - playerResults.totalDefense);
        return (playerResults, enemyResults);
    }

    /// <summary>
    /// Applies the elemental effectiveness for a card clash
    /// </summary>
    /// <param name="cardResults">Result holder for the card</param>
    /// <param name="attacking">The attacking card</param>
    /// <param name="defending">The defending card</param>
    /// <returns>The adjusted stat results</returns>
    private Field_Card_Results ApplyElementalWeakness(Field_Card_Results cardResults, Active_Card attacking, Active_Card defending)
    {
        (float mult, bool isWeakness, bool advantage) = ElementalMultResults(attacking, defending);
        cardResults.totalAttack *= mult;
        cardResults.flashMiddle = isWeakness;
        cardResults.advantage = advantage;
        return cardResults;
    }

    /// <summary>
    /// Calculates the elemental damage multiplier for a card clash
    /// </summary>
    /// <param name="attacking">The attacking card</param>
    /// <param name="defending">The defending card</param>
    /// <returns>
    /// 1. The damage multiplier
    /// 2. If the damage multiplier was not neutral
    /// 3. If the damage multiplier was positive
    /// </returns>
    private (float, bool, bool) ElementalMultResults(Active_Card attacking, Active_Card defending)
    {
        float mult = ElementalMult(attacking, defending);
        return (mult, mult != 1.0f, mult > 1);
    }

    /// <summary>
    /// Gets the multiplier number for an elemental clash
    /// </summary>
    /// <param name="attacking">The attacking card</param>
    /// <param name="defending">The defending card</param>
    /// <returns>Elemental multiplier</returns>
    private float ElementalMult(Active_Card attacking, Active_Card defending)
    {
        CardElement attackingElement = attacking?.GetElement() ?? CardElement.Nil;
        CardElement defendingElement = defending?.GetElement() ?? CardElement.Nil;
        return ElementMethods.EffectivenessMult(attackingElement, defendingElement);
    }

    /// <summary>
    /// Determines if a clash will have a positive elemental mult
    /// </summary>
    /// <param name="attacking">The attacking card</param>
    /// <param name="defending">The defending card</param>
    /// <returns>If the elemental mult is positive</returns>
    private bool IsSuperEffective(Active_Card attacking, Active_Card defending)
    {
        return ElementalMult(attacking, defending) > 1;
    }

    /// <summary>
    /// Determines if a clash will have a negative elemental mult
    /// </summary>
    /// <param name="attacking">The attacking card</param>
    /// <param name="defending">The defending card</param>
    /// <returns>If the elemental mult is negative</returns>
    private bool IsNotVeryEffective(Active_Card attacking, Active_Card defending)
    {
        return ElementalMult(attacking, defending) < 1;
    }

    /// <summary>
    /// Applies elemental adjacency buffs for a card
    /// </summary>
    /// <param name="cardResults">Result holder for a card</param>
    /// <param name="pos">Field position for the card</param>
    /// <param name="cards">List of user's cards on the field</param>
    /// <param name="card">Card being applied to</param>
    /// <returns></returns>
    private Field_Card_Results ApplyAdjacencyBuffs(Field_Card_Results cardResults, int pos, Field_Space[] cards, Active_Card card)
    {
        CardElement cardElement = card?.GetElement() ?? CardElement.Nil;

        // Nil doesn't get buffs
        if (cardElement == CardElement.Nil) { return cardResults; }

        // Check card to left
        if (pos > 0)
        {
            CardElement leftElement = cards[pos - 1].GetCard()?.GetElement() ?? CardElement.Nil;
            if (cardElement == leftElement)
            {
                cardResults.totalAttack *= Consts.adj;
                cardResults.flashLeft = true;
                cardResults.effParams.adjacency++;
            }
        }

        // Check card to right
        if (pos < 3)
        {
            CardElement rightElement = cards[pos + 1].GetCard()?.GetElement() ?? CardElement.Nil;
            if (cardElement == rightElement)
            {
                cardResults.totalAttack *= Consts.adj;
                cardResults.flashRight = true;
                cardResults.effParams.adjacency++;
            }
        }

        return cardResults;
    }

    /// <summary>
    /// Calculates effect parameters that only care about the individual card
    /// </summary>
    /// <param name="results">Result holder</param>
    /// <returns>Relevent effect parameters</returns>
    private Field_Card_Results CalculateEffectParametersIndividual(Field_Card_Results results)
    {
        results.effParams.power = results.totalAttack;
        results.effParams.defense = results.totalDefense;

        return results;
    }

    /// <summary>
    /// Calculates effect parameters that will be used by both cards
    /// </summary>
    /// <param name="playerResults">Player's result holder</param>
    /// <param name="enemyResults">Enemy's result holder</param>
    /// <returns>Effect parameters</returns>
    private (Field_Card_Results, Field_Card_Results) CalculateEffectParametersShared(Field_Card_Results playerResults, Field_Card_Results enemyResults)
    {
        int playerHandSize = GM.CPH.HandSize();
        int enemyHandSize = GM.CEH.HandSize();

        playerResults.effParams.handSize = playerHandSize;
        playerResults.effParams.opponentHandSize = enemyHandSize;

        enemyResults.effParams.handSize = enemyHandSize;
        enemyResults.effParams.opponentHandSize = playerHandSize;

        return (playerResults, enemyResults);
    }

    /// <summary>
    /// Returns the position of the first empty space on the player's field
    /// </summary>
    /// <returns>The position of the first empty space on the player's field</returns>
    public int NextFreePlayerSpace()
    {
        if (this.fieldLocked) { return -1; }

        int? nextSpace = this.playerSpaces
            .Where(s => s.GetCard() == null)
            .Select(s => (int?)s.GetPosition())
            .FirstOrDefault();
        return nextSpace ?? -1;
    }

    /// <summary>
    /// Checks if all player spaces are full
    /// </summary>
    /// <returns>True if there are no empty spaces remains</returns>
    private bool PlayerSpacesFull()
    {
        return !this.playerSpaces.Any(s => s.GetCard() == null);
    }

    /// <summary>
    /// Returns the position of the first empty space on the enemy's field
    /// </summary>
    /// <returns>The position of the first empty space on the enemy's field</returns>
    public int NextFreeEnemySpace()
    {
        if (this.fieldLocked) { return -1; }

        int? nextSpace = this.enemySpaces
            .Where(s => s.GetCard() == null)
            .Select(s => (int?)s.GetPosition())
            .FirstOrDefault();
        return nextSpace ?? -1;
    }

    /// <summary>
    /// Checks if all enemy spaces are full
    /// </summary>
    /// <returns>True if there are no empty spaces remains</returns>
    private bool EnemySpacesFull()
    {
        return !this.enemySpaces.Any(s => s.GetCard() == null);
    }

    private void InitiateField()
    {
        // Resets the player's field with empty card slots
        for (int i = 0; i < this.playerSpaces.Length; i++)
        {
            this.playerSpaces[i] = new Player_Field_Space(i);
        }

        // Resets the enemy's field with empty card slots
        for (int i = 0; i < this.enemySpaces.Length; i++)
        {
            this.enemySpaces[i] = new Enemy_Field_Space(i);
        }
    }

    /// <summary>
    /// Resets field with empty card spaces
    /// </summary>
    public void ResetField()
    {
        // Resets the player's field with empty card slots
        for (int i = 0; i < this.playerSpaces.Length; i++)
        {
            Destroy(this.playerSpaces[i].GetCardObject());
            this.playerSpaces[i].ClearCard();
        }

        // Resets the enemy's field with empty card slots
        for (int i = 0; i < this.enemySpaces.Length; i++)
        {
            Destroy(this.enemySpaces[i].GetCardObject());
            this.enemySpaces[i].ClearCard();
        }
    }

    /// <summary>
    /// Gets designated player field space
    /// </summary>
    /// <param name="position">Field position</param>
    /// <returns>Designated field space</returns>
    public Player_Field_Space GetPlayerSpace(int position)
    {
        if (position >= 0 && position < this.playerSpaces.Length)
        {
            return this.playerSpaces[position];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Sets a card in a player field slot
    /// </summary>
    /// <param name="position">Position of card</param>
    /// <param name="card">The card</param>
    public void SetPlayerSpace(int position, GameObject card)
    {
        if (position >= 0 && position < this.playerSpaces.Length)
        {
            this.playerSpaces[position].SetCard(card);
            if (PlayerSpacesFull() && this.fieldProcessing == null)
            {
                this.fieldProcessing = StartCoroutine(ProcessField());
            }
        }
    }

    /// <summary>
    /// Gets designated enemy field space
    /// </summary>
    /// <param name="position">Field position</param>
    /// <returns>Designated field space</returns>
    public Enemy_Field_Space GetEnemySpace(int position)
    {
        if (position >= 0 && position < this.enemySpaces.Length)
        {
            return this.enemySpaces[position];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Sets a card in an enemy field slot
    /// </summary>
    /// <param name="position">Position of card</param>
    /// <param name="card">The card</param>
    public void SetEnemySpace(int position, GameObject card)
    {
        if (position >= 0 && position < this.enemySpaces.Length)
        {
            this.enemySpaces[position].SetCard(card);
            if (EnemySpacesFull() && this.fieldProcessing == null)
            {
                this.fieldProcessing = StartCoroutine(ProcessField());
            }
        }
    }

    public void SetFieldTimer(Field_Timer val)
    {
        this.fieldTimer = val;
    }

    public bool FieldLocked()
    {
        return this.fieldLocked;
    }

    public class Field_Card_Results
    {
        internal GameObject card;

        internal List<CardEffects> effects = new List<CardEffects>();
        internal CardEffectParameters effParams;

        internal bool flashLeft = false;
        internal bool flashMiddle = false;
        internal bool flashRight = false;

        internal bool advantage = false;

        internal float totalDamage = 0;
        internal float totalAttack = 0;
        internal float totalDefense = 0;
    }
}
