using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the inventory window during exploration.
/// </summary>
public class Inventory_Box_Controller : MonoBehaviour
{
    private InventoryModes currentMode = InventoryModes.Deck;

    [SerializeField]
    private GameObject innerBox;

    // UI variables for the deck
    private const int CardsPerRow = 5;
    private const float CardMarginY = 60.0f;
    private const float CardPaddingX = 40.0f;
    private const float CardPaddingY = 40.0f;

    private List<GameObject> deckCards = new List<GameObject>();

    [SerializeField]
    private GameObject cardBase;

    // UI variables for the item list

    public void SetupMode()
    {
        switch (this.currentMode)
        {
            case InventoryModes.Deck:
                SetupDeck();
                break;
            case InventoryModes.Items:
                SetupItems();
                break;
        }
    }

    private void SetupDeck()
    {
        List<Card_Base> playerDeck = GameManager.instance.PM.GetDeck();

        float boxWidth = this.innerBox.GetComponent<RectTransform>().rect.width - (CardPaddingX * 2);

        for (int i = 0; i < playerDeck.Count; i++)
        {
            GameObject tempCard = Instantiate(this.cardBase, this.innerBox.transform);
            this.deckCards.Add(tempCard);
        }

        StartCoroutine(SetCardPositions());
    }

    private IEnumerator SetCardPositions()
    {
        yield return new WaitForEndOfFrame();

        if (this.deckCards.Count == 0) { yield break; }

        RectTransform rectBox = this.innerBox.GetComponent<RectTransform>();
        float boxWidth = rectBox.rect.width - (CardPaddingX * 2);
        GameObject firstCard = this.deckCards[0];
        float cardWidth = firstCard.GetComponent<RectTransform>().rect.width * firstCard.transform.localScale.x;
        float cardHeight = firstCard.GetComponent<RectTransform>().rect.height * firstCard.transform.localScale.y;
        float cardSpacingX = cardWidth + ((boxWidth - (cardWidth * CardsPerRow)) / (CardsPerRow - 1));
        float cardSpacingY = cardHeight + CardMarginY;

        int highestRow = 1;

        for (int i = 0; i < this.deckCards.Count; i++)
        {
            int collumn = i % 5;
            int row = Mathf.FloorToInt(i / 5);
            highestRow = row;

            GameObject tempCard = this.deckCards[i];

            Vector3 cardPos = tempCard.transform.localPosition;
            cardPos.x += CardPaddingX + (cardSpacingX * collumn);
            cardPos.y -= CardPaddingY + (cardSpacingY * row);
            tempCard.transform.localPosition = cardPos;
        }

        float totalHeight = (cardSpacingY * highestRow) + (CardPaddingY * 2) + cardHeight;

        Rect rect = rectBox.rect;
        rectBox.sizeDelta = new Vector2(rect.width, totalHeight);
    }

    private void SetupItems()
    {

    }

    public void SetVisible(bool val)
    {
        gameObject.SetActive(val);
    }
}

enum InventoryModes
{
    Deck,
    Items
}