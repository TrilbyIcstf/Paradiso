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

    private Queue<PopupType> popups = new Queue<PopupType>();

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

    public void AddPopup(PopupType popup)
    {
        this.popups.Enqueue(popup);
    }

    public void NextPopup()
    {
        GetUI().BaseUIVisibility(false);

        if (this.popups.Count == 0)
        {
            GM.EP.SetMovementLock(false);
            GetUI().BaseUIVisibility(true);
        } 
        else
        {
            PopupType type = this.popups.Dequeue();
            switch (type)
            {
                case PopupType.Upgrade:
                    GM.EU.TriggerUpgrade();
                    break;
            }
        }
    }
}
