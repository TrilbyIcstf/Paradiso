using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interaction of the player hovering over an item in combat
/// </summary>
public class Combat_Item_Hover : MonoBehaviour
{
    [SerializeField]
    private GameObject infoBox;

    [SerializeField]
    private Combat_Item itemHolder;

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

    private void OnMouseEnter()
    {
        CostPreviewOn();
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    private void OnMouseExit()
    {
        CostPreviewOff();
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
        if (this.itemHolder.GetItem() != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            this.tempInfoBox = Instantiate(infoBox, mousePos, Quaternion.identity);
            Effect_Info_Box boxScript = this.tempInfoBox.GetComponent<Effect_Info_Box>();
            Item_Description item = GameManager.instance.STR.GetItemDescription(this.itemHolder.GetItem().item);
            boxScript.UpdateText(item);
            this.tempInfoBox.SetActive(true);
        }
    }

    /// <summary>
    /// Turns on the preview showing the item's energy cost on the player's energy bar
    /// </summary>
    private void CostPreviewOn()
    {
        if (this.itemHolder.GetItem() != null)
        {
            GameManager.instance.CUI.SetEnergyPreview(this.itemHolder.GetItem().energyCost);
        }
    }

    /// <summary>
    /// Turns off the preview showing the item's energy cost on the player's energy bar
    /// </summary>
    private void CostPreviewOff()
    {
        GameManager.instance.CUI.RemoveEnergyPreview();
    }
}
