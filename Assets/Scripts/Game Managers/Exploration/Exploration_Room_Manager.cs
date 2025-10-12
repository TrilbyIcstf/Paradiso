using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploration_Room_Manager : ManagerBehavior
{
    private Room_Controller currentRoom;

    public void SetupCurrentRoom()
    {
        this.currentRoom.SetupRoom(GM.EL.GetCurrentRoom());
    }

    public void SetCurrentRoom(Room_Controller val)
    {
        this.currentRoom = val;
    }
}
