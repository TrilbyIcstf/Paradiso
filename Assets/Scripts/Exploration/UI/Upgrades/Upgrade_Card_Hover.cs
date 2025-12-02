using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Upgrade_Card_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float SelectSpeed = 2.5f;
    private const float SelectSize = 1.15f;

    public Card_Base cardBase;
    public Card_Base cardUpgrade;

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

        this.cardUI.SetCardBase(this.cardUpgrade);
        this.emphasisCo = StartCoroutine(HoverCard());
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (this.emphasisCo != null)
        {
            StopCoroutine(this.emphasisCo);
            this.emphasisCo = null;
        }

        this.cardUI.SetCardBase(this.cardBase);
        this.emphasisCo = StartCoroutine(UnhoverCard());
    }

    public void SetCards(Card_Base cardBase, Card_Base cardUpgrade)
    {
        this.cardBase = cardBase;
        this.cardUpgrade = cardUpgrade;

        this.cardUI.SetCardBase(cardBase);
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
}
