using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Item : MonoBehaviour
{
    [SerializeField]
    private Item_Active item;
    [SerializeField]
    private SpriteRenderer itemSprite;

    void Start()
    {
        if (item != null)
        {
            this.itemSprite.sprite = item.sprite;
        }
    }

    private void OnMouseDown()
    {
        if (item == null)
        {
            return;
        }

        if (GameManager.instance.CS.CanAffordEnergy(this.item.energyCost))
        {
            GameManager.instance.CS.SubtractEnergy(this.item.energyCost, true);
            item.Activate();
        }
        else
        {
            GameManager.instance.CUI.InvalidEnergyCost();
        }
    }
}
