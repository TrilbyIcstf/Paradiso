using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploration_Item_Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Item_Room_Object currentRoom = (Item_Room_Object)GameManager.instance.EL.GetCurrentRoom();
            Item_Base itemStats = Static_Object_Manager.instance.GetItem(currentRoom.GetItem());
            GameManager.instance.PM.AddItem(itemStats);
            currentRoom.SetItemTaken(true);
        }
    }
}
