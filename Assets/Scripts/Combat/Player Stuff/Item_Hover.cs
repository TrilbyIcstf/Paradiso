using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Hover : MonoBehaviour
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
    }

    private void OnMouseEnter()
    {
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    private void OnMouseExit()
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

    IEnumerator DisplayInfoOnHover()
    {
        yield return new WaitForSeconds(this.hoverDuration);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        this.tempInfoBox = Instantiate(infoBox, mousePos, Quaternion.identity);
        Effect_Info_Box boxScript = this.tempInfoBox.GetComponent<Effect_Info_Box>();
        Item_Description item = GameManager.instance.STR.GetItemDescription(this.itemHolder.GetItem().item);
        boxScript.UpdateText(item);
        this.tempInfoBox.SetActive(true);
    }
}
