using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mouse_Hover_Card : ManagerBehavior
{
    [SerializeField]
    private GameObject infoBox;

    [SerializeField]
    private bool isPlayerCard;

    private List<GameObject> tempInfoBoxes = new List<GameObject>();

    private float hoverDuration = 0.5f;
    private Coroutine hoverCoroutine;

    private void Update()
    {
        if (GM.CUI.isHoldingCard)
        {
            DestroyBox();
        }
    }

    private void OnDestroy()
    {
        DestroyBox();
    }

    protected void MouseEnter()
    {
        if (GetEffects().Count == 0) { return; }
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        this.hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    protected void MouseExit()
    {
        DestroyBox();
    }

    private void DestroyBox()
    {
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(this.hoverCoroutine);
        }
        foreach (GameObject box in this.tempInfoBoxes)
        {
            Destroy(box);
        }
    }

    /// <summary>
    /// Displays info box if mouse is held over card for certain duration
    /// </summary>
    IEnumerator DisplayInfoOnHover()
    {
        yield return new WaitForSeconds(this.hoverDuration);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        List<CardEffect> effectList = GetEffects();
        for (int i = 0; i < effectList.Count; i++)
        {
            GameObject newBox = Instantiate(infoBox, mousePos, Quaternion.identity);
            Effect_Info_Box boxScript = newBox.GetComponent<Effect_Info_Box>();
            Card_Effect_Description eff = GameManager.instance.STR.GetCardEffectDescription(effectList[i]);
            boxScript.UpdateText(eff, this.isPlayerCard, i);
            newBox.SetActive(true);
            this.tempInfoBoxes.Add(newBox);
        }
    }

    protected abstract List<CardEffect> GetEffects();
}
