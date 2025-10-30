using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the textbox that explains the item upon pickup
/// </summary>
public class Item_Pickup_Box_Controller : MonoBehaviour
{
    [SerializeField]
    private Text itemTitle;

    [SerializeField]
    private Text itemDescription;

    [SerializeField]
    private Image itemSprite;

    public void SetTextBox(Item_Base item)
    {
        this.itemTitle.text = item.GetName();
        this.itemDescription.text = GameManager.instance.STR.GetItemDescription(item.GetItem()).description;
        this.itemSprite.sprite = item.GetSprite();

        gameObject.SetActive(true);
    }

    public void HideTextBox()
    {
        Debug.Log("Button!");
        gameObject.SetActive(false);
    }
}
