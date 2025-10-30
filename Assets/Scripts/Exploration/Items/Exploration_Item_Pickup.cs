using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploration_Item_Pickup : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        this.sr = GetComponent<SpriteRenderer>();

        Item_Room_Object currentRoom = CurrentRoom();
        Items roomItem = currentRoom.GetItem();
        Item_Base itemObject = Static_Object_Manager.instance.GetItem(roomItem);

        this.sr.sprite = itemObject.GetSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Item_Room_Object currentRoom = CurrentRoom();
            Item_Base itemStats = Static_Object_Manager.instance.GetItem(currentRoom.GetItem());
            GameManager.instance.PM.AddItem(itemStats);
            GameManager.instance.ER.GetUI().OnItemPickup(itemStats);
            currentRoom.SetItemTaken(true);
        }
    }

    private Item_Room_Object CurrentRoom()
    {
        return (Item_Room_Object)GameManager.instance.EL.GetCurrentRoom();
    }
}
