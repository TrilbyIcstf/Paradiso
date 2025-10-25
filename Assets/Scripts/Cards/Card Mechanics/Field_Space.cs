using UnityEngine;

/// <summary>
/// Tracks the contents of the playing field
/// </summary>
public class Field_Space
{
    private int position;
    private Active_Card card;
    private GameObject cardObject;

    public Field_Space(int position)
    {
        this.position = position;
        this.card = null;
    }

    public int GetPosition()
    {
        return position;
    }

    public Active_Card GetCard()
    {
        return card;
    }

    public GameObject GetCardObject()
    {
        return cardObject;
    }

    public void SetCard(GameObject card)
    {
        Active_Card activeCard = card.GetComponent<Active_Card>();
        this.card = activeCard;
        this.cardObject = card;
    }

    public void ClearCard()
    {
        this.cardObject = null;
        this.card = null;
    }
}
