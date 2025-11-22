using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the info box used to display card effect details
/// </summary>
public class Effect_Info_Box : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private SpriteRenderer effectIcon;

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text description;

    public void UpdateText(Card_Effect_Description eff, bool isPlayerCard)
    {
        this.effectIcon.sprite = eff.effectSprite;
        this.title.text = eff.title;
        this.description.text = isPlayerCard ? eff.playerDescription : eff.EnemyDescription;
    }

    public void UpdateText(Item_Description item)
    {
        this.effectIcon.sprite = item.itemSprite;
        this.title.text = item.title;
        this.description.text = item.description;
    }
}
