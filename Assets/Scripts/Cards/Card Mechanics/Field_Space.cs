using UnityEngine;

public class Field_Space
{
    private int position;
    private Card_Base card;
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

    public Card_Base GetCard()
    {
        return card;
    }

    public GameObject GetCardObject()
    {
        return cardObject;
    }

    public void SetCard(GameObject card)
    {
        Card_Base cardBase = card.GetComponent<Card_Base>();
        this.card = cardBase;
        this.cardObject = card;
    }

    public void ClearCard()
    {
        this.cardObject = null;
        this.card = null;
    }
}
