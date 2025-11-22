using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the inventory window during exploration.
/// </summary>
public class Inventory_Box_Controller : MonoBehaviour
{
    private InventoryModes currentMode = InventoryModes.Deck;

    [SerializeField]
    private GameObject innerBox;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private Inventory_Health_Amount healthText;

    // UI variables for the deck
    private const int CardsPerRow = 5;
    private const float CardMarginY = 60.0f;
    private const float CardPaddingX = 40.0f;
    private const float CardPaddingY = 40.0f;

    private List<GameObject> deckCards = new List<GameObject>();

    [SerializeField]
    private GameObject cardBase;

    // UI variables for the item list
    private const int ItemsPerRow = 3;
    private const float ItemMarginY = 100.0f;
    private const float ItemPaddingX = 80.0f;
    private const float ItemPaddingY = 80.0f;

    private List<GameObject> itemList = new List<GameObject>();

    [SerializeField]
    private GameObject itemBase;

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

    public void SetDeckMode()
    {
        if (this.currentMode != InventoryModes.Deck)
        {
            this.currentMode = InventoryModes.Deck;
            SetupMode();
        }
    }

    public void SetItemMode()
    {
        if (this.currentMode != InventoryModes.Items)
        {
            this.currentMode = InventoryModes.Items;
            SetupMode();
        }
    }

    private void SetupDeck()
    {
        DestroyTempItems();

        List<Card_Base> playerDeck = GameManager.instance.PM.GetDeck();

        float boxWidth = this.innerBox.GetComponent<RectTransform>().rect.width - (CardPaddingX * 2);

        for (int i = 0; i < playerDeck.Count; i++)
        {
            Card_Base deckCard = playerDeck[i];
            GameObject tempCard = Instantiate(this.cardBase, this.innerBox.transform);
            Card_UI_Controller tempController = tempCard.GetComponent<Card_UI_Controller>();
            tempController.SetPower(deckCard.GetPower());
            tempController.SetDefense(deckCard.GetDefense());
            tempController.SetElement(deckCard.GetElement());
            tempController.SetAffinity(deckCard.GetAffinity());
            tempController.SetEffects(deckCard.GetEffects());
            Inventory_Mouse_Hover_Card hoverScript = tempCard.GetComponent<Inventory_Mouse_Hover_Card>();
            hoverScript.SetBaseStats(deckCard);
            ScrollRect_Drag_Handler dragHandler = tempCard.GetComponent<ScrollRect_Drag_Handler>();
            dragHandler.SetScrollRect(this.scrollRect);

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
            int collumn = i % CardsPerRow;
            int row = Mathf.FloorToInt(i / CardsPerRow);
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
        this.scrollRect.verticalNormalizedPosition = 1.0f;
    }

    private void DestroyTempCards()
    {
        for (int i = 0; i < this.deckCards.Count; i += 0)
        {
            GameObject card = this.deckCards[i];
            this.deckCards.Remove(card);
            Destroy(card);
        }
    }

    private void SetupItems()
    {
        DestroyTempCards();

        List<Item_Base> playerItems = GameManager.instance.PM.GetItems();

        float boxWidth = this.innerBox.GetComponent<RectTransform>().rect.width - (CardPaddingX * 2);
        for (int i = 0; i < playerItems.Count; i++)
        {
            Item_Base item = playerItems[i];
            GameObject tempItem = Instantiate(this.itemBase, this.innerBox.transform);
            Inventory_Item_Holder tempController = tempItem.GetComponent<Inventory_Item_Holder>();
            tempController.SetupItem(item);
            ScrollRect_Drag_Handler dragHandler = tempItem.GetComponent<ScrollRect_Drag_Handler>();
            dragHandler.SetScrollRect(this.scrollRect);

            this.itemList.Add(tempItem);
        }

        StartCoroutine(SetItemPositions());
    }

    private IEnumerator SetItemPositions()
    {
        yield return new WaitForEndOfFrame();

        if (this.itemList.Count == 0) { yield break; }

        RectTransform rectBox = this.innerBox.GetComponent<RectTransform>();
        float boxWidth = rectBox.rect.width - (ItemPaddingX * 2);
        GameObject firstItem = this.itemList[0];
        float itemWidth = firstItem.GetComponent<RectTransform>().rect.width * firstItem.transform.localScale.x;
        float itemHeight = firstItem.GetComponent<RectTransform>().rect.height * firstItem.transform.localScale.y;
        float itemSpacingX = itemWidth + ((boxWidth - (itemWidth * ItemsPerRow)) / (ItemsPerRow - 1));
        float itemSpacingY = itemHeight + ItemMarginY;

        int highestRow = 1;

        for (int i = 0; i < this.itemList.Count; i++)
        {
            int collumn = i % ItemsPerRow;
            int row = Mathf.FloorToInt(i / ItemsPerRow);
            highestRow = row;

            GameObject tempItem = this.itemList[i];

            Vector3 itemPos = tempItem.transform.localPosition;
            itemPos.x += ItemPaddingX + (itemSpacingX * collumn);
            itemPos.y -= ItemPaddingY + (itemSpacingY * row);
            tempItem.transform.localPosition = itemPos;
        }

        float totalHeight = (itemSpacingY * highestRow) + (ItemPaddingY * 2) + itemHeight;

        Rect rect = rectBox.rect;
        rectBox.sizeDelta = new Vector2(rect.width, totalHeight);
        this.scrollRect.verticalNormalizedPosition = 1.0f;
    }

    private void DestroyTempItems()
    {
        for (int i = 0; i < this.itemList.Count; i += 0)
        {
            GameObject item = this.itemList[i];
            this.itemList.Remove(item);
            Destroy(item);
        }
    }

    public void OpenInventory()
    {
        SetVisible(true);
        SetupMode();
        this.healthText.UpdateHealth();
    }

    public void CloseInventory()
    {
        DestroyTempCards();
        DestroyTempItems();
        SetVisible(false);
    }

    private void SetVisible(bool val)
    {
        gameObject.SetActive(val);
    }
}

[SerializeField]
public enum InventoryModes
{
    Deck,
    Items
}