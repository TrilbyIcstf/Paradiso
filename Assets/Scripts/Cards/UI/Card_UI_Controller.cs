using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_UI_Controller : MonoBehaviour
{
    [SerializeField]
    private Text powerText;
    [SerializeField]
    private Text defenseText;

    [SerializeField]
    private Image elementIcon;

    [SerializeField]
    private List<Image> effectIcons;

    [SerializeField]
    private Canvas uiCanvas;

    private int sortingOrder = -1;

    /// <summary>
    /// Sets the card to interactable after a short delay. Used to prevent it from messing with mouse clicks when spawning on top of the deck.
    /// </summary>
    public IEnumerator OnSpawnInteractionDelay()
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
        GameObject spriteObject = gameObject;
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
        this.elementIcon.sprite = Static_Object_Manager.instance.GetElementIcon(val);
    }

    public void SetEffects(List<CardEffects> val)
    {
        for (int i = 0; i < val.Count; i++)
        {
            CardEffects effect = val[i];
            switch (effect)
            {
                case CardEffects.None:
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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = val;
        }
        this.uiCanvas.sortingOrder = val;
    }

    public void SetToDefaultSorting(int val)
    {
        this.uiCanvas.sortingLayerID = 0;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerID = 0;
        }
        SetSortingOrder(val);
    }

    public int GetSortingOrder()
    {
        return this.sortingOrder;
    }
}
