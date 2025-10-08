using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_UI : MonoBehaviour
{
    [SerializeField]
    private Text powerText;
    [SerializeField]
    private Text defenseText;

    [SerializeField]
    private SpriteRenderer cardSprite;

    [SerializeField]
    private SpriteRenderer elementIcon;

    /// <summary>
    /// List of element sprites:
    /// 0 - Nill
    /// 1 - Fire
    /// 2 - Wind
    /// 3 - Earth
    /// 4 - Water
    /// </summary>
    [SerializeField]
    private List<Sprite> elementSpriteList;

    [SerializeField]
    private SpriteRenderer effectIcon;

    [SerializeField]
    private Canvas uiCanvas;

    private int sortingOrder = -1;

    public IEnumerator EmphasizeCard()
    {
        GameObject spriteObject = cardSprite.gameObject;
        Vector3 originalScale = spriteObject.transform.localScale;
        float sizeGoal = originalScale.x * 1.25f;
        yield return new WaitUntilOrTimeout(() =>
        {
            Vector3 localScale = spriteObject.transform.localScale;
            localScale *= 1.005f;
            spriteObject.transform.localScale = localScale;
            return localScale.x >= sizeGoal;
        }, 0.5f);
        yield return new WaitForSeconds(0.15f);
        yield return new WaitUntilOrTimeout(() =>
        {
            Vector3 localScale = spriteObject.transform.localScale;
            localScale /= 1.002f;
            spriteObject.transform.localScale = localScale;
            return localScale.x <= originalScale.x;
        }, 0.5f);
        spriteObject.transform.localScale = originalScale;
    }

    public void EmphasizeCardCo()
    {
        StartCoroutine(EmphasizeCard());
    }

    public void SetPower(int power)
    {
        this.powerText.text = power.ToString();
    }

    public void SetDefense(int defense)
    {
        this.defenseText.text = defense.ToString();
    }

    public void SetElement(CardElement val)
    {
        this.elementIcon.sprite = val switch
        {
            CardElement.Nill => this.elementSpriteList[0],
            CardElement.Fire => this.elementSpriteList[1],
            CardElement.Wind => this.elementSpriteList[2],
            CardElement.Earth => this.elementSpriteList[3],
            CardElement.Water => this.elementSpriteList[4],
            _ => this.elementSpriteList[0]
        };
    }

    public void SetEffect(CardEffects val)
    {
        switch (val)
        {
            case CardEffects.None:
                this.effectIcon.enabled = false;
                break;
            default:
                this.effectIcon.enabled = true;
                break;
        }
    }

    public void SetSortingOrder(int val)
    {
        this.sortingOrder = val;
        this.cardSprite.sortingOrder = val;
        this.uiCanvas.sortingOrder = val;
        this.elementIcon.sortingOrder = val;
        this.effectIcon.sortingOrder = val;
    }

    public void SetToDefaultSorting(int val)
    {
        this.cardSprite.sortingLayerID = 0;
        this.uiCanvas.sortingLayerID = 0;
        this.elementIcon.sortingLayerID = 0;
        this.effectIcon.sortingLayerID = 0;
        SetSortingOrder(val);
    }

    public int GetSortingOrder()
    {
        return this.sortingOrder;
    }
}
