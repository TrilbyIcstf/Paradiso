using UnityEngine;

public class Combat_Field_Manager : MonoBehaviour
{
    private Player_Field_Space[] playerSpaces = new Player_Field_Space[4];
    private Enemy_Field_Space[] enemySpaces = new Enemy_Field_Space[4];

    private void Awake()
    {
        ResetField();
    }

    /// <summary>
    /// Resets field in preperation for new combat
    /// </summary>
    public void NewCombat()
    {
        ResetField();
    }

    /// <summary>
    /// Resets field with empty card spaces
    /// </summary>
    public void ResetField()
    {
        // Resets the player's field with empty card slots
        for (int i = 0; i < playerSpaces.Length; i++)
        {
            this.playerSpaces[i] = new Player_Field_Space(i);
        }

        // Resets the enemy's field with empty card slots
        for (int i = 0; i < enemySpaces.Length; i++)
        {
            this.enemySpaces[i] = new Enemy_Field_Space(i);
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
    public void SetPlayerSpace(int position, Card_Base card)
    {
        if (position >= 0 && position < this.playerSpaces.Length)
        {
            this.playerSpaces[position].SetCard(card);
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
    public void SetEnemySpace(int position, Card_Base card)
    {
        if (position >= 0 && position < this.enemySpaces.Length)
        {
            this.enemySpaces[position].SetCard(card);
        }
    }
}
