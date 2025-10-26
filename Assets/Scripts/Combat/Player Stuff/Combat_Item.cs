using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Object representation of one of the player's active items during combat
/// </summary>
public class Combat_Item : ManagerBehavior
{
    [SerializeField]
    private Item_Active item;
    [SerializeField]
    private int position;
    [SerializeField]
    private SpriteRenderer itemSprite;
    [SerializeField]
    private Image cooldownOverlay;

    private bool inCooldown = false;
    private float cooldownTime = 1;
    private float cooldownTimer = 0;

    private bool itemLocked = false;

    void Start()
    {
        GameManager.instance.CPI.SetItemSlot(gameObject, this.position);
        if (this.item != null)
        {
            this.itemSprite.sprite = this.item.sprite;
        }
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void OnMouseDown()
    {
        if (this.item == null)
        {
            return;
        }

        if (this.inCooldown || this.itemLocked) { return; }

        if (this.item.CanActivate())
        {
            if (GM.CPS.CanAffordEnergy(this.item.energyCost))
            {
                GM.CPS.SubtractEnergy(this.item.energyCost, true);
                this.item.Activate();

                if (this.item.GetCooldown() > 0)
                {
                    StartCooldown();
                }
            }
            else
            {
                GM.CUI.InvalidEnergyCost();
            }
        }
    }

    private void StartCooldown()
    {
        this.cooldownTime = this.item.GetCooldown();
        this.cooldownTimer = 0;
        this.inCooldown = true;
        this.cooldownOverlay.fillAmount = 1;
    }

    private void UpdateCooldown()
    {
        if (this.itemLocked)
        {
            this.cooldownOverlay.fillAmount = 1;
        } else if (this.inCooldown)
        {
            this.cooldownTimer += Time.deltaTime;
            this.cooldownOverlay.fillAmount = 1 - (this.cooldownTimer / this.cooldownTime);
            if (this.cooldownTimer >= this.cooldownTime)
            {
                this.inCooldown = false;
            }
        } else
        {
            this.cooldownOverlay.fillAmount = 0;
        }
    }

    public Item_Active GetItem()
    {
        return this.item;
    }

    public void SetItem(Item_Active val)
    {
        this.item = val;

        if (this.item != null)
        {
            this.itemSprite.sprite = this.item.sprite;
        }
    }

    public void SetLock(bool val)
    {
        if (this.item != null)
        {
            this.itemLocked = val;
        }
    }
}
