using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_UI_Controller : MonoBehaviour
{
    [SerializeField]
    protected Text powerText;
    [SerializeField]
    protected Text defenseText;

    [SerializeField]
    protected Image elementIcon;

    [SerializeField]
    protected Image affinityIcon;

    [SerializeField]
    protected List<Image> effectIcons;

    [SerializeField]
    protected Canvas uiCanvas;

    protected int sortingOrder = -1;

    /// <summary>
    /// Sets the card to interactable after a short delay. Used to prevent it from messing with mouse clicks when spawning on top of the deck.
    /// </summary>
    protected IEnumerator OnSpawnInteractionDelay()
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
    public IEnumerator EmphasizeCard(float goal = 1.25f)
    {
        GameObject spriteObject = gameObject;
        Vector3 originalScale = spriteObject.transform.localScale;
        float sizeGoal = originalScale.x * goal;

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
        StartCoroutine(EmphasizeCard(1.25f));
    }

    public void EmphasizeCardSmallCo()
    {
        StartCoroutine(EmphasizeCard(1.1f));
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

    public void SetAffinity(CardAffinity val)
    {
        this.affinityIcon.sprite = Static_Object_Manager.instance.GetAffinityIcon(val);
    }

    public void SetEffects(List<CardEffect> val)
    {
        foreach (Image im in this.effectIcons)
        {
            im.enabled = false;
        }

        Dictionary<int, CardEffect> effectPositions = new Dictionary<int, CardEffect>();
        int pos = 0;

        foreach (CardEffect effect in val)
        {
            int effectPos = effect.DirectionOverride() switch
            {
                Direction.Left => 2,
                Direction.Right => 1,
                _ => pos
            };

            if (!effectPositions.ContainsKey(effectPos))
            {
                effectPositions[effectPos] = effect;
            } else
            {
                effectPositions[effectPos + 1] = effectPositions[effectPos];
                effectPositions[effectPos] = effect;
            }

            if (effectPos == pos) { pos++; }
        }

        foreach (KeyValuePair<int, CardEffect> effect in effectPositions)
        {
            switch (effect.Value)
            {
                case CardEffect.None:
                    this.effectIcons[effect.Key].enabled = false;
                    break;
                default:
                    this.effectIcons[effect.Key].enabled = true;
                    Sprite effectSprite = GameManager.instance.STR.GetCardEffectDescription(effect.Value).effectSprite;
                    this.effectIcons[effect.Key].sprite = effectSprite;
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
