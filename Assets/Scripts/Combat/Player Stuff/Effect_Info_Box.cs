using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Info_Box : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private SpriteRenderer effectIcon;

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text Description;

    private void Update()
    {
        float halfWidth = sr.bounds.size.x / 2;

        Vector3 mousePos = Input.mousePosition;
        float screenCenter = Screen.width / 2;
        bool mouseLeftHalf = mousePos.x < screenCenter;

        Vector3 adjustedMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        adjustedMousePos.z = 0;
        adjustedMousePos.x += (halfWidth + 0.5f) * (mouseLeftHalf ? 1.0f : -1.0f);

        gameObject.transform.position = adjustedMousePos;
    }

    public void UpdateText(Card_Effect_Description eff, bool isPlayerCard)
    {
        this.effectIcon.sprite = eff.effectSprite;
        this.title.text = eff.title;
        this.Description.text = isPlayerCard ? eff.playerDescription : eff.EnemyDescription;
    }

    public void UpdateText(Item_Description item)
    {
        this.effectIcon.sprite = item.itemSprite;
        this.title.text = item.title;
        this.Description.text = item.description;
    }
}
