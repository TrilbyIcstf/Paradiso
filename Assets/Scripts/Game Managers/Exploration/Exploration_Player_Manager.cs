using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player object during exploration
/// </summary>
public class Exploration_Player_Manager : MonoBehaviour
{
    private bool playerMovementLock = false;

    public bool GetMovementLock()
    {
        return this.playerMovementLock;
    }

    public void SetMovementLock(bool val)
    {
        this.playerMovementLock = val;
        ApplyMovementLock();
    }

    public void ApplyMovementLock()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Player_Movement moveScript = player.GetComponent<Player_Movement>();
            moveScript.SetMoveLock(this.playerMovementLock);
        }
    }
}
