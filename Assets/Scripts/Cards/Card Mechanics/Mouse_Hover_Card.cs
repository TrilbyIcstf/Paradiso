using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Hover_Card : ManagerBehavior
{
    [SerializeField]
    private GameObject infoBox;

    [SerializeField]
    private Active_Card cardStats;

    [SerializeField]
    private bool isPlayerCard;

    private GameObject tempInfoBox;

    private float hoverDuration = 0.5f;
    private Coroutine hoverCoroutine;

    private void Update()
    {
        if (GM.CUI.isHoldingCard)
        {
            DestroyBox();
        }
    }

    private void OnMouseEnter()
    {
        if (this.cardStats.GetEffect() == CardEffects.None) { return; }
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
        Card_Effect_Description eff = GameManager.instance.STR.GetCardEffectDescription(this.cardStats.GetEffect());
        boxScript.UpdateText(eff, this.isPlayerCard);
        this.tempInfoBox.SetActive(true);
    }
}
