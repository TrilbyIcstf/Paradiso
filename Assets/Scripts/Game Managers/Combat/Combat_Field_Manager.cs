using UnityEngine;
using System.Linq;
using System.Collections;

public class Combat_Field_Manager : MonoBehaviour
{
    private Player_Field_Space[] playerSpaces = new Player_Field_Space[4];
    private Enemy_Field_Space[] enemySpaces = new Enemy_Field_Space[4];

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
        yield return new WaitForSeconds(1.0f);
        this.fieldLocked = true;
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
            Active_Card playerCard = this.playerSpaces[i].GetCard();
            Active_Card enemyCard = this.enemySpaces[i].GetCard();

            float playersCardDamage = CalculateDamage(i, this.playerSpaces, this.enemySpaces);
            float enemysCardDamage = CalculateDamage(i, this.enemySpaces, this.playerSpaces);

            debugPlayerAttacks += $"Card {i} deals {playersCardDamage}, ";
            debugEnemyAttacks += $"Card {i} deals {enemysCardDamage}, ";

            damageToEnemy += playersCardDamage;
            damageToPlayer += enemysCardDamage;
        }

        debugPlayerAttacks += $"dealing {damageToEnemy} in total.";
        debugEnemyAttacks += $"dealing {damageToPlayer} in total.";

        Debug.Log(debugPlayerAttacks);
        Debug.Log(debugEnemyAttacks);

        GameManager.instance.CES.DealDamage(damageToEnemy);
        GameManager.instance.CS.DealDamage(damageToPlayer);

        ResetField();
        this.fieldLocked = false;
        this.fieldProcessing = null;
    }

    private float CalculateDamage(int pos, Field_Space[] attackingCards, Field_Space[] defendingCards)
    {
        float damage = 0;

        Active_Card attackingCard = attackingCards[pos].GetCard();
        Active_Card defendingCard = defendingCards[pos].GetCard();

        float attackingPower = attackingCard?.GetPower() ?? 0;
        float defendingDefense = defendingCard?.GetDefense() ?? 0;

        if (attackingPower <= 0) { return 0; }

        attackingPower = ApplyElementalWeakness(attackingPower, attackingCard, defendingCard);
        attackingPower = ApplyAdjacencyBuffs(attackingPower, pos, attackingCards, attackingCard);
        attackingPower = Mathf.Ceil(attackingPower);

        defendingDefense = ApplyAdjacencyBuffs(defendingDefense, pos, defendingCards, defendingCard);
        defendingDefense = Mathf.Ceil(defendingDefense);

        damage = Mathf.Max(0, attackingPower - defendingDefense);

        return damage;
    }

    private float ApplyElementalWeakness(float baseDamage, Active_Card attacking, Active_Card defending)
    {
        CardElement attackingElement = attacking?.GetElement() ?? CardElement.Nill;
        CardElement defendingElement = defending?.GetElement() ?? CardElement.Nill;
        return baseDamage * ElementMethods.EffectivenessMult(attackingElement, defendingElement);
    }

    private float ApplyAdjacencyBuffs(float baseVal, int pos, Field_Space[] cards, Active_Card card)
    {
        CardElement cardElement = card?.GetElement() ?? CardElement.Nill;
        if (cardElement == CardElement.Nill) { return baseVal; }

        if (pos > 0)
        {
            CardElement leftElement = cards[pos - 1].GetCard()?.GetElement() ?? CardElement.Nill;
            if (cardElement == leftElement)
            {
                baseVal *= Consts.adj;
            }
        }

        if (pos < 3)
        {
            CardElement rightElement = cards[pos + 1].GetCard()?.GetElement() ?? CardElement.Nill;
            if (cardElement == rightElement)
            {
                baseVal *= Consts.adj;
            }
        }

        return baseVal;
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

    public bool FieldLocked()
    {
        return this.fieldLocked;
    }
}
