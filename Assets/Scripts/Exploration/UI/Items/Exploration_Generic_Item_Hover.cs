using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Exploration_Generic_Item_Hover : Mouse_Hover_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item_Holder itemHolder;

    private void OnMouseDown()
    {
        DestroyBox();
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyBox();
    }

    protected override Item_Base GetBase()
    {
        return this.itemHolder.GetItem();
    }
}
