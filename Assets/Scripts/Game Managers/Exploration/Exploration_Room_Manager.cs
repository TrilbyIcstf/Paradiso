using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the current room during exploration
/// </summary>
public class Exploration_Room_Manager : ManagerBehavior
{
    private Room_Controller currentRoom;
    private Direction enteredDirection = Direction.None;

    public Direction EnteredDirection
    {
        get { return enteredDirection; }
        set { this.enteredDirection = value; }
    }

    public void SetupCurrentRoom()
    {
        this.currentRoom.SetupRoom(GM.EL.GetCurrentRoom(), this.enteredDirection);
    }

    public void SetCurrentRoom(Room_Controller val)
    {
        this.currentRoom = val;
    }

    public void SetRoomActive(bool val)
    {
        currentRoom.gameObject.SetActive(val);
    }

    public Room_Controller GetCurrentRoom()
    {
        return this.currentRoom;
    }

    public Room_UI_Coordinator GetUI()
    {
        return this.currentRoom.GetUI();
    }
}
