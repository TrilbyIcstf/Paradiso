using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all UI and animations for card objects
/// </summary>
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
    private List<SpriteRenderer> effectIcons;

    [SerializeField]
    private Canvas uiCanvas;

    private int sortingOrder = -1;

    private void Awake()
    {
        StartCoroutine(OnSpawnInteractionDelay());
    }

    /// <summary>
    /// Sets the card to interactable after a short delay. Used to prevent it from messing with mouse clicks when spawning on top of the deck.
    /// </summary>
    private IEnumerator OnSpawnInteractionDelay()
    {
        Transform[] cardObjects = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform obj in cardObjects)
        {
            obj.gameObject.layer = 2;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (Transform obj in cardObjects)
        {
            obj.gameObject.layer = 0;
        }
    }

    /// <summary>
    /// Emphasizes the card by making it pulse larger for a short time.
    /// </summary>
    public IEnumerator EmphasizeCard()
    {
        GameObject spriteObject = cardSprite.gameObject;
        Vector3 originalScale = spriteObject.transform.localScale;
        float sizeGoal = originalScale.x * 1.25f;

        // Pulse larger
        yield return new WaitUntilOrTimeout(() =>
        {
            Vector3 localScale = spriteObject.transform.localScale;
            localScale *= 1.0f + (4.0f * Time.deltaTime);
            spriteObject.transform.localScale = localScale;
            return localScale.x >= sizeGoal;
        }, 0.5f);

        // Hold for a moment
        yield return new WaitForSeconds(0.15f);

        // Pulse smaller
        yield return new WaitUntilOrTimeout(() =>
        {
            Vector3 localScale = spriteObject.transform.localScale;
            localScale /= 1.0f + (1.5f * Time.deltaTime);
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
            CardElement.Nil => this.elementSpriteList[0],
            CardElement.Fire => this.elementSpriteList[1],
            CardElement.Wind => this.elementSpriteList[2],
            CardElement.Earth => this.elementSpriteList[3],
            CardElement.Water => this.elementSpriteList[4],
            _ => this.elementSpriteList[0]
        };
    }

    public void SetEffects(List<CardEffect> val)
    {
        for (int i = 0; i < val.Count; i++)
        {
            CardEffect effect = val[i];
            switch (effect)
            {
                case CardEffect.None:
                    this.effectIcons[i].enabled = false;
                    break;
                default:
                    this.effectIcons[i].enabled = true;
                    Sprite effectSprite = GameManager.instance.STR.GetCardEffectDescription(effect).effectSprite;
                    this.effectIcons[i].sprite = effectSprite;
                    break;
            }
        }
    }

    public void SetSortingOrder(int val)
    {
        this.sortingOrder = val;
        this.cardSprite.sortingOrder = val;
        this.uiCanvas.sortingOrder = val;
        this.elementIcon.sortingOrder = val;
        foreach (SpriteRenderer icon in this.effectIcons)
        {
            icon.sortingOrder = val;
        }
    }

    public void SetToDefaultSorting(int val)
    {
        this.cardSprite.sortingLayerID = 0;
        this.uiCanvas.sortingLayerID = 0;
        this.elementIcon.sortingLayerID = 0;
        foreach (SpriteRenderer icon in this.effectIcons)
        {
            icon.sortingLayerID = 0;
        }
        SetSortingOrder(val);
    }

    public int GetSortingOrder()
    {
        return this.sortingOrder;
    }
}
