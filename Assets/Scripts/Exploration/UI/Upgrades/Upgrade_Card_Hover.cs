using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Upgrade_Card_Hover : Mouse_Hover_Card, IPointerEnterHandler, IPointerExitHandler
{
    private const float SelectSpeed = 2.5f;
    private const float SelectSize = 1.15f;

    private Vector3 baseScale;

    [SerializeField]
    private Upgrade_Card_UI cardUI;

    [SerializeField]
    private GameObject spriteObject;

    private Coroutine emphasisCo;

    private void Awake()
    {
        this.baseScale = this.spriteObject.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (this.emphasisCo != null)
        {
            StopCoroutine(this.emphasisCo);
            this.emphasisCo = null;
        }

        this.cardUI.UpgradeCard();
        this.emphasisCo = StartCoroutine(HoverCard());

        MouseEnter();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (this.emphasisCo != null)
        {
            StopCoroutine(this.emphasisCo);
            this.emphasisCo = null;
        }

        this.cardUI.BaseCard();
        this.emphasisCo = StartCoroutine(UnhoverCard());

        MouseExit();
    }

    private IEnumerator HoverCard()
    {
        float ratio = spriteObject.transform.localScale.magnitude / this.baseScale.magnitude;

        yield return new WaitUntil(() =>
        {
            float time = Time.deltaTime;
            ratio += time * SelectSpeed;
            ratio = Mathf.Min(SelectSize, ratio);
            Vector3 scale = this.baseScale * ratio;
            spriteObject.transform.localScale = scale;

            return ratio >= SelectSize;
        });
    }

    private IEnumerator UnhoverCard()
    {
        float ratio = spriteObject.transform.localScale.magnitude / this.baseScale.magnitude;

        yield return new WaitUntil(() =>
        {
            float time = Time.deltaTime;
            ratio -= time * SelectSpeed;
            ratio = Mathf.Max(1.0f, ratio);
            Vector3 scale = this.baseScale * ratio;
            spriteObject.transform.localScale = scale;

            return ratio <= 1;
        });
    }

    protected override List<CardEffect> GetEffects()
    {
        Card_Base cardUpgrade = GetComponent<Upgrade_Card>().GetUpgrade();
        return cardUpgrade.GetEffects();
    }
}
