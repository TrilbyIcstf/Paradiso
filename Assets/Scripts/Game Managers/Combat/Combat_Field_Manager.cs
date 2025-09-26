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
        int enemyDamage = 0;
        int playerDamage = 0;

        for (int i = 0; i < this.playerSpaces.Length; i++)
        {
            if (this.playerSpaces[i].GetCard() != null && this.enemySpaces[i].GetCard() == null)
            {
                enemyDamage += 10;
            } else if (this.playerSpaces[i].GetCard() == null && this.enemySpaces[i].GetCard() != null)
            {
                playerDamage += 10;
            }
        }

        GameManager.instance.CES.DealDamage(enemyDamage);
        GameManager.instance.CS.DealDamage(playerDamage);

        ResetField();
        this.fieldLocked = false;
        this.fieldProcessing = null;
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
