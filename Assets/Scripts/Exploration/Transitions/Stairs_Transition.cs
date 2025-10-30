using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs_Transition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player_Movement>() != null)
        {
            GameManager.instance.EL.RandomizeFloor(5, 5);
            GameManager.instance.ER.EnteredDirection = Directions.None;
            GameManager.instance.TR.FadeTransition("Scenes/TestMovementRoom", postAction: UnlockMovement);
        }
    }

    private void UnlockMovement()
    {
        GameManager.instance.EP.SetMovementLock(false);
    }
}
