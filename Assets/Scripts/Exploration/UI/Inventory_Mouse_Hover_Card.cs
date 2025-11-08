using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Mouse_Hover_Card : Mouse_Hover_Card, IPointerEnterHandler, IPointerExitHandler
{
    private Card_Base baseCardStats;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExit();
    }

    public void SetBaseStats(Card_Base val)
    {
        this.baseCardStats = val;
    }

    protected override List<CardEffects> GetEffects()
    {
        return this.baseCardStats.GetEffects();
    }
}
