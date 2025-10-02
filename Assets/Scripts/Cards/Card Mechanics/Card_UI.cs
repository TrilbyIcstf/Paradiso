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
    private Canvas uiCanvas;

    private int sortingOrder = -1;

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

    public void SetSortingOrder(int val)
    {
        this.sortingOrder = val;
        this.cardSprite.sortingOrder = val;
        this.uiCanvas.sortingOrder = val;
        this.elementIcon.sortingOrder = val;
    }

    public int GetSortingOrder()
    {
        return this.sortingOrder;
    }
}
