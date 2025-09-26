using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth = 100;

    public int GetMaxHealth()
    {
        return this.maxHealth;
    }

    public int GetCurrentHealth()
    {
        return this.currentHealth;
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
