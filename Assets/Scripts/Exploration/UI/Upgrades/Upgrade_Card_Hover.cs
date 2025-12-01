using System.Collections;
using UnityEngine;

public class Upgrade_Card_Hover : MonoBehaviour
{
    private const float SelectTime = 0.5f;
    private const float SelectSize = 0.3f;

    private bool selected = false;

    private Card_Base cardBase;
    private Card_Base cardUpgrade;

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

    private void OnMouseEnter()
    {
        if (this.emphasisCo != null)
        {
            StopCoroutine(this.emphasisCo);
            this.emphasisCo = null;
        }

        //this.cardUI.SetCardBase(this.cardUpgrade);
        this.emphasisCo = StartCoroutine(HoverCard());
    }

    private void OnMouseExit()
    {
        if (this.emphasisCo != null)
        {
            StopCoroutine(this.emphasisCo);
            this.emphasisCo = null;
        }

        //this.cardUI.SetCardBase(this.cardBase);
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
        float time = 0.0f;

        yield return new WaitUntil(() =>
        {
            time += Time.deltaTime;
            float timeRatio = time / SelectTime;
            float sizeRatio = SelectSize * timeRatio;
            Vector3 scale = this.baseScale * (1.0f + sizeRatio);
            spriteObject.transform.localScale = scale;

            return time >= SelectTime;
        });
    }

    private IEnumerator UnhoverCard()
    {
        float time = 0.0f;

        yield return new WaitUntil(() =>
        {
            time += Time.deltaTime;
            float timeRatio = 1.0f - (time / SelectTime);
            float sizeRatio = SelectSize * timeRatio;
            Vector3 scale = this.baseScale * (1.0f + sizeRatio);
            spriteObject.transform.localScale = scale;

            return time >= SelectTime;
        });
    }
}
