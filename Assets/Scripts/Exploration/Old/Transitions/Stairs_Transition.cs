using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs_Transition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.EL.IncrementFloorNumber();
            if (GameManager.instance.EL.GetFloorNumber() == 5)
            {
                GameManager.instance.EL.BossFloor();
            }
            else
            {
                GameManager.instance.EL.RandomizeFloor(5, 5);
            }
            GameManager.instance.ER.EnteredDirection = Direction.None;
            GameManager.instance.TR.FadeTransition("Scenes/TestMovementRoom", postAction: UnlockMovement);
        }
    }

    private void UnlockMovement()
    {
        GameManager.instance.EP.SetMovementLock(false);
    }
}
