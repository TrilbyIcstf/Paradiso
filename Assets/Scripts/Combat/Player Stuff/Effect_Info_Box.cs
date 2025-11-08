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

    private int pos = 0;

    private void Awake()
    {
        SetPos();
    }

    private void Update()
    {
        SetPos();
    }

    private void SetPos()
    {
        float halfWidth = sr.bounds.size.x / 2;
        float height = sr.bounds.size.y;

        Vector3 mousePos = Input.mousePosition;
        float screenCenter = Screen.width / 2;
        bool mouseLeftHalf = mousePos.x < screenCenter;

        Vector3 adjustedMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        adjustedMousePos.z = 0;
        adjustedMousePos.x += (halfWidth + 0.5f) * (mouseLeftHalf ? 1.0f : -1.0f);
        switch (pos)
        {
            case 1:
                adjustedMousePos.y += height + 0.5f;
                break;
            case 2:
                adjustedMousePos.y -= height + 0.5f;
                break;
            default:
                break;
        }

        gameObject.transform.position = adjustedMousePos;
    }

    public void UpdateText(Card_Effect_Description eff, bool isPlayerCard, int pos = 0)
    {
        this.effectIcon.sprite = eff.effectSprite;
        this.title.text = eff.title;
        this.description.text = isPlayerCard ? eff.playerDescription : eff.EnemyDescription;
        this.pos = pos;
    }

    public void UpdateText(Item_Description item)
    {
        this.effectIcon.sprite = item.itemSprite;
        this.title.text = item.title;
        this.description.text = item.description;
    }
}
