using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Exploration_Generic_Item_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject infoBox;

    [SerializeField]
    private Item_Holder itemHolder;

    private GameObject tempInfoBox;

    private float hoverDuration = 0.5f;
    private Coroutine hoverCoroutine;

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
        Debug.Log("LLKJ");
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

    private void DestroyBox()
    {
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(this.hoverCoroutine);
        }
        if (this.tempInfoBox != null)
        {
            Destroy(this.tempInfoBox);
        }
    }

    /// <summary>
    /// Displays the item's info box after a certain amount of time if player keeps mouse over item
    /// </summary>
    IEnumerator DisplayInfoOnHover()
    {
        yield return new WaitForSeconds(this.hoverDuration);
        if (this.itemHolder.HasItem())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            this.tempInfoBox = Instantiate(infoBox, mousePos, Quaternion.identity);
            Effect_Info_Box boxScript = this.tempInfoBox.GetComponent<Effect_Info_Box>();
            Item_Description item = GameManager.instance.STR.GetItemDescription(this.itemHolder.GetItemType());
            boxScript.UpdateText(item);
            this.tempInfoBox.SetActive(true);
        }
    }
}
