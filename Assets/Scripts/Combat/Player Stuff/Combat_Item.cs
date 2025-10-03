using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat_Item : MonoBehaviour
{
    [SerializeField]
    private Item_Active item;
    [SerializeField]
    private SpriteRenderer itemSprite;
    [SerializeField]
    private Image cooldownOverlay;

    private bool inCooldown = false;
    private float cooldownTime = 0;
    private float cooldownTimer = 0;

    void Start()
    {
        if (this.item != null)
        {
            this.itemSprite.sprite = this.item.sprite;
        }
    }

    private void Update()
    {
        if (this.inCooldown)
        {
            this.cooldownTimer += Time.deltaTime;
            this.cooldownOverlay.fillAmount = 1 - (this.cooldownTimer / this.cooldownTime);
            if (this.cooldownTimer >= this.cooldownTime)
            {
                this.inCooldown = false;
                this.cooldownOverlay.enabled = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (this.item == null)
        {
            return;
        }

        if (this.inCooldown) { return; }

        if (this.item.CanActivate())
        {
            if (GameManager.instance.CS.CanAffordEnergy(this.item.energyCost))
            {
                GameManager.instance.CS.SubtractEnergy(this.item.energyCost, true);
                this.item.Activate();

                if (this.item.GetCooldown() > 0)
                {
                    StartCooldown();
                }
            }
            else
            {
                GameManager.instance.CUI.InvalidEnergyCost();
            }
        }
    }

    private void StartCooldown()
    {
        this.cooldownTime = this.item.GetCooldown();
        this.cooldownTimer = 0;
        this.inCooldown = true;
        this.cooldownOverlay.enabled = true;
        this.cooldownOverlay.fillAmount = 1;
    }

    public void SetItem(Item_Active val)
    {
        this.item = val;

        if (this.item != null)
        {
            this.itemSprite.sprite = this.item.sprite;
        }
    }
}
