using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth = 100;

    [SerializeField]
    private List<Item_Base> playerItems = new List<Item_Base>();

    public int GetMaxHealth()
    {
        return this.maxHealth;
    }

    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public List<Item_Base> GetItems()
    {
        return this.playerItems;
    }

    public List<Item_Active> GetActiveItems()
    {
        return this.playerItems.Where(i => i is Item_Active).Select(i => (Item_Active)i).ToList();
    }

    public void SetMaxHealth(int val)
    {
        this.maxHealth = val;
    }

    public void SetCurrentHealth(int val)
    {
        this.currentHealth = Mathf.Min(val, this.maxHealth);
    }
}
