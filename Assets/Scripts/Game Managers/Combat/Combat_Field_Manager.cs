using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Combat_Field_Manager : ManagerBehavior
{
    private Player_Field_Space[] playerSpaces = new Player_Field_Space[4];
    private Enemy_Field_Space[] enemySpaces = new Enemy_Field_Space[4];

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

    private IEnumerator ProcessField()
    {
        yield return StartCoroutine(DelayedLockField());

        yield return new WaitForSeconds(1.0f);

        // TODO: Special effects of cards doing attacks
        float damageToEnemy = 0;
        float damageToPlayer = 0;

        string debugPlayerAttacks = "Player attacks: ";
        string debugEnemyAttacks = "Enemy attacks: ";

        for (int i = 0; i < this.playerSpaces.Length; i++)
        {
            PassiveEffectParameters passParams = PassiveEffectParameters.TriggeredCard(this.playerSpaces[i].GetCardObject());

            GM.PM.ActivatePassiveItems(EffectTiming.CardScoredBefore, passParams);
            (Field_Card_Results playerResults, Field_Card_Results enemyResults) = CalculatePosition(i, this.playerSpaces, this.enemySpaces);
            GM.PM.ActivatePassiveItems(EffectTiming.CardScoredAfter, passParams);

            yield return StartCoroutine(GM.CUI.PlayFieldResultAnimations(i, playerResults, enemyResults));

            debugPlayerAttacks += $"Card {i} deals {playerResults.totalDamage}, ";
            debugEnemyAttacks += $"Card {i} deals {enemyResults.totalDamage}, ";

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

        ResetField();
        SetFieldLock(false);
        this.fieldProcessing = null;
    }

    private (Field_Card_Results, Field_Card_Results) CalculatePosition(int pos, Field_Space[] playerCards, Field_Space[] enemyCards)
    {
        Field_Card_Results playerResults = CalculateCard(pos, playerCards, enemyCards);
        Field_Card_Results enemyResults = CalculateCard(pos, enemyCards, playerCards);
        (playerResults, enemyResults) = CalculateDamage(playerResults, enemyResults);
        (playerResults, enemyResults) = CalculateEffectParametersShared(playerResults, enemyResults);

        return (playerResults, enemyResults);
    }

    private Field_Card_Results CalculateCard(int pos, Field_Space[] attackingCards, Field_Space[] defendingCards)
    {
        Field_Card_Results results = new Field_Card_Results();

        Active_Card attackingCard = attackingCards[pos].GetCard();
        Active_Card defendingCard = defendingCards[pos].GetCard();

        results.card = attackingCards[pos].GetCardObject();
        results.effects = attackingCard?.GetEffects() ?? new List<CardEffects>();
        results.effParams = new CardEffectParameters();
        results.totalAttack = attackingCard?.GetPower() ?? 0;
        results.totalDefense = attackingCard?.GetDefense() ?? 0;

        results = ApplyElementalWeakness(results, attackingCard, defendingCard);
        results = ApplyAdjacencyBuffs(results, pos, attackingCards, attackingCard);

        results.totalAttack = Mathf.Ceil(results.totalAttack);
        results.totalDefense = Mathf.Ceil(results.totalDefense);

        if (results.effects.Count != 0)
        {
            results = CalculateEffectParametersIndividual(results);
        }

        return results;
    }

    private (Field_Card_Results, Field_Card_Results) CalculateDamage(Field_Card_Results playerResults, Field_Card_Results enemyResults)
    {
        playerResults.totalDamage = Mathf.Max(0, playerResults.totalAttack - enemyResults.totalDefense);
        enemyResults.totalDamage = Mathf.Max(0, enemyResults.totalAttack - playerResults.totalDefense);
        return (playerResults, enemyResults);
    }

    private Field_Card_Results ApplyElementalWeakness(Field_Card_Results cardResults, Active_Card attacking, Active_Card defending)
    {
        (float mult, bool isWeakness, bool advantage) = ElementalMultResults(attacking, defending);
        cardResults.totalAttack *= mult;
        cardResults.flashMiddle = isWeakness;
        cardResults.advantage = advantage;
        return cardResults;
    }

    private (float, bool, bool) ElementalMultResults(Active_Card attacking, Active_Card defending)
    {
        float mult = ElementalMult(attacking, defending);
        return (mult, mult != 1.0f, mult > 1);
    }

    private float ElementalMult(Active_Card attacking, Active_Card defending)
    {
        CardElement attackingElement = attacking?.GetElement() ?? CardElement.Nill;
        CardElement defendingElement = defending?.GetElement() ?? CardElement.Nill;
        return ElementMethods.EffectivenessMult(attackingElement, defendingElement);
    }

    private bool IsSuperEffective(Active_Card attacking, Active_Card defending)
    {
        return ElementalMult(attacking, defending) > 1;
    }

    private bool IsNotVeryEffective(Active_Card attacking, Active_Card defending)
    {
        return ElementalMult(attacking, defending) < 1;
    }

    private Field_Card_Results ApplyAdjacencyBuffs(Field_Card_Results cardResults, int pos, Field_Space[] cards, Active_Card card)
    {
        CardElement cardElement = card?.GetElement() ?? CardElement.Nill;
        if (cardElement == CardElement.Nill) { return cardResults; }

        if (pos > 0)
        {
            CardElement leftElement = cards[pos - 1].GetCard()?.GetElement() ?? CardElement.Nill;
            if (cardElement == leftElement)
            {
                cardResults.totalAttack *= Consts.adj;
                cardResults.flashLeft = true;
                cardResults.effParams.adjacency++;
            }
        }

        if (pos < 3)
        {
            CardElement rightElement = cards[pos + 1].GetCard()?.GetElement() ?? CardElement.Nill;
            if (cardElement == rightElement)
            {
                cardResults.totalAttack *= Consts.adj;
                cardResults.flashRight = true;
                cardResults.effParams.adjacency++;
            }
        }

        return cardResults;
    }

    private Field_Card_Results CalculateEffectParametersIndividual(Field_Card_Results results)
    {
        results.effParams.power = results.totalAttack;
        results.effParams.defense = results.totalDefense;

        return results;
    }

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
