using System.Collections.Generic;
using UnityEngine;

public class Exploration_Map_Manager : ManagerBehavior
{
    public bool MovementLock { get; set; } = false;

    public Map_Handler CurrentFloor { get; set; }

    private Queue<PopupType> popups = new Queue<PopupType>();

    public Map_UI_Coordinator GetUI()
    {
        return this.CurrentFloor.UI;
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
            this.MovementLock = false;
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
