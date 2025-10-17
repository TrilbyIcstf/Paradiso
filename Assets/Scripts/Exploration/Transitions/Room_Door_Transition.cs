using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Door_Transition : MonoBehaviour
{
    [SerializeField]
    Directions doorDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player_Movement>() != null)
        {
            Room_Object newRoom = GameManager.instance.EL.MoveInDirection(this.doorDirection);
            if (newRoom.GetRoomType() != RoomTypes.Enemy)
            {
                GameManager.instance.TR.FadeTransition("Scenes/TestMovementRoom");
            } else
            {
                GameManager.instance.TR.FadeTransition("Scenes/EnemyRoom");
            }
        }
    }
}
