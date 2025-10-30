using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles moving between rooms upon contact with player
/// </summary>
public class Room_Door_Transition : MonoBehaviour
{
    [SerializeField]
    Directions doorDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player_Movement>() != null)
        {
            GameManager.instance.EP.SetMovementLock(true);
            Room_Object newRoom = GameManager.instance.EL.MoveInDirection(this.doorDirection);
            if (newRoom.GetRoomType() == RoomTypes.Item)
            {
                GameManager.instance.TR.FadeTransition("Scenes/ItemRoom", postAction: UnlockMovement);
            } else if (newRoom.GetRoomType() == RoomTypes.Enemy)
            {
                GameManager.instance.TR.FadeTransition("Scenes/EnemyRoom", postAction: UnlockMovement);
            } else if (newRoom.GetRoomType() == RoomTypes.Stairs)
            {
                GameManager.instance.TR.FadeTransition("Scenes/StairsRoom", postAction: UnlockMovement);
            } else
            {
                GameManager.instance.TR.FadeTransition("Scenes/TestMovementRoom", postAction: UnlockMovement);
            }
        }
    }

    private void UnlockMovement()
    {
        GameManager.instance.EP.SetMovementLock(false);
    }
}
