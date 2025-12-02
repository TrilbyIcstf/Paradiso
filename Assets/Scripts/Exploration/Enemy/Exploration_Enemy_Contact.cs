using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interaction of touching an enemy during exploration
/// </summary>
public class Exploration_Enemy_Contact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.EP.SetMovementLock(true);
            GameManager.instance.TR.FadeTransition("Combat", true, () => {
                Destroy(gameObject);
                GameManager.instance.TR.InitCombat();
                Enemy_Room_Object currentRoom = (Enemy_Room_Object)GameManager.instance.EL.GetCurrentRoom();
                currentRoom.SetEnemyDefeated(true);
                GameManager.instance.ER.GetCurrentRoom().UnlockRoom(currentRoom);
                GameManager.instance.ER.SetRoomActive(false);
            });
        }
    }
}
